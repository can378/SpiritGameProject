using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUIController : MonoBehaviour
{
    [field: SerializeField]IStatUI[] m_StatUI;
    Player m_Player;

    void Awake()
    {
        m_Player = Player.instance;

        m_StatUI[0].SetStat(m_Player.playerStats.HPMax);
        m_StatUI[1].SetStat(m_Player.playerStats.AttackPower);
        m_StatUI[2].SetStat(m_Player.playerStats.SkillPower);
        m_StatUI[3].SetStat(m_Player.playerStats.MoveSpeed);

        m_StatUI[4].SetStat(m_Player.playerStats.AttackSpeed);
        m_StatUI[5].SetStat(m_Player.playerStats.CriticalChance);
        m_StatUI[6].SetStat(m_Player.playerStats.CriticalDamage);
        m_StatUI[7].SetStat(m_Player.playerStats.SkillCoolTime);
        m_StatUI[8].SetStat(m_Player.playerStats.DefensivePower);
    }


}
