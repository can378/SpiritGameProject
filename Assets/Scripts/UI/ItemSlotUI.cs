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

        // World ��ǥ�� ��ȯ�� �� ������ ���
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        // ���� ���� (Ŀ���� �ö��� ���� �ƴ� �� ����)
        Gizmos.color = new Color(1, 1, 0, 0.5f);

        // �簢�� ä���
        Gizmos.DrawCube(rect.position, rect.rect.size * rect.lossyScale);
    }
}
