using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField, Header("行く先の座標")]
    public Transform target;  // プレイヤーの座標
    [SerializeField, Header("剣のヒットボックス")]
    public Transform hitbox;  // 剣の当たり判定取得
    public float moveSpeed = 3f;  // 移動速度
    public float rangeAngle = 30f;  // ターゲット前方ベクトルの範囲角度（±角度）
    private bool isMoving = false;  // 移動中かどうかのフラグ

    void Update()
    {
        if (target == null) return;

        // ターゲット方向を向く
        Vector3 directionToTarget = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToTarget);

        // ターゲットの前方ベクトル方向にオブジェクトがいるかを確認
        Vector3 targetForward = target.forward;  // ターゲットの前方方向
        Vector3 toObject = transform.position - target.position;  // ターゲットからこのオブジェクトまでのベクトル

        // 前方ベクトルとオブジェクトとの角度を計算
        float angle = Vector3.Angle(targetForward, toObject);

        // 指定した範囲内にオブジェクトがいる場合、移動を開始
        if (angle <= rangeAngle)
        {
            // ターゲット方向にオブジェクトがいる場合は移動を開始
            MoveTowardsTarget();
        }
        else
        {
            // 条件を満たさなくなった場合、移動を停止
            if (isMoving)
            {
                isMoving = false;
            }
        }
    }

    void MoveTowardsTarget()
    {
        // 移動する
        if (!isMoving)
        {
            isMoving = true;
        }

        // ターゲットに向かって移動
        Vector3 moveDirection = (target.position - transform.position).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    // Gizmosを使って範囲を可視化
    void OnDrawGizmos()
    {
        if (target == null) return;

        // ターゲットの前方ベクトル
        Vector3 targetForward = target.forward;
        Vector3 targetPosition = target.position;

        // 範囲角度の両端を計算（左右にrangeAngle分だけ回転させる）
        Quaternion leftRotation = Quaternion.AngleAxis(-rangeAngle, Vector3.up);
        Quaternion rightRotation = Quaternion.AngleAxis(rangeAngle, Vector3.up);

        // 範囲角度内を視覚化するために円弧を描画
        Gizmos.color = Color.green;  // 範囲を緑で表示
        float arcLength = 10f;  // 範囲を描画する長さ
        Vector3 arcStart = targetPosition + leftRotation * targetForward * arcLength;
        Vector3 arcEnd = targetPosition + rightRotation * targetForward * arcLength;

        Gizmos.DrawLine(targetPosition, arcStart);  // 左側の範囲
        Gizmos.DrawLine(targetPosition, arcEnd);    // 右側の範囲

        // 範囲内にいる場合は対象オブジェクトを可視化
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);  // 対象オブジェクトを赤で表示
    }

    // ヒットボックスに接触した場合にオブジェクトを削除
    private void OnTriggerEnter(Collider other)
    {
        // ヒットボックスと接触した場合に削除
        if (other.transform == hitbox)
        {
            Destroy(gameObject);  // このオブジェクトを削除
        }
    }

    // OnCollisionEnterを使用する場合の方法
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.transform == hitbox)
    //     {
    //         Destroy(gameObject);  // このオブジェクトを削除
    //     }
    // }
}
