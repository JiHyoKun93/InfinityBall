using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using System.IO;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GameState {
    PAUSE, PLAY, DIE, LOADING
}

public class GameMng : SingleTon<GameMng> {

	[SerializeField] public GameObject Ball;
    
    GameData data;

	protected override void Start(){
        base.Start();
        Ball = FileMng.Instance.Ball;
        data = new GameData();
        Instantiate(Ball);

		SoundMng.Instance.PlayBgm("InGame");
	}

    public Ball GetballComponent() {
		Ball ball = Ball.GetComponent<Ball>();
        return ball;
    }
}

// Timer Âü°í 
// https://codefinder.janndk.com/4