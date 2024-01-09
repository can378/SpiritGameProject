using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Main과 Map에 공통되는 UI

public class UIManager : MonoBehaviour
{
    public List<GameObject> AllPanel;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }



    public void QuitBtn()
    {
        PlayerPrefs.Save();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    

}
