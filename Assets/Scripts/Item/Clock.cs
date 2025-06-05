using System.Collections;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform hourHand;


    /// 12시 정각 방향에서 시작해서 rotationDuration동안 시계 방향으로 한 바퀴 회전
    public IEnumerator ClockStart(float rotationDuration)
    {
        float startAngle = 0f;
        float endAngle = 360f;
        float elapsed = 0f;

        // set start angle
        hourHand.rotation = Quaternion.Euler(0f, 0f, -startAngle);

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t);
            hourHand.rotation = Quaternion.Euler(0f, 0f, -currentAngle); // 시계방향 회전

            yield return null;
        }

        // set end angle(360)
        hourHand.rotation = Quaternion.Euler(0f, 0f, -endAngle);
    }
}
