using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	#region 변수
	private Rigidbody _rigid;
	private Animator _anim;
	// public CharacterController _characterController;

	#region 정보
	[Tooltip("캐릭터 기본 이동 속도")] [SerializeField] [Range(0f, 10f)]
	private float _basicMoveSpeed = 10f;
	// 캐릭터 이동 속도
	private float _moveSpeed = 10f;
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
		_rigid = GetComponent<Rigidbody>();
		_anim = transform.GetComponentInChildren<Animator>();
		// _characterController = gameObject.GetComponent<CharacterController>();
	}

	// Use this for initialization
	void Start ()
    {
		
	}

	private void FixedUpdate()
	{
		MovePosition();
	}

	// Update is called once per frame
	void Update ()
    {
		InputKey();
	}

	// 키 입력
    private void InputKey()
    {
		_keyVertical = Input.GetAxis("Vertical");
		_keyHorizontal = Input.GetAxis("Horizontal");

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

		_anim.SetFloat("_Speed", Mathf.Abs(_keyVertical));

		if (Input.GetKeyDown(_jumpKey) && !(_jumpState))
		{
			//_rigid.AddForce(Vector3.up * 10f, ForceMode.Impulse);
			_anim.SetTrigger("_Jump");
		}			
	}
	#endregion

	#region 캐릭터 이동 관련 함수
	// 이동 제어
	private void MovePosition()
	{
		//transform.Translate(Vector3.forward * _keyVertical * _moveSpeed * Time.deltaTime);
		//_rigid.AddForce(transform.rotation * Vector3.forward * _keyVertical * _moveSpeed);
		//_characterController.Move(Vector3.forward * _keyVertical * _moveSpeed);
	}

	// 회전 제어
	private void RotateCharacter()
	{

	}
	#endregion

	private void OnCollisionStay(Collision collision)
	{
		if(collision.transform.tag == "Ground") // Ground
		{
			if (_jumpState) _jumpState = false;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if(collision.transform.tag == "Ground") // Ground
		{
			if (!_jumpState) _jumpState = true;
		}
	}
}
