using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lerp function
public class LerpScript 
{
    public static float lerp(float startValue, float endValue, float t)
    {
        return (startValue + (endValue - startValue) * t);
    }
}
