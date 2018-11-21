using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	#region 변수
	private Rigidbody _rigid;
	private Animator _anim;

	// 수직 키 입력값
	private float _keyVertical = 0f;

	// 수평 키 입력값
	private float _keyHorizontal = 0f;

	[Tooltip("캐릭터 이동속도")] [SerializeField] [Range(0f, 10f)]
	private float _moveSpeed = 8f;

	// 달리기 키
	private KeyCode _runKey = KeyCode.LeftShift;

	// 점프 키
	private KeyCode _jumpKey = KeyCode.Space;

	// 달리고있는지 검사할 변수
	private bool _runState = false;

	// 캐릭터가 공중에 있는지 검사할 변수
	private bool _jumpState = false;
	#endregion

	#region 기본 함수
	private void Awake()
	{
		_rigid = GetComponent<Rigidbody>();
		_anim = transform.GetComponentInChildren<Animator>();
	}

	// Use this for initialization
	void Start ()
    {
		
	}

	private void FixedUpdate()
	{
		
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

		Debug.Log(_keyVertical);

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

		_anim.SetFloat("_Speed", _keyVertical);

		if (Input.GetKeyDown(_jumpKey))
		{
			_rigid.AddForce(Vector3.up * 30f, ForceMode.Impulse);
			_anim.SetTrigger("_Jump");
		}			
	}
	#endregion

	#region 캐릭터 이동 관련 함수
	// 이동 제어
	private void MovePosition()
	{
		_rigid.AddForce(transform.rotation * Vector3.forward);
	}

	// 회전 제어
	private void RotateCharacter()
	{

	}
	#endregion
}
