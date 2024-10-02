using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class test_makesliced : MonoBehaviour
{
    //�ؒf�ʂ̐F
    public Material Slice_Color;

    //�ؒf����Layer
    public LayerMask Slice_Mask;


    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown("space")))
        {

            Collider[] objectsToSlice = Physics.OverlapBox(transform.position, new Vector3(1.0f, 0.1f, 0.1f), transform.rotation, Slice_Mask);

            //�S�R���C�_�[���Ƃɐؒf����
            foreach (Collider objectToSlice in objectsToSlice)
            {
                //���I�u�W�F�N�g�̐ؒf
                SlicedHull slicedObject = SliceObject(objectToSlice.gameObject, Slice_Color);

                //��ʑ��̃I�u�W�F�N�g�̐���
                GameObject upperHullGameObject = slicedObject.CreateUpperHull(objectToSlice.GetComponent<Collider>().gameObject, Slice_Color);
                MakeItPhysical(upperHullGameObject);
                Change_LayerNumber(upperHullGameObject);


                //���ʑ��̃I�u�W�F�N�g�̐���
                GameObject lowHullGameObject = slicedObject.CreateLowerHull(objectToSlice.GetComponent<Collider>().gameObject, Slice_Color);
                MakeItPhysical(lowHullGameObject);
                Change_LayerNumber(lowHullGameObject);


                //���I�u�W�F�N�g�̍폜
                Destroy(objectToSlice.gameObject);
            }
        }
    }

    //�ؒf���ɐ�������I�u�W�F�N�g��Ԃ�
    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        //Ezy-Slice�t���[�����[�N�𗘗p���ăX���C�X�����Ă���
        return obj.Slice(transform.position, transform.up, crossSectionMaterial);
    }

    //�I�u�W�F�N�g��������MeshCollider��Rigidbody���A�^�b�`����
    private void   MakeItPhysical(GameObject obj,Material mat =null)
    {
        //MeshCollider��Convex��true�ɂ��Ȃ��ƁA���蔲���Ă��܂��̂Œ���
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();

    }

    private void Change_LayerNumber(GameObject obj)
    {
        int layerNumber = Mathf.RoundToInt(Mathf.Log(Slice_Mask.value, 2));
        obj.layer = layerNumber;
    }

}



