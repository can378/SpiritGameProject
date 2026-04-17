using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field :SerializeField] public ToolTipUI toolTipUI { get; private set; }
    [field: SerializeField] public ItemInstance itemInstance { get; private set; } = null;
    [field: SerializeField] public Image itemImage { get; private set; }

    [field: SerializeField] public bool isHover { get; private set; }

    private void Awake()
    {
        if (itemInstance == null || !itemInstance.IsValid())
            itemImage.enabled = false;
    }

    public void SetItemInstance(ItemInstance _itemInstance = null)
    {
        // 안전한 null 체크: ItemInstance 자체뿐 아니라 내부의 ScriptableObject(ItemData)도 검사
        if (_itemInstance == null)
        {
            itemInstance.itemData = null;
            itemImage.sprite = null;
            itemImage.enabled = false;

            if (isHover)
            {
                toolTipUI.OpenToolTipUI(itemInstance);
                toolTipUI.ChangePosition(ToolTipUIPos.InventorySlot);
            }

            return;
        }

        itemInstance = _itemInstance;
        itemImage.sprite = itemInstance.itemData.sprite;
        itemImage.enabled = true;
        itemImage.preserveAspect = true;

        if (isHover)
        {
            toolTipUI.OpenToolTipUI(itemInstance);
            toolTipUI.ChangePosition(ToolTipUIPos.InventorySlot);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스 오버 시 툴팁을 열기 전에 ItemInstance와 내부 ItemData가 유효한지 확인
        isHover = true;
        if (!itemInstance.IsValid())
        {
            return;
        }

        if (toolTipUI != null)
        {
            toolTipUI.OpenToolTipUI(itemInstance);
            toolTipUI.ChangePosition(ToolTipUIPos.InventorySlot);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
        toolTipUI.CloseToolTipUI();
    }

    void OnDrawGizmos()
    {
        RectTransform rect = GetComponent<RectTransform>();

        // World 좌표로 변환된 네 꼭짓점 얻기
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        // 색상 선택 (커서가 올라갔을 때와 아닐 때 구분)
        Gizmos.color = new Color(1, 1, 0, 0.5f);

        // 사각형 채우기
        Gizmos.DrawCube(rect.position, rect.rect.size * rect.lossyScale);
    }
}
