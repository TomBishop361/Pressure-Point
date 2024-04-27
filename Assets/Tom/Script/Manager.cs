using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    [Header("Object References")]
    [SerializeField] WaterRise water;
    [SerializeField] TextMeshPro DepthCounter;
    [Header("Misc")]
    public int BrokenCount;
    int randomMulti = 15;
    int depth;
    int timeRemaining = 300;
    [SerializeField]GameObject deathScreen;

 
    [SerializeField]Component[] components;
    Dictionary<Component, bool> Breakables =
        new Dictionary<Component, bool>();

   

    private void Start()
    {
        //singleton
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }


        foreach(Component comp in components)
        {
            Breakables.Add(comp, false);
            Debug.Log(comp.name);
        }
        
    }

    // Start is called before the first frame update
    public void GameStart()
    {
        //InvokeRepeating("randomEvents", 1, 3);
        //InvokeRepeating("ChangeRandomMulti", 1, 60);
        StartCoroutine("timerUpdate");
    }

    private IEnumerator timerUpdate()
    {
        while (timeRemaining > -1)
        {
            
            depth = (300 - timeRemaining) * 20;
            if (timeRemaining % 60 == 0) ChangeRandomMulti();
            if(timeRemaining % 3 ==0) randomEvents();
            timeRemaining--;
            DepthCounter.text= depth.ToString();
            yield return new WaitForSeconds(1f);
        }
        timerEnd();
    }

    private void timerEnd()
    {
        throw new System.NotImplementedException();
    }

    private void ChangeRandomMulti()
    {
        
        randomMulti -=5;
        
    }

    private void randomEvents()
    {        
        int random = genRandom(randomMulti);
        if(random == 1)
        {
            selectBreak();            
        }
    }

    int genRandom(int max)
    {
        return Random.Range(1, max);
    }

    void selectBreak()
    {
        int random = genRandom(components.Length);
        if (components.ElementAt(random) == true)
        {
            components.ElementAt(random).gameObject.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            selectBreak();
        }
    }

    // Update is called once per frame
    void Update()
    {
        water.riseCalc(BrokenCount);
        //transform.position += Vector3.up * 0.1f * Time.deltaTime;
    }

    public void breakSystem(Component comp)
    {
        BrokenCount++;
        Breakables[comp] = true;
        
    }

    public void Death(int DeathType)
    {
        deathScreen.SetActive(true);
        
    }

    public void fix(Component comp)
    {
        BrokenCount--;
        Breakables[comp] = false;
    }



}
