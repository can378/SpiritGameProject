using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDungeon : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
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
                MapUIManager.instance.endPanel.SetActive(true);
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
