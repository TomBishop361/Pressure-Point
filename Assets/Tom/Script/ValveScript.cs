using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveScript : MonoBehaviour
{

    [SerializeField]Manager manager;
    [SerializeField] GameObject player;
    StarterAssetsInputs StatInputs;
    [SerializeField] GameObject alertLight;
    [SerializeField] GameObject Steam;
    public Material[] mats;
    private bool broken;
    public Vector2 interactPos;
    public float progress = 0;

    //sets Valve to be in "Broken" state (resetting values)
    void breakValve()
    {
        manager.breakSystem(GetComponent<ValveScript>());
        alertLight.GetComponent<MeshRenderer>().material = mats[0];
        Steam.GetComponent<ParticleSystem>().Play();
        progress = 0;
        broken = true;
        
    }

    private void Start()
    {
        //breakValve();
        StatInputs = player.GetComponent<StarterAssetsInputs>();
    }

    public void rotate(Vector2 dir)
    {
        if (broken)
        {
            transform.Rotate(new Vector3(0, dir.x, 0) * Time.deltaTime * 20);
            progress += dir.x;
            //checks if task is complete
            if (progress >= 1000)
            {
                StatInputs.playerState = 0;
                alertLight.GetComponent<MeshRenderer>().material = mats[1];
                Steam.GetComponent<ParticleSystem>().Stop();
                manager.fix(GetComponent<ValveScript>());
                broken = false;
            }
        }
    }

    private void Update()
    {

    }


}
