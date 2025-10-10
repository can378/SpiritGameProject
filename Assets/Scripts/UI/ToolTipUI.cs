using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;


public class ToolTipUI : MonoBehaviour
{

    // nearObject
    [Header("���� ����")]
    [Tooltip("������ �̸�")]
    public TMP_Text ToolTipNameText;
    [Tooltip("������ ��ġ : ���� �Ǵ� ��ų�� ���ط�, ���� ���� ��")]
    public TMP_Text ToolTipNum;
    [Tooltip("������ ��ġ : ���� �Ǵ� ��ų�� ���ط�, ���� ���� ��")]
    public TMP_Text ToolTipNumText;
    [Tooltip("������ �з� ")]
    public TMP_Text ToolTipType;
    [Tooltip("������ �з� : ���� �Ǵ� ��ų�� �з�, ���� ��")]
    public TMP_Text ToolTipTypeText;
    [Tooltip("������ ����")]
    public TMP_Text ToolTipDescriptionText;
    SelectItem ToolTipCurItem;

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
            ToolTipNum.text = "���ݷ�";
            ToolTipNumText.text = weapon.weaponData.attackPower.ToString();
            ToolTipType.text = "�з�";
            ToolTipTypeText.text = weapon.weaponData.TypeToKorean();
        }
        else if (ToolTipCurItem is SkillItem skill)
        {
            ToolTipNum.text = skill.skillData.Update_Num();
            ToolTipNumText.text = skill.skillData.Update_NumText(Player.instance.playerStats);
            ToolTipType.text = "�з�";
            ToolTipTypeText.text = skill.skillData.TypeToKorean();
        }
        else
        {
            if (ToolTipCurItem.itemData.selectItemType == SelectItemType.Equipments)
            {
                ToolTipNum.text = "";
                ToolTipNumText.text = "";
                ToolTipType.text = "";
                ToolTipTypeText.text = "";
            }
            else if (ToolTipCurItem.itemData.selectItemType == SelectItemType.Consumable)
            {
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
