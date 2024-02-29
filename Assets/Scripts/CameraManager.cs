using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    [SerializeField]
    Vector3 cameraPosition;

    [SerializeField]
    Vector2 center;                 
    public Vector2 postCenter;
    [SerializeField]
    public Vector2 mapSize;

    [SerializeField]
    float cameraMoveSpeed;


    float height;
    float width;
    Transform playerTransform;

    public bool isCameraChasing = true;

    public static CameraManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerTransform = Player.GetComponent<Transform>();


        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;

    }

    void FixedUpdate()
    {
        if(isCameraChasing) CameraChasing();
        
    }

    void CameraChasing()
    {
        transform.position = Vector3.Lerp(transform.position,
                                          playerTransform.position + cameraPosition,
                                          Time.deltaTime * cameraMoveSpeed);
        center = Vector3.Lerp(center, postCenter, Time.deltaTime * cameraMoveSpeed);

        float lx = mapSize.x - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }

    


    public void CameraMove(GameObject obj) 
    {
        transform.position = obj.transform.position;
        transform.position+= new Vector3(0,0,-10f);
    }
    public void CenterMove(GameObject obj) 
    {
        postCenter = obj.transform.position;
        center=obj.transform.position;
    }
}
