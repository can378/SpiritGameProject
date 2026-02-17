using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance = null;

    public List<GameObject> prefabs;
    List<List<GameObject>> pools;

    #region Scene Load Event

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"새로운 씬 로드됨: {scene.name}");
        if (pools == null) return;

        ClearAll();
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
            Destroy(this.gameObject);
            return;
        }

        // pools 초기화
        pools = new List<List<GameObject>>();
        for (int index = 0; index < prefabs.Count; ++index)
            pools.Add(new List<GameObject>());

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // 살아남는 인스턴스만 해제
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //get object with index
    public GameObject Get(int index, Vector3 _Position = default)
    {
        GameObject select = null;

        // 최소 방어 (인덱스 실수로 크래시 방지)
        if (index < 0 || index >= prefabs.Count) return null;

        pools[index].RemoveAll(item => item == null);

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

    //get object with name
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
        select = Instantiate(prefabs[0], transform);
        pools[0].Add(select);
        select.transform.position = _Position;
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

        // �ش� ������ �߰�
        prefabs.Add(_Prefab);
        pools.Add(new List<GameObject>());

        // 
        select = Instantiate(_Prefab, transform);
        select.transform.position = _Position;

        //
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
        if (pools == null) return;
        if (index < 0 || index >= pools.Count) return;

        foreach (GameObject item in pools[index])
            if (item) item.SetActive(false);
    }


    public void ClearAll()
    {
        if (pools == null) return;
        for (int index = 0; index < pools.Count; index++)
            foreach (GameObject item in pools[index])
                if (item) item.SetActive(false);
    }



}
