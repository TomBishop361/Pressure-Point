using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Breach : MonoBehaviour
{
    [SerializeField] GameObject breachmdl;
    [SerializeField] ParticleSystem water;
    [SerializeField] StarterAssetsInputs inputs;
    [SerializeField] AudioSource soundWater;
    [SerializeField] AudioSource soundBreak;
    bool isBroken;
    float progress = 0f;

  
    public void Break()
    {
        breachmdl.SetActive(true);
        Debug.Log("Breach");
        water.Play();
        soundWater.Play();
        soundBreak.Play();
        isBroken = true;
        progress = 0f;

    }

    public void Fix()
    {
        isBroken = false;
        water.Stop();
        soundWater.Stop();
        Manager.Instance.fix(GetComponent<Breach>());
        breachmdl.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBroken)
        {
            if (inputs != null && inputs.isRepairing && Vector3.Distance(transform.position, inputs.transform.position) < 2.0f)
            {
                if (isBroken) progress += Time.deltaTime;

            }
            else
            {
                inputs.isRepairing = false;
            }
            if (progress >= 5)
            {
                Fix();
            }
        }
    }
}
