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
[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data")]
public class ItemData : ScriptableObject {
	[SerializeField] string ItemName;
	[SerializeField] int Price;
	[SerializeField] bool IsUnLock;

	public string GetItemName() { return ItemName; }
	public int GetPrice() { return Price; }
	public bool GetUnLock() { return IsUnLock; }
	public void SetUnLock() {
		IsUnLock = true;
	}

}