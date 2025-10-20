using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field :SerializeField] public ToolTipUI toolTipUI { get; private set; }
    [field: SerializeField] public ItemInstance itemInstance { get; private set; }
    [field: SerializeField] public Image itemImage { get; private set; }

    public void SetItemData(ItemInstance _itemInstance = null)
    {
        itemInstance = _itemInstance;
        if (itemInstance == null)
            return;
        itemImage.sprite = _itemInstance.itemData.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInstance == null)
            return;
        toolTipUI.OpenToolTipUI(itemInstance);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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
