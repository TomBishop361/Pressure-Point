using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class FuseBox : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] Manager manager;
    [SerializeField] GameObject alertLight;
    [SerializeField] GameObject hinge;
    [Header("Materials")]
    [SerializeField] Material[] mats;
    [Header("Public Variables")]
    public bool doorISOpen;
    public bool isBroken;
    public int switchCount;
    public bool lerping;
    float Roty;
    int sCount;
    

    
    public void screwCount(int num)
    {
        //counts number of unscrewed screws
        sCount += num; 
        if(sCount == 4) //If 4 (all) are unscrewed then open
        {
            StartCoroutine(OpenLerp());
        }
        if (isBroken == false && sCount == 0) //if is not broken && all screws are screwed
        {
            alertLight.GetComponent<Renderer>().material = mats[1]; // green material (Fixed)
            manager.fix(GetComponent<FuseBox>());
        }
    }

    //Lerp to open door
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

    //lerp to close door
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

    //Sets Fuse box to Broken state
    public void BreakFuses()
    {
        isBroken = true;
        manager.breakSystem(GetComponent<FuseBox>());
        alertLight.GetComponent<Renderer>().material = mats[0];
        BroadcastMessage("breakSwitch",null, SendMessageOptions.DontRequireReceiver);//Calls event in all switches to go to broken state
    }



    // Start is called before the first frame update
    void Start()
    {
        BreakFuses(); 
        
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
