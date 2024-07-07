using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class NPCbasic : MonoBehaviour
{
    public bool isTalking;

    public GameObject DialogPanel;
    //public TMP_Text DialogTextMesh;
    public int chapter = 0;
    public int side = 0;    // 0 : �߸�, 1 : �Ʊ�, 2 : ����
    int index = 0;

    protected ScriptManager scriptManager;

    void Awake()
    {
        scriptManager = GetComponent<ScriptManager>();
    }

    #region Interaction

    //��ȭ
    public virtual void Conversation()
    {
        
        DialogPanel.SetActive(true);

        if (scriptManager.NPCScript(chapter, index) == "border")
        {
            index--;
            DialogPanel.GetComponent<TMP_Text>().text = scriptManager.NPCScript(chapter, index - 1);
        }
        else if (scriptManager.NPCScript(chapter, index) == "wrong")
        {
            index++;
            DialogPanel.GetComponent<TMP_Text>().text = scriptManager.NPCScript(chapter, index + 1);
        }
        else
        {
            DialogPanel.GetComponent<TMP_Text>().text = scriptManager.NPCScript(chapter, index);
        }
        index++;
    }

    public virtual void ConversationOut()
    {
        DialogPanel.SetActive(false);
    }

    #endregion Interaction


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interaction") && DialogPanel != null)
        {
            ConversationOut();
        }
    }

}
