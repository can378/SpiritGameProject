using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //dot damage ����
            print("damage to player");
            //���� ������ �����Ǵ��� Ȯ���ϱ�
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //dot damage �ߴ�
            print("damage stop");
        }
    }
}
