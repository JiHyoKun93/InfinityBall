using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InGameUI : MonoBehaviour{
	[Header("Button")]
	[SerializeField] Button ReplayBtn;
	[SerializeField] Button HomeBtn;
	[SerializeField] Button BackBtn;

	[Header("Option")]
	[SerializeField] Canvas OptionCanvas;
	[SerializeField] Slider BgmSlider;
	[SerializeField] Slider SfxSlider;

	[Header("Finish Game Canvas")]
	[SerializeField] Canvas FinishCanvas;
	[SerializeField] TextMeshProUGUI Timer;
	[SerializeField] TextMeshProUGUI Coin;
	[SerializeField] TextMeshProUGUI BestTime;
	
	[Header("Timer")]
	[SerializeField] TextMeshProUGUI TimerText;
	public float timer;
	float startTime;
	bool IsStartTimer;

	[Header("BackGround")]
	[SerializeField] Image BackGround;

	Ball ball;
	bool IsPause;

	SoundMng sound;

	private void Start() {
		ReplayBtn.onClick.AddListener(OnReplayBtn);
		HomeBtn.onClick.AddListener(OnHomeBtn);
		BackBtn.onClick.AddListener(OnBackBtn);

		OptionCanvas.gameObject.SetActive(false);
		FinishCanvas.gameObject.SetActive(false);
		ReplayBtn.gameObject.SetActive(false);
		HomeBtn.gameObject.SetActive(false);
		BackBtn.gameObject.SetActive(false);

		BgmSlider.value = SoundMng.Instance.BgmPlayer.volume;
		SfxSlider.value = SoundMng.Instance.SfxPlayer.volume;
		BgmSlider.onValueChanged.AddListener(OnSliderBGM);
		SfxSlider.onValueChanged.AddListener(OnSliderSFX);

		TimerText.gameObject.SetActive(true);

		sound = SoundMng.Instance;

		ResetTime();
		StartCoroutine(FadeOutImage());

		Cursor.visible = false;
	}

	private void Update() {
		if (ball == null) {
			Findball();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			PauseOption();
		}

		if (ball.state == GameState.PLAY) {
			TimeScore();
		}
	}

	void Findball() {
		ball = FindObjectOfType<Ball>();
	}

	void PauseOption() {
		Cursor.visible = true;
		if (FinishCanvas.gameObject.activeSelf) return;
		if (ball.state == GameState.PLAY) {
			ball.state = GameState.PAUSE;
		}
		sound.PlaySfx("Button");
		if (!IsPause) {
			WallMng.Instance.gameObject.SetActive(false);
			Time.timeScale = 0;
			IsPause = true;
			OptionCanvas.gameObject.SetActive(true);
			ReplayBtn.gameObject.SetActive(true);
			HomeBtn.gameObject.SetActive(true);
			BackBtn.gameObject.SetActive(true);
		}
		else if (IsPause) {
			Cursor.visible = false;
			WallMng.Instance.gameObject.SetActive(true);
			IsPause = false;
			if (ball.state == GameState.PAUSE) {
				ball.state = GameState.PLAY;
			}
			OptionCanvas.gameObject.SetActive(false);
			ReplayBtn.gameObject.SetActive(false);
			HomeBtn.gameObject.SetActive(false);
			BackBtn.gameObject.SetActive(false);
			Time.timeScale = 1;
		}
	}

	void OnReplayBtn() {
		// 새게임 다시시작
		Cursor.visible = false;
		sound.PlaySfx("Button");
		Time.timeScale = 1;
		FileMng.Instance.playCoin = 0;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	void OnHomeBtn() {
		// 홈화면으로 돌아가기
		sound.PlaySfx("Button");
		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}
		SceneManager.LoadScene(0);
	}
	void OnBackBtn() {
		Cursor.visible = false;
		sound.PlaySfx("Button");
		WallMng.Instance.gameObject.SetActive(true);
		ItemMng.Instance.gameObject.SetActive(false);
		OptionCanvas.gameObject.SetActive(false);
		ReplayBtn.gameObject.SetActive(false);
		HomeBtn.gameObject.SetActive(false);
		BackBtn.gameObject.SetActive(false);
		Time.timeScale = 1;
		if (ball.state == GameState.PAUSE) {
			ball.state = GameState.PLAY;
		}
		IsPause = false;
	}

	public void FinishGame() {
		Cursor.visible = true;
		TimerText.gameObject.SetActive(false);
		FinishCanvas.gameObject.SetActive(true);
		HomeBtn.gameObject.SetActive(true);
		ReplayBtn.gameObject.SetActive(true);
		WallMng.Instance.gameObject.SetActive(false);
		ItemMng.Instance.gameObject.SetActive(false);
		if (timer > FileMng.Instance.GetBestTime()) {
			FileMng.Instance.SetBestTime(timer);
		}

		Timer.text = "Time : " + TimerText.text;
		Coin.text = "Coin : " + FileMng.Instance.GetCoin().ToString() + " + " + FileMng.Instance.playCoin;
		BestTime.text = "Best Time : " + $"{FileMng.Instance.GetBestTime():N2}";
		FileMng.Instance.SetCoin(FileMng.Instance.playCoin);
	}

	// Option Slider
	public void OnSliderBGM(float value) {
		SoundMng.Instance.SetVolumeBGM(value);
	}
	public void OnSliderSFX(float value) {
		SoundMng.Instance.SetVolumeSFX(value);
	}

	void TimeScore() {
		if (!IsStartTimer) {
			ResetTime();
			IsStartTimer = true;
		}
		timer = Time.time - startTime;
		TimerText.text = $"{timer:N2}";
	}

	void ResetTime() {
		startTime = Time.time;
		timer = 0f;
		// $ = String ??
		TimerText.text = $"{timer:N2}";
	}

	IEnumerator FadeOutImage() {
		for (float i = 0; i < 1; i += 0.01f) {
			yield return new WaitForSeconds(0.01f);
			Color c = BackGround.color;
			c.a = 1 - i;
			BackGround.color = c;
		}
		BackGround.gameObject.SetActive(false);
		ball.state = GameState.LOADING;
	}
}
