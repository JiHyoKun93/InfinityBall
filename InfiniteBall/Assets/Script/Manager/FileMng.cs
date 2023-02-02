using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using System.IO;
using DG.Tweening.Plugins.Core.PathCore;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

class GameData {
	public int Coin;
	public float BestTime;
	public string Ball;
	public string Wall;
	public string Trail;
	public List<string> SkinName;

	public GameData() {
		Coin = 0;
		BestTime = 0;
		Ball = "WhiteBall";
		Wall = "BlueWall";
		Trail = "WhiteTrail";
		SkinName = new List<string>();
		SkinName.Add("WhiteBall");
		SkinName.Add("BlueWall");
		SkinName.Add("WhiteTrail");
	}
}

public class FileMng : SingleTon<FileMng> {

	[Header("Ball Dictionary")]
	public GameObject Ball;
	public Dictionary<string, GameObject> BallDic;
	[SerializeField] List<string> BallName;
	[SerializeField] List<GameObject> BallPrefeb;

	[Header("Wall Dictionary")]
	public GameObject Wall;
	public Dictionary<string, GameObject> WallDic;
	[SerializeField] List<string> WallName;
	[SerializeField] List<GameObject> WallPrefeb;

	[Header("Trail Dictionary")]
	public Material Trail;
	public Dictionary<string, Material> TrailDic;
	[SerializeField] List<string> TrailName;
	[SerializeField] List<Material> TrailPrefeb;

	GameData data;
	string path;
	public int playCoin;

	protected override void Awake() {
		DontDestroyOnLoad(this);
		base.Awake();
	}

	private void Start() {
		data = new GameData();
		path = Application.persistentDataPath + "/GameData.txt";
		
		ListToDicBall();
		ListToDicWall();
		ListToDicTriller();
		LoadJson<GameData>(ref data, path);

		StartSetting();
		playCoin = 0;

		SoundMng.Instance.PlayBgm("Main");
	}

	void ListToDicBall() {
		BallDic = new Dictionary<string, GameObject>();
		for (int i = 0; i < BallName.Count; i++) {
			BallDic.Add(BallName[i], BallPrefeb[i]);
		}
	}

	void ListToDicWall() {
		WallDic = new Dictionary<string, GameObject>();
		for (int i = 0; i < WallName.Count; i++) {
			WallDic.Add(WallName[i], WallPrefeb[i]);
		}
	}

	void ListToDicTriller() {
		TrailDic = new Dictionary<string, Material>();
		for (int i = 0; i < TrailName.Count; i++) {
			TrailDic.Add(TrailName[i], TrailPrefeb[i]);
		}
	}


	void StartSetting() {
		// 저장된 정보 불러와서 적용
		if (data.Ball == "") data.Ball = "WhiteBall";
		if (data.Wall == "") data.Wall = "BlueWall";
		if (data.Trail == "") data.Trail = "WhiteTrail";
		Ball = BallDic[data.Ball];
		Wall = WallDic[data.Wall];
		Trail = TrailDic[data.Trail];
	}

	public void SaveBall(string name) {
		data.Ball = name;
		SaveData();
	}
	public void SaveWall(string name) {
		data.Wall = name;
		SaveData();
	}
	public void SaveTrail(string name) {
		data.Trail = name;
		SaveData();
	}

	public void LoadFile<T>(ref Dictionary<string, T> datas, string path) where T : Object {
		datas = new Dictionary<string, T>();
		T[] files = Resources.LoadAll<T>(path);
		foreach(var file in files) {
			datas.Add(file.name, file);
		}
	}

	public void SaveJson<T>(T obj, string path) where T : class {
		string jdata = JsonUtility.ToJson(obj);
		File.WriteAllText(path, jdata);
	}

	public void SaveData() {
		SaveJson<GameData>(data, path);
	}

	public void LoadJson<T>(ref T obj, string path) where T : class {
		if (!File.Exists(path)) {
			SaveJson<T>(obj, path);
			LoadJson<T>(ref obj, path);
			return;
		}
		string jdata = File.ReadAllText(path);
		obj = JsonUtility.FromJson<T>(jdata);
	}

	public int GetCoin() {
		return data.Coin;
	}
	public void SetCoin(int value) {
		data.Coin += value;
	}
	public void SetPlayCoin(int value) {
		playCoin += value;
	}

	public float GetBestTime() {
		return data.BestTime;
	}

	public void SetBestTime(float value) {
		data.BestTime = value;
	}


	public void UnLockSkin(string name) {
		data.SkinName.Add(name);
		SaveData();
	}

	public bool IsSkinUnLock(string name) {
		return data.SkinName.Contains(name);
	}
}
