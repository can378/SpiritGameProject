using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MainUIManager : MonoBehaviour
{

    public GameObject mainPanel;
    public GameObject taskPanel;
    public GameObject settingPanel;
    public GameObject warningPanel;
    public ToolTipUI toolTipPanel;

    public TimeLineController timeLineController;
    bool wasTimelinePausedByUI = false;
    bool isPlayCutScene = false; // 그냥 대충 한 것이므로 수정할 것


    void Start()
    {
        mainPanel.SetActive(true);

        // 초기에 패널 다 꺼줌
        taskPanel.SetActive(false);
        settingPanel.SetActive(false);
        warningPanel.SetActive(false);
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

            // UI가 하나라도 켜져있으면 일시 중지
            // 그냥 대충 한 것이므로 수정할 것
            HandleTimelineByUI();
        }

        if (timeLineController.GetPlayableState() == PlayState.Playing || IsAnyUIOpen())
        {
            Time.timeScale = 0f;
        }
        else { Time.timeScale = 1f; }
    }

    private void FixedUpdate()
    {
        UpdateNearObjectToolTipUI();
    }

    private bool IsAnyUIOpen()
    {
        return mainPanel.gameObject.activeSelf ||
               taskPanel.gameObject.activeSelf ||
               settingPanel.gameObject.activeSelf ||
               warningPanel.gameObject.activeSelf;
    }

    private void HandleTimelineByUI()
    {
        if (IsAnyUIOpen())
        {
            if (timeLineController.GetPlayableState() == PlayState.Playing)
            {
                timeLineController.Pause();
                wasTimelinePausedByUI = true;
            }
        }
        else
        {
            if (wasTimelinePausedByUI)
            {
                timeLineController.Resume();
                wasTimelinePausedByUI = false;
            }
        }
    }


    public void StartBtn() //게임 시작 버튼
    {
        AudioManager.instance.UIClickAudio();
        //SceneManager.LoadScene("Map");

        // 씬 최초 진입시만 컷신 재생
        // 그냥 대충 한 것이므로 수정할 것
        if (!isPlayCutScene) {
            timeLineController.Play();
            isPlayCutScene = true;
        }
        else if (timeLineController.GetPlayableState() == PlayState.Paused)
        {
            timeLineController.Resume();
        }
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

    public void UpdateNearObjectToolTipUI()
    {

        // 현재 플레이어 근처에 아이템이 있는 지 확인한다.
        if (Player.instance == null || Player.instance.playerStatus == null || Player.instance.playerStatus.nearObject == null)
        {
            toolTipPanel.CloseToolTipUI();
            return;
        }

        SelectItem curItem = Player.instance.playerStatus.nearObject.GetComponent<SelectItem>();
        if (curItem == null)
            return;

        if (!toolTipPanel.gameObject.activeSelf || curItem.itemInstance != toolTipPanel.ToolTipCurItem)
        {
            toolTipPanel.OpenToolTipUI(curItem.itemInstance);
            toolTipPanel.ChangePosition(ToolTipUIPos.InGameItem);
        }


    }
}
