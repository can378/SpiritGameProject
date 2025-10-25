using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;

public enum ToolTipUIPos
{
    InventorySlot,
    InGameItem,
    END
}

public class ToolTipUI : MonoBehaviour
{
    [field: SerializeField] ToolTipUIPos toolTipUIPos;
    [field: SerializeField] Vector2[] LeftTopPos = new Vector2[(int)ToolTipUIPos.END];

    // nearObject
    [Header("아이템 이름")]
    [Tooltip("아이템 이름")]
    public TMP_Text ToolTipNameText;


    [Header("아이템 정보")]
    [Header("아이템 간단정보")]
    [Tooltip("아이템 간단 정보")]   // 방어구는 비활성화
    public GameObject ToolTipSimpleInfo;
    [Tooltip("아이템 수치 : 무기 또는 스킬은 피해량")]
    public TMP_Text ToolTipNum;
    [Tooltip("아이템 수치 : 무기 또는 스킬은 피해량")]
    public TMP_Text ToolTipNumText;
    [Tooltip("아이템 기타 ")]
    public TMP_Text ToolTipType;
    [Tooltip("아이템 기타 : 무기는 분류, 스킬은 재사용 대기 시간")]
    public TMP_Text ToolTipTypeText;


    [Header("아이템 설명")]
    [Tooltip("아이템 설명")]
    public TMP_Text ToolTipDescriptionText;
    public ItemInstance ToolTipCurItem { get; private set; }

    public void OpenToolTipUI(ItemInstance _ItemInstance)
    {

        if (!_ItemInstance.IsValid())
        {
            gameObject.SetActive(false);
            return;
        }
        ToolTipCurItem = _ItemInstance;

        gameObject.SetActive(true);

        ToolTipNameText.text = ToolTipCurItem.itemData.selectItemName;
        ToolTipNameText.color = ToolTipCurItem.GetRatingColor();

        ToolTipDescriptionText.text = ToolTipCurItem.itemData.Update_Description(Player.instance.playerStats);

        if (ToolTipCurItem is WeaponInstance weapon)
        {
            ToolTipSimpleInfo.SetActive(true);
            ToolTipNum.text = "공격력";
            ToolTipNumText.text = weapon.weaponData.attackPower.ToString();
            ToolTipType.text = "분류";
            ToolTipTypeText.text = weapon.weaponData.TypeToKorean();
        }
        else if (ToolTipCurItem is SkillInstance skill)
        {
            ToolTipSimpleInfo.SetActive(true);
            ToolTipNum.text = skill.skillData.Update_Num();
            ToolTipNumText.text = skill.skillData.Update_NumText(Player.instance.playerStats);
            ToolTipType.text = "대기 시간";
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

    public void ChangePosition(ToolTipUIPos _pos)
    {
        toolTipUIPos = _pos;
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = LeftTopPos[(int)_pos];
    }
}
