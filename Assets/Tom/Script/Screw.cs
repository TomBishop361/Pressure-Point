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
        //Checks if interating should Screw or unscrew depending on Fbox Broken state & screw state
        if (FBox.doorISOpen == false)
        {
            if (!isUnscrewed)
            {
                if (FBox.isBroken)
                {
                    StartCoroutine(UnScrewLerp());
                }
            }
            else
            {
                StartCoroutine(ScrewLerp());
            }
        }
        
    }

    //lerps screw to mimic an unscrewing motion
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
        isUnscrewed = true;
    }

    //lerps screw to mimic screwing 
    IEnumerator ScrewLerp()
    {
        FBox.screwCount(-1);
        isUnscrewed = false;
        lerping = true;
        float time = 0;
        while (time < 1f)
        {
            float perc = 0;
            perc = Easing.Linear(time);
            Rotz = LerpScript.lerp(180, 0, perc);
            Posx = LerpScript.lerp(transform.position.x, transform.position.x - 0.001f, perc);
            time += Time.deltaTime;
            yield return null;
        }
        lerping = false;
        

    }

    // Update is called once per frame
    void Update()
    {
        //While lerping is true update position adn local euler angles
        if (lerping)
        {
            transform.position = new Vector3 (Posx, transform.position.y, transform.position.z);
            transform.localEulerAngles = new Vector3(0, 90, Rotz);
        }
    }
}
