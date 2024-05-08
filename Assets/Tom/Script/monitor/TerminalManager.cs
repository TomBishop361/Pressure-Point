using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class TerminalManager : MonoBehaviour
{
    //Visible Variables    
    [Header("UI Prefabs")]
    [SerializeField] GameObject UIInputPanel;
    [SerializeField] GameObject UITextResponse;
    [Header("UI Components")]
    public TMP_InputField inputField;
    [SerializeField] GameObject content;
    [SerializeField] ScrollRect scrollRect;

    [Header("Convict logs")]
    [SerializeField] GameObject ScientistLog;
    [SerializeField] GameObject SaboteurLog;
    [SerializeField] GameObject FactoryLog;
    [SerializeField] GameObject ConvictLog;

    [Header("Audio")]
    public AudioMixer masterMixer;




    //Hidden Variables
    bool isFullScreen;
    Vector2 screenRes;
    int count;
    float volumePerc;


    public void input(TMP_InputField input)
    {
        //Creates a text panel with the command just entered
        GameObject UIResponse = Instantiate(UITextResponse, content.transform, false);
        UIResponse.GetComponent<TMP_Text>().text = (@"C:\ Systems >  " + input.text);
        //Deactivate input panel then check input to generate correct response
        UIInputPanel.SetActive(false);
        UIResponse = Instantiate(UITextResponse, content.transform, false);
        string volpercString;
        
        //Gets volume % int from string
        if (input.text.ToLower().Contains("/mastersoundset "))
        {
            volpercString = input.text.Substring(15);
            volpercString.Replace(" ", "");
            volumePerc = int.Parse(volpercString);
            input.text = ("/mastersoundset");
        }

        switch (input.text.ToLower().Replace(" ", string.Empty))
        {
            
            //Commands and their responses
            case "/help":
                UIResponse.GetComponent<TMP_Text>().text = "Here are a list of Commands:\r\nReset Oxygen            /OxygenReset\r\nStart Mission           /Start\r\nMission Logs            /logs\r\nQuit Game               /Quit\r\nGame Settings           /settings";
                break;
            case "/start":
                Manager.Instance.GameStart();
                UIResponse.GetComponent<TMP_Text>().text = "Commencing decent protocol...";
                break;
            case "/settings":
                UIResponse.GetComponent<TMP_Text>().text = "Here are a list of Settings:\r\nShow Resolution Options   /ResolutionList\r\nGet Master Volume         /MasterSoundGet\r\nSet Master Volume         /MasterSoundSet [Volume%]\r\nSet Resolution            /Resolution [X,Y]\r\nGet Current Resolution    /GetResolution";
                break;
            case "/resolutionlist":
                UIResponse.GetComponent<TMP_Text>().text = "Here are a list of Resolutions:\r\n2560 * 1440\r\n1920 * 1080\r\n1280 * 720";
                break;
            case "/mastersoundget":
                masterMixer.GetFloat("MasterVol", out float vol);
                //convert dB float to %
                float volume = 100 - (((vol * -1) / 80) * 100);
                UIResponse.GetComponent<TMP_Text>().text = "Master Volume at: " + volume;
                break;
            case "/mastersoundset":
                if(volumePerc > 100 ||  volumePerc < 0)
                {
                    UIResponse.GetComponent<TMP_Text>().text = ("Invalid Volume %");
                }
                else
                {
                    UIResponse.GetComponent<TMP_Text>().text = ("Master Volume Set To: " + volumePerc + "%");
                    //Converts % into float then into dB
                    volumePerc = (80 - (80 * (volumePerc / 100))) * -1;
                    Debug.Log(volumePerc);
                    masterMixer.SetFloat("MasterVol", volumePerc);
                }                
                break;
            case "/resolution1920*1080":
                screenRes = new Vector2(1920, 1080);
                setRes();
                UIResponse.GetComponent<TMP_Text>().text = "Screen Resolution Set!";
                break;
            case "/resolution1280*720":
                screenRes = new Vector2(1280, 720);
                setRes();
                UIResponse.GetComponent<TMP_Text>().text = "Screen Resolution Set!";
                break;
            case "/resolution2560*1440":
                screenRes = new Vector2(2560, 1440);
                setRes();
                UIResponse.GetComponent<TMP_Text>().text = "Screen Resolution Set!";
                break;
            case "/fullscreenfalse":
                isFullScreen = false;
                setRes();
                break;
            case "/fullscreentrue":
                isFullScreen = true;
                setRes();
                break;
            case "/oxygenreset": //Begins the oxygen fixing process
                InvokeRepeating("SendFixCount", 1.67f, 1.67f);

                UIResponse.GetComponent<TMP_Text>().text = "Please Wait...";
                break;
            case "/logs":
                UIResponse.GetComponent<TMP_Text>().text = "Scientist Log        /Log1\r\nSaboteur's Log       /Log2\r\nFactorWorkers Log    /Log3\r\nConvict's Log        /Log4";
                break;
            case "/log1":
                Instantiate(ScientistLog, content.transform, false);
                break;
            case "/log2":
                Instantiate(SaboteurLog, content.transform, false);
                break;
            case "/log3":
                Instantiate(FactoryLog, content.transform, false);
                break;
            case "/log4":
                Instantiate(ConvictLog, content.transform, false);
                break;
            case "/quitgame":
                Application.Quit();
                break;
        }
        //Resets input panel and moves to bottom of vertical layout group
        input.text = "";
        UIInputPanel.transform.SetAsLastSibling();
        UIInputPanel.gameObject.SetActive(true);
        input.ActivateInputField();
        StartCoroutine(ScrollToBottom());

    }

    //sends Count value to PCScript 
    void SendFixCount()
    {
        count++;
        SendMessage("FixCount", count, SendMessageOptions.DontRequireReceiver);
        if (count == 3)
        {
            count = 0;
            CancelInvoke();

        }

    }


    //Sets the screen resolution
    void setRes()
    {
        Screen.SetResolution((int)screenRes.x, (int)screenRes.y, isFullScreen);
    }

    //Scrolls scroll view to bottom
    IEnumerator ScrollToBottom()
    {
        //Scrolls Terminal UI to bottom of scroll view
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void CallExitTerminal()
    {
        StartCoroutine("ExitTerminal");
    }

    //Adds delay to turning boxCollider back on to prevent double accidently entering terminal again
    public IEnumerator ExitTerminal()
    {
        inputField.enabled = false;
        yield return new WaitForSeconds(0.25f);
        transform.GetComponent<BoxCollider>().enabled = true;
    }


}
