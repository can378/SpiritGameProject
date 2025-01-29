using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [Header("���� ���� ����Ʈ")]
    [field: SerializeField]
    List<SafeArea> safeAreas = new List<SafeArea>();
    Dictionary<ObjectBasic, bool> inAreaSafe = new Dictionary<ObjectBasic, bool>();      // ���� �ȿ� �ִ� ������Ʈ ����Ʈ �׸��� ���� ����

    void OnEnable()
    {
        StartCoroutine(Curse());
    }

    // Ȱ��ȭ�Ǵ� ������ ���������� ���ַ� ���� ����
    // ����ȭ�� �켱 ������ ����
    IEnumerator Curse()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.2f);

            // Safe ���� �ȿ� �ִ� ������Ʈ�� Curse ���� �ȿ��� �ִ��� Ȯ���ϰ� �ִٸ� true üũ
            foreach (SafeArea safeArea in safeAreas)
            {
                foreach (ObjectBasic objBasic in safeArea.inObject)
                {
                    if (inAreaSafe.ContainsKey(objBasic))
                    {
                        inAreaSafe[objBasic] = true;
                    }
                }
            }

            for (int i = 0; i < inAreaSafe.Count; ++i)
            {
                KeyValuePair<ObjectBasic, bool> objectBasic = inAreaSafe.ElementAt(i);

                if (!objectBasic.Value)
                {
                    inAreaSafe[objectBasic.Key] = false;
                    objectBasic.Key.Damaged(5);
                }
                else
                    inAreaSafe[objectBasic.Key] = false;

            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            Debug.Log(collision.name);
            ObjectBasic objectBasic = collision.GetComponent<ObjectBasic>();

            // ���� �ȿ� ���� ������Ʈ �߰�
            if (!inAreaSafe.ContainsKey(objectBasic))
            {
                inAreaSafe.Add(objectBasic, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            ObjectBasic objectBasic = collision.GetComponent<ObjectBasic>();

            // �������� ���� ������Ʈ ����
            if (inAreaSafe.ContainsKey(objectBasic))
            {
                inAreaSafe.Remove(objectBasic);
            }
        }
    }

}
