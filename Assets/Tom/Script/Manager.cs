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
    int randomMulti = 5;
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
            Debug.Log(Breakables[comp]);

        }
        
    }

    // Start is called before the first frame update
    public void GameStart()
    {
        
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
        int random = genRandom(components.Length);
        Debug.Log(random);
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
        water.riseCalc(BrokenCount);
        
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
        Debug.Log("Cool'd down");
        
    }


}
