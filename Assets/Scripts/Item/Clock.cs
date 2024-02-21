using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float rotationDur; // 한 바퀴를 도는 데 걸리는 시간 (초)
    public Transform hourHand;

    
    public IEnumerator ClockStart(float rotationDuration)
    {
        while (true)
        {
            // 초기 각도
            float startHourAngle = hourHand.rotation.eulerAngles.z;
            // 목표 각도
            float targetHourAngle = startHourAngle + 360f;
            // 회전 시작 시간
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
