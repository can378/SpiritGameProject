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

    public GameObject[] normalEnemyCh1;
    public GameObject[] normalEnemyCh2;
    public GameObject[] normalEnemyCh3;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            //�� ��ȯ�� �Ǿ�����, ���� ���� �ν��Ͻ��� ��� ����ϱ� ����
            //���ο� ���� ���ӿ�����Ʈ ����
            Destroy(this.gameObject);
        }
    }

}
