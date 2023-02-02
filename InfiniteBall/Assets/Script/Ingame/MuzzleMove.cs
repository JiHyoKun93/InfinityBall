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

public class MuzzleMove : MonoBehaviour{

    Ball ball;

	void FixedUpdate(){
		if(ball == null) {
			ball = FindObjectOfType<Ball>();
			return;
		}
		transform.position = ball.transform.position + (Vector3.up * 23);
	}

}
