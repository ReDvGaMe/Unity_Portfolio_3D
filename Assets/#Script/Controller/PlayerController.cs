using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	#region 변수
	private Rigidbody _rigid;
	private Animator _anim;
	public GameObject _camera;

	#region 정보
	[Tooltip("캐릭터 기본 이동 속도")] [SerializeField] [Range(0f, 10f)]
	private float _basicMoveSpeed = 10f;
	// 캐릭터 이동 속도
	private float _moveSpeed = 10f;
	// 캐릭터 이동 관련 벡터
	private Vector3 _movement;
	[Tooltip("캐릭터 점프 파워")] [SerializeField] [Range(0f, 10f)]
	private float _jumpPower = 1.5f;
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
		_rigid = GetComponent<Rigidbody>();
		_anim = transform.GetComponentInChildren<Animator>();
	}

	private void FixedUpdate()
	{
		RotateCharacter();
		MoveCharacter();
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
            _moveSpeed = _basicMoveSpeed * 2;
            _anim.SetBool("_Run", _runState);
		}
		else if (Input.GetKeyUp(_runKey))
		{
			_runState = false;
            _moveSpeed = _basicMoveSpeed;
            _anim.SetBool("_Run", _runState);
		}

		// 방향 키 입력을 애니메이터로 넘김
		_anim.SetFloat("_SpeedVertical", _keyVertical);
		_anim.SetFloat("_SpeedHorizontal", _keyHorizontal);

        if (Input.GetKeyDown(_jumpKey) && !(_jumpState))
        {
            _jumpState = true;
            _anim.SetBool("_Jump", true);
            _rigid.AddForce(Vector3.up * Mathf.Sqrt(_jumpPower * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            StartCoroutine("JumpAnim");
        }
	}
	#endregion

	#region 캐릭터 이동 관련 함수
	// 이동 제어
	private void MoveCharacter()
	{
        _movement.Set(_keyHorizontal, 0, _keyVertical);

        // 뒤로 이동할때는 느리게
        if (_keyVertical < -0.1f)
            _moveSpeed = _basicMoveSpeed / 2;
        else
            _moveSpeed = _basicMoveSpeed;

        // 이동
        transform.Translate(_movement.normalized * _moveSpeed * Time.fixedDeltaTime);
    }
	// 회전 제어
	private void RotateCharacter()
	{
        // 마우스 움직임에 따라서 캐릭터 회전
        transform.rotation = Quaternion.Euler(0, _yRot, 0);
	}
    #endregion

    private void OnCollisionEnter(Collision collision)
	{
        if (collision.transform.tag == "Ground")
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

    private IEnumerator JumpAnim()
    {
        float _animTime = 0;
        
        while (_jumpState)
        {
            _anim.SetFloat("_JumpAnim", _animTime);
            _animTime += Time.deltaTime;
            yield return null;
        }
	}
	#endregion
}
