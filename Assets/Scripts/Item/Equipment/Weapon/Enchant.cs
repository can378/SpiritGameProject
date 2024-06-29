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
        if(index == 11)
            Thunderbolt(other.transform.position);
    }

    void OnDestroy()
    {
        if(index == 21)
            Explosion();
    }

    #region Effect

    //11
    void Thunderbolt(Vector3 pos)
    {
        Debug.Log("Crack");
        GameObject thunderboltGO = Instantiate(ObjectPoolManager.instance.prefabs[12], pos, gameObject.transform.rotation);
        thunderboltGO.GetComponent<HitDetection>().SetHitDetection(false, -1, false, -1, 5 + this.hitDetection.user.GetComponent<Player>().playerStats.skillPower * 0.1f, 0, 0, 0, null);
        Destroy(thunderboltGO, 0.1f);
    }

    //21
    void Explosion()
    {
        if(!hitDetection.isProjectile)
            return;

        Debug.Log("Bomb");
        GameObject explosionGO = Instantiate(ObjectPoolManager.instance.prefabs[11], gameObject.transform.position, gameObject.transform.rotation);
        explosionGO.GetComponent<HitDetection>().SetHitDetection(false, -1, false, -1, 20f, 0, 0, 0, null);
        Destroy(explosionGO, 0.5f);
    }

    #endregion Effect


}
