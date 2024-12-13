using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class TestSword : MonoBehaviour
{
    // 切断するオブジェクトのタグ名
    [SerializeField, Header("切れるオブジェクトのタグ")]
    public string cut_tag = "Cut";

    // 刀の先端を示す空オブジェクト
    [SerializeField, Header("刀の先端")]
    public Transform swordTop;

    // 刀の柄を示す空オブジェクト
    [SerializeField, Header("刀の柄")]
    public Transform swordHit;

    // 切れた破片の存在する秒数
    [SerializeField, Header("破片が消えるまでの秒数")]
    public float lifetime = 5.0f;

    [SerializeField, Header("切断時に再生する音声")]
    public AudioClip AudioClip_Slash;
    private AudioSource audioSource_S;

    private Vector3 startPos; // 切り始めの刀の位置
    private Vector3 endPos;   // 切り終わりの刀の位置

    private void OnTriggerEnter(Collider other)
    {
        // 切れるオブジェクトかどうか確認
        if (other.CompareTag(cut_tag))
        {
            startPos = this.transform.position;
            Debug.Log("当たった");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        audioSource_S = GetComponent<AudioSource>();
        // 切れるオブジェクトかどうか確認
        if (other.CompareTag(cut_tag))
        {
            Debug.Log("出たよ");

            // 切れるオブジェクトの中心を取得
            Vector3 objectCenter = other.transform.position;

            // 出た時に存在する刀の場所
            endPos = this.transform.position;

            // 剣の移動ベクトルを計算する
            Vector3 swordMovement = endPos - startPos;

            // 剣の柄と先端のベクトルを計算して、剣の向きを取得
            Vector3 swordDirection = (swordTop.position - swordHit.position).normalized;

            // 剣の軌道に垂直な平面を作成
            Vector3 cutNormal = Vector3.Cross(swordMovement, swordDirection).normalized; // 外積の計算と正規化
            cutNormal = other.transform.InverseTransformDirection(cutNormal); // ローカル座標に変換

            // 切れる場所をオブジェクトの中心に設定
            Vector3 slice_pos = other.transform.InverseTransformPoint(objectCenter);
            EzySlice.Plane cutPlane = new EzySlice.Plane(slice_pos, cutNormal);

            // EzySliceで対象をスライスする
            GameObject targetObject = other.gameObject;
            Material cuttingMaterial = targetObject.GetComponent<Dropitem_cutmaterial>().cut_materal;
            SlicedHull slicedObject = targetObject.Slice(cutPlane, cuttingMaterial); // 第2引数は切断面のマテリアル

            if (slicedObject != null)
            {
                // スライスされた部分を生成
                GameObject upperHull = slicedObject.CreateUpperHull(targetObject, cuttingMaterial);
                GameObject lowerHull = slicedObject.CreateLowerHull(targetObject, cuttingMaterial);

                // 新しい部分に物理コンポーネントを追加
                MakeItPhysical(upperHull, other.transform);
                MakeItPhysical(lowerHull, other.transform);

                targetObject.GetComponent<Dropitem_cutmaterial>().Set_itemdrop();
                targetObject.GetComponent<Dropitem_cutmaterial>().CreateItem();

                // 元のオブジェクトを削除
                Destroy(targetObject);
                audioSource_S.PlayOneShot(AudioClip_Slash);
            }
        }
    }

    // オブジェクト生成時にMeshColliderとRigidbodyをアタッチする
    private void MakeItPhysical(GameObject obj, Transform parent)
    {
        // MeshColliderのConvexをtrueに設定
        var collider = obj.AddComponent<MeshCollider>();
        collider.convex = true;

        // Rigidbodyを設定
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.useGravity = true;

        // オブジェクトの位置を親オブジェクトに合わせる
        obj.transform.position = parent.position;
        obj.transform.localScale = parent.lossyScale;

        // 指定秒数後に削除
        Destroy(obj, lifetime);

    }
}
