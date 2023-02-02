using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class MainUI : SingleTon<MainUI> {

	[Header("Main Canvas UI")]
	[SerializeField] Canvas MainCanvas;
	[SerializeField] Button StartBtn;
	[SerializeField] Button SkinBtn;
	[SerializeField] Button OptionBtn;
	[SerializeField] Button ManualBtn;
	[SerializeField] TextMeshProUGUI CoinText;
	[SerializeField] Canvas ManualCanvas;
	
	[Header("Skin Canvas UI")]
	[SerializeField] Canvas SkinCanvas;
	[SerializeField] RectTransform BallScroll;
	[SerializeField] RectTransform WallScroll;
	[SerializeField] RectTransform TrillerScroll;
	[SerializeField] Button BallSkinBtn;
	[SerializeField] Button WallSkinBtn;
	[SerializeField] Button TrillerSkinBtn;
	[SerializeField] TextMeshProUGUI SkinCoinText;
	[SerializeField] TextMeshProUGUI NotCoinText;
	[SerializeField] Sprite BtnSpriteOn;
	[SerializeField] Sprite BtnSpriteOff;
	bool IsNotCoin;

	[Header("Option Cavase UI")]
	[SerializeField] Canvas OptionCanvas;
	[SerializeField] Slider BgmSlider;
	[SerializeField] Slider SfxSlider;

	[Header("Back Button")]
	[SerializeField] Button BackBtn;

	[Header("BackGround")]
	[SerializeField] Image BackGround;

	SoundMng sound;

	private void Start() {

		MainCanvas.gameObject.SetActive(true);
		SkinCanvas.gameObject.SetActive(false);
		OptionCanvas.gameObject.SetActive(false);
		BackBtn.gameObject.SetActive(false);
		
		StartBtn.onClick.AddListener(OnStartBtn);
		SkinBtn.onClick.AddListener(OnSkinBtn);
		OptionBtn.onClick.AddListener(OnOptionBtn);
		ManualBtn.onClick.AddListener(OnManualBtn);

		BgmSlider.onValueChanged.AddListener(OnSliderBGM);
		SfxSlider.onValueChanged.AddListener(OnSliderSFX);

		BallSkinBtn.onClick.AddListener(OnBallSkinBtn);
		WallSkinBtn.onClick.AddListener(OnWallSkinBtn);
		TrillerSkinBtn.onClick.AddListener(OnTrillerSkinBtn);

		BackBtn.onClick.AddListener(OnBackBtn);
		sound = SoundMng.Instance;

		StartOption();
	}

	private void Update() {
		CoinText.text = "Coin : " + FileMng.Instance.GetCoin();
	}

	// Main Button {
	public void OnStartBtn() {
		// Game Start
		sound.PlaySfx("StartButton");
		MainCanvas.gameObject.SetActive(false);
		StartCoroutine(ChangeScene());
	}

	IEnumerator ChangeScene() {
		for(float i=0; i<1; i+= 0.01f) {
			yield return new WaitForSeconds(0.01f);
			Color c = BackGround.color;
			c.a = i;
			BackGround.color = c;
		}
		SceneManager.LoadScene(1);
	}

	public void OnSkinBtn() {
		// Store Open
		sound.PlaySfx("Button");
		SkinCanvas.gameObject.SetActive(true);
		NotCoinText.gameObject.SetActive(false);
		BackBtn.gameObject.SetActive(true);
		MainCanvas.gameObject.SetActive(false);
		SkinCoinText.text = "Coin : " + FileMng.Instance.GetCoin();
		if (BallScroll.gameObject.activeSelf == false) {
			// 만약 Wall 을 키고 Back을 하면 Wall부터 켜져서 처음부터 켜지게 설정
			OnBallSkinBtn();
		}
	}

	public void UpdateSkinCoin() {
		SkinCoinText.text = "Coin : " + FileMng.Instance.GetCoin();
	}

	public void OnOptionBtn() {
		// Option Canvas Open
		sound.PlaySfx("Button");
		OptionCanvas.gameObject.SetActive(true);
		BackBtn.gameObject.SetActive(true);
		MainCanvas.gameObject.SetActive(false);
	}

	public void OnManualBtn() {
		sound.PlaySfx("Button");
		ManualCanvas.gameObject.SetActive(true);
		BackBtn.gameObject.SetActive(true);
		MainCanvas.gameObject.SetActive(false);
	}
	// }

	// Option Slider
	public void OnSliderBGM(float value) {
		SoundMng.Instance.SetVolumeBGM(value);
	}
	public void OnSliderSFX(float value) {
		SoundMng.Instance.SetVolumeSFX(value);
	}

	void StartOption() {
		BgmSlider.value = SoundMng.Instance.BgmPlayer.volume;
		SfxSlider.value = SoundMng.Instance.SfxPlayer.volume;
	}

	// Skin Button {
	public void OnBallSkinBtn() {
		// ball Skin Store Open
		BallSkinBtn.image.sprite = BtnSpriteOn;
		WallSkinBtn.image.sprite = BtnSpriteOff;
		TrillerSkinBtn.image.sprite = BtnSpriteOff;
		sound.PlaySfx("Button");
		BallScroll.gameObject.SetActive(true);
		WallScroll.gameObject.SetActive(false);
		TrillerScroll.gameObject.SetActive(false);
	}

	public void OnWallSkinBtn() {
		// Wall Skin Store Open
		BallSkinBtn.image.sprite = BtnSpriteOff;
		WallSkinBtn.image.sprite = BtnSpriteOn;
		TrillerSkinBtn.image.sprite = BtnSpriteOff;
		sound.PlaySfx("Button");
		WallScroll.gameObject.SetActive(true);
		BallScroll.gameObject.SetActive(false);
		TrillerScroll.gameObject.SetActive(false);

	}

	public void OnTrillerSkinBtn() {
		// Triller Skin Store Open
		BallSkinBtn.image.sprite = BtnSpriteOff;
		WallSkinBtn.image.sprite = BtnSpriteOff;
		TrillerSkinBtn.image.sprite = BtnSpriteOn;
		sound.PlaySfx("Button");
		TrillerScroll.gameObject.SetActive(true);
		BallScroll.gameObject.SetActive(false);
		WallScroll.gameObject.SetActive(false);
	}
	// }
	
	public IEnumerator NotCoinCor() {
		if (IsNotCoin) yield break;
		// 중복클릭 방지
		IsNotCoin = true;
		// 첫 시작 a값 초기화
		NotCoinText.gameObject.SetActive(true);
		Color a = NotCoinText.color;
		a.a = 0;
		NotCoinText.color = a;
		for (float i = 0; i < 1; i += 0.01f) {
			yield return new WaitForSeconds(0.01f);
			Color c = NotCoinText.color;
			c.a = i;
			NotCoinText.color = c;
		}
		for (float i = 0; i < 1; i += 0.01f) {
			yield return new WaitForSeconds(0.01f);
			Color c = NotCoinText.color;
			c.a = 1 - i;
			NotCoinText.color = c;
		}
		NotCoinText.gameObject.SetActive(false);
		IsNotCoin = false;
	}

	// Back Button
	public void OnBackBtn() {
		// Option , Skin Canvas -> Main Back Button
		if (IsNotCoin) return;
		sound.PlaySfx("Button");
		if (SkinCanvas.gameObject.activeSelf == true) {
			NotCoinText.gameObject.SetActive(false);
			SkinCanvas.gameObject.SetActive(false);
		}
		else if (OptionCanvas.gameObject.activeSelf == true) OptionCanvas.gameObject.SetActive(false);
		else if (ManualCanvas.gameObject.activeSelf == true) ManualCanvas.gameObject.SetActive(false);
		MainCanvas.gameObject.SetActive(true);
		BackBtn.gameObject.SetActive(false);
	}
}
