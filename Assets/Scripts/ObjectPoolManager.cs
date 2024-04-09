using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.Progress;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance = null;

    public GameObject[] prefabs;
    List<GameObject>[] pools;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            //�� ��ȯ�� �Ǿ�����, ���� ���� �ν��Ͻ��� ��� ����ϱ� ����
            //���ο� ���� ���ӿ�����Ʈ ����
            Destroy(this.gameObject);
        }


        //pools �ʱ�ȭ
        pools = new List<GameObject>[prefabs.Length];
        for (int index = 0; index < pools.Length; index++)
            pools[index] = new List<GameObject>();
    }

    public GameObject Get(int index)//������Ʈ �̸����� ã�°ŷ� �ٲܱ�?
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }

    public GameObject Get2(string name) 
    {
        GameObject select = null;

        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].name == name)
            {
                select = prefabs[i];
                select.SetActive(true);
                select = Instantiate(prefabs[i], transform);
                pools[i].Add(select);

                return select;

            }    
        
        }

        return prefabs[0];
    }



    public void Clear(int index)
    {
        foreach (GameObject item in pools[index])
            item.SetActive(false);
    }


    public void ClearAll()
    {
        for (int index = 0; index < pools.Length; index++)
            foreach (GameObject item in pools[index])
                item.SetActive(false);
    }



}
