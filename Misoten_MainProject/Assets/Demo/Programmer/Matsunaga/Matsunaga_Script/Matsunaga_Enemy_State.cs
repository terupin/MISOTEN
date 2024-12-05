using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsunaga_Enemy_State : MonoBehaviour
{
    // �G�̏�Ԃ�\���񋓌^
    public enum Enemy_State_
    {
        Idle,       // �ҋ@���
        Walk,       // �ړ����
        Tategiri,   // �c�؂�U�����
        RenGeki,    // �A���U�����
        Stagger,    // �Ђ�ݏ��
        Cooldown,   // �N�[���_�E�����
        Kaihou      // �ϋv�t�B�[���h�W�J���
    };

    [SerializeField, Header("�f�o�b�N���[�h")]
    public bool debug_switch = true; //�f�o�b�O�p�̏����̃X�C�b�`

    private Enemy_State_ E_State; // ���݂̓G�̏�Ԃ��i�[

    [SerializeField, Header("�^�[�Q�b�g�ƂȂ�v���C���[")]
    public GameObject Target_P; // �G���^�[�Q�b�g����v���C���[�I�u�W�F�N�g

    [SerializeField, Header("�T�[�`�˒�(10)")]
    public float SearchLength = 100; // �G���v���C���[��T�m�ł��鋗��

    [SerializeField, Header("�U���˒�(3.5)")]
    public float AttackLength = 3.5f; // �G���U���\�ȋ���

    [SerializeField, Header("�ړ��X�s�[�h(12)")]
    public float MoveSpeed = 12; // �G���ړ����鑬�x

    [SerializeField, Header("�c�؂�U���m��(%)"), Range(0, 100)]
    public int TategiriChance = 60; // �c�؂�U����I������m��

    [SerializeField, Header("�A���U���m��(%)"), Range(0, 100)]
    public int RenGekiChance = 40; // �A���U����I������m��

    private float P_E_Length; // �v���C���[�ƓG�Ƃ̋�����ێ�

    public Animator E01Anim; // �G�̃A�j���[�V�����𐧌䂷��Animator

    public float StateTime = 2.5f; // ��Ԃ��Ƃ̎�������
    private float StateCurrentTime; // ���݂̏�Ԃ��J�n���Ă���̌o�ߎ���

    [SerializeField, Header("�N�[���_�E������")]
    private float CooldownTime = 2.5f; // �N�[���_�E����Ԃ̎�������

    [SerializeField, Header("�Ђ�ގ���")]
    private float StaggerTime = 1.0f; // �Ђ�ݏ�Ԃ̎�������

    //[Header("�ϋv�t�B�[���h�̃I�u�W�F�N�g")]
    //public GameObject durabilityFieldPrefab; // �ϋv�t�B�[���h�̃v���n�u�I�u�W�F�N�g

    //[Header("�ϋv�t�B�[���h�𐶐�������W")]
    //public Vector3[] fieldPositions; // �ϋv�t�B�[���h�𐶐�������W

    //[Header("�ϋv�t�B�[���h�̃X�P�[��")]
    //[SerializeField]
    //public Vector3 fieldScale = new Vector3(1, 1, 1); // �ϋv�t�B�[���h�̃X�P�[���i�f�t�H���g�l: 1, 1, 1�j

    private float currentHP; // �G�̌��݂�HP
    private bool hasUsedDurabilityField100 = false; // HP100%�őϋv�t�B�[���h�𐶐��ς݂����Ǘ�
    private bool hasUsedDurabilityField75 = false; // HP75%�őϋv�t�B�[���h�𐶐��ς݂����Ǘ�
    private bool hasUsedDurabilityField50 = false; // HP50%�őϋv�t�B�[���h�𐶐��ς݂����Ǘ�
    private bool hasUsedDurabilityField25 = false; // HP25%�őϋv�t�B�[���h�𐶐��ς݂����Ǘ�

    //[Header("�o���A�I�u�W�F�N�g")]
    //public GameObject barrierPrefab; // �o���A�̃v���n�u�I�u�W�F�N�g

    //[Header("�o���A�𐶐�������W")]
    //public Vector3[] barrierPosition; // �o���A�𐶐�������W�̔z��

    //[Header("�o���A�̃X�P�[��")]
    //public Vector3 barrierScale = new Vector3(1, 1, 1); // �o���A�̃X�P�[���i�f�t�H���g�l: 1, 1, 1�j

    private float elapsedTime = 0f; // �o�ߎ��Ԃ��L�^

    [Header("�Z�p�`�̔��a")]
    public float radius = 1.0f; // �Z�p�`�̔��a
    [Header("�Z�p���̍���")]
    public float height = 2.0f; // �Z�p���̍���
    [Header("�Z�p���̒������W")]
    public Vector3 centerOffset = Vector3.zero; // �������W�̃I�t�Z�b�g
    [Header("�ӕ����̃}�e���A��")]
    public Material lineMaterial; // ���p�̃}�e���A��
    [Header("�ʕ����̃}�e���A��")]
    public Material faceMaterial; // �ʗp�̃}�e���A��
    [Header("�d�|�̃��f��")]
    public GameObject vertexObjectPrefab; // ���_�ɐ�������I�u�W�F�N�g
    [Header("�d�|�̃X�P�[��")]
    public Vector3 vertexObjectScale = Vector3.one; // ���_�I�u�W�F�N�g�̃X�P�[��

    Vector3[] lowerVertices = new Vector3[6];
    Vector3[] upperVertices = new Vector3[6];

    private float maiclue_space = 10.0f;

    private float maiclue_x;
    private float maiclue_y;
    private float maiclue_z;

    private void Start()
    {
        // ������Ԃ�ݒ�
        E_State = Enemy_State_.Idle;
        StateCurrentTime = 0.0f; // �o�ߎ��Ԃ�������
        currentHP = Matsunaga_Status_E.NowHP / Matsunaga_Status_E.MaxHP; // ����HP��ݒ�
        elapsedTime = 0f; // �o�ߎ��Ԃ�������
        E01Anim.SetBool("Idle", true); // Idle�A�j���[�V������������Ԃɐݒ�
        
        // ���_���v�Z
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60 * i);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            lowerVertices[i] = new Vector3(x, 0, z) + centerOffset;
            upperVertices[i] = new Vector3(x, height, z) + centerOffset;
        }
    }

    private void Update()
    {
        // �v���C���[���ݒ肳��Ă���ꍇ�̂ݕ������������������s
        if (Target_P != null)
        {
            LookAtPlayer(); // �v���C���[�������������Ăяo��
        }

        //�f�o�b�O�p�v���O����
        if (debug_switch)
        {
            // 1�L�[�������ꂽ��HP�����Ԃɕω�
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Debug.Log("HP��75%�ɐݒ肵�܂����I");
                if (currentHP == 1.0f) // ����HP��100%�Ȃ�
                {
                    currentHP = 0.75f;  // HP��75%�ɐݒ�
                    Debug.Log($"dc1-1 HP��75%�ɐݒ肵�܂����I: {currentHP} / 1.0f");
                }
                else if (currentHP == 0.75f) // ����HP��75%�Ȃ�
                {
                    currentHP = 0.50f;  // HP��50%�ɐݒ�
                    Debug.Log($"dc1-2 HP��50%�ɐݒ肵�܂����I: {currentHP} / 1.0f");
                }
                else if (currentHP == 0.50f) // ����HP��50%�Ȃ�
                {
                    currentHP = 0.25f;  // HP��25%�ɐݒ�
                    Debug.Log($"dc1-3 HP��25%�ɐݒ肵�܂����I: {currentHP} / 1.0f");
                }
                else if (currentHP == 0.25f) // ����HP��25%�Ȃ�
                {
                    currentHP = 0f;  // HP��0%�ɐݒ�
                    Debug.Log($"dc1-4 HP��0%�ɐݒ肵�܂����I: {currentHP} / 1.0f");
                }
                else if (currentHP == 0f) // ����HP��0%�Ȃ�
                {
                    currentHP = 1.0f;  // HP��100%�ɐݒ�
                    Debug.Log($"dc1-5 HP��100%�ɐݒ肵�܂���: {currentHP} / 1.0f");
                }
            }

            // 2�L�[�������ꂽ��c�؂�X�e�[�g�����s
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("dc2: �c�؂�X�e�[�g�����s���܂�");
                SetState(Enemy_State_.Tategiri);
            }

            // 3�L�[�������ꂽ��A���X�e�[�g�����s
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("dc3: �A���X�e�[�g�����s���܂�");
                SetState(Enemy_State_.RenGeki);
            }

            // 4�L�[�������ꂽ�狯�݃X�e�[�g�����s
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("dc4: ���݃X�e�[�g�����s���܂�");
                SetState(Enemy_State_.Stagger);
            }

            // 5�L�[�������ꂽ����s�X�e�[�g�����s
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Debug.Log("dc5: ���s�X�e�[�g�����s���܂�");
                //SetState(Enemy_State_.Walk);
                maiclue_x = maiclue_space * Mathf.Sin(Time.time * MoveSpeed);
                maiclue_x = maiclue_x + Target_P.transform.position.x;
                maiclue_y = maiclue_space * Mathf.Cos(Time.time * MoveSpeed);
                maiclue_z = Target_P.transform.position.z;

                this.transform.position = new Vector3(maiclue_x, maiclue_y, maiclue_z);

                Debug.Log($"����{maiclue_x} {maiclue_y} {maiclue_z}");
            }

            // 6�L�[�������ꂽ��idle�X�e�[�g�����s
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Debug.Log("dc6: idle�X�e�[�g�����s���܂�");

                // Idle��ԂɑJ��
                SetState(Enemy_State_.Idle);
            }

            // 7�L�[�������ꂽ�����X�e�[�g�����s
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Debug.Log("dc7: ����X�e�[�g�����s���܂�");
                SetState(Enemy_State_.Kaihou);
                E01Anim.SetBool("Kaihou", true); // ����A�j���[�V�����̃t���O��ݒ�
            }
        }
        else
        {
            currentHP = Matsunaga_Status_E.NowHP / Matsunaga_Status_E.MaxHP;
        }

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

        if (E_State == Enemy_State_.Idle || E_State == Enemy_State_.Walk)
        {
            HandleMovementAndState();
        }

        if (E_State == Enemy_State_.RenGeki)
        {
            HandleRenGeki();
        }

        if (E_State == Enemy_State_.Tategiri)
        {
            HandleTategiri();
        }

        if (E_State == Enemy_State_.Stagger)
        {
            HandleStagger();
        }

        if (E_State == Enemy_State_.Kaihou)
        {
            HandleKaihou();
        }

        // HP�ɉ����đϋv�t�B�[���h�𐶐�
        HandleDurabilityField();

        // ��Ԃɉ����ăA�j���[�V�������X�V
        UpdateAnimations();
    }

    // �V������Ԃ�ݒ肵�o�ߎ��Ԃ����Z�b�g
    private void SetState(Enemy_State_ newState)
    {
        if (E_State == newState) return; // ������Ԃւ̑J�ڂ�h��
        E_State = newState;
        StateCurrentTime = 0.0f;
        Debug.Log($"��Ԃ� {newState} �ɕύX����܂���");
    }


    // �o���A�𐶐�����
    /*
    private void SpawnBarrier()
    {
        foreach (var position in barrierPosition)
        {
            // �o���A�𐶐�
            GameObject barrier = Instantiate(barrierPrefab, position, Quaternion.identity);

            // �X�P�[����K�p
            barrier.transform.localScale = barrierScale;

            Debug.Log($"�o���A�𐶐�: {position}, �X�P�[��: {barrierScale}");
        }
    }
    */

    // �ϋv�t�B�[���h�𐶐�����
    /*
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
    */

    private void HandleMovementAndState()
    {
        // Kaihou��Ԓ��͈ړ������𖳌���
        if (E_State == Enemy_State_.Kaihou)
        {
            Debug.Log("������̂��߈ړ��������X�L�b�v���܂��B");
            return;
        }

        // ����ȊO�̒ʏ�̈ړ�����
        if (debug_switch)
        {
            Debug.Log("�f�o�b�O���[�h���̂��߈ړ������͎��s����܂���B");
            return; // �����𒆒f
        }



            if (StateCurrentTime >= StateTime)
        {
            StateCurrentTime = 0.0f;

            if (P_E_Length <= AttackLength)
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

        if (E_State == Enemy_State_.Idle && P_E_Length < SearchLength)
        {
            Vector3 direction = (Target_P.transform.position - transform.position).normalized;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed);
        }

        if (E_State == Enemy_State_.Walk)
        {
            if (P_E_Length > AttackLength && P_E_Length < SearchLength)
            {
                Vector3 direction = (Target_P.transform.position - transform.position).normalized;
                direction.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed);

                transform.position += direction * MoveSpeed * Time.deltaTime;
            }
        }


    }

    // �U���^�C�v�����肷��
    private void DecideAttackType()
    {
        int randomValue = Random.Range(0, 100); // 0�`100�̃����_���l�𐶐�
        Debug.Log($"DecideAttackType: Random Value = {randomValue}, TategiriChance = {TategiriChance}");

        E01Anim.SetBool("Walk", false); // �A�j���[�V���������Z�b�g
        Debug.Log($"��{E01Anim.GetBool("Walk")}");

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
        if (IsAnimationFinished("Enemy01_Tategiri00"))
        {
            // �c�؂�U��������A�N�[���_�E���ɑJ��
            Debug.Log("�c�؂�U�����������܂����BCooldown ��ԂɑJ�ڂ��܂��B");
            E01Anim.SetBool("Tategiri", false); // �A�j���[�V���������Z�b�g
            SetState(Enemy_State_.Cooldown);
        }
    }

    // �A���U���̏���
    private void HandleRenGeki()
    {
        if (IsAnimationFinished("Enemy01_RtoLtoR"))
        {
            // �A���U��������A�N�[���_�E���ɑJ��
            Debug.Log("�A���U�����������܂����BCooldown ��ԂɑJ�ڂ��܂��B");
            E01Anim.SetBool("RenGeki", false); // �A�j���[�V���������Z�b�g
            SetState(Enemy_State_.Cooldown);
        }
    }

    // �Ђ�ݏ�Ԃ̏���
    private void HandleStagger()
    {
        if (IsAnimationFinished("Enemy01_Hirumi"))
        {
            // �Ђ�ݏ�ԏI����A�ҋ@��ԂɑJ��
            Debug.Log("�Ђ�ݏ�Ԃ��I�����܂����BIdle ��ԂɑJ�ڂ��܂��B");
            E01Anim.SetBool("Hiruimi", false); // �Ђ�݃A�j���[�V�����̃t���O�����Z�b�g
            SetState(Enemy_State_.Idle);
        }
    }

    private void HandleKaihou()
    {
        // �S�A�j���[�V�����������I�����ĉ���A�j���[�V�����ɑJ��
        //E01Anim.CrossFade("Enemy01_Idling", 0.01f);
        //E01Anim.CrossFade("Enemy01_Kaihou", 0.1f); // "Enemy01_Kaihou" �͉���A�j���[�V�����̏�Ԗ�

        E01Anim.Play("Enemy01_Kaihou", 0, 0f);
        // �K�v�Ȃ瑼�̏�ԏ��������s
        SetState(Enemy_State_.Kaihou);

        if (IsAnimationFinished("Enemy01_Kaihou"))
        {
            Debug.Log("����A�j���[�V�������������܂����BIdle ��ԂɑJ�ڂ��܂��B");
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
        if (currentHP <= 1.0f && !hasUsedDurabilityField100)
        {
            //SpawnDurabilityField();
            // ��ʂ̒��_�ɃI�u�W�F�N�g�𐶐�
            GenerateObjectsAtVertices(lowerVertices);
            StartCoroutine(DelayedBarrierSpawn());
            hasUsedDurabilityField100 = true;
            SetState(Enemy_State_.Kaihou);
        }

        if (currentHP <= 0.75f && !hasUsedDurabilityField75)
        {
            //SpawnDurabilityField();
            // ��ʂ̒��_�ɃI�u�W�F�N�g�𐶐�
            GenerateObjectsAtVertices(lowerVertices);
            StartCoroutine(DelayedBarrierSpawn());
            hasUsedDurabilityField75 = true;
            SetState(Enemy_State_.Kaihou);
        }

        if (currentHP <= 0.50f && !hasUsedDurabilityField50)
        {
            //SpawnDurabilityField();
            GenerateObjectsAtVertices(lowerVertices);
            StartCoroutine(DelayedBarrierSpawn());
            hasUsedDurabilityField50 = true;
            SetState(Enemy_State_.Kaihou);
        }

        if (currentHP <= 0.25f && !hasUsedDurabilityField25)
        {
            //SpawnDurabilityField();
            GenerateObjectsAtVertices(lowerVertices);
            StartCoroutine(DelayedBarrierSpawn());
            hasUsedDurabilityField25 = true;
            SetState(Enemy_State_.Kaihou);
        }
    }

    // �o���A������x������R���[�`��
    private IEnumerator DelayedBarrierSpawn()
    {
        yield return new WaitForSeconds(2f); // 2�b�ҋ@
        //SpawnBarrier();

        // �ӂ�`��
        for (int i = 0; i < 6; i++)
        {
            // ������ (��)
            DrawLine(lowerVertices[i], lowerVertices[(i + 1) % 6]);
            // ������ (��)
            DrawLine(upperVertices[i], upperVertices[(i + 1) % 6]);
            // ������
            DrawLine(lowerVertices[i], upperVertices[i]);
        }

        // �ʂ�`��
        CreateMesh(lowerVertices, upperVertices);
    }

    private IEnumerator WaitForKaihouAnimation()
    {
        yield return new WaitUntil(() => IsAnimationFinished("Enemy01_Kaihou"));

        // ����A�j���[�V�������I��������A�t���O�����Z�b�g����Ԃ�Idle�ɑJ��
        E01Anim.SetBool("Kaihou", false);
        Debug.Log("����A�j���[�V�������������܂���");
        SetState(Enemy_State_.Idle);
    }

    // �v���C���[����������
    private void LookAtPlayer()
    {
        // �v���C���[�̕������v�Z
        Vector3 direction = (Target_P.transform.position - transform.position).normalized;

        // Y�������̉�]�̂ݓK�p
        direction.y = 0;

        // �v���C���[������������]���v�Z
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // �X���[�Y�ɉ�]������
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed);
    }

    // �w��A�j���[�V�������I�����Ă��邩�𔻒�
    private bool IsAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = E01Anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }

    // ��Ԃɉ����ăA�j���[�V�������X�V
    private void UpdateAnimations()
    {
        // ��Ԃ��Ƃ̃A�j���[�V�����t���O���X�V
        E01Anim.SetBool("Idle", E_State == Enemy_State_.Idle);
        E01Anim.SetBool("Walk", E_State == Enemy_State_.Walk);
        E01Anim.SetBool("Tategiri", E_State == Enemy_State_.Tategiri);
        E01Anim.SetBool("RenGeki", E_State == Enemy_State_.RenGeki);
        E01Anim.SetBool("Hirumi", E_State == Enemy_State_.Stagger);
        E01Anim.SetBool("Kaihou", E_State == Enemy_State_.Kaihou);
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObject = new GameObject("Line");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] { start, end });
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    void CreateMesh(Vector3[] lowerVertices, Vector3[] upperVertices)
    {
        GameObject meshObject = new GameObject("HexagonalPrism");
        MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        /*
        // ����
        for (int i = 1; i < 5; i++)
        {
            vertices.Add(lowerVertices[0]);
            vertices.Add(lowerVertices[i]);
            vertices.Add(lowerVertices[i + 1]);

            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
        }

        // ���
        for (int i = 1; i < 5; i++)
        {
            vertices.Add(upperVertices[0]);
            vertices.Add(upperVertices[i + 1]);
            vertices.Add(upperVertices[i]);

            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
        }
        */

        // ����
        for (int i = 0; i < 6; i++)
        {
            int next = (i + 1) % 6;

            vertices.Add(lowerVertices[i]);
            vertices.Add(upperVertices[i]);
            vertices.Add(upperVertices[next]);
            vertices.Add(lowerVertices[next]);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
        }

        // ���_�ƎO�p�`�����b�V���ɐݒ�
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        // ���b�V����ݒ�
        meshFilter.mesh = mesh;
        meshRenderer.material = faceMaterial;
    }

    void GenerateObjectsAtVertices(Vector3[] lowerVertices)
    {
        if (vertexObjectPrefab == null)
        {
            Debug.LogWarning("Vertex object prefab is not assigned.");
            return;
        }

        // ��ʂ̒��_�ɃI�u�W�F�N�g�𐶐�
        for (int i = 0; i < lowerVertices.Length; i++)
        {
            GameObject vertexObject = Instantiate(vertexObjectPrefab, lowerVertices[i], Quaternion.identity, transform);
            vertexObject.transform.localScale = vertexObjectScale;
        }
    }
}