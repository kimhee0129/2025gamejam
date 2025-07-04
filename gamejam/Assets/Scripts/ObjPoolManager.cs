using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

/*
전체적인 구조:
    1. 시작할 때 public list로 object의 정보를 받음
    2. 그에 맞춰 각 오브젝트의 풀을 생성 (딕셔너리1: 이름-풀객체, 딕셔너리2: 이름-프리펩)
    3. 사용자 함수에서는 전역변수 (objectName)을 바꿔서 처리하는 방식을 사용용
*/
public class ObjPoolManager : MonoBehaviour
{
    public static ObjPoolManager instance;
    //public Canvas canvas; // Canvas 위에도 Instantiate 필요할 때!

    [SerializeField]
    public List<ObjectInfo> obj_info = new List<ObjectInfo>();
    private Dictionary<string, IObjectPool<GameObject>> pool_dict = new Dictionary<string, IObjectPool<GameObject>>();
    private Dictionary<string, GameObject> prefab_dict = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("two ObjPoolManager...");
            Destroy(this);
        }

        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        foreach (ObjectInfo info in obj_info)
        {
            string name = info.ObjName;
            GameObject prefab = info.prefab;

            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                () => CreateObject(prefab),
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyObject,
                false,
                info.init_capacity,
                info.init_capacity*2
            );
            pool_dict.Add(name, pool);

            prefab_dict.Add(name, prefab);
        }
    }
//------------밑으로: 풀 생성 지정 함수--------------
    private GameObject CreateObject(GameObject prefab)
    {
        if (false) // Canvas 위에서 작동하는 오브젝트는 이렇게!
        {
            //return Instantiate(prefab_dict[objectName], canvas.transform);
        }
        else
        {
            return Instantiate(prefab, transform);
        }
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    // --------------밑으로: 내가 만든 사용자 함수-------------
    public GameObject InstantiateFromPool(string name) // 풀에서 하나 꺼내오기
    {
        return pool_dict[name].Get();
    }

    public void Release(GameObject obj, string name) // 풀에 반납하기
    {
        pool_dict[name].Release(obj);
    }

    [System.Serializable]
    public class ObjectInfo
    {
        public string ObjName;
        public GameObject prefab;
        public int init_capacity;
    }
}
