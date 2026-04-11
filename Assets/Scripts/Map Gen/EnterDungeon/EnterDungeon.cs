using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDungeon : MonoBehaviour, Interactable
{
    private Collider2D portalCollider;
    private float activateDelay = 2.0f;
    private bool canEnter = false;

    public ActionDescription mActionDescription;

    public event System.Action InteractEvent;

    public string GetInteractText()
    {
        return mActionDescription.m_Description;
    }

    public void Interact()
    {
        if (!canEnter) return;

        InteractEvent?.Invoke();

        Enter();
    }

    private void Awake()
    {
        portalCollider = GetComponent<Collider2D>();
        portalCollider.enabled = false;
        StartCoroutine(EnablePortal());
    }

    private IEnumerator EnablePortal()
    {
        yield return new WaitForSeconds(activateDelay);
        portalCollider.enabled = true;
        canEnter = true;
    }

    void Enter()
    {
        UserData userData = Player.instance.userData;

        AudioManager.instance.Bgm_normal(userData.nowChapter);

        if (userData.nowChapter == 0)
        {
            userData.nowChapter++;
            DataManager.instance.SavePlayerStatsToUserData();
            SceneManager.LoadScene("Map");
        }
        else
        {
            MapUIManager.instance.Ending();
        }
    }
}
