using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TerminalManager : MonoBehaviour
{
    //Visible Variables
    [SerializeField] Manager manager;
    [Header("UI Prefabs")]
    [SerializeField] GameObject UIInputPanel;
    [SerializeField] GameObject UITextResponse;
    [Header("UI Components")]
    public TMP_InputField inputField;
    [SerializeField] GameObject content;
    [SerializeField] ScrollRect scrollRect;
    
    

    //Hidden Variables
    bool isFullScreen;
    Vector2 screenRes;
    int count;


    public void input(TMP_InputField input)
    {
        //Creates a text panel with the command just entered
        GameObject UIResponse =  Instantiate(UITextResponse, content.transform, false);
        UIResponse.GetComponent<TMP_Text>().text = (@"C:\ Systems >  " + input.text);
        //Deactivate input panel then check input to generate correct response
        UIInputPanel.SetActive(false);
        switch (input.text.ToLower().Replace(" ", string.Empty))
        {
            //Commands and their responses
            case "/help":
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Here are a list of Commands:\r\nCheck Oxygen Level      /OxygenLevel\r\nReset Oxygen            /OxygenReset\r\nStart Mission           /Start\r\nMission Logs            /logs\r\nQuit Game               /Quit\r\nGame Settings           /settings";
                break;
            case "/start":
                manager.GameStart();
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Commencing decent protocol...";
                break;
            case "/settings":
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Here are a list of Settings:\r\nShow Resolution Options   /ResolutionList\r\nGet Master Volume         /MasterSoundGet\r\nSet Master Volume         /MasterSoundSet [Volume%]\r\nSet Resolution            /Resolution [X,Y]\r\nGet Current Resolution    /GetResolution";
                break;
            case "/resolutionlist":
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Here are a list of Resolutions:\r\n2560 * 1440\r\n1920 * 1080\r\n1280 * 720";
                break;
            case "/mastersoundget":
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Master Volume at XXX%";
                break;
            case "/mastersoundset":
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Master Volume Set To XXX%";
                break;
            case "/resolution1920*1080":
                screenRes = new Vector2(1920, 1080);
                setRes();
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Screen Resolution Set!";
                break;
            case "/resolution1280*720":
                screenRes = new Vector2(1280, 720);
                setRes();
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Screen Resolution Set!";
                break;
            case "/resolution2560*1440":
                screenRes = new Vector2(2560, 1440);
                setRes();
                UIResponse = Instantiate(UITextResponse, content.transform, false);
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
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Please Wait...";
                break;
           


            default:
                UIResponse = Instantiate(UITextResponse, content.transform, false);
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
        if(count == 3)
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

    public IEnumerator ExitTerminal()
    {
        inputField.enabled = false;
        yield return new WaitForSeconds(1f);
        transform.GetComponent<BoxCollider>().enabled = true;
    }


}
