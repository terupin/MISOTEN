using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.EventSystems;

public class test_sword : MonoBehaviour
{
    public Transform sowrdTop;  //���̐�[
    public Transform sowrdHit;  //���̕�

    public Vector3 startPos;  //�؂�n�߂̌��̈ʒu
    public Vector3 endPos;  //���I���̌��̈ʒu
    
    public LayerMask slice_Mask;  //�؂��Ώۂ̃��C���[
    private int layer_number;  //���C���[�̔ԍ����擾

    //�ؒf�ʂ̐F
    public Material Slice_Color;

    public string cut_tag="Cut";
    public Vector3 cutNormal; // Plane�̖@��

    private void Start()
    {
        //���C���[���r�b�g�}�X�N����ԍ��ɕύX����
        layer_number = Mathf.RoundToInt(Mathf.Log(slice_Mask.value, 2));
    }


    //�؂����̂ɓ���������
    private void OnTriggerEnter(Collider other)
    {
        //���肪�؂��Ώۂł��邩���m�F
        if (other.gameObject.tag==cut_tag)
        {
            Debug.Log("��������");

            //�����������ɑ��݂��铁�̏ꏊ
            startPos = sowrdTop.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("�o����");

        endPos = sowrdHit.position;

        //���̋O���Ɋ�Â��J�b�g���镽�ʂ̖@�����v�Z
        Vector3 cutNormal = Vector3.Cross(endPos - startPos, startPos);

        //�J�b�g���ʂ̍쐬
        EzySlice.Plane cutPlane=new EzySlice.Plane(startPos,cutNormal);
        DrawPlane(cutPlane);

        //EzySlice�ő�����X���C�X����
        GameObject targetObject = other.gameObject;
        SlicedHull slicedObject = targetObject.Slice(cutPlane, null);  //��Q�����͐؂�ꂽ�f�ʂ̃}�e���A��

        if (slicedObject != null)
        {
            //�X���C�X���ꂽ�������擾
            GameObject lowerHull = slicedObject.CreateLowerHull(targetObject, null);
            GameObject upperHull = slicedObject.CreateUpperHull(targetObject, null);

            //�V�������������R���|�[�l���g��ǉ�
            MakeItPhysical(lowerHull);
            MakeItPhysical(upperHull);

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


    private void DrawPlane(EzySlice.Plane plane)
    {
        // Plane�̃��b�V���𐶐�
        GameObject planeObject = new GameObject("Cut Plane");
        MeshFilter meshFilter = planeObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = planeObject.AddComponent<MeshRenderer>();

        // ���b�V���̒��_�A�O�p�`�AUV�Ȃǂ�ݒ�
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[6];

        // Plane�̒��S�_���擾�i���ʏ�̔C�ӂ̓_�j
        Vector3 origin = startPos; // ���ʂ̊J�n�ʒu���g�p
        Vector3 normal = cutNormal; // �����ł̖@���x�N�g�����g�p

        // Plane��4�̒��_���v�Z
        Vector3 right = Vector3.Cross(normal, Vector3.up).normalized;
        Vector3 up = Vector3.Cross(normal, right).normalized;

        // ���b�V���̒��_��ݒ�
        vertices[0] = origin + right * 5 + up * 5; // ����
        vertices[1] = origin + right * 5 + up * -5; // ����
        vertices[2] = origin + right * -5 + up * -5; // �E��
        vertices[3] = origin + right * -5 + up * 5; // �E��

        // �O�p�`��ݒ�
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 2;

        // ���b�V���Ƀf�[�^��K�p
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // ���b�V���̖@�����Čv�Z
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        // �ގ���ݒ�i�K�v�ɉ����ĕύX�j
        meshRenderer.material = new Material(Shader.Find("Unlit/Color")) { color = Color.red };

        // Plane�̈ʒu��ݒ�
        planeObject.transform.position = origin;
        planeObject.transform.up = normal; // Plane�̖@��������ݒ�
    }


}
