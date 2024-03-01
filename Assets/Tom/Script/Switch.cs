using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField]FuseBox FuseManager;
    bool isBroken = false;
    [SerializeField] Material[] mats;
    [SerializeField] GameObject statusLight;

    public void flip()
    {
        if (isBroken)
        {
            //fix
            statusLight.GetComponent<Renderer>().material = mats[0];
            isBroken = false;
            transform.localEulerAngles = new Vector3(-20, -90, 180);
            FuseManager.switchCount++;
        }
        else
        {
            //break
            statusLight.GetComponent<Renderer>().material = mats[1];
            isBroken = true;
            transform.localEulerAngles = new Vector3(20, -90, 180);
            FuseManager.switchCount--;
        }
    }

    public void breakSwitch() {
        transform.localEulerAngles = new Vector3(20, -90, 180);
        isBroken = true;
        statusLight.GetComponent <Renderer>().material = mats[1];
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
