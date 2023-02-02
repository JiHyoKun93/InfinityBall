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

public class FixedResolution : MonoBehaviour {

    int Width = 540;
    int Height = 960;

	int deviceW;
    int deviceH;

	void Awake() {
        Screen.SetResolution(Width, Height, false);
	}

    public void SetResolutionSize() {
        deviceW = Screen.width;
        deviceH = Screen.height;

        Screen.SetResolution((int)(((float)deviceH / deviceW) * Width), Height, false);

        if ((float)Width / Height < (float)deviceW / deviceH) {
            float newWidth = ((float)Width / Height) / ((float)deviceW / deviceH);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else {
            float newHight = ((float)Width / Height) / ((float)deviceW / deviceH);
            Camera.main.rect = new Rect(0f, (1f - newHight) / 2f, 1f, newHight);
        }

	}

    

}