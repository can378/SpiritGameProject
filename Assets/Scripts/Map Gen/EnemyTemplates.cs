using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTemplates : MonoBehaviour
{
    public static EnemyTemplates instance = null;

    public GameObject[] normalEnemy;
    public GameObject[] miniBossEnemy;
    public GameObject[] bossEnemy;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            //씬 전환이 되었을때, 이전 씬의 인스턴스를 계속 사용하기 위해
            //새로운 씬의 게임오브젝트 제거
            Destroy(this.gameObject);
        }
    }

}
