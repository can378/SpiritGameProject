using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [Header("안전 영역 리스트")]
    [field: SerializeField]
    List<SafeArea> safeAreas = new List<SafeArea>();
    Dictionary<ObjectBasic, bool> inAreaSafe = new Dictionary<ObjectBasic, bool>();      // 영역 안에 있는 오브젝트 리스트 그리고 안전 여부

    void OnEnable()
    {
        StartCoroutine(Curse());
    }

    // 활성화되는 동안은 지속적으로 저주로 인한 피해
    // 최적화는 우선 개한테 있음
    IEnumerator Curse()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.2f);

            // Safe 영역 안에 있는 오브젝트가 Curse 영역 안에도 있는지 확인하고 있다면 true 체크
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

            // 영역 안에 들어온 오브젝트 추가
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

            // 영역에서 나간 오브젝트 제거
            if (inAreaSafe.ContainsKey(objectBasic))
            {
                inAreaSafe.Remove(objectBasic);
            }
        }
    }

}
