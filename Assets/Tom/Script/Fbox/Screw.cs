using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Screw : MonoBehaviour
{

    [SerializeField] FuseBox FBox;
    Vector3 startPos;
    bool isUnscrewed;
    bool lerping;
    float Rotz;
    float Posx;

    private void Start()
    {
        startPos = transform.position;
    }

    public void UnScrew()
    {
        //Checks if interating should Screw or unscrew depending on Fbox Broken state & screw state
        if (FBox.doorISOpen == false)//if closed
        {
            if (!isUnscrewed)// if is screwed
            {
                if (FBox.isBroken && lerping == false)//if box is broken
                {
                    StartCoroutine(UnScrewLerp());
                }
            }
            else
            {
                if(lerping == false) StartCoroutine(ScrewLerp());
                
            }
        }
        
    }

    //lerps screw to mimic an unscrewing motion
    IEnumerator UnScrewLerp()
    {
        float targetpos = transform.position.x + 0.1f;
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
        isUnscrewed = true;//is unscrewed
        transform.position = new Vector3(targetpos, transform.position.y, transform.position.z);
    }

    //lerps screw to mimic screwing 
    IEnumerator ScrewLerp()
    {
        float targetpos = transform.position.x - 0.001f;
        FBox.screwCount(-1);
        isUnscrewed = false; // is screwed
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
        transform.position = new Vector3 (targetpos, transform.position.y, transform.position.z);
        

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

    public void resetFbox()
    {
        Debug.Log("WWWW");
        if (isUnscrewed) StartCoroutine("ScrewLerp");
        
    }
}
