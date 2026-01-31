using System;
//using System.Collections;
using System.Collections.Generic;
//using System.Reflection;
//using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;
using UnityEngine.SceneManagement;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance = null;

    public List<GameObject> prefabs;
    List<List<GameObject>> pools;

    #region Scene Load Event

    void OnEnable()
    {
        // 씬 로드 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // 오브젝트가 파괴되거나 비활성화될 때 구독 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 로드될 때 실행될 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"새로운 씬 로드됨: {scene.name}");
        DeleteAllChildren();
    }

    // 모든 자식 오브젝트 삭제
    void DeleteAllChildren()
    {
        // 자식 제거
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 풀 초기화
        foreach (List<GameObject> pool in pools)
        {
            pool.Clear();
        }
    }

    #endregion

    void Awake()
    {
        //Singleton pattern/////////////////////
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

        //pools 초기화/////////////////////////
        pools = new List<List<GameObject>>();
        for (int index = 0; index < prefabs.Count; ++index)
            pools.Add(new List<GameObject>());
        //print(pools.Count);
    }

    //get object with index
    public GameObject Get(int index, Vector3 _Position = default)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.transform.position = _Position;

                // 만약 몬스터라면 부활 함수 호출
                EnemyBasic enemyBasic = select.GetComponent<EnemyBasic>();
                if (enemyBasic)
                {
                    enemyBasic.Revive(1.0f);
                }

                select.SetActive(true);

                return select;
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

    //get object iwth name
    public GameObject Get(string _name, Vector3 _Position = default)
    {
        GameObject select = null;
       
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].name == _name)
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


    //get object with prefab
    public GameObject Get(GameObject _Prefab, Vector3 _Position = default)
    {
        GameObject select = null;

        int PrefabIndex = prefabs.FindIndex(x => x == _Prefab);
        //print(PrefabIndex);
        if (PrefabIndex != -1)
        {
            select = Get(PrefabIndex, _Position);
            return select;
        }

        print("WARNING:there is no name!!!");

        // if Not Found _Prefab in prefabs
        PrefabIndex = prefabs.Count;

        prefabs.Add(_Prefab);
        pools.Add(new List<GameObject>());
        select = Instantiate(_Prefab, _Position, Quaternion.identity);
        pools[PrefabIndex].Add(select);
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
        for (int index = 0; index < pools.Count; index++)
            foreach (GameObject item in pools[index])
                item.SetActive(false);
    }



}
