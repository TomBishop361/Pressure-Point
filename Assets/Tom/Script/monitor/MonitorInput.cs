using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class MonitorInput : MonoBehaviour
{
    [SerializeField] LayerMask LMask = ~0;
    [SerializeField] UnityEvent<Vector2> OnCursorInput = new UnityEvent<Vector2>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray mouseRay =  Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, LMask))
        {
            if (hit.transform != null)
            {
                if (hit.transform.CompareTag("Terminal")) {
                    //Debug.Log(hit.textureCoord);
                    OnCursorInput.Invoke(hit.textureCoord);
                }
                
            }
        }

    }
}
