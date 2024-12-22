using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwmove : MonoBehaviour
{
    private Rigidbody rb;
    private bool isMoving = false; // 移動中フラグ

    [SerializeField, Header("待機時間")]
    public float waitTime = 2.0f;

    [SerializeField, Header("動く速さ")]
    public float itemspeed = 5;

    [SerializeField, Header("飛ばす高さ")]
    public float Tall;

    private Vector3 StartPoint;
    private Vector3 EndPoint;
    private float CenterPoint;

    private float movestart;

    public float progress;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(WaitAndMove());   // コルーチンで待機後に移動を開始
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            rb.velocity = Vector3.zero;

            float elapsedTime = Time.time - movestart;  //時間のリセット
             progress = elapsedTime * itemspeed;

            if (progress > 1.0f) // 終了条件
            {
                isMoving = false;
                return;
            }

            // XZ座標の直線移動
            Vector3 currentXZ = Vector3.Lerp(StartPoint, EndPoint, progress);

            float height = Mathf.Sin(progress * Mathf.PI) * Tall;  //高さを決める

            if (height < EndPoint.y && progress > 0.5f)
                height = EndPoint.y;

            //新しい位置を設定
            transform.position = new Vector3(currentXZ.x,height,currentXZ.z);

        }

    }

    IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(waitTime);  //指定時間待機

        isMoving = true;

        //始めと最後の点を取得
        StartPoint = transform.parent.position;
        EndPoint = Vector3.zero;

        //Y軸だけ0だとめり込むので上に移動
        StartPoint.y = transform.position.y * 2;
        EndPoint.y = transform.position.y * 2;

        Debug.Log(StartPoint);
        Debug.Log(EndPoint);

        movestart = Time.time;

        Collider item_col = this.gameObject.GetComponent<Collider>();

        //トリガーの有効と重力の無効
        item_col.isTrigger = true;
        rb.useGravity = false;

    }

}
