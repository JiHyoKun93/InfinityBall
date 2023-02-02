using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;


public class Ball : MonoBehaviour{

	Animator animator;
	SpriteRenderer sprite;
	TrailRenderer trail;
	Rigidbody2D rigid;

	bool IsMinusSize;
	InGameUI Ingame;

	public bool IsDie;

	public GameState state;

	Coroutine CameraCo;

	private void Start() {
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
		trail = GetComponent<TrailRenderer>();
		rigid = GetComponent<Rigidbody2D>();

		Ingame = FindObjectOfType<InGameUI>();

		trail.material = FileMng.Instance.Trail;
		
		state = GameState.LOADING;

	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Wall")) {
			if (IsDie == true) return;
			StartCoroutine(DieBall());
		}
	}

	IEnumerator DieBall() {
		SoundMng.Instance.PlaySfx("GameOver");
		SoundMng.Instance.BgmPlayer.Stop();
		state = GameState.DIE;
		IsDie = true;
		animator.SetBool("Die", true);
		CameraCo = StartCoroutine(CameraMng.Instance.CameraShake(0.1f, 10));
		FileMng.Instance.SaveData();
		yield return new WaitForSeconds(0.2f);
		Ingame.FinishGame();
		yield return new WaitForSeconds(0.3f);
		gameObject.SetActive(false);
	}
}