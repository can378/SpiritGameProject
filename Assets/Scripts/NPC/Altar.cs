using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : NPCbasic
{
    public override void Conversation()
    {
        base.Conversation();
        MapUIManager.instance.statSelectPanel.SetActive(true);
    }

    public override void ConversationOut()
    {
        base.ConversationOut();
        MapUIManager.instance.statSelectPanel.SetActive(false);
    }
}
