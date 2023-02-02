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

public class Wall : MonoBehaviour{

	Rigidbody2D rigid;
	SpriteRenderer sprite;

	void Start() {
		rigid = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
	}

	void Update() {
		Clipping();
	}

	void Clipping() {
		// �������� ����ԵǸ� ��Ȱ��ȭ
		Vector2 veiw = CameraMng.Instance.MainCamera.WorldToViewportPoint(transform.position);
		if (veiw.y < -0.2f) {
			gameObject.SetActive(false);
		}
	}

	virtual public void Spawn(Vector2 pos) {
		// ��Ȱ�� �Ǵ� ������
		gameObject.SetActive(true);

		if (rigid == null) rigid = GetComponent<Rigidbody2D>();
		if (sprite == null) sprite = GetComponent<SpriteRenderer>();

		transform.position = pos;
	}

}
