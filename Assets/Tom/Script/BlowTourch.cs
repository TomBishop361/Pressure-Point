using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowTourch : MonoBehaviour
{
    [SerializeField] StarterAssetsInputs input;
    [SerializeField] ParticleSystem flame;

    private void Update()
    {
        if(input.isRepairing && !flame.isPlaying)
        {
            flame.Play();
        }else if (!input.isRepairing)
        {
            flame.Stop();
        }
    }
}
