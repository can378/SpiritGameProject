using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MapUIManager : MonoBehaviour
{
    
    public static MapUIManager instance;

    //Panel
    public GameObject tabkeyPanel;
    public GameObject esckeyPanel;
    public GameObject diePanel;
    public GameObject settingPanel;


    //Player
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
            if (esckeyPanel) { Time.timeScale = 1; }
            else { Time.timeScale = 0; }

            AudioManager.instance.TestAudioPlay();
            esckeyPanel.SetActive(!esckeyPanel.activeSelf);
            settingPanel.SetActive(false);
            
        }
    }

    #region player UI
    public void UpdateHealthUI()
    {
        
        // DataManager�� �ʱ�ȭ���� �ʾҰų� instance�� null�̸� �� �̻� �������� ����
        if (DataManager.instance == null || DataManager.instance.userData == null)
        {
            return;
        }

        float normalizedHealth 
            = (DataManager.instance.userData.playerHP / DataManager.instance.userData.playerHPMax)*100;
        //Debug.Log("Player Health: " + DataManager.instance.userData.playerHP+ DataManager.instance.userData.playerHPMax);
        //Debug.Log("Normalized Health: " + normalizedHealth);
        Hpslider.value = normalizedHealth;
        //Hpscrollbar.GetComponentInChildren<Text>().text = "Health: " + DataManager.instance.userData.playerHP; //�ϴ� ���� �ؽ�Ʈ ����


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


    #endregion


    #region Button

    public void ContinueBtn()
    {
        esckeyPanel.SetActive(false);
        Time.timeScale = 1;
    }
    

    public void HomeBtn() //Ȩ ��ư
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main");
    }


    public void RestartBtn() //����� ��ư
    {
        SceneManager.LoadScene("Map");
        DataManager.instance.InitData();
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
    public void SettingBtn() 
    {
        settingPanel.SetActive(true);
        tabkeyPanel.SetActive(false);
        esckeyPanel.SetActive(false);
        diePanel.SetActive(false);

    }
    public void BackBtn() 
    {
        AudioManager.instance.TestAudioPlay();
        esckeyPanel.SetActive(!esckeyPanel.activeSelf);
        settingPanel.SetActive(false);
    }
    #endregion


}