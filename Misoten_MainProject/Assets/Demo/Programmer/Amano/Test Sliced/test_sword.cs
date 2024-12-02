using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.EventSystems;
using System.Net.Sockets;

public class test_sword : MonoBehaviour
{
    //�ؒf�ʂ̃}�e���A��
    [SerializeField,Header("�ؒf�ʂ̃}�e���A��")]
    public Material Slice_Color;

    //�ؒf����I�u�W�F�N�g�̃^�O��
    [SerializeField, Header("�؂��I�u�W�F�N�g�̃^�O")]
    public string cut_tag = "Cut";

    //���̐�[��������I�u�W�F�N�g
    [SerializeField,Header("���̐�[")]
    public Transform swordTop;

    //���̕���������I�u�W�F�N�g
    [SerializeField, Header("���̕�")]
    public Transform swordHit;

    private Vector3 startPos;  //�؂�n�߂̓��̈ʒu
    private Vector3 endPos;  //���I���̓��̈ʒu
    private Vector3 cut_ObjPos;//�؂��I�u�W�F�N�g�̃|�W�V����

    private void OnTriggerEnter(Collider other)
    {
        //�؂��I�u�W�F�N�g���H
        if (other.tag == cut_tag || other.tag == "Denchiku")
        {
            //�����������ɑ��݂��铁�̏ꏊ
            startPos = this.transform.position;
            Debug.Log("��������");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == cut_tag)
        {
            Debug.Log("�o����");

            //�؂��I�u�W�F�N�g�̃|�W�V�������擾
            cut_ObjPos = other.transform.position;

            //�o�����ɑ��݂��铁�̏ꏊ
            endPos = this.transform.position;

            //���̈ړ��x�N�g�����v�Z����
            Vector3 swordMovement = endPos - startPos;

            //���̕��Ɛ�[�̃x�N�g�����v�Z���āA���̌������擾
            Vector3 swordDirection = (swordTop.position - swordHit.position).normalized;

            //���̋O���ɐ����ȕ��ʂ��쐬
            Vector3 cutNormal =Vector3.Cross(swordMovement, swordDirection); //�O�ς̌v�Z
            cutNormal = other.transform.InverseTransformDirection(cutNormal); //���[�J�����W�ɕϊ�

            //�؂��ꏊ�̌v�Z
            Vector3 slice_pos = other.transform.InverseTransformDirection(endPos-cut_ObjPos); 
            EzySlice.Plane cutPlane = new EzySlice.Plane(slice_pos, cutNormal);  

            //EzySlice�ő�����X���C�X����
            GameObject targetObject = other.gameObject;
            SlicedHull slicedObject = targetObject.Slice(cutPlane, Slice_Color);  //��Q�����͐؂�ꂽ�f�ʂ̃}�e���A��

            if (slicedObject != null)
            {
                //�X���C�X���ꂽ�����𐶐�
                GameObject upperHull = slicedObject.CreateUpperHull(targetObject, null);
                GameObject lowerHull = slicedObject.CreateLowerHull(targetObject, null);

                //�V�������������R���|�[�l���g��ǉ�
                MakeItPhysical(upperHull);
                MakeItPhysical(lowerHull);

                //���̃I�u�W�F�N�g���폜
                Destroy(targetObject);
            }
        }
    }


    //�I�u�W�F�N�g��������MeshCollider��Rigidbody���A�^�b�`����
    private void MakeItPhysical(GameObject obj, Material mat = null)
    {
        //MeshCollider��Convex��true�ɂ��Ȃ��ƁA���蔲���Ă��܂��̂Œ���
        obj.AddComponent<MeshCollider>().convex = true;

        //Rigidbody�֌W
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.useGravity = true;

        //�؂ꂽ���̂�������x�؂��悤�ɂ��邽�߂̃^�O�t��
        obj.gameObject.tag = cut_tag;
    }

}