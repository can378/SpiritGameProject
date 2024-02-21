using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float rotationDur; // �� ������ ���� �� �ɸ��� �ð� (��)
    public Transform hourHand;

    
    public IEnumerator ClockStart(float rotationDuration)
    {
        while (true)
        {
            // �ʱ� ����
            float startHourAngle = hourHand.rotation.eulerAngles.z;
            // ��ǥ ����
            float targetHourAngle = startHourAngle + 360f;
            // ȸ�� ���� �ð�
            float startTime = Time.time;


            while (Time.time - startTime < rotationDuration)
            {
                float t = (Time.time - startTime) / rotationDuration;
                float currentHourAngle = Mathf.Lerp(startHourAngle, targetHourAngle, t);

                // rotation
                hourHand.rotation = Quaternion.Euler(0f, 0f, -currentHourAngle);

                yield return null;
            }


            break;
        }
    }

}
