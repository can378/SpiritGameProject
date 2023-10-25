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
    public void StartBtn() //���� ���� ��ư
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowOption() //���� ��ư
    {
        optionPanel.SetActive(true);
        taskPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void ShowTask() //���� ����, ã�� NPC ���� ��ư
    {
        optionPanel.SetActive(false);
        taskPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void GoBackToTitle() //�ڷΰ��� ��ư
    {
        SceneManager.LoadScene("Title");
        mainPanel.SetActive(true);
    }

    public void QuitGame() //���� ������ ��ư
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

