using System.Collections;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera targetCamera;
    private GameObject goto_player;
    private Rigidbody rb;
    public float speeeed;
    public float waitTime = 2.0f; // 待機時間
    private bool isMoving = false; // 移動中フラグ

    void Start()
    {
        targetCamera = Camera.main;
        goto_player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody>();

        // コルーチンで待機後に移動を開始
        StartCoroutine(WaitAndMove());
    }

    void Update()
    {
        Vector3 p = targetCamera.transform.position;
        p.y = transform.position.y;

        if (isMoving) // 移動中のみ実行
        {
            Goto_player(goto_player);
        }

        // LookAtの代わりに正面を調整
        Vector3 direction = (p - transform.position).normalized;
        transform.forward = -direction; // Quadの向きに合わせるため反転
    }

    void Goto_player(GameObject play)
    {
        Vector3 force = new Vector3();
        Vector3 Length = this.transform.position - play.transform.position;

        // 進む方向
        if (Length.x > 0) { force.x = -speeeed; } else { force.x = speeeed; }
        if (Length.z > 0) { force.z = -speeeed; } else { force.z = speeeed; }

        this.transform.position += force * Time.deltaTime; // Time.deltaTimeでフレーム依存を回避
    }

    IEnumerator WaitAndMove()
    {
        // 指定時間待機
        yield return new WaitForSeconds(waitTime);

        // 移動を開始
        isMoving = true;
    }
}
