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

public class Item : MonoBehaviour{

    Rigidbody2D rigid;
    SpriteRenderer sprite;

	private void Start() {
		rigid = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
	}

	void Update(){
        Clipping();
    }

    public void Spawn(Vector2 pos) {
		gameObject.SetActive(true);

		if (rigid == null) rigid = GetComponent<Rigidbody2D>();
		if (sprite == null) sprite = GetComponent<SpriteRenderer>();

		transform.position = pos;
	}

    void Clipping() {
		Vector2 veiw = CameraMng.Instance.MainCamera.WorldToViewportPoint(transform.position);
		if (veiw.y < -0.2f) {
			gameObject.SetActive(false);
		}
	}

	virtual protected void OnTriggerBall(Collider2D collision) {
		SoundMng.Instance.PlaySfx("Coin");
		FileMng.Instance.SetPlayCoin(1);
		gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Ball")) {
			OnTriggerBall(collision);
		}
	}
}