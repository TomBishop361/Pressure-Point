using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropDownChange : MonoBehaviour
{
    Vector2 resolution = new Vector2(1920,1080);
    int i;
    public bool fullscreen = true;

    public void OnValueChange(TMP_Dropdown dropdown)
    {
        i = dropdown.value;
        switch (i)
        {
            case 1:
                Screen.SetResolution(1920, 1080,fullscreen);
                resolution = new Vector2(1920, 1080);
                break;
            case 2:
                Screen.SetResolution(1280, 720, fullscreen);
                resolution = new Vector2(1280, 720);
                break;
            case 3: // 960 540
                Screen.SetResolution(960,540,fullscreen);
                resolution = new Vector2(960,540);
                break;
        }

    }


    public void toggleFullScreen(Toggle toggle)
    {
        fullscreen = toggle.isOn;
        Debug.Log(fullscreen);
        SetResolution();
    }

    private void SetResolution()
    {
        Screen.SetResolution((int)resolution.x, (int)resolution.y, fullscreen);
    }
}
