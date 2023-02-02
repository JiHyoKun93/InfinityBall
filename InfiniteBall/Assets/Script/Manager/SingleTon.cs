using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour {

	public static T instance;

	public static T Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<T>();
				if (instance == null) {
					GameObject go = new GameObject();
					instance = go.AddComponent<T>();
					go.name = typeof(T).ToString() + "(Sington)";
					DontDestroyOnLoad(go);
				}
			}
			return instance;
		}
	}

	virtual protected void Awake() {
		
	}
	virtual protected void Start() {
		if (instance != null) {
			Destroy(this.gameObject);
		}
	}
}
