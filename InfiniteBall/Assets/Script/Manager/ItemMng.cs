using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using System.Drawing;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemMng : SingleTon<ItemMng> {
	
	[SerializeField] List<Item> ItemList;
    [SerializeField] List<Item> ItemPrefeb;

	[SerializeField] Transform Muzzle;

	public float ResponPosX;
    public float ResponPosY;
	float OriginalY;

	float SpecialItemTimer;

	void Update(){
		if (OriginalY != ResponPosY) {
			OriginalY = ResponPosY;
			int random = Random.Range(0, 10);
			if (random == 0) {
				CreateItem(new Vector2(ResponPosX, ResponPosY));
			}
		}
	}

	GameObject GetNameToGameObject(string name) {
		for (int i = 0; i < ItemPrefeb.Count; i++) {
			if (ItemPrefeb[i].name.Equals(name)) {
				return ItemPrefeb[i].gameObject;
			}
		}
		return null;
	}
	
	void CreateItem(Vector2 pos, string name = "Item") {
		bool IsFind = false;
		for (int i = 0; i < ItemList.Count; i++) {
			if (ItemList[i].gameObject.activeSelf == false && ItemList[i].name.Equals(name)) {
				IsFind = true;
				ItemList[i].Spawn(pos);
				break;
			}
		}

		if (!IsFind) {
			Item item = Instantiate(GetNameToGameObject(name)).GetComponent<Item>();
			item.transform.parent = transform;
			item.Spawn(pos);
			item.name = name;
			ItemList.Add(item);
		}
	}
}
