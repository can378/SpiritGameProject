using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveIconUI : MonoBehaviour
{
    [field:SerializeField] public PassiveData m_PassiveData {private set; get;}
    [SerializeField] UnityEngine.UI.Image m_IconImage;

    public void Init(PassiveData _PassiveData)
    {
        m_PassiveData = _PassiveData;
        m_IconImage.sprite = m_PassiveData.PSprite;
    }
}
