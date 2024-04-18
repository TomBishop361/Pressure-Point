using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Breach : MonoBehaviour
{
    [SerializeField] Manager manager;
    [SerializeField] StarterAssetsInputs inputs;
    bool isBroken;
    float progress = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //BreachHull();
    }

    void BreachHull()
    {
        isBroken = true;
        progress = 0f;
        manager.breakSystem(GetComponent<Breach>());
    }

    void FixHull()
    {
        isBroken = false;
        manager.fix(GetComponent<Breach>());
        Destroy(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(inputs != null && inputs.isRepairing && Vector3.Distance(transform.position,inputs.transform.position) < 2.0f)
        {
            if(isBroken) progress += Time.deltaTime;

        }
        else
        {
            inputs.isRepairing = false;
        }
        if(progress >= 5)
        {
            FixHull();
        }
        
    }
}
