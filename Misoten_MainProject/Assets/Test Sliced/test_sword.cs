using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.EventSystems;

public class test_sword : MonoBehaviour
{
    //�ؒf�ʂ̃}�e���A��
    public Material Slice_Color;

    //�؂����̂̃^�O�̖��O
    [SerializeField, Header("�؂��I�u�W�F�N�g�̃^�O")]
    public string cut_tag = "Cut";

    public Transform swordTop;  //���̐�[
    public Transform swordHit;  //���̕�

    public Vector3 startPos;  //�؂�n�߂̌��̈ʒu
    public Vector3 endPos;  //���I���̌��̈ʒu

    public Vector3 cutNormal; // Plane�̖@��

    void Start()
    {
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //�؂��I�u�W�F�N�g���H
        if (other.tag == cut_tag)
        {
            Debug.Log("��������");

            //�����������ɑ��݂��铁�̏ꏊ
            startPos = this.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("�o����");

        //�o�����ɑ��݂��铁�̏ꏊ
        endPos = this.transform.position;

        //���̈ړ��x�N�g�����v�Z����
        Vector3 swordMovement = endPos - startPos;

        //���̕��Ɛ�[�̃x�N�g�����v�Z���āA���̌������擾
        Vector3 swordDirection = swordTop.position - swordHit.position;

        //���̋O���ɐ����ȕ��ʂ��쐬
        Vector3 cutNormal = Vector3.Cross(swordMovement, swordDirection).normalized;
        EzySlice.Plane cutPlane = new EzySlice.Plane(cutNormal, endPos);

        //EzySlice�ő�����X���C�X����
        GameObject targetObject = other.gameObject;
        SlicedHull slicedObject = targetObject.Slice(cutPlane, Slice_Color);  //��Q�����͐؂�ꂽ�f�ʂ̃}�e���A��

        if (slicedObject != null)
        {
            //�X���C�X���ꂽ�������擾
            GameObject upperHull = slicedObject.CreateUpperHull(targetObject, null);
            GameObject lowerHull = slicedObject.CreateLowerHull(targetObject, null);

            //�V�������������R���|�[�l���g��ǉ�
            MakeItPhysical(upperHull);
            MakeItPhysical(lowerHull);

            //���̃I�u�W�F�N�g���폜
            Destroy(targetObject);
        }
    }


    //�I�u�W�F�N�g��������MeshCollider��Rigidbody���A�^�b�`����
    private void MakeItPhysical(GameObject obj, Material mat = null)
    {
        //MeshCollider��Convex��true�ɂ��Ȃ��ƁA���蔲���Ă��܂��̂Œ���
        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
    }



}