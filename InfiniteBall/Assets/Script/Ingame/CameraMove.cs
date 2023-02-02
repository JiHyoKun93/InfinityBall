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

public class CameraMove : MonoBehaviour{

    Camera cam;
    float y;
    [SerializeField] BallMove ball;

    private void Start() {
		cam = Camera.main;
        ball = FindObjectOfType<BallMove>();
	}

    void Update(){
        transform.position = new Vector2(0, ball.transform.position.y);
    }
}