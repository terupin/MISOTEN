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

    // �L�����N�^�[�̃X�e�[�^�X�֌W
    [Header("�̗�")]
    public int HitPoint;
    [Header("�U�艺�낵���̍U����")]
    public int Attack_point_hit_to_down;
    [Header("�A�����̍U����")]
    public int Attack_point_double_attack;

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

    [Header("�U��グ�̔����m��")]
    [Range(0.0f, 1.0f)] public float hitToDownProbability = 0.3f;
    [Header("�A���̔����m��")]
    [Range(0.0f, 1.0f)] public float doubleAttackProbability = 0.5f;

    private attack_patern[] routine; // �U���p�^�[���̔z��
    private Coroutine attackRoutineCoroutine; // ���s���̍U�����[�`���̎Q��
    private bool isExecutingFieldAction = false; // �ϋv�t�B�[���h�W�J�����ǂ����̃t���O

    void Start()
    {
        // ������
        randomValue = -1;
        CurrentTime = 10.0f;

        HitPoint = 10000;
        Attack_point_hit_to_down = 1;
        Attack_point_double_attack = 1;

        // 3�̍U���p�^�[����������
        routine = new attack_patern[3];
        routine[0] = new attack_patern();
        routine[1] = new attack_patern();
        routine[2] = new attack_patern();
    }

    void Update()
    {
        AnimatorStateInfo animatorStateInfo = Enemy_Animator.GetCurrentAnimatorStateInfo(0);

        if (playersword != null) Enemy_Animator.SetFloat("playersword_angul", playersword.transform.eulerAngles.y);
        if (enemysword != null) Enemy_Animator.SetFloat("enemysword_angul", enemysword.transform.eulerAngles.y);

        // �U�����[�`�����J�n
        if (attackRoutineCoroutine == null)
        {
            attackRoutineCoroutine = StartCoroutine(ExecuteAttackRoutineWithDelay(SelectRandomRoutine()));
        }

        if (target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;
            directionToTarget.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);

            float distanceToTarget = directionToTarget.magnitude;
            if (distanceToTarget > maintainDistance)
            {
                Vector3 moveDirection = directionToTarget.normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }

        if ((HitPoint <= 7500 && HitPoint > 5000) ||
            (HitPoint <= 5000 && HitPoint > 2500) ||
            (HitPoint <= 2500 && HitPoint > 0))
        {
            if (!isExecutingFieldAction)
            {
                if (attackRoutineCoroutine != null)
                {
                    StopCoroutine(attackRoutineCoroutine);
                }
                attackRoutineCoroutine = StartCoroutine(ExecuteResistFieldAndResumeRoutine());
            }
        }

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

            if (animatorStateInfo.IsName(Enemy_Anim_name) && animatorStateInfo.normalizedTime >= 0.9f && !animatorStateInfo.loop)
            {
                Enemy_Animator.SetBool(Enemy_Anim_bool, false);
                AnimCurrentTime = 0.0f;
            }

            if (CurrentTime == 0.0f)
            {
                randomValue = UnityEngine.Random.Range(0, 8);
            }

            CurrentTime += Time.deltaTime;
        }
    }

    private attack_patern SelectRandomRoutine()
    {
        float random = UnityEngine.Random.Range(0f, 1f);
        if (random < hitToDownProbability)
        {
            return routine[0];
        }
        else if (random < hitToDownProbability + doubleAttackProbability)
        {
            return routine[1];
        }
        else
        {
            return routine[2];
        }
    }

    IEnumerator ExecuteResistFieldAndResumeRoutine()
    {
        isExecutingFieldAction = true;
        ProcessAttack(attack_part.registans_field);
        yield return new WaitForSeconds(0.5f);

        int nextRoutineIndex = UnityEngine.Random.Range(0, routine.Length);
        attackRoutineCoroutine = StartCoroutine(ExecuteAttackRoutineWithDelay(routine[nextRoutineIndex]));
        isExecutingFieldAction = false;
    }

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

        yield return new WaitForSeconds(0.5f);
        attackRoutineCoroutine = StartCoroutine(ExecuteAttackRoutineWithDelay(SelectRandomRoutine()));
    }

    void ProcessAttack(attack_part attack)
    {
        switch (attack)
        {
            case attack_part.hit_to_down:
                Debug.Log("�U�艺�낵�U��");
                break;
            case attack_part.double_attack:
                Debug.Log("�A��");
                break;
            case attack_part.registans_field:
                Debug.Log("�ϋv�t�B�[���h�W�J");
                break;
        }
    }
}
