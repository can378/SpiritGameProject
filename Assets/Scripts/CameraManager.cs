using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    [SerializeField]
    GameObject blindSprite;

    float height;
    float width;
    Transform playerTransform;
    Camera cam;
    float startOrtSize;



    [HideInInspector]
    public bool isCameraChasing = true;
    [HideInInspector]
    public bool isShowingBoss = false;

    public static CameraManager instance;

    private void Awake()
    {
        instance = this;
        cam = GetComponent<Camera>();
        startOrtSize = cam.orthographicSize;
    }

    void Start()
    {
        playerTransform = Player.GetComponent<Player>().CenterPivot;
    }

    void FixedUpdate()
    {
        if (isCameraChasing) CameraChasing();


        if (Player.GetComponent<PlayerStats>().blind > 0)
        {
            blindSprite.SetActive(true);
            Player.GetComponent<PlayerStats>().blind -= Time.deltaTime;
        }
        else { blindSprite.SetActive(false); }
    }

    void CameraChasing()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;

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


    //Boss room enter effect
    private bool isCameraMoving = true;
    public IEnumerator BossRoomEnterEffect(ObjectBasic boss, GameObject room)
    {
        isShowingBoss = true;
        //startSize = cam.orthographicSize;

        //boss zoom in
        StartCoroutine(CameraZoom(boss, boss, 2f, true));
        while (isCameraMoving) { yield return null; }

        //zoom out
        StartCoroutine(CameraZoom(boss, FindObj.instance.Player.GetComponent<ObjectBasic>(), 2f, false));
        while (isCameraMoving) { yield return null; }


        //Finish
        yield return new WaitForSeconds(0.5f);
        isCameraChasing = true;
        isShowingBoss = false;
    }


    //Directly move to "obj.transform.position"/////////////////////////////////////////
    public void CameraMove(GameObject obj)
    {
        isCameraChasing = false;
        transform.position = obj.transform.position + new Vector3(0, 0, -10f);
        StartCoroutine(ResumeChasingAfterDelay(0.1f));
    }

    private IEnumerator ResumeChasingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isCameraChasing = true;
    }

    public void CenterMove(GameObject obj)
    {
        postCenter = obj.transform.position;
        center = obj.transform.position;
    }
    //Slowly move to "obj"//////////////////////////////////////////////////////////////
    /*
    void CameraMoveSlowly(GameObject obj)
    {
        isCameraMoving = true;

        float duration = 2.0f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while(true)
        {
            // 경과된 시간을 갱신
            elapsedTime += Time.deltaTime;

            // 목표 위치로 Lerp
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector3.Lerp(startPosition, obj.transform.position, t);

            // 시간이 다 됐으면 이동 멈춤
            if (elapsedTime >= duration)
            {
                break;
            }
        }

        isCameraMoving = false;

    }
    */
    public IEnumerator CameraZoom(GameObject target, float duration, bool zoomIn)
    {

        isCameraMoving = true;

        float startSize = zoomIn ? startOrtSize : 2f;
        float targetSize = zoomIn ? 2f : startOrtSize; // 줌 인이면 2, 줌 아웃이면 원래 사이즈

        Vector3 startPosition = cam.transform.position;
        Vector3 targetPosition = target.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // 카메라의 orthographicSize를 Lerp로 계산
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);

            // 카메라 위치 타겟을 기준으로 이동
            if (target.CompareTag("Player") == true) { targetPosition = FindObj.instance.Player.transform.position; }
            cam.transform.position = Vector3.Lerp(startPosition, new Vector3(targetPosition.x, targetPosition.y, startPosition.z), elapsedTime / duration);

            yield return null;
        }

        // 최종 값으로 세팅
        cam.orthographicSize = targetSize;
        cam.transform.position = new Vector3(targetPosition.x, targetPosition.y, startPosition.z);

        isCameraMoving = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="targetPos"></param>
    /// <param name="duration"></param>
    /// <param name="zoomIn">true : ZoomIn, false : ZoomOut</param>
    /// <returns></returns>
    public IEnumerator CameraZoom(ObjectBasic startOB, ObjectBasic targetOB, float duration, bool zoomIn)
    {
        isCameraMoving = true;

        float startSize = zoomIn ? startOrtSize : 8f;
        float targetSize = zoomIn ? 8f : startOrtSize; // 줌 인이면 2, 줌 아웃이면 원래 사이즈

        Vector3 startPosition = startOB.CenterPivot.position;
        Vector3 targetPosition = targetOB.CenterPivot.position;

        startPosition.z = cam.transform.position.z;
        targetPosition.z = cam.transform.position.z;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // 카메라의 orthographicSize를 Lerp로 계산
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            cam.transform.position = Vector3.Lerp(startPosition, new Vector3(targetPosition.x, targetPosition.y, startPosition.z), elapsedTime / duration);

            yield return null;
        }

        // 최종 값으로 세팅
        cam.orthographicSize = targetSize;
        cam.transform.position = new Vector3(targetPosition.x, targetPosition.y, startPosition.z);

        isCameraMoving = false;
    }

}
