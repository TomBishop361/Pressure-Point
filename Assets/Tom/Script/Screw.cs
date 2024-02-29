using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Screw : MonoBehaviour
{

    [SerializeField] FuseBox FBox;
    bool isUnscrewed;
    bool lerping;
    float Rotz;
    float Posx; 



    public void UnScrew()
    {
        if (!isUnscrewed)
        {            
            StartCoroutine(UnScrewLerp());
        }
        else
        {            
            StartCoroutine(ScrewLerp());
        }
    }

    IEnumerator UnScrewLerp()
    {
        lerping = true;
        float time = 0;
        while (time < 1f)
        {
            float perc = 0;
            perc = Easing.Linear(time);
            Rotz = LerpScript.lerp(-90, 180, perc);
            Posx = LerpScript.lerp(transform.position.x, transform.position.x + 0.001f, perc);
            time += Time.deltaTime;
            yield return null;
        }
        lerping = false;
        FBox.screwCount(1);
    }

    IEnumerator ScrewLerp()
    {
        lerping = true;
        float time = 0;
        while (time < 1f)
        {
            float perc = 0;
            perc = Easing.Linear(time);
            Rotz = LerpScript.lerp(180, 0, perc);
            Posx = LerpScript.lerp(transform.position.x, transform.position.x+0.001f, perc);
            time += Time.deltaTime;
            yield return null;
        }
        lerping = false;
        FBox.screwCount(-1);
    }

    // Update is called once per frame
    void Update()
    {
        if (lerping)
        {
            transform.position = new Vector3 (Posx, transform.position.y, transform.position.z);
            transform.localEulerAngles = new Vector3(0, 90, Rotz);
        }
    }
}
