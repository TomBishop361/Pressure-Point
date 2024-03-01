using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRise : MonoBehaviour
{
    public float Speed;

    public void riseCalc(int brokenCount)
    {
        Speed = brokenCount * 0.01f;
    }

    private void Update()
    {
        transform.position += Vector3.up * Speed * Time.deltaTime;
    }
}
