using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveScript : MonoBehaviour
{
    
    public Vector2 interactPos;


    public void rotate(Vector2 dir)
    {
        transform.Rotate(new Vector3(0, dir.x, 0) * Time.deltaTime * 20);

    }

    private void Update()
    {
    }


}
