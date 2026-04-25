using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환용
using TMPro; // TextMeshPro 사용 시 필요

public class EndingScroll : MonoBehaviour
{
    [Header("설정")]
    public float scrollSpeed = 60f;     // 올라가는 속도
    public float endPositionY = 1500f;  // 이 좌표까지 올라가면 종료 (인스펙터에서 조절)
    //public string nextSceneName = "MainMenu"; // 종료 후 이동할 씬 이름

    [Header("입력")]
    public bool canSkip = true;         // 아무 키나 눌러서 스킵 가능하게 할지

    private bool isFinished = false;

    void Update()
    {
        if (isFinished) return;

        // 1. 위로 이동
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

        // 2. 종료 체크 (설정한 위치보다 높이 올라갔을 때)
        if (transform.localPosition.y >= endPositionY)
        {
            Finish();
        }

        // 3. 스킵 체크
        if (canSkip && Input.anyKeyDown)
        {
            Finish();
        }
    }

    void Finish()
    {
        isFinished = true;
        Debug.Log("엔딩 크레딧 종료");

        // 다음 씬으로 이동 (씬 이름이 정확해야 합니다)
        //SceneManager.LoadScene(nextSceneName);
    }
}