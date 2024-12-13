using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class TestSword : MonoBehaviour
{
    // �ؒf����I�u�W�F�N�g�̃^�O��
    [SerializeField, Header("�؂��I�u�W�F�N�g�̃^�O")]
    public string cut_tag = "Cut";

    // ���̐�[��������I�u�W�F�N�g
    [SerializeField, Header("���̐�[")]
    public Transform swordTop;

    // ���̕���������I�u�W�F�N�g
    [SerializeField, Header("���̕�")]
    public Transform swordHit;

    // �؂ꂽ�j�Ђ̑��݂���b��
    [SerializeField, Header("�j�Ђ�������܂ł̕b��")]
    public float lifetime = 5.0f;

    [SerializeField, Header("�ؒf���ɍĐ����鉹��")]
    public AudioClip AudioClip_Slash;
    private AudioSource audioSource_S;

    private Vector3 startPos; // �؂�n�߂̓��̈ʒu
    private Vector3 endPos;   // �؂�I���̓��̈ʒu

    private void OnTriggerEnter(Collider other)
    {
        // �؂��I�u�W�F�N�g���ǂ����m�F
        if (other.CompareTag(cut_tag))
        {
            startPos = this.transform.position;
            Debug.Log("��������");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        audioSource_S = GetComponent<AudioSource>();
        // �؂��I�u�W�F�N�g���ǂ����m�F
        if (other.CompareTag(cut_tag))
        {
            Debug.Log("�o����");

            // �؂��I�u�W�F�N�g�̒��S���擾
            Vector3 objectCenter = other.transform.position;

            // �o�����ɑ��݂��铁�̏ꏊ
            endPos = this.transform.position;

            // ���̈ړ��x�N�g�����v�Z����
            Vector3 swordMovement = endPos - startPos;

            // ���̕��Ɛ�[�̃x�N�g�����v�Z���āA���̌������擾
            Vector3 swordDirection = (swordTop.position - swordHit.position).normalized;

            // ���̋O���ɐ����ȕ��ʂ��쐬
            Vector3 cutNormal = Vector3.Cross(swordMovement, swordDirection).normalized; // �O�ς̌v�Z�Ɛ��K��
            cutNormal = other.transform.InverseTransformDirection(cutNormal); // ���[�J�����W�ɕϊ�

            // �؂��ꏊ���I�u�W�F�N�g�̒��S�ɐݒ�
            Vector3 slice_pos = other.transform.InverseTransformPoint(objectCenter);
            EzySlice.Plane cutPlane = new EzySlice.Plane(slice_pos, cutNormal);

            // EzySlice�őΏۂ��X���C�X����
            GameObject targetObject = other.gameObject;
            Material cuttingMaterial = targetObject.GetComponent<Dropitem_cutmaterial>().cut_materal;
            SlicedHull slicedObject = targetObject.Slice(cutPlane, cuttingMaterial); // ��2�����͐ؒf�ʂ̃}�e���A��

            if (slicedObject != null)
            {
                // �X���C�X���ꂽ�����𐶐�
                GameObject upperHull = slicedObject.CreateUpperHull(targetObject, cuttingMaterial);
                GameObject lowerHull = slicedObject.CreateLowerHull(targetObject, cuttingMaterial);

                // �V���������ɕ����R���|�[�l���g��ǉ�
                MakeItPhysical(upperHull, other.transform);
                MakeItPhysical(lowerHull, other.transform);

                targetObject.GetComponent<Dropitem_cutmaterial>().Set_itemdrop();
                targetObject.GetComponent<Dropitem_cutmaterial>().CreateItem();

                // ���̃I�u�W�F�N�g���폜
                Destroy(targetObject);
                audioSource_S.PlayOneShot(AudioClip_Slash);
            }
        }
    }

    // �I�u�W�F�N�g��������MeshCollider��Rigidbody���A�^�b�`����
    private void MakeItPhysical(GameObject obj, Transform parent)
    {
        // MeshCollider��Convex��true�ɐݒ�
        var collider = obj.AddComponent<MeshCollider>();
        collider.convex = true;

        // Rigidbody��ݒ�
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.useGravity = true;

        // �I�u�W�F�N�g�̈ʒu��e�I�u�W�F�N�g�ɍ��킹��
        obj.transform.position = parent.position;
        obj.transform.localScale = parent.lossyScale;

        // �w��b����ɍ폜
        Destroy(obj, lifetime);

    }
}
