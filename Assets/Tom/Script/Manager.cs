using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }
    [SerializeField] AudioSource pump;
    [Header("Object References")]
    [SerializeField] WaterRise water;
    [SerializeField] TextMeshPro DepthCounter;
    [SerializeField] TutorialAudioMang tutorial;
    [Header("Misc")]
    public int BrokenCount;
    int randomMulti = 9;
    int depth;
    int timeRemaining = 300;
    [SerializeField]GameObject deathScreen;
    [HideInInspector]bool playing = false;
    [HideInInspector] public bool electricOn = true;
    [SerializeField] MissionCompleteSounds missioncomplete;

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

        //puts list components into dictionary
        foreach(Component comp in components)
        {
            Breakables.Add(comp, false);
           
        }   
        
    }

    // Start is called before the first frame update
    public void GameStart()
    {
        //stops tutorial 
        tutorial.audioSource.Stop();
        tutorial.gameObject.SetActive(false);

        //fixes everything broken in tutorial
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
        //Starts main game logic
        StartCoroutine("timerUpdate");
        playing = true;


    }

    //timer counting down from 300 seconds
    private IEnumerator timerUpdate()
    {
        while (timeRemaining > -1)
        {
            
            depth = (300 - timeRemaining) * 20;
            if (timeRemaining % 60 == 0) ChangeRandomMulti(); // every minute change random multiplier
            if(timeRemaining % 3 ==0) randomEvents(); //every 3 seconds call randomevent
            timeRemaining--;
            DepthCounter.text= depth.ToString();
            yield return new WaitForSeconds(1f);
        }
        timerEnd();
    }

    private void timerEnd() //When timer reaches 0 / 5 mins has passed
    {
        //start end game logic
        playing = false;
        BrokenCount = 0;
        missioncomplete.missionComplete();

    }

    private void ChangeRandomMulti()
    {
        
        randomMulti -=1;
        
    }

    private void randomEvents()
    {        
        //Rolls to see if a system should break
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
        //if everything is broken return
        if (Breakables.Values.All<bool>(value => value))
        {
            Debug.Log("all are broken");
            return;
        }
        int random = genRandom(components.Length);
        //Check if component is broken
        if (Breakables.ElementAt(random).Value == false)
        {
            Breakables.ElementAt(random).Key.gameObject.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
            BrokenCount++;
            Breakables[Breakables.ElementAt(random).Key] = true;
        }
        else //pick something else to break;
        {
            selectBreak();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playing) water.riseCalc(BrokenCount);
        if(!pump.isPlaying && electricOn)
        {
            pump.Play();
        }else if (pump.isPlaying && !electricOn) {
            pump.Stop();
        }
    }

   

    public void Death(int DeathType)
    {
        deathScreen.SetActive(true);
        StartCoroutine("DeathDelay");
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
        
        
    }


    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(0);
        
    }


}
