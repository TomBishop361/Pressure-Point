using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class TriggerScript : MonoBehaviour
{
    public UnityEvent triggerEvent;

    private void OnTriggerEnter(Collider col)
    {
        
        triggerEvent.Invoke();

    }

}

