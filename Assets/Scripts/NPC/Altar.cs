using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : NPCbasic
{
    public bool check;      // 얻었으면 true;
    public List<int> table;
    public int offering;
    
    protected override void Start()
    {
        base.Start();
        table = CombinationRandom.CombRandom(3, 0, Player.instance.playerStats.playerStat.Length);
    }

    public override void Conversation()
    {
        base.Conversation();
        if (check)
            return;
        if(Player.instance.playerStats.coin < offering)
            return;

        Player.instance.playerStats.coin -= offering;
        MapUIManager.instance.statSelectPanel.SetActive(true);
        MapUIManager.instance.statSelectPanel.GetComponent<StatSelectUI>().SetStatSelectUI(this);
    }

    public override void ConversationOut()
    {
        base.ConversationOut();
        MapUIManager.instance.statSelectPanel.SetActive(false);
        MapUIManager.instance.statSelectPanel.GetComponent<StatSelectUI>().ExitStatSelectUI();
    }
}
