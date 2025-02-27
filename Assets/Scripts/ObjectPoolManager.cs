using System;
//using System.Collections;
using System.Collections.Generic;
//using System.Reflection;
//using Unity.VisualScripting;
//using UnityEditor.SceneManagement;
using UnityEngine;
//using static UnityEditor.Progress;

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
            //씬 전환이 되었을때, 이전 씬의 인스턴스를 계속 사용하기 위해
            //새로운 씬의 게임오브젝트 제거
            Destroy(this.gameObject);
        }


        //pools 초기화
        pools = new List<GameObject>[prefabs.Length];
        for (int index = 0; index < pools.Length; index++)
            pools[index] = new List<GameObject>();
    }

    public GameObject Get(int index, Vector3 _Position = default)//get object with index
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.transform.position = _Position;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            select.transform.position = _Position;
            pools[index].Add(select);
        }

        return select;
    }

    public GameObject Get(string name, Vector3 _Position = default)//get object iwth name
    {
        GameObject select = null;
       
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].name == name)
            {
                select = Get(i, _Position);
                return select;
            }    
        
        }

        print("WARNING:there is no name!!!");
        select = Instantiate(prefabs[0]);
        pools[0].Add(select);
        return select;
    }

    public GameObject ExplosionSFX(Sprite sprite) 
    {

        GameObject select = Get("explosionSFX");
        select.GetComponent<SpriteRenderer>().sprite = sprite;
        
        return select;
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
