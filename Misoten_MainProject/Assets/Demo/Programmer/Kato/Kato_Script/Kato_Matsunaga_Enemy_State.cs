using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Matsunaga_Enemy_State : MonoBehaviour
{
    // �G�̏�Ԃ�\���񋓌^
    public enum Enemy_State_
    {
        Idle,       // �ҋ@���
        Walk,       // �ړ����
        Tategiri,   // �c�؂�U�����
        RenGeki,    // �A���U�����
        Stagger,    // �Ђ�ݏ��
        Cooldown    // �N�[���_�E�����
    };

    private Enemy_State_ E_State; // ���݂̓G�̏�Ԃ��i�[

    [SerializeField, Header("�^�[�Q�b�g�ƂȂ�v���C���[")]
    public GameObject Target_P; // �G���^�[�Q�b�g����v���C���[�I�u�W�F�N�g

    [SerializeField, Header("�T�[�`�˒�(10)")]
    public float SearchLength = 10; // �G���v���C���[��T�m�ł��鋗��

    [SerializeField, Header("�U���˒�(3.5)")]
    public float AttackLength = 3.5f; // �G���U���\�ȋ���

    [SerializeField, Header("�ړ��X�s�[�h(12)")]
    public float MoveSpeed = 12; // �G���ړ����鑬�x

    [SerializeField, Header("�c�؂�U���m��(%)"), Range(0, 100)]
    public int TategiriChance ; // �c�؂�U����I������m��

    [SerializeField, Header("�A���U���m��(%)"), Range(0, 100)]
    public int RenGekiChance ; // �A���U����I������m��

    private float P_E_Length; // �v���C���[�ƓG�Ƃ̋�����ێ�

    public Animator E01Anim; // �G�̃A�j���[�V�����𐧌䂷��Animator

    private float StateTime = 2.5f; // ��Ԃ��Ƃ̎�������
    private float StateCurrentTime; // ���݂̏�Ԃ��J�n���Ă���̌o�ߎ���

    [SerializeField, Header("�N�[���_�E������")]
    private float CooldownTime = 2.5f; // �N�[���_�E����Ԃ̎�������

    [SerializeField, Header("�Ђ�ގ���")]
    private float StaggerTime = 1.0f; // �Ђ�ݏ�Ԃ̎�������

    [Header("�ϋv�t�B�[���h�̃I�u�W�F�N�g")]
    public GameObject durabilityFieldPrefab; // �ϋv�t�B�[���h�̃v���n�u�I�u�W�F�N�g

    [Header("�ϋv�t�B�[���h�𐶐�������W")]
    public Vector3[] fieldPositions; // �ϋv�t�B�[���h�𐶐�������W

    [Header("�ϋv�t�B�[���h�̃X�P�[��")]
    [SerializeField]
    public Vector3 fieldScale = new Vector3(1, 1, 1); // �ϋv�t�B�[���h�̃X�P�[���i�f�t�H���g�l: 1, 1, 1�j

    private float currentHP; // �G�̌��݂�HP
    private bool hasUsedDurabilityField75 = false; // HP75%�őϋv�t�B�[���h�𐶐��ς݂����Ǘ�
    private bool hasUsedDurabilityField50 = false; // HP50%�őϋv�t�B�[���h�𐶐��ς݂����Ǘ�
    private bool hasUsedDurabilityField25 = false; // HP25%�őϋv�t�B�[���h�𐶐��ς݂����Ǘ�

    [Header("�o���A�I�u�W�F�N�g")]
    public GameObject barrierPrefab; // �o���A�̃v���n�u�I�u�W�F�N�g

    [Header("�o���A�𐶐�������W")]
    public Vector3[] barrierPosition; // �o���A�𐶐�������W�̔z��

    private float elapsedTime = 0f; // �o�ߎ��Ԃ��L�^

    private void Start()
    {
        // ������Ԃ�ݒ�
        E_State = Enemy_State_.Idle;
        StateCurrentTime = 0.0f; // �o�ߎ��Ԃ�������
        currentHP = Matsunaga_Status_E.NowHP / Matsunaga_Status_E.MaxHP; // ����HP��ݒ�
        elapsedTime = 0f; // �o�ߎ��Ԃ�������
        E01Anim.SetBool("Idle", true); // Idle�A�j���[�V������������Ԃɐݒ�
    }

    private void Update()
    {
        Debug.Log($"currentHP: {currentHP}");

        // 1�L�[�������ꂽ��HP��75%�ɐݒ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentHP = 0.75f * Matsunaga_Status_E.MaxHP;  // HP��75%�ɐݒ�
            Debug.Log("HP��75%�ɐݒ肵�܂����I");
        }


        currentHP = Matsunaga_Status_E.NowHP / Matsunaga_Status_E.MaxHP;
        // �^�[�Q�b�g���ݒ肳��Ă��Ȃ��ꍇ�͌x����\���������𒆒f
        if (Target_P == null)
        {
            Debug.LogWarning("Target_P ���ݒ肳��Ă��܂���I");
            return;
        }

        // �v���C���[�ƓG�̋������v�Z
        P_E_Length = Vector3.Distance(Target_P.transform.position, gameObject.transform.position);
        Debug.Log($"�v���C���[�Ƃ̋���: {P_E_Length}");

        // ��Ԃ��Ƃ̌o�ߎ��Ԃ��X�V
        StateCurrentTime += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        // �e��Ԃɉ������������Ăяo��
        if (E_State == Enemy_State_.Cooldown)
        {
            HandleCooldown();
        }
        else if (E_State == Enemy_State_.Idle || E_State == Enemy_State_.Walk)
        {
            HandleMovementAndState();
        }
        else if (E_State == Enemy_State_.RenGeki)
        {
            HandleRenGeki();           
        }
        else if (E_State == Enemy_State_.Tategiri)
        {
            HandleTategiri();
        }
        else if (E_State == Enemy_State_.Stagger)
        {
            HandleStagger();
        }

        // HP�ɉ����đϋv�t�B�[���h�𐶐�
        HandleDurabilityField();

        // ��Ԃɉ����ăA�j���[�V�������X�V
        UpdateAnimations();
    }

    // �V������Ԃ�ݒ肵�o�ߎ��Ԃ����Z�b�g
    private void SetState(Enemy_State_ newState)
    {
        E_State = newState;
        StateCurrentTime = 0.0f;
    }

    // �o���A�𐶐�����
    private void SpawnBarrier()
    {
        Instantiate(barrierPrefab, barrierPosition[0], Quaternion.identity); // ���݂̈ʒu�Ƀo���A�𐶐�
        Debug.Log("�o���A�𐶐����܂���");
    }

    // �ϋv�t�B�[���h�𐶐�����
    private void SpawnDurabilityField()
    {
        foreach (var position in fieldPositions)
        {
            // �ϋv�t�B�[���h�𐶐�
            GameObject field = Instantiate(durabilityFieldPrefab, position, Quaternion.identity);

            // �X�P�[����K�p
            field.transform.localScale = fieldScale;

            Debug.Log($"�ϋv�t�B�[���h�𐶐�: {position}, �X�P�[��: {fieldScale}");
        }
    }

    // �ҋ@�܂��͈ړ���Ԃł̏���
    private void HandleMovementAndState()
    {
        if (StateCurrentTime >= StateTime)
        {
            // ��ԑJ�ڃ^�C�~���O�����Z�b�g
            StateCurrentTime = 0.0f;

            if (P_E_Length < AttackLength)
            {
                // �U���͈͓��Ƀv���C���[������ꍇ�A�U�����J�n
                Debug.Log("�U���͈͂ɓ������̂ōU�����J�n�I");
                DecideAttackType();
                //UnityEditor.EditorApplication.isPaused = true;
            }
            else if (P_E_Length < SearchLength)
            {
                // �T�[�`�͈͓��Ƀv���C���[������ꍇ�A�ړ����J�n
                Debug.Log("�v���C���[���T�[�`�͈͓��ɂ��܂����U���͈͊O�ł��B�ړ����J�n���܂��B");
                SetState(Enemy_State_.Walk);
            }
            else
            {
                // �v���C���[���͈͊O�̏ꍇ�A�ҋ@��Ԃɖ߂�
                Debug.Log("�v���C���[���͈͊O�ł��B�ҋ@��Ԃɖ߂�܂��B");
                SetState(Enemy_State_.Idle);
            }
        }

        // Idle��Ԃ̏ꍇ�ł��v���C���[�̕���������
        if (E_State == Enemy_State_.Idle && P_E_Length < SearchLength)
        {
            Vector3 direction = (Target_P.transform.position - transform.position).normalized; // �v���C���[����
            direction.y = 0; // Y����]��}��
            Quaternion targetRotation = Quaternion.LookRotation(direction); // �v���C���[������������]
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed); // �X���[�Y�ȉ�]
        }

        // Walk��Ԃ̏ꍇ�A�v���C���[�Ɍ������Ĉړ�
        if (E_State == Enemy_State_.Walk)
        {
            if (P_E_Length > AttackLength && P_E_Length < SearchLength)
            {
                Vector3 direction = (Target_P.transform.position - transform.position).normalized; // �v���C���[����
                direction.y = 0; // Y����]��}��
                Quaternion targetRotation = Quaternion.LookRotation(direction); // �v���C���[������������]
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed); // �X���[�Y�ȉ�]

                transform.position += direction * MoveSpeed * Time.deltaTime; // �v���C���[�Ɍ������Ĉړ�
            }
        }
    }


    // �U���^�C�v�����肷��
    private void DecideAttackType()
    {
        int randomValue = Random.Range(0, 100); // 0�`100�̃����_���l�𐶐�
        Debug.Log($"DecideAttackType: Random Value = {randomValue}, TategiriChance = {TategiriChance}");

        if (randomValue < TategiriChance)
        {
            // �c�؂�U����I��
            Debug.Log("�c�؂�U����I�����܂����I");
            SetState(Enemy_State_.Tategiri);
            //UnityEditor.EditorApplication.isPaused = true;
        }
        else
        {
            // �A���U����I��
            Debug.Log("�A���U����I�����܂����I");
            SetState(Enemy_State_.RenGeki);
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }


    // �c�؂�U���̏���
    private void HandleTategiri()
    {

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            // �c�؂�U��������A�N�[���_�E���ɑJ��
            Debug.Log("�c�؂�U�����������܂����BCooldown ��ԂɑJ�ڂ��܂��B");
            E01Anim.SetBool("Tategiri", false); // �A�j���[�V���������Z�b�g
            SetState(Enemy_State_.Cooldown);            
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }

    // �A���U���̏���
    private void HandleRenGeki()
    {
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren2") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            // �A���U��������A�N�[���_�E���ɑJ��
            Debug.Log("�A���U�����������܂����BCooldown ��ԂɑJ�ڂ��܂��B");
            E01Anim.SetBool("Rengeki", false); // �A�j���[�V���������Z�b�g
            SetState(Enemy_State_.Cooldown);
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }

    // �Ђ�ݏ�Ԃ̏���
    private void HandleStagger()
    {
        if (StateCurrentTime >= StaggerTime)
        {
            // �Ђ�ݏ�ԏI����A�ҋ@��ԂɑJ��
            Debug.Log("�Ђ�ݏ�Ԃ��I�����܂����BIdle ��ԂɑJ�ڂ��܂��B");
            SetState(Enemy_State_.Idle);
        }
    }

    // �N�[���_�E����Ԃ̏���
    private void HandleCooldown()
    {
        if (StateCurrentTime >= CooldownTime)
        {
            // �N�[���_�E���I����A�ҋ@��ԂɑJ��
            Debug.Log("�N�[���_�E�����I�����܂����BIdle ��ԂɑJ�ڂ��܂��B");
            SetState(Enemy_State_.Idle);
        }
    }

    // HP�ɉ������ϋv�t�B�[���h�̐���
    private void HandleDurabilityField()
    {
        if (currentHP <= 0.75f && !hasUsedDurabilityField75)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField75 = true;
        }
        if (currentHP <= 0.50f && !hasUsedDurabilityField50)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField50 = true;
        }
        if (currentHP <= 0.25f && !hasUsedDurabilityField25)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField25 = true;
        }
    }

    // �w��A�j���[�V�������I�����Ă��邩�𔻒�
    private bool IsAnimationFinished(string animationName)
    {
        return //E01Anim.GetCurrentAnimatorStateInfo(0).IsName(animationName)
            /*&&*/ E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
    }

    // ��Ԃɉ����ăA�j���[�V�������X�V
    private void UpdateAnimations()
    {
        // ��Ԃ��Ƃ̃A�j���[�V�����t���O���X�V
        E01Anim.SetBool("Idle", E_State == Enemy_State_.Idle || E_State == Enemy_State_.Cooldown);
        E01Anim.SetBool("Walk", E_State == Enemy_State_.Walk);
        E01Anim.SetBool("Tategiri", E_State == Enemy_State_.Tategiri);
        E01Anim.SetBool("Rengeki", E_State == Enemy_State_.RenGeki);
        KatoUpdateAnim();
    }
    private float Check_Current_Time;//���͊J�n����o�߂�������

    //�c�؂� �ő���͗P�\ 1.7�b
    //�A��1 �ő���͗P�\ 1.2�b
    //�A��2 �ő���͗P�\ 0.5�b
    [SerializeField, Header("�c�؂� �ő���͗P�\ 1.7�b")]
    public float Check_Time0;
    [SerializeField, Header("�A��1 �ő���͗P�\ 1.2�b")]
    public float Check_Time1;
    [SerializeField, Header("�A��2 �ő���͗P�\ 0.5�b")]
    public float Check_Time2;

    static public bool UkeL;
    static public bool UkeR;
    static public bool RenUke01;
    static public bool RenUke02;

    private void KatoUpdateAnim()
    {
        //�c�؂�U��グ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            //UnityEditor.EditorApplication.isPaused = true;
            if (Miburo_State._Uke_Input)
            {

                if (Check_Current_Time > 0.0f && Check_Time0 >= Check_Current_Time)
                {
                    Debug.Log("aaaaaaaa" + Check_Current_Time);
                   // UnityEditor.EditorApplication.isPaused = true;
                    //�󂯗�������
                    Debug.Log(Check_Current_Time);
                    if (Miburo_State._Katana_Direction==0|| Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        UkeL = true;
                        E01Anim.SetBool("UkeL", true);
                        UnityEditor.EditorApplication.isPaused = true;
                    }
                    else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        UkeR = true;
                        E01Anim.SetBool("UkeR", true);
                        UnityEditor.EditorApplication.isPaused = true;
                    }
                }
            }
            else
            {

                Check_Current_Time += Time.deltaTime;
            }
        }
        else
        {
            E01Anim.SetBool("UkeL", false);
            E01Anim.SetBool("UkeR", false);
            Check_Current_Time = 0;
        }

        //�c�؂�U�肨�낵
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
        {
            Debug.Log(Check_Current_Time);


            Check_Current_Time = 0;
            //UnityEditor.EditorApplication.isPaused = true;
        }

        //�A��1�U��グ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren01"))
        {
            if (Miburo_State._Uke_Input)
            {
                Debug.Log("iiiiiiiii" + Check_Current_Time);
                UnityEditor.EditorApplication.isPaused = true;
                if (Check_Current_Time > 0.0f && Check_Time1 >= Check_Current_Time)
                {
                    //�󂯗�������
                    Debug.Log(Check_Current_Time);
                    UnityEditor.EditorApplication.isPaused = true;
                    //Enemy01_Animator.SetBool("RenUke01", true);
                    RenUke01 = true;
                    E01Anim.SetBool("RenUke01", true);
                }
            }
            else
            {
                Check_Current_Time += Time.deltaTime;
            }
        }
        else
        {
            RenUke01 = false;
            E01Anim.SetBool("RenUke01", false);
            //Enemy01_Animator.SetBool("RenUke01", false);
            //Check_Current_Time = 0;
        }

        //�A��1�U�肨�낵
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
        {
            Debug.Log(Check_Current_Time);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time = 0;
        }

        //�A��2�U��グ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren02"))
        {
            if (Miburo_State._Uke_Input)
            {
                Debug.Log("uuuuuuuu" + Check_Current_Time);
                UnityEditor.EditorApplication.isPaused = true;
                if (Check_Current_Time > 0.0f && Check_Time2 >= Check_Current_Time)
                {
                    //�󂯗�������
                    RenUke02 = true;
                    E01Anim.SetBool("RenUke02", true);
                    Debug.Log(Check_Current_Time);
                    //UnityEditor.EditorApplication.isPaused = true;
                }
            }
            else
            {
                Check_Current_Time += Time.deltaTime;
            }
        }
        else
        {
            RenUke02 = false;
            E01Anim.SetBool("RenUke02", false);
            //Check_Current_Time = 0;
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Check_Current_Time = 0;
        }

         //�A��2�U�肨�낵         
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren2"))
        {

            Debug.Log(Check_Current_Time);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time = 0;
        }
    }
}