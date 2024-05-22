using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enchant : MonoBehaviour
{
    // �⺻ ���� ���� Ư�� ȿ�� ��ũ��Ʈ
    // �켱�� �÷��̾� ���� ��ũ��Ʈ
    // ��æƮ�� �ݵ�� �Ѱ��� ����

    [field: SerializeField] public int index { get; set; }

    HitDetection hitDetection;
    /*
    0 Ư�� ȿ�� ����

    1 ~ 10  �����̻� �߰�

    11 ~ 20 ���� Ư�� ȿ��
    11 Ÿ�� ���� �� ����

    21 ~ 30 ���Ÿ� ���� Ư�� ȿ��
    21 �ı� �� ����

    */
    void Awake() 
    {
        hitDetection = GetComponent<HitDetection>();
    }

    public void SetEnchant(int index)
    {
        this.index = index;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Thunderbolt();
    }

    void OnDestroy()
    {
        if(index == 21)
            Explosion();
    }

    #region Effect

    //11
    void Thunderbolt()
    {
        Debug.Log("Crack");
        //Destroy(Instantiate(explosionField, gameObject.transform.position, gameObject.transform.rotation), explosionTime);
    }

    //21
    void Explosion()
    {
        Debug.Log("Bomb");
        //Destroy(Instantiate(explosionField, gameObject.transform.position, gameObject.transform.rotation), explosionTime);
    }

    #endregion Effect


}
