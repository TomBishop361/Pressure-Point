using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }
    
    [Header("Object References")]
    [SerializeField] WaterRise water;
    [SerializeField] TextMeshPro DepthCounter;
    [SerializeField] TutorialAudioMang tutorial;
    [Header("Misc")]
    public int BrokenCount;
    int randomMulti = 6;
    int depth;
    int timeRemaining = 300;
    [SerializeField]GameObject deathScreen;
    bool playing = false;

   
    [SerializeField]Component[] components;
    public Dictionary<Component, bool> Breakables =
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
            Debug.Log(comp.ToString());
        }   
        
    }

    // Start is called before the first frame update
    public void GameStart()
    {
        
        tutorial.audioSource.Stop();
        tutorial.gameObject.SetActive(false);
        for (int i = 0; i < Breakables.Count; i++)
        {            
            Component key = Breakables.ElementAt(i).Key;
            key.gameObject.SendMessage("Fix", SendMessageOptions.DontRequireReceiver);
            if(i == 2)//powerbox
            {
                Breakables.ElementAt(i).Key.gameObject.BroadcastMessage("resetFbox", SendMessageOptions.DontRequireReceiver);
            }
        }
        BrokenCount = 0;
        StartCoroutine("timerUpdate");
        playing = true;


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
        playing = false;
        Debug.Log("Winner");
    }

    private void ChangeRandomMulti()
    {
        
        randomMulti -=1;
        
    }

    private void randomEvents()
    {        
        int random = genRandom(randomMulti);
        Debug.Log("randomEvent" + random);
        if(random == 1)
        {
            selectBreak();            
        }
    }

    int genRandom(int max)
    {
        return Random.Range(0, max);
    }

    void selectBreak()
    {
        if (Breakables.Values.All<bool>(value => value))
        {
            Debug.Log("all are broken");
            return;
        }
        int random = genRandom(components.Length);
        if (Breakables.ElementAt(random).Value == false)
        {
            Breakables.ElementAt(random).Key.gameObject.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
            BrokenCount++;
            Breakables[Breakables.ElementAt(random).Key] = true;
        }
        else
        {
            selectBreak();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playing) water.riseCalc(BrokenCount);
        
    }

   

    public void Death(int DeathType)
    {
        deathScreen.SetActive(true);
        
    }

    public void fix(Component comp)
    {
        BrokenCount--;
        StartCoroutine(CoolDown(comp));
        
    }

    private IEnumerator CoolDown(Component comp)
    {
        yield return new WaitForSeconds(6f);
        Breakables[comp] = false;
        Debug.Log("Cool'd down",comp);
        
    }


}
