using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class test_makesliced : MonoBehaviour
{
    //切断面の色
    public Material Slice_Color;

    //切断するLayer
    public LayerMask Slice_Mask;


    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown("space")))
        {

            Collider[] objectsToSlice = Physics.OverlapBox(transform.position, new Vector3(1.0f, 0.1f, 0.1f), transform.rotation, Slice_Mask);

            //全コライダーごとに切断する
            foreach (Collider objectToSlice in objectsToSlice)
            {
                //元オブジェクトの切断
                SlicedHull slicedObject = SliceObject(objectToSlice.gameObject, Slice_Color);

                //上面側のオブジェクトの生成
                GameObject upperHullGameObject = slicedObject.CreateUpperHull(objectToSlice.GetComponent<Collider>().gameObject, Slice_Color);
                MakeItPhysical(upperHullGameObject);
                Change_LayerNumber(upperHullGameObject);


                //下面側のオブジェクトの生成
                GameObject lowHullGameObject = slicedObject.CreateLowerHull(objectToSlice.GetComponent<Collider>().gameObject, Slice_Color);
                MakeItPhysical(lowHullGameObject);
                Change_LayerNumber(lowHullGameObject);


                //元オブジェクトの削除
                Destroy(objectToSlice.gameObject);
            }
        }
    }

    //切断時に生成するオブジェクトを返す
    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        //Ezy-Sliceフレームワークを利用してスライスをしている
        return obj.Slice(transform.position, transform.up, crossSectionMaterial);
    }

    //オブジェクト生成時にMeshColliderとRigidbodyをアタッチする
    private void   MakeItPhysical(GameObject obj,Material mat =null)
    {
        //MeshColliderのConvexをtrueにしないと、すり抜けてしまうので注意
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();

    }

    private void Change_LayerNumber(GameObject obj)
    {
        int layerNumber = Mathf.RoundToInt(Mathf.Log(Slice_Mask.value, 2));
        obj.layer = layerNumber;
    }

}



