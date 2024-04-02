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
    [SerializeField] Manager manager;
    public bool isBroken;
    float oxygenLevel = 100;
    float vgIntensityEnd;
    float speed;

    
    // Start is called before the first frame update
    void Start()
    {
       volume.profile.TryGet<Vignette>(out vg);
        breakOxygen();
    }

    void breakOxygen()
    {
        isBroken = true;
        //manager.BrokenCount++;
        speed = 0.00025f;
        vgIntensityEnd = 1;
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
            //Death behaviour
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
