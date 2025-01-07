using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [Header("���������� �ִ� ������Ʈ")]
    public List<ObjectBasic> inObject = new List<ObjectBasic>();      // Safe ���� �ִ� ������Ʈ��

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            ObjectBasic objectBasic = collision.GetComponentInParent<ObjectBasic>();

            // ���� �ȿ� ���� ������Ʈ �߰�
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
