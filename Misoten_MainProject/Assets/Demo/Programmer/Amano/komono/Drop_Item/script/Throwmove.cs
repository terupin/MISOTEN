using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwmove : MonoBehaviour
{
    private Rigidbody rb;
    private bool isMoving = false; // 移動中フラグ

    [SerializeField, Header("待機時間")]
    public float waitTime = 2.0f;

    [SerializeField, Header("飛ばす高さ")]
    public float Tall;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndMove());   // コルーチンで待機後に移動を開始

        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(waitTime);  //指定時間待機

        Decide_Parabola();

        isMoving = true;
    }

    //ベジェ曲線を用いて放物線を描く
    public void Decide_Parabola()
    {
        //中点を求める
        Vector3 StartPoint =transform.position;
        Vector3 EndPoint =Vector3.zero;
        Vector3 CenterPoint = StartPoint - EndPoint;
    }

    //ベジェ曲線の補間
    Vector3 CalcLerpPoint(Vector3 start, Vector3 center,Vector3 end)
    {
        Vector3 a = Vector3.Lerp(start, center, Time.deltaTime);
        Vector3 b = Vector3.Lerp(center,end,Time.deltaTime);
        return Vector3.Lerp(a, b, Time.deltaTime);
    }
}
