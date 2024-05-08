using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVolumeChange : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("MasterVolume")) 
        {
            PlayerPrefs.SetFloat("Mastervolume", 1);
            Load();
        }

        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        save();

    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
    }

    private void save()
    {
        PlayerPrefs.SetFloat("MasterVolume", volumeSlider.value);
    }
}