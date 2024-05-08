using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAmbience : MonoBehaviour
{
    [SerializeField] AudioSource[] sources;
    int randomtime;
    public int minTime;
    public int Maxtime;
    

    // Start is called before the first frame update
    void Start()
    {
        randomtime = Random.Range(minTime, Maxtime);
        InvokeRepeating("Counter", randomtime, randomtime);
    }


    void Counter()
    {
        int random = Random.Range(0, sources.Length-1);
        sources[random].Play();
    }

}
