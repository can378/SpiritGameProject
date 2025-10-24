using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using UnityEditor.EditorTools;

public class MapUIManager : MonoBehaviour
{
    
    public static MapUIManager instance;

    [Header("UI")]
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
    public GameObject statSelectPanel;      // ���� ����â
    public GameObject nearObjectPanel;      // ��ȣ�ۿ�
    public GameObject inventoryPanel;       // ��� â
    public ToolTipUI toolTipPanel;

    [SerializeField] GameObject BossProgressPanel;

    //Player Stats
    [Header("�÷��̾� ����")]
    [SerializeField] Slider Hpslider;
    //[SerializeField] Slider ExpSlider;
    //[SerializeField] TMP_Text ExpTxt;
    [SerializeField] TMP_Text CoinTxt;
    [SerializeField] TMP_Text KeyTxt;
    //[SerializeField] Image itemImg;
    [SerializeField] Image skillImg;
    public Image skillCoolImg;


    //Boss Stats
    [Header("���� ����")]
    [SerializeField] EnemyBasic Boss;
    [SerializeField] Slider BossHpslider;
    [SerializeField] Transform BossBuffbar;
    [SerializeField] TMP_Text BossName;

    // Inventory
    [Header("�κ��丮 ����")]
    [SerializeField] ItemSlotUI InvenWeaponSlot;                                                                  // ��� �̹���
    [SerializeField] Image[] InvenSkillImage = new Image[5];                                                  // ��ų �̹���
    [SerializeField] Image InvenItemImage;
    [SerializeField] ItemSlotUI[] InvenEquipmentsSlot = new ItemSlotUI[3];                                             // ��� �̹���
    [SerializeField] TMP_Text[] InvenStatsValueTxt = new TMP_Text[10];                                        // ���� ��ġ

    // nearObject
    [Header("��ó ������ ����")]
    public TMP_Text nearObjectInteraction;

    //gameObject
    [Header("é�� ����")]
    public TMP_Text chapterTxt;

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
            if (inventoryPanel.activeSelf == true) {
                inventoryPanel.SetActive(false);
                
            }
            else {
                inventoryPanel.SetActive(true);
                toolTipPanel.gameObject.SetActive(false);
            }
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
        UpdateSkillCoolTimeUI();

        UpdateInventoryUI();
        UpdateNearObjectUI();
        UpdateNearObjectToolTipUI();

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
        // �������� ��� �ּ�ó���س���
        // Main �� �Ȱ�ġ�� Map �� ���� �ɸ��� ��
        // �ƴϸ� final �� ���� ������ ���ؼ� �� ��

        InvenStatsValueTxt[0].text = Player.instance.playerStats.HPMax.Value.ToString();
        InvenStatsValueTxt[1].text = Player.instance.playerStats.AttackPower.Value.ToString();
        InvenStatsValueTxt[2].text = ((Player.instance.playerStats.attackSpeed) * 100).ToString() + " %";
        InvenStatsValueTxt[3].text = (Player.instance.playerStats.CriticalChance.Value * 100).ToString() + " %";
        InvenStatsValueTxt[4].text = (Player.instance.playerStats.CriticalDamage.Value * 100).ToString() + " %";
        InvenStatsValueTxt[5].text = Player.instance.playerStats.SkillPower.Value.ToString();
        InvenStatsValueTxt[6].text = (Player.instance.playerStats.SkillCoolTime.Value * 100).ToString() + " %";
        InvenStatsValueTxt[7].text = (Player.instance.playerStats.DefensivePower.Value * 100).ToString() + " %";
        InvenStatsValueTxt[8].text = Player.instance.playerStats.MoveSpeed.Value.ToString();
        
    }

    void UpdateHealthUI()
    {
        float normalizedHealth = (Player.instance.stats.HP / Player.instance.stats.HPMax.Value) *100;
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
            skillImg.GetComponent<Image>().sprite = Player.instance.skillList[Player.instance.playerStats.skill[Player.instance.playerStatus.skillIndex]].GetComponent<SkillBase>().skillData.sprite;
        }
        else { skillImg.GetComponent<Image>().sprite = null; }
    }


    private float fillValue;
    private float skillCoolTime_max;
    private float skillCoolTime_now;
    void UpdateSkillCoolTimeUI() 
    {
        // ���� ��� (��: now = ���� ��, max = �ִ� ��).
        fillValue = skillCoolTime_now / skillCoolTime_max;
        skillCoolImg.fillAmount = Mathf.Clamp(fillValue, 0f, 1f);

    }
    public void UpdateSkillCoolTime(float max, float now)
    {
        skillCoolTime_max = max;
        skillCoolTime_now = now;
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

        // ���� �̹���
        if (Player.instance.playerStats.weapon.weaponInstance.IsValid())
        {
            InvenWeaponSlot.SetItemData(Player.instance.playerStats.weapon.weaponInstance);
        }
        else { InvenWeaponSlot.SetItemData(); }

        // ��� �̹���
        for (int i = 0; i < Player.instance.playerStats.maxEquipment; i++)
        {
            if (Player.instance.playerStats.equipments[i].IsValid())
            {
                InvenEquipmentsSlot[i].SetItemData(Player.instance.playerStats.equipments[i]);
            }
            else { InvenEquipmentsSlot[i].SetItemData(); }
        }

        // ��ų �̹���
        for (int i = 0; i < Player.instance.playerStats.maxSkillSlot; i++)
        {
            
            if (Player.instance.playerStats.skill[i] != 0)
            {
                InvenSkillImage[i].GetComponent<Image>().sprite = GameData.instance.skillList[Player.instance.playerStats.skill[i]].GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else { InvenSkillImage[i].GetComponent<Image>().sprite =  null; }
        }

        // �Ҹ�ǰ �̹���
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
    

    //���â ������Ʈ
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

        nearObjectInteraction.text = Player.instance.playerStatus.nearObject.name;
        nearObjectInteraction.text = Player.instance.playerStatus.nearObject.GetComponent<Interactable>().GetInteractText();   // GetComponent �����ϱ�
    }

    // ���� �÷��̾ ������ ��ó�� ������ ������ ���� ������ �����Ѵ�.
    public void UpdateNearObjectToolTipUI()
    {
        if (inventoryPanel.activeSelf)
            return;

        // ���� �÷��̾� ��ó�� �������� �ִ� �� Ȯ���Ѵ�.
        if (Player.instance.playerStatus.nearObject == null)
        {
            toolTipPanel.CloseToolTipUI();
            return;
        }

        ItemInstance curItem = Player.instance.playerStatus.nearObject.GetComponent<SelectItem>().itemInstance;
        if (curItem == null)
            return;

        if (!toolTipPanel.gameObject.activeSelf || curItem != toolTipPanel.ToolTipCurItem)
            toolTipPanel.OpenToolTipUI(curItem);


    }


    #endregion

    #region Boss UI

    // UI�� ������ ������ ���� �ʹٸ� ����
    public void SetBossProgress(EnemyBasic enemy)
    {
        Boss = enemy;
        enemy.buffTF = BossBuffbar;
    }

    void OnOffBossProgress()
    {
        if(Boss == null || Boss.isActiveAndEnabled==false)
            return;

        if (Boss.stats.HP <= 0)
        {
            BossProgressPanel.SetActive(false);
            return;
        }

        if(Boss.enemyStatus.EnemyTarget && !BossProgressPanel.activeSelf)
        {
            BossProgressPanel.SetActive(true);
            return;
        }
        
    }

    void UpdateBossHealthUI()
    {
        if(Boss == null||Boss.isActiveAndEnabled==false)
            return;

        if(!Boss.enemyStatus.EnemyTarget)
            return;

        float normalizedHealth = (Boss.stats.HP / Boss.stats.HPMax.Value) * 100;
        BossHpslider.value = normalizedHealth;
    }

    void UpdateBossName()
    {
        if(Boss == null || Boss.isActiveAndEnabled == false)
            return;

        if(!Boss.enemyStatus.EnemyTarget)
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
        Player.instance.UnEquipEquipment(index);
        //UpdateEquipmentUI();
    }

    #endregion

    //set random passive skill


    //get passive skill
    


}
