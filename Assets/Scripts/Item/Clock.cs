using System.Collections;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform hourHand;


    /// 12�� ���� ���⿡�� �����ؼ� rotationDuration���� �ð� �������� �� ���� ȸ��
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
            hourHand.rotation = Quaternion.Euler(0f, 0f, -currentAngle); // �ð���� ȸ��

            yield return null;
        }

        // set end angle(360)
        hourHand.rotation = Quaternion.Euler(0f, 0f, -endAngle);
    }
}
