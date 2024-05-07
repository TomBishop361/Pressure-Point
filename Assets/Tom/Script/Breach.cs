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
    bool isBroken;
    float progress = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    public void Break()
    {
        breachmdl.SetActive(true);
        Debug.Log("Breach");
        water.Play();
        isBroken = true;
        progress = 0f;

    }

    void FixHull()
    {
        isBroken = false;
        water.Stop();
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
                FixHull();
            }
        }
    }
}
