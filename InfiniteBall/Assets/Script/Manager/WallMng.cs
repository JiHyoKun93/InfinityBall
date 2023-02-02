using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WallMng : SingleTon<WallMng> {
	
	[SerializeField] GameObject Wall;
	[SerializeField] List<Wall> WallList;

	Vector2 ResponPosL;
	Vector2 ResponPosR;
	public float x;
	// pixel 250 = 1, 500 = 0.5f, 1000 = 0.25f .....
	float[] random = { -0.25f, 0.25f };

	[SerializeField] float posX;
	public float posY;

	// pixel 250 = 1, 500 = 0.5f, 1000 = 0.25f .....
	[SerializeField] float size;
	[SerializeField] float distance;
	int MaxdecreaseCount;
	int decreaseCount;

	public float minusSize = 0;

	public bool IsItemPlus;

	float minusTimer;
	float startSize;
	[SerializeField] Transform Muzzle;

	Ball ball;

	private void Start() {
		Wall = FileMng.Instance.Wall;
		if (Wall == null) Wall = FileMng.Instance.WallDic["BlueWall"];

		StartCreateWall();
	}

	private void Update() {
		if (ball == null) {
			ball = FindObjectOfType<Ball>();
			return;
		}
		// 현재 y는 이전 벽의 + 1,
		// x 는 이전 벽의 +1 또는 -1;
		XPos();
		UpdateWall();
	}

	void UpdateWall() {
		// Muzzle의 y포지션 위로 갈경우 생성중단
		if (Muzzle.position.y > posY) {
			SetPosition(minusSize);
			CreateWall(ResponPosL);
			CreateWall(ResponPosR);
			// 벽 옆에 추가로 벽 세우기 ( BackGround 대신 )
			for (int i = 0; i < 7; i++) {
				CreateWall(ResponPosL + Vector2.left * (size + (size + i * 0.5f)));
				CreateWall(ResponPosR + Vector2.right * (size + (size + i * 0.5f)));
			}
		}
		// 20초마다 벽의 간격이 줄어듬, MaxdecreaseCount 만큼
		if (ball.state == GameState.PLAY) {
			if (decreaseCount >= MaxdecreaseCount) return;
			minusTimer += Time.deltaTime;
			float dis = 0.25f;
			if (minusTimer >= 20.0f) {
				StartCoroutine(ChangeWallDistance(startSize, startSize + dis, -1));
				startSize += dis;
				minusTimer = 0;
				decreaseCount++;
			}
		}
	}

	/// <summary>
	/// PNM = plus and minus
	/// plus = 1, minus = -1
	/// </summary>
	/// <param name="distance"></param>
	/// <param name="PNM"></param>
	/// <returns></returns>
	public IEnumerator ChangeWallDistance(float startSize, float distance, float PNM) {
		for (float i = startSize; i <= distance; i += 0.01f) {
			minusSize = i * PNM;
			yield return new WaitForSeconds(0.3f);
		}
	}

	void XPos() {
		float m = Input.GetAxisRaw("Horizontal");
		if (m != 0) {
			x = -(m);
		}
	}

	void StartCreateWall() {
		posX = 1f;
		posY = -1f;
		size = 0.25f;
		distance = 5;
		MaxdecreaseCount = 3;
		for (int i = 0; i < 6; i++) {
			StartPosition(minusSize);
			CreateWall(ResponPosR);
			CreateWall(ResponPosL);
		}
	}

	void StartPosition(float minus) {
		posX += -0.25f;
		posY += size;
		ResponPosL = new Vector2(posX, posY) + (Vector2.left * ((size * distance) + minus));
		ResponPosR = new Vector2(posX, posY) + (Vector2.right * ((size * distance) + minus));
	}

	void SetPosition(float minus = 0f) {
		int index = Random.Range(0, 2);
		posX += random[index];
		posY += size;
		ResponPosL = new Vector2(posX, posY) + (Vector2.left * ((size * distance) + minus));
		ResponPosR = new Vector2(posX, posY) + (Vector2.right * ((size * distance) + minus));

		ItemMng.Instance.ResponPosX = posX; 
		ItemMng.Instance.ResponPosY = posY;
	}

	void CreateWall(Vector2 pos, string name = "Wall") {
		bool IsFind = false;
		// 비활성화된 벽을 찾았으면 재활용
		for(int i=0; i<WallList.Count; i++) {
			if (WallList[i].gameObject.activeSelf == false && WallList[i].name.Equals(name)) {
				IsFind = true;
				WallList[i].Spawn(pos);
				break;
			}
		}
		// 비활성화된 벽이 없거나 생성된 벽이 없을경우 새로 생성
		if (!IsFind) {
			Wall wall = Instantiate(Wall).GetComponent<Wall>();
			wall.transform.parent = transform;
			wall.Spawn(pos);
			wall.name = name;
			WallList.Add(wall);
		}
	}
}
