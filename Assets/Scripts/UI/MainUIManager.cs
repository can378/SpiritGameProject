using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{

    public GameObject mainPanel;
    public GameObject taskPanel;
    public GameObject settingPanel;
    public GameObject warningPanel;

    void Start()
    {
        mainPanel.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.instance.TestAudioPlay();
            mainPanel.SetActive(!mainPanel.activeSelf);
            settingPanel.SetActive(false);
            taskPanel.SetActive(false);
            warningPanel.SetActive(false);
        }
    }

    public void StartBtn() //���� ���� ��ư
    {
        //SceneManager.LoadScene("Map");
        mainPanel.SetActive(false);
    }



    public void TaskBtn() //���� ����, ã�� NPC ���� ��ư
    {
        settingPanel.SetActive(false);
        taskPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void BackBtn() //�ڷΰ��� ��ư
    {
        AudioManager.instance.TestAudioPlay();
        mainPanel.SetActive(!mainPanel.activeSelf);
        settingPanel.SetActive(false);
        taskPanel.SetActive(false);
        warningPanel.SetActive(false);
    }
    public void SettingBtn() 
    {
        settingPanel.SetActive(true);
        taskPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void ExitBtn()
    {
        warningPanel.SetActive(true);
        mainPanel.SetActive(false);
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
