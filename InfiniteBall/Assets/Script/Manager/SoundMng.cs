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

class SoundData {
	public float BGMVolume;
	public float SFXVolume;

	public SoundData() {
		BGMVolume = 1.0f;
		SFXVolume = 1.0f;
	}
}

public class SoundMng : SingleTon<SoundMng> {

	string path;

	[SerializeField] public AudioSource BgmPlayer;
	[SerializeField] public AudioSource SfxPlayer;

	Dictionary<string, AudioClip> BgmSounds = new Dictionary<string, AudioClip>();
	Dictionary<string, AudioClip> SfxSounds = new Dictionary<string, AudioClip>();

	SoundData data;

	protected override void Awake() {
		if (instance != null) {
			Destroy(this.gameObject);
			return;
		}
		DontDestroyOnLoad(this);
		
		path = Application.persistentDataPath + "/SoundData.txt";

		FileMng.Instance.LoadFile<AudioClip>(ref BgmSounds, "Sound/BGM");
		FileMng.Instance.LoadFile<AudioClip>(ref SfxSounds, "Sound/SFX");

		SetStartVolume();

		base.Awake();
	}

	public void PlaySfx(string name) {
		SfxPlayer.PlayOneShot(SfxSounds[name]);
	}

	public void PlayBgm(string name) {
		BgmPlayer.Stop();
		BgmPlayer.clip = BgmSounds[name];
		BgmPlayer.Play();
	}

	public void SetVolumeBGM(float volume) {
		data.BGMVolume = volume;
		BgmPlayer.volume = volume;

		FileMng.Instance.SaveJson<SoundData>(data, path);

	}

	public void SetVolumeSFX(float volume) {
		data.SFXVolume = volume;
		SfxPlayer.volume = volume;
		
		FileMng.Instance.SaveJson<SoundData>(data, path);
		
	}

	public void SetStartVolume() {
		data = new SoundData();
		FileMng.Instance.LoadJson<SoundData>(ref data, path);
		BgmPlayer.volume = data.BGMVolume;
		SfxPlayer.volume = data.SFXVolume;
	}
}
