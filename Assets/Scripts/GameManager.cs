using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private UserData userData;
    public GameObject coinPrefab;
    public GameObject touchedObject;//���콺 Ŭ���� ������Ʈ
    
    //������
    public GameObject nowRoom;//���� �÷��̾ �ִ� ��
    public Room nowRoomScript;
    public EnemyTemplates enemyTemplates;
    //public GameObject canvas;

    

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

    
    void Start()
    {   
        userData = DataManager.instance.userData;
        LoadNowScene();
    }
    void Update()
    {
        MouseClick();
    }

    void LoadNowScene() 
    {
        if (userData.nowChapter == 0)
        { SceneManager.LoadScene("Main"); }
        else if (userData.nowChapter == 4)
        { SceneManager.LoadScene("FinalMap"); }
        else
        { SceneManager.LoadScene("Map"); }
    }

    

    void MouseClick() 
    {

        if (Input.GetMouseButton(0))
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPos, Camera.main.transform.forward);

            if (hit.collider != null)
            {
                touchedObject = hit.transform.gameObject;
                //print(touchedObject.name);
            }
        }

    }


    public void dropCoin(int coinCount, Vector3 pos) 
    {
        for (int i = 0; i < coinCount; i++)
        {
            GameObject c=Instantiate(coinPrefab);
            c.transform.position = pos;
        
        }
    }
}
