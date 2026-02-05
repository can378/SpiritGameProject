using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : NPCbasic
{
    public bool check;      // 얻었으면 true;
    public StatSelector[] statSelectors;
    public int offering;
    
    void Start()
    {
        List<int> table = CombinationRandom.CombRandom(3, 0, Player.instance.playerStats.playerStat.Length);
        for(int i = 0;i< statSelectors.Length; i++)
        {
            statSelectors[i].SetStatIndex((Player.StatID)table[i]);
            statSelectors[i].InteractEvent += Finish;
        }
    }

    void Finish()
    {
        check = true;
        for (int i = 0; i < statSelectors.Length; i++)
        {
            statSelectors[i].DisableSelector();
        }
    }

    public override void Interact()
    {
        
        base.Interact();
        /*
        if (check)
            return;
        if(Player.instance.playerStats.coin < offering)
            return;

        Player.instance.playerStats.coin -= offering;
        MapUIManager.instance.statSelectPanel.SetActive(true);
        MapUIManager.instance.statSelectPanel.GetComponent<StatSelectUI>().SetStatSelectUI(this);
        */
    }

    public override void ConversationOut()
    {
        base.ConversationOut();
        /*MapUIManager.instance.statSelectPanel.SetActive(false);
        MapUIManager.instance.statSelectPanel.GetComponent<StatSelectUI>().ExitStatSelectUI();*/
    }
}
