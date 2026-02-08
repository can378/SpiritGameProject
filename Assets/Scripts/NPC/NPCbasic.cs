using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class NPCbasic : MonoBehaviour, Interactable
{
    [field: SerializeField] public ActionDescription m_ActionDescription { get; private set; }
    public bool isTalking;

    public GameObject DialogPanel;
    //public TMP_Text DialogTextMesh;
    public int chapter = 0;
    public int side = 0;    // 0 : 중립, 1 : 아군, 2 : 적군
    int index = 0;


    protected ScriptManager scriptManager;

    public event System.Action InteractEvent;

    void Awake()
    {
        scriptManager = GetComponent<ScriptManager>();
    }

    #region Interaction

    public string GetInteractText()
    {
        return m_ActionDescription.m_Description;
    }

    //대화
    public virtual void Interact()
    {
        isTalking = true;
        if (DialogPanel==null) { return;}
        
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

        InteractEvent?.Invoke();
    }

    public virtual void ConversationOut()
    {
        isTalking = false;
        if (DialogPanel) { DialogPanel.SetActive(false); }
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
