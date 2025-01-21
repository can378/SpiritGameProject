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

    private float deltaTime = 0f;

    [SerializeField] private int size = 25;
    [SerializeField] private Color color = Color.red;

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

        AudioManager.instance.Bgm_normal(userData.nowChapter);
        LoadNowScene();
    }
    void Update()
    {
        MouseClick();
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(30, 30, Screen.width, Screen.height);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = size;
        style.normal.textColor = color;

        float ms = deltaTime * 1000f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, ms);

        GUI.Label(rect, text, style);
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
