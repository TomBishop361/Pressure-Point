using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] WaterRise water;
    [SerializeField] TextMeshPro DepthCounter;
    [Header("Misc")]
    public int BrokenCount;
    int randomMulti;
    float timeCount;
    int depth;
    GameObject[] activeEvent;
    GameObject[] inactiveEvent;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("randomEvents", 1, 3);
        InvokeRepeating("ChangeRandomMulti", 1, 60);
    }
    
    private void ChangeRandomMulti()
    {
        randomMulti -=5;
        
    }

    private void randomEvents()
    {
        int random = Random.Range(0,randomMulti);
    }

    private void depthCalc()
    {
        timeCount += Time.deltaTime;
        depth = (int)timeCount * 20;
        DepthCounter.text = depth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        depthCalc();
        water.riseCalc(BrokenCount);
        //transform.position += Vector3.up * 0.1f * Time.deltaTime;
    }
}
