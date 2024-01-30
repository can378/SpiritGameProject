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
    public GameObject startPanel;

    //Player status
    public Slider Hpslider;
    public TMP_Text ExpTxt;
    public TMP_Text CoinTxt;
    public TMP_Text KeyTxt;
    public Image itemImg;
    public TMP_Text WeaponTxt;
    public TMP_Text SkillTxt;

    //gameObject
    public TMP_Text chapterTxt;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateHealthUI();
        UpdateCoinUI();
        UpdateKeyUI();
        UpdateExpUI();
        UpdateWeaponUI();
        UpdateSkillUI();
        updateItemUI(null);

        setUpgradePanel();
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

    public void updateItemUI(GameObject obj) 
    {

        if (obj != null)
        {

            itemImg.GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
        }
        else { itemImg.GetComponent<Image>().sprite = null; }

    }
    public void UpdateCoinUI() 
    {
        CoinTxt.text = DataManager.instance.userData.coin.ToString();
    }
    public void UpdateKeyUI()
    {
        KeyTxt.text = DataManager.instance.userData.key.ToString();
    }

    public void UpdateExpUI() 
    {
        ExpTxt.text = DataManager.instance.userData.playerExp.ToString();
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
    

    public void HomeBtn() //홈 버튼
    {
        //PlayerPrefs.Save();
        //SceneManager.LoadScene("Main");
    }


    public void RestartBtn() //재시작 버튼
    {
        DataManager.instance.InitData();
        SceneManager.LoadScene("Main");
        
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
    public void StartBtn() 
    {
        startPanel.SetActive(false);
    }
    #endregion

    void setUpgradePanel() 
    {
        chapterTxt.text = "Chapter " + DataManager.instance.userData.nowChapter.ToString();

        //스탯 할당
    
    }

}
