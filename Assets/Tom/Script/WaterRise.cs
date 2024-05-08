using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRise : MonoBehaviour
{
   
    public float Speed;
    public float waterLevel = 7.442f;

    public void riseCalc(int brokenCount)
    {
        if (brokenCount > 0)
        {
            if (Manager.Instance.electricOn == true)
            {
                Speed = brokenCount * 0.01f;

            }
            else
            {
                Speed = brokenCount * 0.03f;
            }
        }
        else
        {
            if (waterLevel > 7.442f)
            {
                Speed = -0.01f;
            }
        }
        
    }

    private void Update()
    {
        transform.position += Vector3.up * Speed * Time.deltaTime;
        waterLevel = transform.position.y;
        if(waterLevel < 10.218f)
        {
            Manager.Instance.Death(1);
        }
    }
}
