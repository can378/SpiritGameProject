using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [Header("안전영역에 있는 오브젝트")]
    public List<ObjectBasic> inObject = new List<ObjectBasic>();      // Safe 존에 있는 오브젝트들

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            ObjectBasic objectBasic = collision.GetComponentInParent<ObjectBasic>();

            // 영역 안에 들어온 오브젝트 추가
            if (!inObject.Contains(objectBasic))
            {
                inObject.Add(objectBasic);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            ObjectBasic objectBasic = collision.GetComponentInParent<ObjectBasic>();

            inObject.Remove(objectBasic);
        }
    }
}
