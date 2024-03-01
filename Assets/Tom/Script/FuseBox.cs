using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class FuseBox : MonoBehaviour
{
    [SerializeField] Manager manager;
    [SerializeField] GameObject alertLight;
    [SerializeField] Material[] mats;
    public bool doorISOpen;
    public bool isBroken;
    public int switchCount;
    public bool lerping;
    float Roty;
    int sCount;
    [SerializeField] GameObject hinge;
    public void screwCount(int num)
    {
        sCount += num;
        if(sCount == 4)
        {
            StartCoroutine(OpenLerp());
        }
        if (isBroken == false && sCount == 0)
        {
            alertLight.GetComponent<Renderer>().material = mats[1];
            manager.BrokenCount--;
        }
    }

    IEnumerator OpenLerp()
    {
        doorISOpen = true;
        lerping = true;
        float time = 0;
        while (time < 1f)
        {
            float perc = 0;
            perc = Easing.Linear(time);
            Roty = LerpScript.lerp(0, 145, perc);            
            time += Time.deltaTime;
            yield return null;
        }
        lerping = false;        
    }

    IEnumerator CloseLerp()
    {
        lerping = true;
        float time = 0;
        while (time < 1f)
        {
            float perc = 0;
            perc = Easing.Linear(time);
            Roty = LerpScript.lerp(145, 0, perc);
            time += Time.deltaTime;
            yield return null;
        }
        lerping = false;
        doorISOpen=false;
    }


    public void BreakFuses()
    {
        isBroken = true;
        manager.BrokenCount++;
        alertLight.GetComponent<Renderer>().material = mats[0];
        BroadcastMessage("breakSwitch");
    }



    // Start is called before the first frame update
    void Start()
    {
        BreakFuses();
        //hinge.transform.eulerAngles = new Vector3 (-90, 145, 180);
    }

    // Update is called once per frame
    void Update()
    {
        if (lerping)
        {
            hinge.transform.localEulerAngles = new Vector3(-90, Roty, 180);
        }

        if (switchCount >= 3)
        {
            isBroken = false; switchCount = 0;
            StartCoroutine(CloseLerp());
        }
        

    }
}
