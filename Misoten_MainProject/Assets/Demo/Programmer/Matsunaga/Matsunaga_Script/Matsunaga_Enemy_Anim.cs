using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsunaga_Enemy_Anim : MonoBehaviour
{
    // �A�j���[�^�[����уA�j���[�V�����Ǘ��p�̕ϐ�
    public Animator Enemy_Animator;
    public string Enemy_Anim_bool; // �I�������m�������A�j���[�V�����̃t���O
    public string Enemy_Anim_name; // �I�������m�������A�j���[�V�����̖��O
    public static int randomValue = -1; // �����_���Ȓl���i�[����

    // �U���֘A�̕ϐ��ƃt���O
    public GameObject PkatanaHitbox;
    public GameObject Ejoint;
    private int Katana_Direction = -1;
    private bool U_Ukenagasi_Flg = false; // �󂯗����t���O
    public float CullTime;
    private float CurrentTime = 0.0f;
    private float AnimCurrentTime = 0.0f;
    private float AttackTime = 3.0f;

    // �v���C���[�ƓG�̌��̊p�x�擾�p�̕ϐ�
    public GameObject playersword;
    public GameObject enemysword;

    // �^�[�Q�b�g�Ɍ������Ĉړ��E�������߂̕ϐ�
    public Transform target; // �ڕW�̃I�u�W�F�N�g
    public float maintainDistance = 5.0f;  // �ۂׂ�����
    public float moveSpeed = 2.0f; // �ړ����x

    // �U���̎�ނ��`����enum
    enum attack_part
    {
        hit_to_down,    // �U����U��
        double_attack,  // �A��
        registans_field // �ϋv�t�B�[���h
    }

    // �U���p�^�[���̍\���̂��`
    struct attack_patern
    {
        public attack_part one, two, three, four, five, six;

        // �e�U���A�N�V�������󂯎��R���X�g���N�^
        public attack_patern(attack_part one, attack_part two, attack_part three, attack_part four, attack_part five, attack_part six)
        {
            this.one = one;
            this.two = two;
            this.three = three;
            this.four = four;
            this.five = five;
            this.six = six;
        }
    }

    private attack_patern[] routine; // �U���p�^�[���̔z��
    private Coroutine attackRoutineCoroutine; // ���s���̍U�����[�`���̎Q��

    void Start()
    {
        randomValue = -1;
        CurrentTime = 10.0f;

        // 3�̍U���p�^�[����������
        routine = new attack_patern[3];
        routine[0] = new attack_patern(attack_part.hit_to_down, attack_part.hit_to_down, attack_part.hit_to_down, attack_part.hit_to_down, attack_part.hit_to_down, attack_part.hit_to_down);
        routine[1] = new attack_patern(attack_part.double_attack, attack_part.double_attack, attack_part.double_attack, attack_part.double_attack, attack_part.double_attack, attack_part.double_attack);
        routine[2] = new attack_patern(attack_part.registans_field, attack_part.registans_field, attack_part.registans_field, attack_part.registans_field, attack_part.registans_field, attack_part.registans_field);
    }

    void Update()
    {
        // �A�j���[�^�[�̌��݂̏�Ԃ��擾
        AnimatorStateInfo animatorStateInfo = Enemy_Animator.GetCurrentAnimatorStateInfo(0);

        // �v���C���[�ƓG�̌��̊p�x���A�j���[�^�[�ɐݒ�
        if (playersword != null) Enemy_Animator.SetFloat("playersword_angul", playersword.transform.eulerAngles.y);
        if (enemysword != null) Enemy_Animator.SetFloat("enemysword_angul", enemysword.transform.eulerAngles.y);

        // �U���p�^�[���������_���ɑI�����A���[�`�������݂��Ȃ��ꍇ�Ɏ��s
        int selectedRoutineIndex = UnityEngine.Random.Range(0, routine.Length);
        if (attackRoutineCoroutine == null)
        {
            attackRoutineCoroutine = StartCoroutine(ExecuteAttackRoutineWithDelay(routine[selectedRoutineIndex]));
        }

        // �^�[�Q�b�g���ݒ肳��Ă���ꍇ�A�^�[�Q�b�g�̕����������A�w�苗����ۂ��Ȃ���ړ�
        if (target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;
            directionToTarget.y = 0; // ���������̂݌���
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);

            Debug.Log("�����]�����s���܂����B�^�[�Q�b�g�Ɍ����Ă��܂��B"); // �����]�����̃��O�o��

            // ���̋�����ۂ��Ȃ���^�[�Q�b�g�Ɍ������Ĉړ�
            float distanceToTarget = directionToTarget.magnitude;
            if (distanceToTarget > maintainDistance)
            {
                Vector3 moveDirection = directionToTarget.normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                Debug.Log("�ړ����s���Ă��܂��B�^�[�Q�b�g�Ƃ̋�����ۂ��Ă��܂��B"); // �ړ����̃��O�o��
            }
        }

        // �A�j���[�V�����I���ƍU���p�^�[���̃��Z�b�g����
        if (Enemy_Animator.enabled)
        {
            if (CurrentTime > CullTime)
            {
                Enemy_Animator.SetBool(Enemy_Anim_bool, false);
                randomValue = -1;
                CurrentTime = 0.0f;
            }
            else
            {
                Enemy_Animator.SetBool(Enemy_Anim_bool, true);
            }

            // �A�j���[�V����������v���Ă���A�������Ă���΃��Z�b�g
            if (animatorStateInfo.IsName(Enemy_Anim_name) && animatorStateInfo.normalizedTime >= 0.9f && !animatorStateInfo.loop)
            {
                Enemy_Animator.SetBool(Enemy_Anim_bool, false);
                AnimCurrentTime = 0.0f;
                Debug.Log("�A�j���[�V�������������܂���: " + Enemy_Anim_name); // �A�j���[�V�����������̃��O�o��
            }

            // �����_���l�̃��Z�b�g
            if (CurrentTime == 0.0f)
            {
                randomValue = UnityEngine.Random.Range(0, 8);
            }

            CurrentTime += Time.deltaTime;
        }
    }

    // �U�����[�`�����w��̒x���Ƌ��Ɏ��s
    IEnumerator ExecuteAttackRoutineWithDelay(attack_patern selectedRoutine)
    {
        ProcessAttack(selectedRoutine.one);
        yield return new WaitForSeconds(0.1f);

        ProcessAttack(selectedRoutine.two);
        yield return new WaitForSeconds(0.1f);

        ProcessAttack(selectedRoutine.three);
        yield return new WaitForSeconds(0.1f);

        ProcessAttack(selectedRoutine.four);
        yield return new WaitForSeconds(0.1f);

        ProcessAttack(selectedRoutine.five);
        yield return new WaitForSeconds(0.1f);

        ProcessAttack(selectedRoutine.six);

        // ���̍U���p�^�[���Ɉڍs
        int nextRoutineIndex = UnityEngine.Random.Range(0, routine.Length);
        yield return new WaitForSeconds(0.5f);

        attackRoutineCoroutine = StartCoroutine(ExecuteAttackRoutineWithDelay(routine[nextRoutineIndex]));
    }

    // �e�U���p�[�g�ɉ������A�N�V���������s
    void ProcessAttack(attack_part attack)
    {
        switch (attack)
        {
            case attack_part.hit_to_down:
                Enemy_Animator.SetTrigger("HitToDown");
                Debug.Log("�A�N�V����: �U����U��"); // �U���A�N�V�������O
                break;

            case attack_part.double_attack:
                Enemy_Animator.SetTrigger("DoubleAttack");
                Debug.Log("�A�N�V����: �A��"); // �U���A�N�V�������O
                break;

            case attack_part.registans_field:
                Enemy_Animator.SetTrigger("RegistansField");
                Debug.Log("�A�N�V����: �ϋv�t�B�[���h�W�J"); // �U���A�N�V�������O
                break;
        }
    }
}