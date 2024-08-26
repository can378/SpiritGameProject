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
    public GameObject minimapPanel;
    public GameObject statSelectPanel;      // 스탯 선택창
    public GameObject nearObjectPanel;      // 상호작용
    public GameObject inventoryPanel;       // 장비 창
    [SerializeField] GameObject BossProgressPanel;

    //Player Stats
    [SerializeField] Slider Hpslider;
    //[SerializeField] Slider ExpSlider;
    //[SerializeField] TMP_Text ExpTxt;
    [SerializeField] TMP_Text CoinTxt;
    [SerializeField] TMP_Text KeyTxt;
    //[SerializeField] Image itemImg;
    [SerializeField] Image skillImg;

    //Boss Stats
    [SerializeField] EnemyBasic Boss;
    [SerializeField] Slider BossHpslider;
    [SerializeField] Transform BossBuffbar;
    [SerializeField] TMP_Text BossName;

    // Inventory
    [SerializeField] Image InvenWeaponImage;                                                                  // 장비 이미지
    [SerializeField] Image[] InvenSkillImage = new Image[5];                                                  // 스킬 이미지
    [SerializeField] Image InvenItemImage;
    [SerializeField] Image[] InvenEquipmentsImage = new Image[3];                                             // 장비 이미지
    [SerializeField] TMP_Text[] InvenStatsValueTxt = new TMP_Text[10];                                        // 스탯 수치

    // nearObject
    public TMP_Text nearObjectInteraction;

    //gameObject
    public TMP_Text chapterTxt;



    //private bool sidePanelVisible = false;
    //private bool EquipmentPanelVisible = false;
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
        //UpdateExpUI();
        //UpdateWeaponUI();
        //UpdateSkillUI();
        //UpdatePointUI();
        //if (Player.instance.playerStats.item == 0)
        //{ updateItemUI(null); }

        //setUpgradePanel();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            AudioManager.instance.UIClickAudio();
            tabkeyPanel.SetActive(!tabkeyPanel.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (esckeyPanel) { Time.timeScale = 1; }
            else { Time.timeScale = 0; }

            AudioManager.instance.UIClickAudio();
            esckeyPanel.SetActive(!esckeyPanel.activeSelf);
            settingPanel.SetActive(false);
            
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPanel.activeSelf == true) { inventoryPanel.SetActive(false); }
            else { inventoryPanel.SetActive(true); }
        }


        if(esckeyPanel.activeSelf || settingPanel.activeSelf || warningPanel.activeSelf || resetPanel.activeSelf || restartPanel.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else { Time.timeScale = 1f; }
    }

    void FixedUpdate() 
    {
        UpdateHealthUI();
        UpdateCoinUI();
        UpdateKeyUI();
        UpdateStatUI();
        UpdateSkillUI();
        UpdateInventoryUI();
        UpdateNearObjectUI();

        OnOffBossProgress();
        UpdateBossHealthUI();
        UpdateBossName();

    }

    void setUpgradePanel()
    {
        chapterTxt.text = "Chapter " + DataManager.instance.userData.nowChapter.ToString();
    }


    #region Player UI Update

    void UpdateStatUI()
    {
        //오류나서 잠깐 주석처리해놨음
        // Main 씬 안거치고 Map 씬 들어가면 걸리는 듯
        // 아니면 final 씬 에서 설정을 안해서 일 듯

        InvenStatsValueTxt[0].text = Player.instance.playerStats.HPMax.ToString();
        InvenStatsValueTxt[1].text = Player.instance.playerStats.attackPower.ToString();
        InvenStatsValueTxt[2].text = ((Player.instance.playerStats.attackSpeed) * 100).ToString() + " %";
        InvenStatsValueTxt[3].text = (Player.instance.playerStats.criticalChance * 100).ToString() + " %";
        InvenStatsValueTxt[4].text = (Player.instance.playerStats.criticalDamage * 100).ToString() + " %";
        InvenStatsValueTxt[5].text = Player.instance.playerStats.skillPower.ToString();
        InvenStatsValueTxt[6].text = (Player.instance.playerStats.skillCoolTime * 100).ToString() + " %";
        InvenStatsValueTxt[7].text = (Player.instance.playerStats.defensivePower * 100).ToString() + " %";
        InvenStatsValueTxt[8].text = Player.instance.playerStats.moveSpeed.ToString();
        
    }

    void UpdateHealthUI()
    {
        float normalizedHealth = (Player.instance.stats.HP / Player.instance.stats.HPMax) *100;
        Hpslider.value = normalizedHealth;
    }

    /*
    public void UpdateExpUI()
    {
        if(Player.instance.playerStats.exp%10==0) 
        {
            Player.instance.playerStats.level++;
            Player.instance.playerStats.point++;
            Player.instance.playerStats.exp = 0;
            UpdatePointUI();
        }
        
        float normalizedEXP = Player.instance.playerStats.exp;
        ExpSlider.value = normalizedEXP;

        //print("update exp ui" + normalizedEXP);
        ExpTxt.text = Player.instance.playerStats.exp.ToString();
    }
    */

    /*
    public void updateItemUI(GameObject obj) 
    {
        if (obj != null)
        {
            itemImg.GetComponent<Image>().sprite = obj.GetComponentInChildren<SpriteRenderer>().sprite;
        }
        else { itemImg.GetComponent<Image>().sprite = null; }

    }
    */

    void UpdateSkillUI()
    {
        if (Player.instance.playerStats.skill[Player.instance.playerStatus.skillIndex] != 0)
        {
            skillImg.GetComponent<Image>().sprite = GameData.instance.skillList[Player.instance.playerStats.skill[Player.instance.playerStatus.skillIndex]].GetComponentInChildren<SpriteRenderer>().sprite;
        }
        else { skillImg.GetComponent<Image>().sprite = null; }
    }

    void UpdateCoinUI() 
    {
        CoinTxt.text = Player.instance.playerStats.coin.ToString();
    }

    void UpdateKeyUI()
    {
        KeyTxt.text = Player.instance.playerStats.key.ToString();
    }


    void UpdateInventoryUI() 
    {
        if(inventoryPanel.activeSelf == false)
            return;

        // 무기 이미지
        if (Player.instance.playerStats.weapon != 0)
        {
            InvenWeaponImage.GetComponent<Image>().sprite = GameData.instance.weaponList[Player.instance.playerStats.weapon].GetComponentInChildren<SpriteRenderer>().sprite; ;
        }
        else { InvenWeaponImage.GetComponent<Image>().sprite = null;}

        // 장비 이미지
        for (int i = 0; i < Player.instance.playerStats.maxEquipment; i++)
        {
            if (Player.instance.playerStats.equipments[i] != 0)
                InvenEquipmentsImage[i].GetComponent<Image>().sprite = GameData.instance.equipmentList[Player.instance.playerStats.equipments[i]].GetComponentInChildren<SpriteRenderer>().sprite;
            else
                InvenEquipmentsImage[i].GetComponent<Image>().sprite = null;
        }

        // 스킬 이미지
        for (int i = 0; i < Player.instance.playerStats.maxSkillSlot; i++)
        {
            
            if (Player.instance.playerStats.skill[i] != 0)
            {
                InvenSkillImage[i].GetComponent<Image>().sprite = GameData.instance.skillList[Player.instance.playerStats.skill[i]].GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else { InvenSkillImage[i].GetComponent<Image>().sprite =  null; }
        }

        // 소모품 이미지
        if (Player.instance.playerStats.item != 0)
        {
            InvenItemImage.GetComponent<Image>().sprite = GameData.instance.selectItemList[Player.instance.playerStats.item].GetComponentInChildren<SpriteRenderer>().sprite;
        }
        else { InvenItemImage.GetComponent<Image>().sprite = null; }

    }

    /*
    public void UpdatePointUI()
    {
        PointTxt.text = Player.instance.playerStats.point.ToString();
    }
    

    //장비창 업데이트
    public void UpdateEquipmentUI()
    {
        for(int i = 0;i<Player.instance.playerStats.maxEquipment; i++)
        {
            if(Player.instance.playerStats.equipments[i] != 0)
                EquipmentsTxt[i].text = Player.instance.equipmentList[Player.instance.playerStats.equipments[i]].selectItemName;
            else
                EquipmentsTxt[i].text ="";
        }
    }
    */

    public void UpdateMinimapUI(bool tf)
    {
        minimapPanel.SetActive(tf);
    
    }

    public void UpdateNearObjectUI()
    {
        if(Player.instance.playerStatus.nearObject == null)
        {
            nearObjectPanel.SetActive(false);
            return;
        }

        nearObjectPanel.SetActive(true);
        switch(Player.instance.playerStatus.nearObject.tag)
        {
            case "Npc":
                nearObjectInteraction.text = Player.instance.playerStatus.nearObject.name;
                nearObjectInteraction.text += " 대화";
                break;
            case "SelectItem":
                nearObjectInteraction.text = Player.instance.playerStatus.nearObject.GetComponent<SelectItem>().selectItemName;
                nearObjectInteraction.text += " 획득";
                break;
            default:
                break;
        }
    }

    #endregion

    #region Boss UI

    // UI에 보스의 정보를 띄우고 싶다면 설정
    public void SetBossProgress(EnemyBasic enemy)
    {
        Boss = enemy;
        enemy.buffTF = BossBuffbar;
    }

    void OnOffBossProgress()
    {
        if(Boss == null)
            return;

        if (Boss.stats.HP <= 0)
        {
            BossProgressPanel.SetActive(false);
            return;
        }

        if(Boss.enemyStatus.enemyTarget && !BossProgressPanel.activeSelf)
        {
            BossProgressPanel.SetActive(true);
            return;
        }
        
    }

    void UpdateBossHealthUI()
    {
        if(Boss == null)
            return;

        if(!Boss.enemyStatus.enemyTarget)
            return;

        float normalizedHealth = (Boss.stats.HP / Boss.stats.HPMax) * 100;
        BossHpslider.value = normalizedHealth;
    }

    void UpdateBossName()
    {
        if(Boss == null)
            return;

        if(!Boss.enemyStatus.enemyTarget)
            return;
        
        BossName.text = Boss.enemyStats.enemyName;
    }

    #endregion Boss UI

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

        AudioManager.instance.Bgm_normal(userData.nowChapter);
        SceneManager.LoadScene("Main");
    }


    public void RestartBtn() //now chapter restart
    {
        DataManager.instance.LoadData();

        AudioManager.instance.Bgm_normal(userData.nowChapter);
        if (userData.nowChapter == 4) 
        {
            SceneManager.LoadScene("FinalMap");
        }
        else 
        {
            SceneManager.LoadScene("Map"); 
        }
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
        AudioManager.instance.UIClickAudio();
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
        //UpdateEquipmentUI();
    }

    #endregion

    //set random passive skill


    //get passive skill
    


}
