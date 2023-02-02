using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class BallMove : MonoBehaviour{

    Rigidbody2D rigid;
    Vector2 MoveDir;
    float x;
    float speed;
    public float MaxSpeed = 4;
    float speedTime;

    Ball ball;

    void Start(){
        rigid = GetComponent<Rigidbody2D>();
        transform.position = Vector2.zero;
        ball = FindObjectOfType<Ball>();
        x = 1;
		speed = 2;
	}

    void Update() {
		float s = Input.GetAxisRaw("Space");
		if (s != 0) ball.state = GameState.PLAY;
		if (ball.state != GameState.PLAY) return;
		if (ball.state == GameState.DIE) return;
		Move();
        SpeedPlus();
    }

    void FixedUpdate() {
        if(ball.state == GameState.DIE) return;
		if (ball.state != GameState.PLAY) return;
		rigid.velocity = MoveDir;
	}

    void Move() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			SoundMng.Instance.PlaySfx("Turn");
            // x = 1 로 설정한뒤 방향 지정
            x *= -1;
            // Deg2Rad = 라디안값으로 변경
            MoveDir = new Vector2(Mathf.Sin(45 * x * Mathf.Deg2Rad), Mathf.Cos(45 * Mathf.Deg2Rad)) * speed;
		}
    }

    void SpeedPlus() {
        if (speed >= MaxSpeed) return;
        speedTime += Time.deltaTime;
        if(speedTime >= 30.0f) {
			speed += 0.25f;
            speedTime = 0;
		}
	}
}
