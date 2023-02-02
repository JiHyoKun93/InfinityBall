using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraMng : SingleTon<CameraMng> {
    Vector3 OriginalPos;
    public Camera MainCamera;
    Ball ball;

    void Start(){
        MainCamera = Camera.main;
        OriginalPos = MainCamera.transform.position;
    }

    void Update(){
        if (MainCamera == null) SetMainCamera();
        if(ball == null) {
            ball = FindObjectOfType<Ball>();
            //transform.parent = ball.transform;
            return;
        }
        CameraMove();
    }

    public void SetMainCamera() {
        MainCamera = Camera.main;
        OriginalPos = MainCamera.transform.position;
    }

    void CameraMove() {
        Vector3 targetPos = new Vector3(ball.transform.position.x, ball.transform.position.y + 1, -10);
		OriginalPos = MainCamera.transform.position;
		transform.position = Vector3.Lerp(transform.position, targetPos, 1); //Time.deltaTime * 2f
	}

	public IEnumerator CameraShake(float time, float power) {
        // GameOver 일 경우 카메라 흔들림
		MainCamera.transform.position = OriginalPos;

		while (time > 0) {
			float dtPower = power * Time.deltaTime;
			MainCamera.transform.position += new Vector3(Random.Range(-dtPower, dtPower), Random.Range(-dtPower, dtPower), 0);
			time -= Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}

		MainCamera.transform.position = OriginalPos;
	}
}