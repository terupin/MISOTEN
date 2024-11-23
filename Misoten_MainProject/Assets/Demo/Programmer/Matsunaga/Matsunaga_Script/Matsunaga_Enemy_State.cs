using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsunaga_Enemy_State : MonoBehaviour
{
    public enum Enemy_State_
    {
        Idle,
        Walk,
        Tategiri,
        RenGeki_First,
        RenGeki_Second,
        Ukenagasare,
        Cooldown
    };

    private Enemy_State_ E_State;

    [SerializeField, Header("�^�[�Q�b�g�ƂȂ�v���C���[")]
    public GameObject Target_P;

    [SerializeField, Header("�T�[�`�˒�(10)")]
    public float SearchLength = 10;

    [SerializeField, Header("�U���˒�(3.5)")]
    public float AttackLength = 3.5f;

    [SerializeField, Header("�ړ��X�s�[�h(12)")]
    public float MoveSpeed = 12;

    [SerializeField, Header("�c�؂�U���m��(%)"), Range(0, 100)]
    public int TategiriChance = 60;

    [SerializeField, Header("�A���U���m��(%)"), Range(0, 100)]
    public int RenGekiChance = 40;

    private float P_E_Length;

    public Animator E01Anim;

    private float StateTime = 2.5f;
    private float StateCurrentTime;

    [SerializeField, Header("�N�[���_�E������")]
    private float CooldownTime = 2.5f;

    [Header("�ϋv�t�B�[���h�̃I�u�W�F�N�g")]
    public GameObject durabilityFieldPrefab;

    [Header("�ϋv�t�B�[���h�𐶐�������W")]
    public Vector3[] fieldPositions;

    private float currentHP;
    private bool hasUsedDurabilityField75 = false;
    private bool hasUsedDurabilityField50 = false;
    private bool hasUsedDurabilityField25 = false;

    [Header("�o���A�I�u�W�F�N�g")]
    public GameObject barrierPrefab;

    private float elapsedTime = 0f;

    private void Start()
    {
        E_State = Enemy_State_.Idle;
        StateCurrentTime = 0.0f;
        currentHP = 100f;
        elapsedTime = 0f;
        E01Anim.SetBool("Idle", true);
    }

    private void Update()
    {
        if (Target_P == null)
        {
            Debug.LogWarning("Target_P ���ݒ肳��Ă��܂���I");
            return;
        }

        P_E_Length = Vector3.Distance(Target_P.transform.position, gameObject.transform.position);
        Debug.Log($"�v���C���[�Ƃ̋���: {P_E_Length}");

        StateCurrentTime += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        if (E_State == Enemy_State_.Cooldown)
        {
            HandleCooldown();
        }

        if (E_State == Enemy_State_.Idle || E_State == Enemy_State_.Walk)
        {
            HandleMovementAndState();
        }

        if (E_State == Enemy_State_.RenGeki_First || E_State == Enemy_State_.RenGeki_Second)
        {
            HandleRenGeki();
        }

        if (E_State == Enemy_State_.Tategiri)
        {
            HandleTategiri();
        }

        HandleDurabilityField();

        UpdateAnimations();
    }


    private void SetState(Enemy_State_ newState)
    {
        E_State = newState;
        StateCurrentTime = 0.0f;
    }

    private void SpawnBarrier()
    {
        Instantiate(barrierPrefab, transform.position, Quaternion.identity);
        Debug.Log("�o���A�𐶐����܂���");
    }

    private void SpawnDurabilityField()
    {
        foreach (var position in fieldPositions)
        {
            Instantiate(durabilityFieldPrefab, position, Quaternion.identity);
            Debug.Log($"�ϋv�t�B�[���h�𐶐�: {position}");
        }
    }

    private void HandleMovementAndState()
    {
        if (StateCurrentTime >= StateTime)
        {
            StateCurrentTime = 0.0f;

            if (P_E_Length < AttackLength)
            {
                Debug.Log("�U���͈͂ɓ������̂ōU�����J�n�I");
                DecideAttackType();
            }
            else if (P_E_Length < SearchLength)
            {
                Debug.Log("�v���C���[���T�[�`�͈͓��ɂ��܂����U���͈͊O�ł��B�ړ����J�n���܂��B");
                SetState(Enemy_State_.Walk);
            }
            else
            {
                Debug.Log("�v���C���[���͈͊O�ł��B�ҋ@��Ԃɖ߂�܂��B");
                SetState(Enemy_State_.Idle);
            }
        }

        if (E_State == Enemy_State_.Walk)
        {
            if (P_E_Length > AttackLength && P_E_Length < SearchLength)
            {
                Vector3 direction = (Target_P.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);
                transform.position += direction * MoveSpeed * Time.deltaTime;
            }
        }
    }

    private void DecideAttackType()
    {
        int randomValue = Random.Range(0, 100);
        Debug.Log($"DecideAttackType: Random Value = {randomValue}, TategiriChance = {TategiriChance}");

        if (randomValue < TategiriChance)
        {
            Debug.Log("�c�؂�U����I�����܂����I");
            SetState(Enemy_State_.Tategiri);
        }
        else
        {
            Debug.Log("�A���U����I�����܂����I");
            SetState(Enemy_State_.RenGeki_First);
        }
    }

    private void HandleTategiri()
    {
        if (E_State == Enemy_State_.Tategiri)
        {
            if (IsAnimationFinished("Tategiri"))
            {
                Debug.Log("�c�؂�U�����������܂����BCooldown ��ԂɑJ�ڂ��܂��B");
                E01Anim.SetBool("Tategiri", false); // �A�j���[�V�����p�����[�^�����Z�b�g
                SetState(Enemy_State_.Cooldown);    // �N�[���_�E����Ԃ�
            }
        }
    }


    private void HandleRenGeki()
    {
        if (E_State == Enemy_State_.RenGeki_First)
        {
            if (IsAnimationFinished("RenGeki_First"))
            {
                SetState(Enemy_State_.RenGeki_Second);
            }
        }
        else if (E_State == Enemy_State_.RenGeki_Second)
        {
            if (IsAnimationFinished("RenGeki_Second"))
            {
                SetState(Enemy_State_.Cooldown);
                StateTime = CooldownTime;
            }
        }
    }

    private void HandleCooldown()
    {
        if (StateCurrentTime < CooldownTime)
        {
            return;
        }

        SetState(Enemy_State_.Idle);
        Debug.Log("�N�[���_�E�����I�����AIdle��ԂɑJ�ڂ��܂����B");
    }

    private void HandleDurabilityField()
    {
        if (currentHP <= 75f && !hasUsedDurabilityField75)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField75 = true;
        }
        else if (currentHP <= 50f && !hasUsedDurabilityField50)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField50 = true;
        }
        else if (currentHP <= 25f && !hasUsedDurabilityField25)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField25 = true;
        }
    }

    private bool IsAnimationFinished(string stateName)
    {
        AnimatorStateInfo stateInfo = E01Anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f;
    }

    private void UpdateAnimations()
    {
        E01Anim.SetBool("Walk", E_State == Enemy_State_.Walk);
        E01Anim.SetBool("Idle", E_State == Enemy_State_.Idle);
        E01Anim.SetBool("Tategiri", E_State == Enemy_State_.Tategiri);
        E01Anim.SetBool("RenGeki", E_State == Enemy_State_.RenGeki_First || E_State == Enemy_State_.RenGeki_Second);
    }
}
