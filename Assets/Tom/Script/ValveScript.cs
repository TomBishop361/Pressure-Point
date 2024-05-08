using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveScript : MonoBehaviour
{

    [SerializeField] GameObject player;
    StarterAssetsInputs StatInputs;
    [SerializeField] GameObject alertLight;
    [SerializeField] GameObject Steam;
    [SerializeField] AudioSource SoundSteam;
    [SerializeField] AudioSource SoundBreak;
    public Material[] mats;
    private bool broken;
    public Vector2 interactPos;
    public float progress = 0;

    //sets Valve to be in "Broken" state (resetting values)
    void Break()
    {
        alertLight.GetComponent<MeshRenderer>().material = mats[0];
        Steam.GetComponent<ParticleSystem>().Play();
        SoundSteam.Play();
        SoundBreak.Play();
        progress = 0;
        broken = true;
        
    }

    private void Start()
    {
        
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
                Fix();
            }
        }
    }


    void Fix()
    {                
        alertLight.GetComponent<MeshRenderer>().material = mats[1];
        Steam.GetComponent<ParticleSystem>().Stop();
        SoundSteam.Stop();
        Manager.Instance.fix(GetComponent<ValveScript>());
        broken = false;
    }

}
