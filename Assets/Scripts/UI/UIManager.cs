using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Main�� Map�� ����Ǵ� UI

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
