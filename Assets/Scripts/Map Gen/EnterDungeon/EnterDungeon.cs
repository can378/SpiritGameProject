using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDungeon : MonoBehaviour
{
    private Collider2D portalCollider;
    private float activateDelay = 2.0f;
    private bool canEnter = false;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canEnter) return;
        if (other.CompareTag("Player"))
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


            /*
            if (userData.nowChapter < 3)
            {

                userData.nowChapter++;
                DataManager.instance.SaveUserData();
                SceneManager.LoadScene("Map");
            }
            else if (userData.nowChapter == 3)
            {

                userData.nowChapter++;
                DataManager.instance.SaveUserData();
                SceneManager.LoadScene("FinalMap");
            }
            else if (userData.nowChapter == 4)
            {
                DataManager.instance.InitData();
                DataManager.instance.SaveUserData();
                SceneManager.LoadScene("Main");
            }
            */
        }

    }
}
