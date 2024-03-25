using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalManager : MonoBehaviour
{
    [SerializeField] GameObject UIInputPanel;
    [SerializeField] GameObject UITextResponse;
    public TMP_InputField inputField;
    [SerializeField] GameObject content;
    

    public void input(TMP_InputField input)
    {
        Debug.Log(input.text);
        GameObject UIResponse =  Instantiate(UITextResponse, content.transform, false);
        UIResponse.GetComponent<TMP_Text>().text = (@"C:\ Systems >  " + input.text);
        UIInputPanel.SetActive(false);
        if(input.text.ToLower() == ("/help"))
        {
            UIResponse = Instantiate(UITextResponse, content.transform, false);
            UIResponse.GetComponent<TMP_Text>().text = "Here are a list of commands: \n Oxygen levels     /oxygen \n Oxygen Reset      /ResetOxygen";
        }
        else
        {
            UIResponse = Instantiate(UITextResponse, content.transform, false);
            
        }
        input.text = "";
        UIInputPanel.transform.SetAsLastSibling();
        UIInputPanel.gameObject.SetActive(true);
        input.ActivateInputField();
        //LayoutRebuilder.ForceRebuildLayoutImmediate();
        //text with entered text
        //spawn repsonse
        //move inputpanel to bottom
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
