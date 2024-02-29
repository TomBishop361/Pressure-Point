using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class FuseBox : MonoBehaviour
{
    bool lerping;
    float Roty;
    int sCount;
    [SerializeField] GameObject hinge;
    public void screwCount(int num)
    {
        sCount += num;
        if(sCount == 4)
        {
            //hinge.transform.rotation = Quaternion.Slerp(hinge.transform.rotation, Quaternion.Euler(-90, 145, 180), 1f);
            StartCoroutine(OpenLerp());
        }
    }

    IEnumerator OpenLerp()
    {
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



    // Start is called before the first frame update
    void Start()
    {
        
        //hinge.transform.eulerAngles = new Vector3 (-90, 145, 180);
    }

    // Update is called once per frame
    void Update()
    {
        if (lerping)
        {
            hinge.transform.localEulerAngles = new Vector3(-90, Roty, 180);
        }
    }
}
