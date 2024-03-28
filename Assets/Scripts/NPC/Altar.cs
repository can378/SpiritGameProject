using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : NPCbasic
{
    public bool check;      // 얻었으면 true;
    public List<int> table;
    
    protected override void Start()
    {
        base.Start();
        table = CombinationRandom.CombRandom(3,0,Player.instance.stats.playerStat.Length);
    }

    public override void Conversation()
    {
        base.Conversation();
        if (check)
            return;
        MapUIManager.instance.statSelectPanel.SetActive(true);
        MapUIManager.instance.UpdateStatSelectUI(table);
    }

    public override void ConversationOut()
    {
        base.ConversationOut();
        MapUIManager.instance.statSelectPanel.SetActive(false);
    }
}
