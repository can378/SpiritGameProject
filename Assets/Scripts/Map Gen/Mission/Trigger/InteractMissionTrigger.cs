using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMissionTrigger : MissionTriggerBase
{

    [field : SerializeField, Tooltip("상호작용 오브젝트")] GameObject m_InterObject;
    [field : SerializeField, Tooltip("상호작용 대상의 스프라이트들")] List<SpriteRenderer> m_InterSprite;

    public float disappear_time;//클수록 천천히 사라짐
    Interactable m_Interactable;

    public override MISSION_TRIGGER_TYPE GetTriggerType()
    {
        return MISSION_TRIGGER_TYPE.Interact;
    }

    public override void SetTrigger()
    {
        m_Interactable = m_InterObject.GetComponent<Interactable>();

        // 중복 등록 방지
        m_Interactable.InteractEvent -= m_Owner.StartMission;
        m_Interactable.InteractEvent -= ObjectDisapear;

        // 등록
        m_Interactable.InteractEvent += m_Owner.StartMission;
        m_Interactable.InteractEvent += ObjectDisapear;

    }

    void OnDestroy()
    {
        m_Interactable.InteractEvent -= m_Owner.StartMission;
        m_Interactable.InteractEvent -= ObjectDisapear;
    }

    void ObjectDisapear()
    {
        StartCoroutine("ObjectDisapearCoru");
    }

    IEnumerator ObjectDisapearCoru()
    {
        float alpha = disappear_time;
        
        while (alpha > 0f)
        {
            for(int i = 0; i<m_InterSprite.Count; ++i)
            {
                m_InterSprite[i].color = new Color(1f, 1f, 1f,alpha);
            }
            alpha -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        m_InterObject.SetActive(false);
        m_Interactable.InteractEvent -= m_Owner.StartMission;
        m_Interactable.InteractEvent -= ObjectDisapear;
    }
    
/*    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.OnEnterMap(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.OnExitMap(gameObject);
        }
    }*/
}


