using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField, Header("�s����̍��W")]
    public Transform target;  // �v���C���[�̍��W
    [SerializeField, Header("���̃q�b�g�{�b�N�X")]
    public Transform hitbox;  // ���̓����蔻��擾
    public float moveSpeed = 3f;  // �ړ����x
    public float rangeAngle = 30f;  // �^�[�Q�b�g�O���x�N�g���͈̔͊p�x�i�}�p�x�j
    private bool isMoving = false;  // �ړ������ǂ����̃t���O

    void Update()
    {
        if (target == null) return;

        // �^�[�Q�b�g����������
        Vector3 directionToTarget = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToTarget);

        // �^�[�Q�b�g�̑O���x�N�g�������ɃI�u�W�F�N�g�����邩���m�F
        Vector3 targetForward = target.forward;  // �^�[�Q�b�g�̑O������
        Vector3 toObject = transform.position - target.position;  // �^�[�Q�b�g���炱�̃I�u�W�F�N�g�܂ł̃x�N�g��

        // �O���x�N�g���ƃI�u�W�F�N�g�Ƃ̊p�x���v�Z
        float angle = Vector3.Angle(targetForward, toObject);

        // �w�肵���͈͓��ɃI�u�W�F�N�g������ꍇ�A�ړ����J�n
        if (angle <= rangeAngle)
        {
            // �^�[�Q�b�g�����ɃI�u�W�F�N�g������ꍇ�͈ړ����J�n
            MoveTowardsTarget();
        }
        else
        {
            // �����𖞂����Ȃ��Ȃ����ꍇ�A�ړ����~
            if (isMoving)
            {
                isMoving = false;
            }
        }
    }

    void MoveTowardsTarget()
    {
        // �ړ�����
        if (!isMoving)
        {
            isMoving = true;
        }

        // �^�[�Q�b�g�Ɍ������Ĉړ�
        Vector3 moveDirection = (target.position - transform.position).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    // Gizmos���g���Ĕ͈͂�����
    void OnDrawGizmos()
    {
        if (target == null) return;

        // �^�[�Q�b�g�̑O���x�N�g��
        Vector3 targetForward = target.forward;
        Vector3 targetPosition = target.position;

        // �͈͊p�x�̗��[���v�Z�i���E��rangeAngle��������]������j
        Quaternion leftRotation = Quaternion.AngleAxis(-rangeAngle, Vector3.up);
        Quaternion rightRotation = Quaternion.AngleAxis(rangeAngle, Vector3.up);

        // �͈͊p�x�������o�����邽�߂ɉ~�ʂ�`��
        Gizmos.color = Color.green;  // �͈͂�΂ŕ\��
        float arcLength = 10f;  // �͈͂�`�悷�钷��
        Vector3 arcStart = targetPosition + leftRotation * targetForward * arcLength;
        Vector3 arcEnd = targetPosition + rightRotation * targetForward * arcLength;

        Gizmos.DrawLine(targetPosition, arcStart);  // �����͈̔�
        Gizmos.DrawLine(targetPosition, arcEnd);    // �E���͈̔�

        // �͈͓��ɂ���ꍇ�͑ΏۃI�u�W�F�N�g������
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);  // �ΏۃI�u�W�F�N�g��Ԃŕ\��
    }

    // �q�b�g�{�b�N�X�ɐڐG�����ꍇ�ɃI�u�W�F�N�g���폜
    private void OnTriggerEnter(Collider other)
    {
        // �q�b�g�{�b�N�X�ƐڐG�����ꍇ�ɍ폜
        if (other.transform == hitbox)
        {
            Destroy(gameObject);  // ���̃I�u�W�F�N�g���폜
        }
    }

    // OnCollisionEnter���g�p����ꍇ�̕��@
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.transform == hitbox)
    //     {
    //         Destroy(gameObject);  // ���̃I�u�W�F�N�g���폜
    //     }
    // }
}
