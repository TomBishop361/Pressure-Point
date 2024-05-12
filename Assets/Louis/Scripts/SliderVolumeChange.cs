using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderVolumeChange : MonoBehaviour
{
    [SerializeField] GameObject options; 
    public AudioMixer masterMix;


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
        options.SetActive(false);
    }

    public void OnValueChange(Slider slider)
    {
        masterMix.SetFloat("MasterVol", slider.value);
        save(slider);

    }

    private void Load()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume");
    }

    private void save(Slider slider)
    {
        PlayerPrefs.SetFloat("MasterVolume", slider.value);
    }
}