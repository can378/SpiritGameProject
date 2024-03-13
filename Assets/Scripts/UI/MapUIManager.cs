using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class MapUIManager : MonoBehaviour
{
    
    public static MapUIManager instance;

    //Panel
    public GameObject tabkeyPanel;
    public GameObject esckeyPanel;
    public GameObject diePanel;
    public GameObject settingPanel;
    public GameObject startPanel;
    public GameObject warningPanel;
    public GameObject restartPanel;
    public GameObject resetPanel;
    public RectTransform sidePanel;
    public RectTransform equipmentPanel;

    //Player status
    public Slider Hpslider;
    public Slider ExpSlider;
    public TMP_Text ExpTxt;
    public TMP_Text CoinTxt;
    public TMP_Text KeyTxt;
    public Image itemImg;
    public TMP_Text WeaponTxt;
    public TMP_Text SkillTxt;
    public TMP_Text PointTxt;

    public TMP_Text[] EquipmentsTxt = new TMP_Text[3];

    //gameObject
    public TMP_Text chapterTxt;



    private bool sidePanelVisible = false;
    private bool EquipmentPanelVisible = false;
    UserData userData;

    private void Awake()
    {
        instance = this;
        userData = DataManager.instance.userData;
    }

    void Start()
    {
        UpdateHealthUI();
        UpdateCoinUI();
        UpdateKeyUI();
        UpdateExpUI();
        //UpdateWeaponUI();
        //UpdateSkillUI();
        UpdatePointUI();
        if (Player.instance.stats.item == "")
        { updateItemUI(null); }

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

        if(esckeyPanel.activeSelf || settingPanel.activeSelf || warningPanel.activeSelf || resetPanel.activeSelf || restartPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else { Time.timeScale = 1f; }
    }

    void setUpgradePanel()
    {
        chapterTxt.text = "Chapter " + DataManager.instance.userData.nowChapter.ToString();
    }


    #region player UI
    public void UpdateHealthUI()
    {

        float normalizedHealth = (Player.instance.stats.HP / Player.instance.stats.HPMax) *100;
        Hpslider.value = normalizedHealth;

    }

    public void UpdateExpUI()
    {
        if(Player.instance.stats.exp%10==0) 
        {
            Player.instance.stats.level++;
            Player.instance.stats.point++;
            Player.instance.stats.exp = 0;
            UpdatePointUI();
        }
        
        float normalizedEXP = Player.instance.stats.exp;
        ExpSlider.value = normalizedEXP;

        //print("update exp ui" + normalizedEXP);
        ExpTxt.text = Player.instance.stats.exp.ToString();
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
        CoinTxt.text = Player.instance.stats.coin.ToString();
    }

    public void UpdateKeyUI()
    {
        KeyTxt.text = Player.instance.stats.key.ToString();
    }


    public void UpdateWeaponUI() 
    {
        if (Player.instance.stats.weapon != null)
        {
            WeaponTxt.text = Player.instance.stats.weapon.equipmentName;
        }
        else {WeaponTxt.text = "";}
    }

    public void UpdateSkillUI() 
    {
        if (Player.instance.stats.skill != null)
        {
            SkillTxt.text = Player.instance.stats.skill.skillName;
        }
        else {SkillTxt.text = "";}
    }

    public void UpdatePointUI()
    {
        PointTxt.text = Player.instance.stats.point.ToString();
    }

    public void UpdateEquipmentUI()
    {
        for(int i = 0;i<Player.instance.stats.maxEquipment; i++)
        {
            if(Player.instance.stats.equipments[i] != null)
                EquipmentsTxt[i].text = Player.instance.stats.equipments[i].equipmentName;
            else
                EquipmentsTxt[i].text ="";
        }
    }

    #endregion


    #region Button

    public void ContinueBtn()
    {
        esckeyPanel.SetActive(false);
        Time.timeScale = 1;
    }
    

    public void ResetBtn()
    {
        DataManager.instance.InitData();
        DataManager.instance.SaveUserData();
        SceneManager.LoadScene("Main");
    }


    public void RestartBtn() //now chapter restart
    {
        DataManager.instance.LoadData();
        if (userData.nowChapter == 5) { SceneManager.LoadScene("FinalMap"); }
        else { SceneManager.LoadScene("Map"); }
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
        warningPanel.SetActive(false);
        restartPanel.SetActive(false);
        resetPanel.SetActive(false);
    }

    public void StartBtn() 
    {
        startPanel.SetActive(false);
    }

    public void SideBtn() 
    {
        UpdateEquipmentUI();
        if (sidePanelVisible) 
        {
            // 패널을 오른쪽으로 이동시킴
            sidePanel.anchoredPosition += new Vector2(sidePanel.rect.width, 0);
            equipmentPanel.anchoredPosition -= new Vector2(0, 900);
            sidePanelVisible = false;
        }
        else
        {
            // 패널을 왼쪽으로 이동시킴
            sidePanel.anchoredPosition -= new Vector2(sidePanel.rect.width, 0);
            equipmentPanel.anchoredPosition += new Vector2(0, 900);
            sidePanelVisible = true;
        }

    }

    public void ExitBtn()
    {
        warningPanel.SetActive(true);
        esckeyPanel.SetActive(false);
    }

    public void ResetCheckBtn()
    {
        resetPanel.SetActive(true);
        esckeyPanel.SetActive(false);
    }

    public void RestartCheckBtn()
    {
        restartPanel.SetActive(true);
        esckeyPanel.SetActive(false);
    }

    public void EquipmentUnEquipBtn(int index)
    {
        print("장비 해제");
        Player.instance.UnEquipEquipment(index);
        UpdateEquipmentUI();
    }

    #endregion

    //set random passive skill


    //get passive skill
    


}
