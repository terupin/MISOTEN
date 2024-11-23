using System.Collections;
using UnityEngine;

public class DenchikuController : MonoBehaviour
{
    private Vector3 originalPosition; // ���̈ʒu
    private bool isReturning = false;

    private Renderer objectRenderer; // ���o�I�ω��̂���
    private Vector3 originalScale;   // ���̃X�P�[��

    [SerializeField, Header("Denchiku�̈ړ���")]
    private Vector3 targetPosition;  // �eDenchiku�p�̈ړ�����W

    private void Start()
    {
        // �����ʒu���L�^
        originalPosition = transform.localPosition;
        objectRenderer = GetComponent<Renderer>();
        originalScale = transform.localScale;
    }

    public void DetachAndMove()
    {
        if (isReturning) return; // ���ɖ߂�r���͍Ď��s���Ȃ�

        // �e�q�֌W���������Ĉړ����J�n
        transform.parent = null;

        // ���o���ʂ��J�n
        StartVisualEffect();

        // �ړ�����
        StartCoroutine(MoveToTarget(targetPosition));
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        float duration = 1.5f; // �ړ��ɂ����鎞��
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            // �X�P�[���ω��̗� (���������Ė߂�����)
            float scaleMultiplier = 1.0f + Mathf.PingPong(elapsedTime * 2, 0.5f);
            transform.localScale = originalScale * scaleMultiplier;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // ���o���ʂ��I��
        EndVisualEffect();

        // 10�b��Ɍ��̈ʒu�ɖ߂�
        yield return new WaitForSeconds(10f);
        ReturnToOriginalPosition();
    }

    private void ReturnToOriginalPosition()
    {
        StartCoroutine(MoveToOriginal());
    }

    private IEnumerator MoveToOriginal()
    {
        isReturning = true;

        float elapsedTime = 0f;
        float duration = 1.5f; // ���̈ʒu�ɖ߂鎞��
        Vector3 startPosition = transform.position;

        StartVisualEffect(); // �߂�Ƃ����G�t�F�N�g��\��

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsedTime / duration);

            // �X�P�[���ω��̗�
            float scaleMultiplier = 1.0f + Mathf.PingPong(elapsedTime * 2, 0.5f);
            transform.localScale = originalScale * scaleMultiplier;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        transform.parent = GameObject.Find("Matsunaga").transform; // ���̐e�ɖ߂�
        isReturning = false;

        EndVisualEffect();
    }

    /// <summary>
    /// ���o�I���ʂ��J�n
    /// </summary>
    private void StartVisualEffect()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.red; // �F��ԂɕύX
        }
    }

    /// <summary>
    /// ���o�I���ʂ��I��
    /// </summary>
    private void EndVisualEffect()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.white; // ���̐F�ɖ߂�
        }

        transform.localScale = originalScale; // �X�P�[�������ɖ߂�
    }

    // �O������ړ����ݒ�
    public void SetTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
    }
}
