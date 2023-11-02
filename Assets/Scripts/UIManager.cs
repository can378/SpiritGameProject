using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject tabkeyPanel;
    public GameObject esckeyPanel;
    public GameObject diePanel;

    private bool isPanelActive = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            tabkeyPanel.SetActive(!tabkeyPanel.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            esckeyPanel.SetActive(!tabkeyPanel.activeSelf);
        }
    }
    public void ContinueBtn()
    {
        esckeyPanel.SetActive(false);
    }
    public void QuitBtn() //저장하고 나가기
    {
        PlayerPrefs.Save();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void HomeBtn() //홈 버튼
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("Title");
    }
    public void RestartBtn() //재시작 버튼
    {
        tabkeyPanel.SetActive(false);
        esckeyPanel.SetActive(false);
        diePanel.SetActive(false);
    }
}
