using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	#region 변수
	// 카메라를 붙일 플레이어
	public GameObject player;
	// 플레이어 y포지션이 0일 때 땅을 바라보지 않게 수정할 변수
	private float playerY = 2f;

	[Header("- Camera Move")]
	[Tooltip("카메라 상하 이동 최대값")] [SerializeField] [Range(20f, 40f)]
	private float xRotMax;
	[Tooltip("카메라 회전 스피드")] [SerializeField] [Range(5f, 30f)]
	private float rotSpeed;
	[Tooltip("카메라 스크롤 스피드")] [SerializeField] [Range(30f, 70f)]
	private float scrollSpeed;

	[Header("- Distance between player and camera")]
	[Tooltip("플레이어와 카메라 사이 거리")] [SerializeField]	[Range(1f, 10f)]
	private float distance;
	[Tooltip("플레이어와 카메라 사이 최소 거리")] [SerializeField] 
	private float minDis;
	[Tooltip("플레이어와 카메라 사이 최대 거리")] [SerializeField]
	private float maxDis;

	// 마우스 좌표 저장용
	[HideInInspector]
	public float xRot;
	[HideInInspector]
	public float yRot;
	// 카메라가 따라갈 좌표 저장용
	private Vector3 playerPos;
	private Vector3 dir;
	#endregion

	// Update is called once per frame
	void Update ()
	{
		xRot += Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
		yRot += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
		// distance += -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
		
		xRot = Mathf.Clamp(xRot, -xRotMax, xRotMax);
		// distance = Mathf.Clamp(distance, minDis, maxDis);

		playerPos = player.transform.position + Vector3.up * playerY;

		dir = Quaternion.Euler(-xRot, yRot, 0f) * Vector3.forward;
		transform.position = playerPos + (dir * -distance);
	}

	private void LateUpdate()
	{
		transform.LookAt(playerPos);
	}
}
