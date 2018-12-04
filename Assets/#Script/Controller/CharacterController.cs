using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum charState
{
	playable,
	ai
}

public abstract class CharacterController : MonoBehaviour {
	#region 변수
	protected Rigidbody _rigid;
	protected Animator _anim;
	[SerializeField]

	protected Camera _camera;
	protected NavMeshAgent _nav;
	[SerializeField]
	protected GameObject _target;

	#region 정보
	[SerializeField]
	protected charState _charState;
	[Tooltip("캐릭터 기본 이동 속도")] [SerializeField] [Range(0f, 10f)]
	protected float _basicMoveSpeed = 10f;
	// 캐릭터 이동 속도
	protected float _moveSpeed = 10f;
	// 캐릭터 이동 관련 벡터
	protected Vector3 _movement;
	[Tooltip("캐릭터 점프 파워")] [SerializeField] [Range(0f, 10f)]
	protected float _jumpPower = 1.5f;
	#endregion

	#region AI
	// AI가 이동을 멈출 거리
	protected float _stopDis = 2f;
	// AI가 달릴 거리
	protected float _runDis = 5f;
	#endregion

	#region 입력 키
	// 수직 키 입력값
	protected float _keyVertical = 0f;

	// 수평 키 입력값
	protected float _keyHorizontal = 0f;

	// 달리기 키
	protected KeyCode _runKey = KeyCode.LeftShift;

	// 점프 키
	protected KeyCode _jumpKey = KeyCode.Space;

	// 앉기 키
	protected KeyCode _crouchKey = KeyCode.LeftControl;

	// 카메라 회전 키
	protected KeyCode _cameraMoveKey = KeyCode.LeftAlt;

	// 캐릭터 변경 키
	protected KeyCode _charChangeKey = KeyCode.Z;

	[HideInInspector]
	// 화면 회전용
	protected float _yRot;
	#endregion

	#region 상태
	// 달리고있는지 검사할 변수
	protected bool _runState = false;

	// 캐릭터가 공중에 있는지 검사할 변수
	protected bool _jumpState = false;
	#endregion
	#endregion

	protected void Awake()
	{
		_rigid = GetComponent<Rigidbody>();
		_anim = transform.GetComponentInChildren<Animator>();
		_camera = GetComponentInChildren<Camera>();
		_nav = GetComponent<NavMeshAgent>();

		if (_charState == charState.playable)
		{
			_anim.SetBool("_AI", false);
			_camera.enabled = true;
		}
		else if (_charState == charState.ai)
		{
			_anim.SetBool("_AI", true);
			_camera.enabled = false;
		}

		// 네비 멈추는 거리 설정
		_nav.stoppingDistance = _stopDis;
	}

	void Update()
	{
		if (_charState == charState.playable)
			InputKey();

		_yRot += Input.GetAxis("Mouse X") * 20 * Time.deltaTime;
	}

	private void FixedUpdate()
	{
		if (_charState == charState.playable)
		{
			RotateCharacter();
			MoveCharacter();
		}
		else if (_charState == charState.ai)
		{
			MoveCharOnAI();
		}

		if (Input.GetKeyDown(_charChangeKey))
			ChangeChar();
	}

	// 캐릭터 전환 함수
	protected void ChangeChar()
	{
		if (_charState == charState.playable)
		{
			_nav.isStopped = false;
			_charState = charState.ai;
			_anim.SetBool("_AI", true);
			_camera.enabled = false;
		}			
		else if (_charState == charState.ai)
		{
			_nav.isStopped = true;
			_charState = charState.playable;
			_anim.SetBool("_AI", false);
			_camera.enabled = true;
		}			
	}

	// AI 이동
	protected void MoveCharOnAI()
	{
		// 목표 지정
		_nav.SetDestination(_target.transform.position);

		_anim.SetFloat("_Dis", _nav.remainingDistance);

		// 거리에 따라 속도 설정
		if (_nav.remainingDistance > _runDis)
			_nav.speed = _basicMoveSpeed * 2.5f;
		else
			_nav.speed = _basicMoveSpeed * 1;
	}



	#region 코루틴
	// 타이머
	protected IEnumerator WaitForSec(float time)
	{
		while (time > 0)
		{
			time -= 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
	}

	protected IEnumerator JumpAnim()
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
	
	#region 프로퍼티
	public bool isRunning { get { return _runState; } }
	#endregion

	protected abstract void InputKey();
	protected abstract void RotateCharacter();
	protected abstract void MoveCharacter();
}
