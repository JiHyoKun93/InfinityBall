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

public class BallSkin : MonoBehaviour {

	[SerializeField] ItemData data;

	[SerializeField] Button Btn;
	[SerializeField] GameObject Lock;

	FileMng filemng;

	private void Start() {
		filemng = FileMng.Instance;

		if (filemng.IsSkinUnLock(gameObject.name)) {
			Lock.SetActive(false);
		}

		Btn.onClick.AddListener(On);
	}

	void On() {
		if (!filemng.IsSkinUnLock(gameObject.name)) {
			if (FileMng.Instance.GetCoin() >= data.GetPrice()) {
				SoundMng.Instance.PlaySfx("Button");
				Lock.SetActive(false);
				filemng.UnLockSkin(gameObject.name);
				FileMng.instance.SetCoin(-(data.GetPrice()));
				FileMng.instance.SaveBall(gameObject.name);
				MainUI.Instance.UpdateSkinCoin();
			}
			else {
				SoundMng.Instance.PlaySfx("NotCoin");
				// 구매 불가 창 팝업
				StartCoroutine(MainUI.Instance.NotCoinCor());
			}
		}
		if (filemng.IsSkinUnLock(gameObject.name)) {
			// 구매 했거나, 이미 구매중이라면 스킨 선택
			SoundMng.Instance.PlaySfx("Button");
			filemng.Ball = filemng.BallDic[data.GetItemName()];
			FileMng.instance.SaveBall(gameObject.name);
		}
	}
}