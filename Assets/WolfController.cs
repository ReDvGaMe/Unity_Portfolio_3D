using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum wolfState
{
	normal,
	moveObj,
	findObj
}

public class WolfController : MonoBehaviour {

	#region 변수
	GameObject _player;
	PlayerController _pc;
	Animator _anim;
	NavMeshAgent _nav;

	// 늑대 기본 이동속도
	private float _basicSpeed = 1f;
	// 늑대가 이동을 멈출 거리
	private float _stopDis = 2f;
	// 늑대가 달릴 거리
	private float _runDis = 5f;
	#endregion

	private void Awake()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
		_anim = GetComponent<Animator>();
		_nav = GetComponent<NavMeshAgent>();
	}

	// Use this for initialization
	void Start ()
	{
		_pc = _player.GetComponent<PlayerController>();
		// 네비 멈추는 거리 설정
		_nav.stoppingDistance = _stopDis;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// 목표 지정
		_nav.SetDestination(_player.transform.position);

		_anim.SetFloat("_Dis", _nav.remainingDistance);

		// 거리에 따라 속도 설정
		if (_nav.remainingDistance > _runDis)
			_nav.speed = _basicSpeed * 2.5;
		else
			_nav.speed = _basicSpeed * 1;
	}
}
