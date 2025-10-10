using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;


public class ToolTipUI : MonoBehaviour
{

    // nearObject
    [Header("툴팁 관련")]
    [Tooltip("아이템 이름")]
    public TMP_Text ToolTipNameText;
    [Tooltip("아이템 수치 : 무기 또는 스킬은 피해량, 방어구는 지울 것")]
    public TMP_Text ToolTipNum;
    [Tooltip("아이템 수치 : 무기 또는 스킬은 피해량, 방어구는 지울 것")]
    public TMP_Text ToolTipNumText;
    [Tooltip("아이템 분류 ")]
    public TMP_Text ToolTipType;
    [Tooltip("아이템 분류 : 무기 또는 스킬은 분류, 방어구는 방어구")]
    public TMP_Text ToolTipTypeText;
    [Tooltip("아이템 설명")]
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
            ToolTipNum.text = "공격력";
            ToolTipNumText.text = weapon.weaponData.attackPower.ToString();
            ToolTipType.text = "분류";
            ToolTipTypeText.text = weapon.weaponData.TypeToKorean();
        }
        else if (ToolTipCurItem is SkillItem skill)
        {
            ToolTipNum.text = skill.skillData.Update_Num();
            ToolTipNumText.text = skill.skillData.Update_NumText(Player.instance.playerStats);
            ToolTipType.text = "분류";
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
