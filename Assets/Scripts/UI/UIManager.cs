using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject tabkeyPanel;
    public GameObject esckeyPanel;
    public GameObject diePanel;

    public Slider Hpslider;
    public TMP_Text LevelTxt;
    public TMP_Text CoinTxt;
    public TMP_Text KeyTxt;
    public TMP_Text WeaponTxt;
    public TMP_Text SkillTxt;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateHealthUI();
        UpdateCoinUI();
        UpdateKeyUI();
        UpdateLevelUI();
        UpdateWeaponUI();
        UpdateSkillUI();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            AudioManager.instance.TestAudioPlay();
            tabkeyPanel.SetActive(!tabkeyPanel.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.instance.TestAudioPlay();
            esckeyPanel.SetActive(!tabkeyPanel.activeSelf);
        }
    }


    public void UpdateHealthUI()
    {
        
        // DataManager가 초기화되지 않았거나 instance가 null이면 더 이상 진행하지 않음
        if (DataManager.instance == null || DataManager.instance.userData == null)
        {
            return;
        }

        float normalizedHealth 
            = (DataManager.instance.userData.playerHP / DataManager.instance.userData.playerHPMax)*100;
        //Debug.Log("Player Health: " + DataManager.instance.userData.playerHP+ DataManager.instance.userData.playerHPMax);
        //Debug.Log("Normalized Health: " + normalizedHealth);
        Hpslider.value = normalizedHealth;
        //Hpscrollbar.GetComponentInChildren<Text>().text = "Health: " + DataManager.instance.userData.playerHP; //일단 숫자 텍스트 보류


    }

    public void UpdateCoinUI() 
    {
        CoinTxt.text = DataManager.instance.userData.coin.ToString();
    }
    public void UpdateKeyUI()
    {
        KeyTxt.text = DataManager.instance.userData.key.ToString();
    }

    public void UpdateLevelUI() 
    {
        LevelTxt.text = DataManager.instance.userData.playerLevel.ToString();
    
    }
    public void UpdateWeaponUI() 
    {
        WeaponTxt.text = DataManager.instance.userData.Weapon;
    
    }

    public void UpdateSkillUI() 
    {
        WeaponTxt.text = DataManager.instance.userData.Skill;
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
        SceneManager.LoadScene("Main");
    }
    public void RestartBtn() //재시작 버튼
    {
        SceneManager.LoadScene("Map"); //현재 씬 다시 로드

        tabkeyPanel.SetActive(false);
        esckeyPanel.SetActive(false);
        diePanel.SetActive(false);
    }
}
