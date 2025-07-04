using UnityEngine;
using System.Collections;


public class Wind : MonoBehaviour
{

    public float VanishTime; // �ٶ��� ������� �ð�
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

        //Debug.Log($"[Wind] ������Ʈ ID: {gameObject.GetInstanceID()} / �Ҹ� ��� ����. ��� �ð�: {VanishTime}��");

        yield return new WaitForSeconds(VanishTime);


        // �ٶ��� ������� ����
        ObjPoolManager.instance.Release(gameObject, "Wind");
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
}
