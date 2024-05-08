using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCompleteSounds : MonoBehaviour
{
    public AudioClip clip2;
    public AudioSource audioSource;
    int i = 0;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void missionComplete()
    {
        audioSource.Play();
        i++;
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && i == 1)
        {
            i++;
            audioSource.clip = clip2;
            StartCoroutine("delay");
        }
        if(i == 3)
        {
            if (!audioSource.isPlaying) {
                Manager.Instance.Death(1);
            }
        }
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(10f);
        audioSource.Play();
        StartCoroutine("delay");
        i = 3;
        
        
    }
}
