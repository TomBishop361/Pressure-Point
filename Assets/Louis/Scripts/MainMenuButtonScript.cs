using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UIElements.Experimental;
using System.Runtime.CompilerServices;

public class MainMenuButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Camera _camera;
    [SerializeField] GameObject pointer;
    int i;
    Vector3 lerpVec;
    [SerializeField] GameObject overlay;
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainmenuCanvas;
    


    private void Start()
    {
        lerpVec = _camera.gameObject.transform.position;
    }

    public void playGame()
    {
        Debug.Log("adawdawd");
        StartCoroutine("CameraLerp");
    }

    IEnumerator CameraLerp()
    {
        Debug.Log("Coroutine");
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

    void Update()
    {
        
        if (_camera != null)
        {
            Debug.Log("Update");
            _camera.gameObject.transform.position = lerpVec;
        }
    }


}
