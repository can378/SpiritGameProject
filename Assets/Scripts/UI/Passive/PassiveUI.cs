using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveUI : MonoBehaviour
{
    [SerializeField] ObjectBasic m_ObjectBasic;

    [SerializeField] GameObject m_IconPrefab;

    [SerializeField] List<GameObject> m_Icons = new List<GameObject>();

    void Start()
    {
        m_ObjectBasic = Player.instance;
        m_ObjectBasic.PassiveApplyEvent += UpdateIcon;
        m_ObjectBasic.PassiveRemoveEvent += UpdateIcon;
    }

    // Update is called once per frame
    void UpdateIcon()
    {

        for (int i = m_Icons.Count - 1; i >= 0; i--)
        {
            GameObject icon = m_Icons[i];
            PassiveIconUI iconUI = icon.GetComponent<PassiveIconUI>();

            // 해당 패시브가 더 이상 없다면
            if (!m_ObjectBasic.FindPassive(iconUI.m_PassiveData.PID, out PassiveData passive))
            {
                m_Icons.RemoveAt(i);
                Destroy(icon);
            }
        }

        // 아이콘 추가
        // 대상에게 있는 패시브인지 확인
        foreach (PassiveData passive in m_ObjectBasic.stats.activePassive.Values)
        {
            bool isExist = false;

            foreach (GameObject icon in m_Icons)
            {
                PassiveIconUI iconUI = icon.GetComponent<PassiveIconUI>();

                // 해당 아이콘이 이미 존재한다.
                if (iconUI.m_PassiveData.PID == passive.PID)
                {
                    isExist = true;
                    break;
                }
            }

            // 해당 아이콘이 없다.
            // 해당 아이콘을 추가한다.
            if (!isExist)
            {
                GameObject icon = Instantiate(m_IconPrefab, transform);
                PassiveIconUI iconUI = icon.GetComponent<PassiveIconUI>();
                iconUI.Init(passive);

                m_Icons.Add(icon);
            }
        }
    }
}
