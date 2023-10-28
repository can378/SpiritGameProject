using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionPanel;
    public GameObject taskPanel;

    void Start()
    {
        mainPanel.SetActive(true);
    }
    public void StartBtn() //게임 시작 버튼
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowOption() //설정 버튼
    {
        optionPanel.SetActive(true);
        taskPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void ShowTask() //도전 과제, 찾은 NPC 보는 버튼
    {
        optionPanel.SetActive(false);
        taskPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void GoBackToTitle() //뒤로가기 버튼
    {
        SceneManager.LoadScene("Title");
        mainPanel.SetActive(true);
    }

    public void QuitGame() //게임 나가기 버튼
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

