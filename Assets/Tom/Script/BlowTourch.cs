using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowTourch : MonoBehaviour
{
    [SerializeField] StarterAssetsInputs input;
    [SerializeField] ParticleSystem flame;
    AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (input.BlowTourchFire && !flame.isPlaying)
        {
            flame.Play();
            audio.Play();
        }else if (!input.BlowTourchFire)
        {
            flame.Stop();
            audio.Stop();
        }
    }
}
