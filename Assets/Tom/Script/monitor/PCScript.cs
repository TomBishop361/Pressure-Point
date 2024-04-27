using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements.Experimental;

public class PCScript : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] Volume volume;   
    Vignette vg;
    [Header("References")]
    [SerializeField] GameObject[] lights;
    [SerializeField] Material[] mats; // 1 = red
    [Header("Other")]    
    public bool isBroken;
    float oxygenLevel = 100;
    float vgIntensityEnd;
    float speed;

    
    // Start is called before the first frame update
    void Start()
    {
       volume.profile.TryGet<Vignette>(out vg);
    }

    void Break()
    {
        isBroken = true;
        //manager.BrokenCount++;
        speed = 0.00016f;
        vgIntensityEnd = 1;
        foreach(GameObject light in lights)
        {
            light.GetComponent<Renderer>().material = mats[1];
        }
        StartCoroutine("OxygenCount");

    }

    public void FixOxygen()
    {
        isBroken = false;
        //manager.BrokenCount--;
        StopCoroutine("OxygenCount");
        vgIntensityEnd = 0.1f;
        speed = 0.001f;
        StartCoroutine("OxygenCount");
        
    }

    //breaks fixing process into 3 stagesl
    void FixCount(int count)
    {        
        switch (count)
        {
            case 1:
                lights[0].GetComponent<Renderer>().material = mats[0];
                //set light1
                break;
            case 2:
                lights[1].GetComponent<Renderer>().material = mats[0];
                //set light 2
                break;
            case 3: // after 5 (aprox)seconds fix oxygen
                //set light 3
                lights[2].GetComponent<Renderer>().material = mats[0];
                FixOxygen();
                break;
        }        
    }



    IEnumerator OxygenCount()
    {        
        float time = 0;
        while (time < 25f)
        {
            vg.intensity.value = LerpScript.lerp(vg.intensity.value, vgIntensityEnd, speed); 
            if (isBroken) oxygenLevel -= Time.deltaTime * 4;
            
            time += Time.deltaTime;    

            yield return null;
        }
        if(oxygenLevel <= 0)
        {
            Manager.Instance.Death(1);
        }
    }


   
  
}
