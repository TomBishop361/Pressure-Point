using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject pointer;
    int i;
 
    public void OnPointerEnter(PointerEventData eventData)
    {
        InvokeRepeating("pointerFlash", 0f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke();
        pointer.SetActive(false);
    }

    void pointerFlash()
    {
        switch (i)
        {
            case 0:
                pointer.SetActive(true);
                i++;
                break;
            case 1:
                pointer.SetActive(false);
                i--;
                break;
        }
    }

    
    
}
