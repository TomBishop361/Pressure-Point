using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialAudioMang : MonoBehaviour
{
    public bool tutorial = false;
    public AudioClip[] TutorialClips;
    AudioSource audioSource;
    int i = 0;
    // Start is called before the first frame update

    public void tutorialStart()
    {
        tutorial = true;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && tutorial && Manager.Instance.BrokenCount == 0)
        {
            if (TutorialClips.Length > i)
            {
                audioSource.clip = TutorialClips[i];
                tutorial = false;
                StartCoroutine("delay");
            }
            else
            {
                tutorial = false;
            }

        }

    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(2f);
        audioSource.Play();
        i++;
        switch (i)
        {
            case 2://Break FuseBox
                Manager.Instance.Breakables.ElementAt(2).Key.gameObject.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
                Manager.Instance.BrokenCount++;
                break;
            case 3://Break Valve
                Manager.Instance.Breakables.ElementAt(1).Key.gameObject.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
                Manager.Instance.BrokenCount++;
                break;
            case 4://Breach Hull
                Manager.Instance.Breakables.ElementAt(3).Key.gameObject.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
                Manager.Instance.BrokenCount++;
                break;
            case 5://Break Oxygen
                Manager.Instance.Breakables.ElementAt(0).Key.gameObject.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
                Manager.Instance.BrokenCount++;
                break;
        }
        tutorial = true;
    }
}
