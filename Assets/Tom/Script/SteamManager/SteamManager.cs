using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    private void Awake()
    {
        try
        {
            Steamworks.SteamClient.Init(2971700);
        }
        catch  (System.Exception e) { 
            Debug.Log("Couldn't initialize steam client");
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDisable()
    {
        Steamworks.SteamClient.Shutdown();
    }


    private void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }
}
