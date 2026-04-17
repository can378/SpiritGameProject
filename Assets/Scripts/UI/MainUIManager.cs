using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class MainUIManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject taskPanel;
    public GameObject settingPanel;
    public GameObject warningPanel;
    public GameObject nearObjectPanel;      // 상호작용
    public GameObject DefaultGuidePanel; //기본 사용법
    public ToolTipUI toolTipPanel;

    public TimeLineController timeLineController;
    bool wasTimelinePausedByUI = false;
    bool isPlayCutScene = false; // 그냥 대충 한 것이므로 수정할 것
    bool isPlayStart = false;

    bool hasHiddenDirectionPanel = false;

    // nearObject
    [Header("근처 아이템 관련")]
    public TMP_Text nearObjectInteraction;

    void Start()
    {
        mainPanel.SetActive(true);
        DefaultGuidePanel.SetActive(true);

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
            //if (isPlayStart) { mainPanel.SetActive(!mainPanel.activeSelf); }
            if (mainPanel.activeSelf) { if (isPlayStart) { mainPanel.SetActive(false); } }
            else { mainPanel.SetActive(true); }
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

        if (!hasHiddenDirectionPanel && timeLineController.CheckFinish()==true)
        {
            HideDefaultGuidePanel();
        }
    }

    private void FixedUpdate()
    {
        UpdateNearObjectToolTipUI();
        UpdateNearObjectUI();
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
        isPlayStart = true;

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

    public void UpdateNearObjectUI()
    {
        if (Player.instance.playerStatus.nearObject == null)
        {
            nearObjectPanel.SetActive(false);
            return;
        }

        if (Player.instance.playerStatus.nearObject.GetComponent<Interactable>().GetInteractText() == "")
        {
            nearObjectPanel.SetActive(false);
            return;
        }

        nearObjectPanel.SetActive(true);

        nearObjectInteraction.text = Player.instance.playerStatus.nearObject.name;
        nearObjectInteraction.text = Player.instance.playerStatus.nearObject.GetComponent<Interactable>().GetInteractText() + "[F]";   // GetComponent 변경하기
    }


    public void HideDefaultGuidePanel()
    {
        if (DefaultGuidePanel.activeSelf==false)
            return;

        hasHiddenDirectionPanel = true;

        StartCoroutine(FadeOutDefaultGuidePanel());
    }

    IEnumerator FadeOutDefaultGuidePanel()
    {
        Debug.Log("기본 사용법 비활성화 시작");

        DefaultGuidePanel.SetActive(true);
        CanvasGroup DefGuideCG = DefaultGuidePanel.GetComponent<CanvasGroup>();
        DefGuideCG.alpha = 1f;

        yield return new WaitForSeconds(4f);

        float t = 0f;
        float duration = 2.0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            DefGuideCG.alpha = 1f - (t / duration);
            yield return null;
        }

        DefGuideCG.alpha = 0f;
        DefaultGuidePanel.SetActive(false);
    }
}
