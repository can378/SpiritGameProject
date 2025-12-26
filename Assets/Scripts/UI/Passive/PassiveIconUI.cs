using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassiveIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field:SerializeField] public PassiveData m_PassiveData {private set; get;}
    [SerializeField] UnityEngine.UI.Image m_IconImage;

    [SerializeField] bool m_IsHover = false;

    public void Init(PassiveData _PassiveData)
    {
        m_PassiveData = _PassiveData;
        m_IconImage.sprite = m_PassiveData.PSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스 오버 시 툴팁을 열기 전에 ItemInstance와 내부 ItemData가 유효한지 확인
        m_IsHover = true;
        if (m_PassiveData == null)
        {
            return;
        }

        // 툴팁 UI를 띄운다.
        /*
        if (toolTipUI != null)
        {
            toolTipUI.OpenToolTipUI(itemInstance);
            toolTipUI.ChangePosition(ToolTipUIPos.InventorySlot);
        }
        */
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_IsHover = false;
        // 툴팁UI를 닫는다.
        //toolTipUI.CloseToolTipUI();
    }

    void Oestroy()
    {
        // 툴팁 UI를 강제로 닫는다.        
    }
}
