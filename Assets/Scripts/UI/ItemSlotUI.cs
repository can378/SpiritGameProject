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

    public void SetItemInstance(ItemInstance _itemInstance = null)
    {
        // ������ null üũ: ItemInstance ��ü�� �ƴ϶� ������ ScriptableObject(ItemData)�� �˻�
        if (_itemInstance == null)
        {
            itemInstance.itemData = null;
            itemImage.sprite = null;
            if (isHover)
            {
                toolTipUI.OpenToolTipUI(itemInstance);
                toolTipUI.ChangePosition(ToolTipUIPos.InventorySlot);
            }
            return;
        }
        itemInstance = _itemInstance;
        itemImage.sprite = itemInstance.itemData.sprite;

        if(isHover)
        {
            toolTipUI.OpenToolTipUI(itemInstance);
            toolTipUI.ChangePosition(ToolTipUIPos.InventorySlot);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���콺 ���� �� ������ ���� ���� ItemInstance�� ���� ItemData�� ��ȿ���� Ȯ��
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

        // World ��ǥ�� ��ȯ�� �� ������ ���
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);

        // ���� ���� (Ŀ���� �ö��� ���� �ƴ� �� ����)
        Gizmos.color = new Color(1, 1, 0, 0.5f);

        // �簢�� ä���
        Gizmos.DrawCube(rect.position, rect.rect.size * rect.lossyScale);
    }
}
