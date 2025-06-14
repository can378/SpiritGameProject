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
            AudioManager.instance.UIClickAudio();
            mainPanel.SetActive(!mainPanel.activeSelf);
            settingPanel.SetActive(false);
            taskPanel.SetActive(false);
            warningPanel.SetActive(false);
        }

        if(mainPanel.activeSelf || taskPanel.activeSelf || settingPanel.activeSelf || warningPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else { Time.timeScale = 1f; }
    }

    public void StartBtn() //게임 시작 버튼
    {
        AudioManager.instance.UIClickAudio();
        //SceneManager.LoadScene("Map");
        mainPanel.SetActive(false);
    }

    public void TaskBtn() //도전 과제, 찾은 NPC 보는 버튼
    {
        AudioManager.instance.UIClickAudio();
        settingPanel.SetActive(false);
        taskPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void BackBtn() //뒤로가기 버튼
    {
        AudioManager.instance.UIClickAudio();
        AudioManager.instance.UIClickAudio();
        mainPanel.SetActive(!mainPanel.activeSelf);
        settingPanel.SetActive(false);
        taskPanel.SetActive(false);
        warningPanel.SetActive(false);
    }
    public void SettingBtn() 
    {
        AudioManager.instance.UIClickAudio();
        settingPanel.SetActive(true);
        taskPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void ExitBtn()
    {
        AudioManager.instance.UIClickAudio();
        warningPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void QuitBtn()
    {
        AudioManager.instance.UIClickAudio();
        PlayerPrefs.Save();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


}
