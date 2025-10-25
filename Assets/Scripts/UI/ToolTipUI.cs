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
            ToolTipNum.text = "���ݷ�";
            ToolTipNumText.text = weapon.weaponData.attackPower.ToString();
            ToolTipType.text = "�з�";
            ToolTipTypeText.text = weapon.weaponData.TypeToKorean();
        }
        else if (ToolTipCurItem is SkillInstance skill)
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

    public void ChangePosition(ToolTipUIPos _pos)
    {
        toolTipUIPos = _pos;
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = LeftTopPos[(int)_pos];
    }
}
