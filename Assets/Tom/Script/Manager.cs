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

 
    [SerializeField]Component[] components;
    Dictionary<Component, bool> Breakables =
        new Dictionary<Component, bool>();

   

    private void Start()
    {
        foreach(Component comp in components)
        {
            Breakables.Add(comp, false);
            Debug.Log(comp.name);
        }
        
    }

    // Start is called before the first frame update
    public void GameStart()
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
        int random = genRandom();
        if (Breakables[components[random]] == false)
        {

        }
        

    }
    int genRandom()
    {
        return Random.Range(0, randomMulti);
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

    public void breakSystem(Component comp)
    {
        BrokenCount++;
        Breakables[comp] = true;
        
    }


    public void fix(Component comp)
    {
        BrokenCount--;
        Breakables[comp] = false;
    }



}
