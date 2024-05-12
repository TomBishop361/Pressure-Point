using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;
using System.Runtime.CompilerServices;

public class MainMenuButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Supported by Tom
    [SerializeField] Camera _camera;
    [SerializeField] GameObject pointer;
    int i;
    Vector3 lerpVec;
    [SerializeField] GameObject overlay;
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainmenuCanvas;
    


    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if(_camera != null ) lerpVec = _camera.gameObject.transform.position;
    }

    public void playGame()
    {
        
        StartCoroutine("CameraLerp");
    }

    IEnumerator CameraLerp()
    {
        
        float time = 0;
        while (time < 1f)
        {
            float perc = 0;
            perc = Easing.Linear(time);
            lerpVec.x = LerpScript.lerp(-22.026f, -21.924f, perc);
            lerpVec.y = LerpScript.lerp(8.897f, 9.169f, perc);
            lerpVec.z = LerpScript.lerp(-22.216f, -30.043f, perc);
            time += Time.deltaTime; 
            yield return null;
        }
        player.SetActive(true);
        _camera.gameObject.SetActive(false);
        overlay.SetActive(true);  
        mainmenuCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }



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

    public void quitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        
        if (_camera != null)
        {
            
            _camera.gameObject.transform.position = lerpVec;
        }
    }


}
