using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObj_Spawn : MonoBehaviour
{
    [Header("�X�e�[�W�ɐݒu����I�u�W�F�N�g")]
    public GameObject[] SpawnObj;

    private Vector3[] SpawnPoints;

    [Header("�X�e�[�W�ɐݒu����I�u�W�F�N�g�̃X�P�[��")]
    public Vector3 SpawnObjScale;

    [Header("�~��ɐݒu����I�u�W�F�N�g�̔��a")]
    public float SpawnObjRadius ; // �~�̔��a�i�C���X�y�N�^�ŕҏW�\�j

    [Header("�z�u�J�n�p�x���炵")]
    public float SetobjRot; // �~�̔��a�i�C���X�y�N�^�ŕҏW�\�j

    // Start is called before the first frame update
    void Start()
    {
        SpawnPoints = new Vector3[SpawnObj.Length];
        SpawnpointSet();
        ObjectSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�X�|�[���ʒu��ݒ�
    private void SpawnpointSet()
    {
        // 0�x���J�n�Ƃ���360�x��6���������U���|�C���g���v�Z
        for (int i = 0; i < 6; i++)
        {
            float angleInRadians = Mathf.Deg2Rad * (i * 60+ SetobjRot); // 60�x�Ԋu�ŕ���
            float SetPosX = Mathf.Cos(angleInRadians) * SpawnObjRadius;
            float SetPosZ = Mathf.Sin(angleInRadians) * SpawnObjRadius;

            SpawnPoints[i] = new Vector3(SetPosX, 0.0f, SetPosZ);
        }
    }

    //�ݒ肵���X�|�[���ʒu�ɃI�u�W�F�N�g�𐶐�
    private void ObjectSpawn()
    {
        // ��ʂ̒��_�ɃI�u�W�F�N�g�𐶐�
        for (int i = 0; i < SpawnObj.Length; i++)
        {
            GameObject vertexObject = Instantiate(SpawnObj[i], SpawnPoints[i], Quaternion.identity, transform);
            vertexObject.transform.localScale = SpawnObjScale;

            // �e��ݒ肹���A���[���h��Ԃɔz�u
            vertexObject.transform.SetParent(null);

            vertexObject.transform.LookAt(Vector3.zero);
        }
    }
}
