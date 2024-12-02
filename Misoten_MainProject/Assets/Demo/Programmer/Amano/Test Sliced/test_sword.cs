using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.EventSystems;
using System.Net.Sockets;

public class test_sword : MonoBehaviour
{
    //切断面のマテリアル
    [SerializeField,Header("切断面のマテリアル")]
    public Material Slice_Color;

    //切断するオブジェクトのタグ名
    [SerializeField, Header("切れるオブジェクトのタグ")]
    public string cut_tag = "Cut";

    //刀の先端を示す空オブジェクト
    [SerializeField,Header("刀の先端")]
    public Transform swordTop;

    //刀の柄を示す空オブジェクト
    [SerializeField, Header("刀の柄")]
    public Transform swordHit;

    private Vector3 startPos;  //切り始めの刀の位置
    private Vector3 endPos;  //霧終わりの刀の位置
    private Vector3 cut_ObjPos;//切れるオブジェクトのポジション

    private void OnTriggerEnter(Collider other)
    {
        //切れるオブジェクトか？
        if (other.tag == cut_tag || other.tag == "Denchiku")
        {
            //当たった時に存在する刀の場所
            startPos = this.transform.position;
            Debug.Log("当たった");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == cut_tag)
        {
            Debug.Log("出たよ");

            //切れるオブジェクトのポジションを取得
            cut_ObjPos = other.transform.position;

            //出た時に存在する刀の場所
            endPos = this.transform.position;

            //剣の移動ベクトルを計算する
            Vector3 swordMovement = endPos - startPos;

            //剣の柄と先端のベクトルを計算して、剣の向きを取得
            Vector3 swordDirection = (swordTop.position - swordHit.position).normalized;

            //剣の軌道に垂直な平面を作成
            Vector3 cutNormal =Vector3.Cross(swordMovement, swordDirection); //外積の計算
            cutNormal = other.transform.InverseTransformDirection(cutNormal); //ローカル座標に変換

            //切れる場所の計算
            Vector3 slice_pos = other.transform.InverseTransformDirection(endPos-cut_ObjPos); 
            EzySlice.Plane cutPlane = new EzySlice.Plane(slice_pos, cutNormal);  

            //EzySliceで相手をスライスする
            GameObject targetObject = other.gameObject;
            SlicedHull slicedObject = targetObject.Slice(cutPlane, Slice_Color);  //第２引数は切られた断面のマテリアル

            if (slicedObject != null)
            {
                //スライスされた部分を生成
                GameObject upperHull = slicedObject.CreateUpperHull(targetObject, null);
                GameObject lowerHull = slicedObject.CreateLowerHull(targetObject, null);

                //新しい部分物理コンポーネントを追加
                MakeItPhysical(upperHull);
                MakeItPhysical(lowerHull);

                //元のオブジェクトを削除
                Destroy(targetObject);
            }
        }
    }


    //オブジェクト生成時にMeshColliderとRigidbodyをアタッチする
    private void MakeItPhysical(GameObject obj, Material mat = null)
    {
        //MeshColliderのConvexをtrueにしないと、すり抜けてしまうので注意
        obj.AddComponent<MeshCollider>().convex = true;

        //Rigidbody関係
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.useGravity = true;

        //切れたものをもう一度切れるようにするためのタグ付け
        obj.gameObject.tag = cut_tag;
    }

}