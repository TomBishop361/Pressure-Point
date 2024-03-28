using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TerminalManager : MonoBehaviour
{
    [SerializeField] GameObject UIInputPanel;
    [SerializeField] GameObject UITextResponse;
    public TMP_InputField inputField;
    [SerializeField] GameObject content;
    [SerializeField] Scrollbar scrollbar;
    bool isFullScreen;

    [SerializeField] ScrollRect scrollRect;
    //[SerializeField] RectTransform contentRect;
    //[SerializeField] ScrollRect scrollRect;
    //[SerializeField] Scroller scroller;
    [SerializeField] ScrollView scrollView;

    public void input(TMP_InputField input)
    {
        //Creates a text panel with the command just entered
        GameObject UIResponse =  Instantiate(UITextResponse, content.transform, false);
        UIResponse.GetComponent<TMP_Text>().text = (@"C:\ Systems >  " + input.text);
        //Deactivate input panel then check input to generate correct response
        UIInputPanel.SetActive(false);
        switch (input.text.ToLower().Replace(" ", string.Empty))
        {
            case "/help":
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Here are a list of Commands:\r\nCheck Oxygen Level      /OxygenLevel\r\nReset Oxygen            /OxygenReset\r\nStart Mission           /Start\r\nMission Logs            /logs\r\nQuit Game               /Quit\r\nGame Settings           /settings";
                break;
            case "/start":
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
                Screen.SetResolution(1920, 1080, isFullScreen);
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Screen Resolution Set!";
                break;
            case "/resolution1280*720":
                Screen.SetResolution(1280, 1080, isFullScreen);
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Screen Resolution Set!";
                break;
            case "/resolution2560*1440":
                Screen.SetResolution(2560, 1440, isFullScreen);
                UIResponse = Instantiate(UITextResponse, content.transform, false);
                UIResponse.GetComponent<TMP_Text>().text = "Screen Resolution Set!";
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

    IEnumerator ScrollToBottom()
    {
        //Scrolls Terminal UI to bottom of scroll view
        yield return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        scrollRect.verticalNormalizedPosition = 0f;
    }
   
  
}
