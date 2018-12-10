using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RobotController : CharacterController {
    // 키 입력
    protected override void InputKey()
    {
		// 방향키 입력을 받아옴
		_keyVertical = Input.GetAxis("Vertical");
		_keyHorizontal = Input.GetAxis("Horizontal");
		
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

	#region 캐릭터 이동 관련 함수
	// 이동 제어
	protected override void MoveCharacter()
	{
        _movement.Set(_keyHorizontal, 0, _keyVertical);

        // 뒤로 이동할때는 느리게
        if (_keyVertical < -0.1f)
            _moveSpeed = _basicMoveSpeed / 2;
        else if(!_runState)
            _moveSpeed = _basicMoveSpeed;

		// 이동
		transform.Translate(_movement.normalized * _moveSpeed * Time.fixedDeltaTime);
	}
	// 회전 제어
	protected override void RotateCharacter()
	{
        // 마우스 움직임에 따라서 캐릭터 회전
        transform.rotation = Quaternion.Euler(0, _yRot, 0);
	}
	#endregion

	protected void OnCollisionEnter(Collision collision)
	{
		// 바닥 검사
		if (collision.transform.tag == "Ground")
		{
			Debug.Log("11");
			_jumpState = false;
			_anim.SetBool("_Jump", false);
		}
	}
}
