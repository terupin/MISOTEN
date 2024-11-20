using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsunaga_Enemy_State : MonoBehaviour
{
    // �G�̏�Ԃ��Ǘ�����񋓌^
    public enum Enemy_State_
    {
        Idle,              // �ҋ@���
        Walk,              // �ړ����
        Tategiri,          // �c�؂�U��
        RenGeki_First,     // �A��1���
        RenGeki_Second,    // �A��2���
        Ukenagasare,       // �󂯗���
        Cooldown           // �N�[���_�E�����
    };

    private Enemy_State_ E_State; // ���݂̓G�̏��

    [SerializeField, Header("�^�[�Q�b�g�ƂȂ�v���C���[")]
    public GameObject Target_P;  // �ΏۂƂȂ�v���C���[

    [SerializeField, Header("�T�[�`�˒�(10)")]
    public float SearchLength = 10; // �v���C���[�����o����͈�

    [SerializeField, Header("�U���˒�(3.5)")]
    public float AttackLength = 3.5f; // �U�����͂��͈�

    [SerializeField, Header("�ړ��X�s�[�h(12)")]
    public float MoveSpeed = 12;  // �G�̈ړ��X�s�[�h

    [SerializeField, Header("�c�؂�U���m��(%)"), Range(0, 100)]
    public int TategiriChance = 60;  // �c�؂�U��������m��

    [SerializeField, Header("�A���U���m��(%)"), Range(0, 100)]
    public int RenGekiChance = 40;  // �A���U��������m��

    private float P_E_Length;  // �v���C���[�Ƃ̋���

    public Animator E01Anim;  // �G�̃A�j���[�^�[

    private float StateTime = 2.5f;  // ��Ԃ̌p������
    private float StateCurrentTime;  // ���݂̏�Ԏ���

    [SerializeField, Header("�N�[���_�E������")]
    private float CooldownTime = 2.5f;  // �N�[���_�E������

    [Header("�ϋv�t�B�[���h�̃I�u�W�F�N�g")]
    public GameObject durabilityFieldPrefab;  // �C���X�y�N�^�Ŏw�肷��ϋv�t�B�[���h�̃I�u�W�F�N�g

    [Header("�ϋv�t�B�[���h�𐶐�������W")]
    public Vector3[] fieldPositions; // �ϋv�t�B�[���h�𐶐�������W�̔z��

    private float currentHP;  // ���݂�HP
    private bool hasUsedDurabilityField75 = false;  // HP��75%�őϋv�t�B�[���h���g������
    private bool hasUsedDurabilityField50 = false;  // HP��50%�őϋv�t�B�[���h���g������
    private bool hasUsedDurabilityField25 = false;  // HP��25%�őϋv�t�B�[���h���g������

    private void Start()
    {
        E_State = Enemy_State_.Idle;  // ������Ԃ�Idle
        StateCurrentTime = 0.0f;  // ��Ԃ̃J�E���^�[��������
        currentHP = 75f;  // ����HP��75%�ɐݒ�
        E01Anim.SetBool("Idle", true);  // �A�j���[�^�[��Idle��Ԃ�ݒ�
    }

    private void Update()
    {
        // �^�[�Q�b�g�v���C���[���ݒ肳��Ă��Ȃ��ꍇ�̌x��
        if (Target_P == null)
        {
            Debug.LogWarning("Target_P ���ݒ肳��Ă��܂���I");
            return;
        }

        // �v���C���[�Ƃ̋������v�Z
        P_E_Length = Vector3.Distance(Target_P.transform.position, gameObject.transform.position);
        Debug.Log($"�v���C���[�Ƃ̋���: {P_E_Length}");

        // ��Ԃ��Ƃ̌o�ߎ��Ԃ𑝉�
        StateCurrentTime += Time.deltaTime;

        // �N�[���_�E����Ԃ̏ꍇ�A���Ԃ��o�߂�����Idle��Ԃɖ߂�
        if (E_State == Enemy_State_.Cooldown && StateCurrentTime >= StateTime)
        {
            SetState(Enemy_State_.Idle);
        }

        // Idle�܂���Walk��Ԃ̏ꍇ�A�ړ��������s��
        if (E_State == Enemy_State_.Idle || E_State == Enemy_State_.Walk)
        {
            HandleMovementAndState();
        }

        // �A���U���̏���
        if (E_State == Enemy_State_.RenGeki_First || E_State == Enemy_State_.RenGeki_Second)
        {
            HandleRenGeki();
        }

        // �ϋv�t�B�[���h�̎g�p����
        HandleDurabilityField();

        // �A�j���[�V�����̍X�V
        UpdateAnimations();
    }

    // �G�̏�Ԃ�ύX����
    private void SetState(Enemy_State_ newState)
    {
        E_State = newState;
        StateCurrentTime = 0.0f;  // ��ԑJ�ڎ��ɃJ�E���^�[�����Z�b�g
    }

    // �ϋv�t�B�[���h�̎g�p���菈��
    private void HandleDurabilityField()
    {
        // HP��75%�ȉ��ŁA�܂��ϋv�t�B�[���h���g�p���Ă��Ȃ��ꍇ
        if (currentHP <= 75f && !hasUsedDurabilityField75)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField75 = true;  // �t���O�𗧂Ă�
        }
        // HP��50%�ȉ��ŁA�܂��ϋv�t�B�[���h���g�p���Ă��Ȃ��ꍇ
        else if (currentHP <= 50f && !hasUsedDurabilityField50)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField50 = true;  // �t���O�𗧂Ă�
        }
        // HP��25%�ȉ��ŁA�܂��ϋv�t�B�[���h���g�p���Ă��Ȃ��ꍇ
        else if (currentHP <= 25f && !hasUsedDurabilityField25)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField25 = true;  // �t���O�𗧂Ă�
        }
    }

    // �ϋv�t�B�[���h���w�肳�ꂽ�ʒu�ɐ�������
    private void SpawnDurabilityField()
    {
        foreach (var position in fieldPositions)
        {
            Instantiate(durabilityFieldPrefab, position, Quaternion.identity);  // �w����W�ɑϋv�t�B�[���h�𐶐�
            Debug.Log($"�ϋv�t�B�[���h�𐶐�: {position}");
        }
    }

    // �ړ��Ə�ԑJ�ڂ̏���
    private void HandleMovementAndState()
    {
        if (StateCurrentTime >= StateTime)  // �o�ߎ��Ԃ���Ԃ̎������Ԃ𒴂����ꍇ
        {
            StateCurrentTime = 0.0f;  // �J�E���^�[���Z�b�g

            // �v���C���[���U���͈͓��ɂ���ꍇ�A�U�����J�n
            if (P_E_Length < AttackLength)
            {
                Debug.Log("�U���͈͂ɓ������̂ōU�����J�n�I");
                DecideAttackType();  // �U���^�C�v������
            }
            // �v���C���[���T�[�`�͈͓��ɂ��邪�U���͈͊O�ɂ���ꍇ�A�ړ����J�n
            else if (P_E_Length < SearchLength)
            {
                Debug.Log("�v���C���[���T�[�`�͈͓��ɂ��܂����U���͈͊O�ł��B�ړ����J�n���܂��B");
                SetState(Enemy_State_.Walk);
            }
            // �v���C���[���͈͊O�ɂ���ꍇ�A�ҋ@��Ԃɖ߂�
            else
            {
                Debug.Log("�v���C���[���͈͊O�ł��B�ҋ@��Ԃɖ߂�܂��B");
                SetState(Enemy_State_.Idle);
            }
        }

        // �ړ���Ԃ̏���
        if (E_State == Enemy_State_.Walk)
        {
            if (P_E_Length > AttackLength && P_E_Length < SearchLength)
            {
                Vector3 direction = (Target_P.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);  // �v���C���[�Ɍ����ĉ�]
                transform.position += direction * MoveSpeed * Time.deltaTime;  // �v���C���[�Ɍ������Ĉړ�
            }
        }
    }

    // �U���^�C�v�����肷��
    private void DecideAttackType()
    {
        int randomValue = Random.Range(0, 100);  // 0����100�܂ł̃����_���l
        if (randomValue < TategiriChance)
        {
            SetState(Enemy_State_.Tategiri);  // �c�؂�U��
        }
        else
        {
            SetState(Enemy_State_.RenGeki_First);  // �A���U��
        }
    }

    // �A���U���̏���
    private void HandleRenGeki()
    {
        if (E_State == Enemy_State_.RenGeki_First)
        {
            if (IsAnimationFinished("RenGeki_First"))
            {
                SetState(Enemy_State_.RenGeki_Second);  // 1��ڂ̘A�����I�������2��ڂɐi��
            }
        }
        else if (E_State == Enemy_State_.RenGeki_Second)
        {
            if (IsAnimationFinished("RenGeki_Second"))
            {
                SetState(Enemy_State_.Cooldown);  // �A�����I�������N�[���_�E����ԂɈڍs
                StateTime = CooldownTime;  // �N�[���_�E�����Ԃ�ݒ�
            }
        }
    }

    // �A�j���[�V�������I�����������m�F����
    private bool IsAnimationFinished(string stateName)
    {
        AnimatorStateInfo stateInfo = E01Anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f;  // �A�j���[�V�������I���������m�F
    }

    // �A�j���[�V�������X�V����
    private void UpdateAnimations()
    {
        E01Anim.SetBool("Walk", E_State == Enemy_State_.Walk);  // �ړ���Ԃ̃A�j���[�V����
        E01Anim.SetBool("Idle", E_State == Enemy_State_.Idle);  // �ҋ@��Ԃ̃A�j���[�V����
        E01Anim.SetBool("Tategiri", E_State == Enemy_State_.Tategiri);  // �c�؂�U���̃A�j���[�V����
        E01Anim.SetBool("RenGeki", E_State == Enemy_State_.RenGeki_First || E_State == Enemy_State_.RenGeki_Second);  // �A���U���̃A�j���[�V����
    }
}
