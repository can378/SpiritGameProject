using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private UserData playerData;
    public GameObject coinPrefab;
    GameObject touchedObject;


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
        
        //playerData = DataManager.instance.userData;
        //DataManager.instance.userData.coin += 50;


        //ScriptManager.instance.ScriptTest();
    }

   


    void Update()
    {
        MouseClick();
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
