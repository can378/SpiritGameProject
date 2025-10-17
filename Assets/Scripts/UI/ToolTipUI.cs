using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;


public class ToolTipUI : MonoBehaviour
{

    // nearObject
    [Header("������ �̸�")]
    [Tooltip("������ �̸�")]
    public TMP_Text ToolTipNameText;


    [Header("������ ����")]
    [Header("������ ��������")]
    [Tooltip("������ ���� ����")]   // ���� ��Ȱ��ȭ
    public GameObject ToolTipSimpleInfo;
    [Tooltip("������ ��ġ : ���� �Ǵ� ��ų�� ���ط�")]
    public TMP_Text ToolTipNum;
    [Tooltip("������ ��ġ : ���� �Ǵ� ��ų�� ���ط�")]
    public TMP_Text ToolTipNumText;
    [Tooltip("������ ��Ÿ ")]
    public TMP_Text ToolTipType;
    [Tooltip("������ ��Ÿ : ����� �з�, ��ų�� ���� ��� �ð�")]
    public TMP_Text ToolTipTypeText;


    [Header("������ ����")]
    [Tooltip("������ ����")]
    public TMP_Text ToolTipDescriptionText;
    public SelectItem ToolTipCurItem { get; private set; }

    public void OpenToolTipUI(SelectItem _SelectItem)
    {

        if (_SelectItem == null)
        {
            gameObject.SetActive(false);
            return;
        }
        ToolTipCurItem = _SelectItem;

        gameObject.SetActive(true);

        ToolTipNameText.text = ToolTipCurItem.itemData.selectItemName;
        ToolTipNameText.color = ToolTipCurItem.GetRatingColor();

        ToolTipDescriptionText.text = ToolTipCurItem.itemData.Update_Description(Player.instance.playerStats);

        if (ToolTipCurItem is Weapon weapon)
        {
            ToolTipSimpleInfo.SetActive(true);
            ToolTipNum.text = "���ݷ�";
            ToolTipNumText.text = weapon.weaponData.attackPower.ToString();
            ToolTipType.text = "�з�";
            ToolTipTypeText.text = weapon.weaponData.TypeToKorean();
        }
        else if (ToolTipCurItem is SkillItem skill)
        {
            ToolTipSimpleInfo.SetActive(true);
            ToolTipNum.text = skill.skillData.Update_Num();
            ToolTipNumText.text = skill.skillData.Update_NumText(Player.instance.playerStats);
            ToolTipType.text = "��� �ð�";
            ToolTipTypeText.text = skill.skillData.Update_CoolTime(Player.instance.playerStats);
        }
        else
        {
            if (ToolTipCurItem.itemData.selectItemType == SelectItemType.Equipments)
            {
                ToolTipSimpleInfo.SetActive(false);
                ToolTipNum.text = "";
                ToolTipNumText.text = "";
                ToolTipType.text = "";
                ToolTipTypeText.text = "";
            }
            else if (ToolTipCurItem.itemData.selectItemType == SelectItemType.Consumable)
            {
                ToolTipSimpleInfo.SetActive(true);
                ToolTipNum.text = "";
                ToolTipNumText.text = "";
                ToolTipType.text = "";
                ToolTipTypeText.text = "";
            }
        }
    }

    public void CloseToolTipUI()
    {
        ToolTipCurItem = null;
        gameObject.SetActive(false);
    }
}
