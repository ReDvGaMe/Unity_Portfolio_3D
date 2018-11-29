using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	#region 변수
	//private Rigidbody _rigid;
	private Animator _anim;
	private CharacterController _cc;
	public GameObject _camera;
	// public CharacterController _characterController;

	#region 정보
	[Tooltip("캐릭터 기본 이동 속도")] [SerializeField] [Range(0f, 10f)]
	private float _basicMoveSpeed = 10f;
	// 캐릭터 이동 속도
	private float _moveSpeed = 10f;
	// 캐릭터 이동 관련 벡터
	private Vector3 _movement;
	[Tooltip("캐릭터 점프 파워")] [SerializeField] [Range(0f, 10f)]
	private float _jumpPower = 5f;
	// 캐릭터 점프 속도
	private float _yVelocity;
	#endregion

	#region 입력 키
	// 수직 키 입력값
	private float _keyVertical = 0f;

	// 수평 키 입력값
	private float _keyHorizontal = 0f;

	// 달리기 키
	private KeyCode _runKey = KeyCode.LeftShift;

	// 점프 키
	private KeyCode _jumpKey = KeyCode.Space;

	// 앉기 키
	private KeyCode _crouchKey = KeyCode.LeftControl;

	// 카메라 회전 키
	private KeyCode _cameraMoveKey = KeyCode.LeftAlt;

	[HideInInspector]
	// 화면 회전용
	public float _yRot;
	#endregion

	#region 상태
	// 달리고있는지 검사할 변수
	private bool _runState = false;

	// 캐릭터가 공중에 있는지 검사할 변수
	private bool _jumpState = false;
	#endregion
	#endregion

	#region 기본 함수
	private void Awake()
	{
		//_rigid = GetComponent<Rigidbody>();
		_anim = transform.GetComponentInChildren<Animator>();
		_cc = gameObject.GetComponent<CharacterController>();
	}

	private void FixedUpdate()
	{
		RotateCharacter();
		MoveCharacter();
		_yVelocity = Physics.gravity.y * Time.deltaTime;

		//_anim.SetFloat("_Velocity_Y", _rigid.velocity.y);
	}

	// Update is called once per frame
	void Update ()
    {
		InputKey();
		_yRot += Input.GetAxis("Mouse X") * 20 * Time.deltaTime;
	}

	// 키 입력
	private void InputKey()
    {
		// 방향키 입력을 받아옴
		//_keyVertical = Input.GetAxis("Vertical");
		//_keyHorizontal = Input.GetAxis("Horizontal");
		_keyVertical = Mathf.Lerp(_anim.GetFloat("_SpeedVertical"), Input.GetAxis("Vertical"), Time.fixedTime);
		_keyHorizontal = Mathf.Lerp(_anim.GetFloat("_SpeedHorizontal"), Input.GetAxis("Horizontal"), Time.fixedTime);
		
		// 달리기 키 검사
		if (Input.GetKey(_runKey))
		{
			if (_keyVertical > 0f) _runState = true;
			else _runState = false;

			_anim.SetBool("_Run", _runState);

		}
		else if (Input.GetKeyUp(_runKey))
		{
			_runState = false;
			_anim.SetBool("_Run", _runState);
		}
		if (_runState) _moveSpeed = _basicMoveSpeed * 2;
		else _moveSpeed = _basicMoveSpeed;

		// 방향 키 입력을 애니메이터로 넘김
		_anim.SetFloat("_SpeedVertical", _keyVertical);
		_anim.SetFloat("_SpeedHorizontal", _keyHorizontal);
		
		if (Input.GetKeyDown(_jumpKey) && !(_jumpState))
		{
			_jumpState = true;
			_anim.SetBool("_Jump", true);
			StartCoroutine("WaitForSec", 0.25f);

			_yVelocity = _jumpPower;
			//_rigid.AddForce(Vector3.up * 7f, ForceMode.Impulse);
		}			
	}
	#endregion

	#region 캐릭터 이동 관련 함수
	// 이동 제어
	private void MoveCharacter()
	{
		_movement.Set(_keyHorizontal, _yVelocity, _keyVertical);
		_movement = _movement.normalized * _basicMoveSpeed * Time.deltaTime;

		transform.Translate(_movement);
	}
	// 회전 제어
	private void RotateCharacter()
	{
		transform.rotation = Quaternion.Euler(0, _yRot, 0);
	}
	#endregion

	private void OnCollisionStay(Collision collision)
	{
		if(collision.transform.tag == "Ground") // Ground
		{
			_jumpState = false;
			_anim.SetBool("_Jump", false);
		}
	}

	#region 코루틴
	// 타이머
	private IEnumerator WaitForSec(float time)
	{
		while(time > 0)
		{
			time -= 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
	}

	//private IEnumerator GetVeolocity()
	//{
	//	while(Mathf.Abs(_rigid.velocity.y) != 0)
	//}
	#endregion
}
