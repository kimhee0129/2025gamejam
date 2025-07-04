using UnityEngine;
using System.Collections;


public class Wind : MonoBehaviour
{

    public float VanishTime; // 바람이 사라지는 시간
    private Transform earthTransform;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void OnEnable()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Transform earth)
    {  
        this.earthTransform = earth;
        VanishTime = CL.Get<float>("WindVanishTime");
        StartCoroutine(VanishRoutine());
    }

    IEnumerator VanishRoutine()
    {

        //Debug.Log($"[Wind] 오브젝트 ID: {gameObject.GetInstanceID()} / 소멸 대기 시작. 대기 시간: {VanishTime}초");

        yield return new WaitForSeconds(VanishTime);


        // 바람이 사라지는 로직
        ObjPoolManager.instance.Release(gameObject, "Wind");
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
}
