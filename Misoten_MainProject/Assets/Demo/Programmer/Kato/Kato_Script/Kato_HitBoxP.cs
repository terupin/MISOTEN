using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_HitBoxP : MonoBehaviour
{
    [SerializeField, Header("����")]
    public GameObject WeponPoint;

    [SerializeField, Header("�����{")]
    public GameObject WeponRoot;

    public GameObject Enemy_Model;

    public static bool Tubazeri_Flg;//�󂯗����t���O 

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {


        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnCollisionExit(Collision collision)
    {
    }
}
