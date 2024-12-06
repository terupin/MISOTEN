using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_E : MonoBehaviour
{
    //�����������
    [SerializeField, Header("�e�X�g�Ɏg���I�u�W�F�N�g")]
    public GameObject Testobj;//�e�X�g�Ɏg���I�u�W�F�N�g

    //�c�؂� �ő���͗P�\ 1.7�b
    //�A��1 �ő���͗P�\ 1.2�b
    //�A��2 �ő���͗P�\ 0.5�b

    [SerializeField, Header("�^�[�Q�b�g�ƂȂ�v���C���[")]
    public GameObject Target_P; // �G���^�[�Q�b�g����v���C���[�I�u�W�F�N�g

    [SerializeField, Header("�c�؂� �ő���͗P�\ 1.7�b")]
    public float Check_Time0;
    [SerializeField, Header("�A��1 �ő���͗P�\ 1.2�b")]
    public float Check_Time1;
    [SerializeField, Header("�A��2 �ő���͗P�\ 0.75�b")]
    public float Check_Time2;

    private float Check_Current_Time0;//���͊J�n����o�߂�������
    private float Check_Current_Time1;//���͊J�n����o�߂�������
    private float Check_Current_Time2;//���͊J�n����o�߂�������

    [SerializeField, Header("�a���G�t�F�N�g")]
    public GameObject S_Effect;
    public bool Effectflg;//���ʂȃG�t�F�N�g���o�Ȃ��悤�ɂ���

    private GameObject Clone_Effect;

    static public bool P_Wait;

    static public bool UkeL;
    static public bool UkeR;

    static public bool UKe__Ren01;
    static public bool UKe__Ren02;
    static public bool Attack;//�U���@�����蔻��Ɏg��

    private bool P_Input;//�p���C���͂��ꂽ���ǂ���

    [SerializeField, Header("�e�X�g�T�E���h")]
    public AudioClip[] _Sound_Test;
    AudioSource audioSource;
    //�����܂ŉ���

    private enum Mai_State_
    {
        Idle,
        Spin,
        Goto,
        Attack,
        Tategiri,
        Rengeki,
        Jumpback,
        Stagger
    };

    [SerializeField, Header("�c�؂�U���m��(%)"), Range(0, 100)]
    public int TategiriChance = 60; // �c�؂�U����I������m��

    [SerializeField, Header("�A���U���m��(%)"), Range(0, 100)]
    public int RenGekiChance = 40; // �A���U����I������m��

    [SerializeField, Header("�U���˒�(3.5)")]
    public float AttackLength = 3.5f; // �G���U���\�ȋ���

    private float P_E_Length; // �v���C���[�ƓG�Ƃ̋�����ێ�

    [SerializeField, Header("�ړ��X�s�[�h(12)")]
    public float MoveSpeed = 12; // �G���ړ����鑬�x

    public float maiclue_radius = 5.0f; //���񂷂�~�̔��a
    private float maiclue_x;    //����v�Z�p��X���W
    private float maiclue_y;    //����v�Z�p��Y���W
    private float maiclue_z;    //����v�Z�p��Z���W
    public float maiclue_speed; //����X�s�[�h

    private float maiclue_attacktime; //���񎞂̍U���Ԋu�̎���(�����i�[�p)
    public float maiclue_maxtime = 5.0f; //���񎞂̍U���Ԋu�̍ő厞��
    public float maiclue_mintime = 3.0f; //���񎞂̍U���Ԋu�̍ŏ�����

    private float maiclue_starttime;
    private float maiclue_elapsedtime;
    private Vector3 targetPoint;
    private bool maiclue_iscount = false;
    private Mai_State_ M_state;
    private int maiclue_spind; //���v��肩�����v��肩(�����i�[�p)
    private float angle = 0.0f; //����v�Z�p�̊p�x

    public Animator E01Anim; // �G�̃A�j���[�V�����𐧌䂷��Animator

    // Start is called before the first frame update
    void Start()
    {
        M_state = Mai_State_.Spin;
        maiclue_iscount = true;
        M_state = Mai_State_.Spin;
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[���ݒ肳��Ă���ꍇ�̂ݕ������������������s
        if (Target_P != null)
        {
            LookAtPlayer(); // �v���C���[�������������Ăяo��
        }
        P_E_Length = Vector3.Distance(Vector3.zero, gameObject.transform.position);

        State();
        SetAnimation();//�A�j���[�V�����Z�b�g
        KatoUpdateAnim();
    }

    // �v���C���[����������
    private void LookAtPlayer()
    {
        // �v���C���[�̕������v�Z
        Vector3 direction = (Vector3.zero - transform.position).normalized;

        // Y�������̉�]�̂ݓK�p
        direction.y = 0;

        // �v���C���[������������]���v�Z
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // �X���[�Y�ɉ�]������
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed);
    }


    // �U���^�C�v�����肷��
    private void DecideAttackType()
    {
        int randomValue = Random.Range(0, 100); // 0�`100�̃����_���l�𐶐�
        Debug.Log($"DecideAttackType: Random Value = {randomValue}, TategiriChance = {TategiriChance}");

        //E01Anim.SetBool("Walk", false); // �A�j���[�V���������Z�b�g
        Debug.Log($"��{E01Anim.GetBool("Walk")}");

        if (randomValue < TategiriChance)
        {
            // �c�؂�U����I��
            Debug.Log("�c�؂�U����I�����܂����I");
            M_state=Mai_State_.Tategiri;
            //UnityEditor.EditorApplication.isPaused = true;
        }
        else
        {
            // �A���U����I��
            Debug.Log("�A���U����I�����܂����I");
            M_state = Mai_State_.Rengeki;
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }

    // �w��A�j���[�V�������I�����Ă��邩�𔻒�
    private bool IsAnimationFinished(string animationName)
    {
        return E01Anim.GetCurrentAnimatorStateInfo(0).IsName(animationName) && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f;
    }

    //�X�e�[�g���s
    private void State()
    {
        switch (M_state)
        {
            //����
            case Mai_State_.Spin:

                if (maiclue_iscount)
                {
                    maiclue_starttime = Time.time;
                    maiclue_attacktime = Random.Range(maiclue_mintime, maiclue_maxtime);
                    maiclue_iscount = !maiclue_iscount;
                    maiclue_spind = Random.Range(1, 3);
                }

                maiclue_elapsedtime = Time.time - maiclue_starttime;

                // �p�x���X�V�i���x���l���j
                angle += maiclue_speed * Time.deltaTime;

                // �~����̈ʒu���v�Z
                maiclue_x = 0.0f + Mathf.Cos(angle) * maiclue_radius;
                //���v���
                if (maiclue_spind == 1)
                {
                    maiclue_z = 0.0f + Mathf.Sin(angle) * maiclue_radius;
                }
                //�����v����
                else
                {
                    maiclue_z = 0.0f - Mathf.Sin(angle) * maiclue_radius;
                }


                // �I�u�W�F�N�g���ړ�
                transform.position = new Vector3(maiclue_x, transform.position.y, maiclue_z);

                if (!(maiclue_elapsedtime <= maiclue_attacktime))
                {
                    M_state = Mai_State_.Goto;
                }

                break;

            //�ڋ�
            case Mai_State_.Goto:


                Vector3 direction = (Vector3.zero - transform.position).normalized;
                direction.y = 0;
                transform.position += direction * MoveSpeed * Time.deltaTime;

                if ((P_E_Length <= AttackLength))
                {
                    //UnityEditor.EditorApplication.isPaused = true;
                    M_state = Mai_State_.Attack;
                }

                break;

            //�U��
            case Mai_State_.Attack:

                
                DecideAttackType();

                break;

            //�U��
            case Mai_State_.Tategiri:

                //UnityEditor.EditorApplication.isPaused = true;
                break;

            //�U��
            case Mai_State_.Rengeki:

                //UnityEditor.EditorApplication.isPaused = true;
                break;

            //���̏ꏊ�ɖ߂�
            case Mai_State_.Jumpback:

                //UnityEditor.EditorApplication.isPaused = true;
                transform.position = targetPoint;

                //WaitForSeconds(2.5f); //�ҋ@

                if (transform.position == targetPoint)
                {
                    maiclue_iscount = !maiclue_iscount;

                    M_state = Mai_State_.Spin;
                }

                break;
        }
    }

    //�A�j���[�V����
    private void SetAnimation()
    {
        //        private enum Mai_State_
        //{
        //    Idle,
        //    Spin,
        //    Goto,
        //    Attack,
        //    Tategiri,
        //    RenGeki,
        //    Jumpback
        //};

        E01Anim.SetBool("Idle", M_state == Mai_State_.Idle);
        E01Anim.SetBool("Walk", M_state == Mai_State_.Spin || M_state == Mai_State_.Goto);
        E01Anim.SetBool("Tategiri", M_state == Mai_State_.Tategiri);
        E01Anim.SetBool("Rengeki", M_state == Mai_State_.Rengeki);
        E01Anim.SetBool("RenUke01", M_state == Mai_State_.Jumpback);

    }

    //�����������
    private void KatoUpdateAnim()
    {


        //�c�؂�U��グ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            //UnityEditor.EditorApplication.isPaused = true;
            if (P_Input)
            {
                if (Check_Current_Time0 > 0.0f && Check_Time0 >= Check_Current_Time0)
                {
                    //�󂯗�������
                    if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        UkeL = true;
                        E01Anim.SetBool("UkeL", true);
                        UkeR = false;
                        E01Anim.SetBool("UkeR", false);
                        Debug.Log("����@����0L");
                        audioSource.PlayOneShot(_Sound_Test[0]);
                        P_Wait = false;
                    }
                    else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        UkeL = false;
                        E01Anim.SetBool("UkeL", false);
                        UkeR = true;
                        E01Anim.SetBool("UkeR", true);
                        P_Wait = false;
                    }
                    else
                    {
                        UkeL = false;
                        UkeR = false;
                        if (!P_Wait)
                        {
                            P_Wait = true;
                        }
                    }
                }
                else
                {
                    Debug.Log("����@���Ԑ؂�@" + Check_Current_Time0);
                    audioSource.PlayOneShot(_Sound_Test[1]);
                    if (!P_Wait)
                    {
                        P_Wait = true;
                    }
                }
            }
            else
            {
                Check_Current_Time0 += Time.deltaTime;
            }
        }
        else
        {

            E01Anim.SetBool("UkeL", false);
            E01Anim.SetBool("UkeR", false);
            Check_Current_Time0 = 0;
        }

        //�c�؂�U�肨�낵
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
        {
            Debug.Log(Check_Current_Time0);
            Check_Current_Time0 = 0;
        }

        //�A��1�U��グ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren01"))
        {
            //UnityEditor.EditorApplication.isPaused = true;
            if (P_Input)
            {
                if (Check_Current_Time1 > 0.0f && Check_Time1 > Check_Current_Time1)
                {
                    if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        if (!UKe__Ren01)
                        {
                            Debug.Log("����@����1");
                            Debug.Log("iiiiiiiii" + Check_Current_Time1);
                            //UnityEditor.EditorApplication.isPaused = true;
                            UKe__Ren01 = true;
                            E01Anim.SetBool("RenUke01", true);
                            audioSource.PlayOneShot(_Sound_Test[0]);
                            P_Wait = false;

                        }
                    }
                    else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        UKe__Ren01 = false;
                        E01Anim.SetBool("RenUke01", false);
                    }

                }
                else
                {
                    Debug.Log("����@���Ԑ؂�1�@" + Check_Current_Time1);
                    audioSource.PlayOneShot(_Sound_Test[1]);
                    if (!P_Wait)
                    {
                        P_Wait = true;
                    }
                    //UnityEditor.EditorApplication.isPaused = true;
                }
            }
            else
            {
                Check_Current_Time1 += Time.deltaTime;
            }



        }
        else
        {

        }

        //�A��1�U�肨�낵
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
        {
            Debug.Log(Check_Current_Time1);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time1 = 0;


        }

        //�A��2�U��グ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren02"))
        {
            if (P_Input)
            {
                if (Check_Current_Time2 > 0.0f && Check_Time2 > Check_Current_Time2)
                {
                    if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        UKe__Ren02 = false;
                        E01Anim.SetBool("RenUke02", false);
                    }
                    else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        if (!UKe__Ren02)
                        {
                            UKe__Ren02 = true;
                            E01Anim.SetBool("RenUke02", true);
                            //UnityEditor.EditorApplication.isPaused = true;
                            Debug.Log("����@����2");
                            audioSource.PlayOneShot(_Sound_Test[0]);
                            P_Wait = false;
                        }
                    }



                }
                else
                {
                    Debug.Log("����@���Ԑ؂�2 " + Check_Current_Time2);
                    audioSource.PlayOneShot(_Sound_Test[1]);
                    if (!P_Wait)
                    {
                        P_Wait = true;
                    }

                }

            }
            else
            {
                Check_Current_Time2 += Time.deltaTime;
                P_Wait = false;
            }
        }
        else
        {

        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereL") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereR"))
        {
            M_state = Mai_State_.Jumpback;
            UkeL = false;
            UkeR = false;
            Check_Current_Time0 = 0;
            Debug.Log("asd" + Check_Current_Time0);
            //UnityEditor.EditorApplication.isPaused = true;
        }

        //�A��2�U�肨�낵         
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
        {

            Debug.Log(Check_Current_Time1);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time1 = 0;
        }

        //�A��2�U�肨�낵         
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren2"))
        {

            Debug.Log(Check_Current_Time2);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time2 = 0;
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Hirumi"))
        {
            M_state = Mai_State_.Stagger;
            //SetState(Enemy_State_.Stagger);
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            UkeL = false;
            UkeR = false;
            P_Input = false;
            Effectflg = false;
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereR") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereL"))
        {

            Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
            if (Clone_Effect == null && !Effectflg)
            {
                Instantiate(S_Effect);
                Effectflg = true;
            }
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("RtoNagasare"))
        {
            Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
            if (Clone_Effect == null && !Effectflg)
            {
                Instantiate(S_Effect);

                Effectflg = true;
            }

            UKe__Ren01 = false;
            Check_Current_Time1 = 0;
            E01Anim.SetBool("RenUke01", false);
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("RtoLtoNagasare"))
        {
            Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
            if (Clone_Effect == null && !Effectflg)
            {
                Instantiate(S_Effect);
                Effectflg = true;
                //UnityEditor.EditorApplication.isPaused = true;
            }

            E01Anim.SetBool("RenUke02", false);
            UKe__Ren02 = false;
            Check_Current_Time2 = 0;
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren2") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
        {
            P_Wait = false;
            Attack = true;
            //UnityEditor.EditorApplication.isPaused = true;
            Effectflg = false;
        }
        else
        {
            Attack = false;
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren01") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren02") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            Testobj.SetActive(true);
            if (Check_Current_Time0 > 0.0f && Check_Time0 > Check_Current_Time0)
            {

                Testobj.transform.localScale += Vector3.one * Time.deltaTime;
            }
            else if (Check_Current_Time0 > Check_Time0)
            {
                Testobj.transform.localScale = Vector3.one;
                Testobj.SetActive(false);
            }

            if (Check_Current_Time1 > 0.0f && Check_Time1 > Check_Current_Time1)
            {
                Testobj.transform.localScale += Vector3.one * Time.deltaTime;
            }
            else if (Check_Current_Time1 > Check_Time1)
            {
                Testobj.transform.localScale = Vector3.one;
                Testobj.SetActive(false);
            }

            if (Check_Current_Time2 > 0.0f && Check_Time2 > Check_Current_Time2)
            {
                Testobj.transform.localScale += Vector3.one * Time.deltaTime;
            }
            else if (Check_Current_Time2 > Check_Time2)
            {
                Testobj.transform.localScale = Vector3.one;
                Testobj.SetActive(false);
            }
        }
        else
        {
            Testobj.transform.localScale = Vector3.one;
            Testobj.SetActive(false);
        }

        if (IsAnimationFinished("NagasereR") || IsAnimationFinished("NagasereL"))
        {
            M_state = Mai_State_.Jumpback;
        }

        if ( IsAnimationFinished("Ren2") || IsAnimationFinished("Tategiri 0"))
        {
            M_state = Mai_State_.Jumpback;
        }

    }


    private IEnumerator WaitUKe__Ren02()
    {

        UKe__Ren02 = true;
        yield return null;
        UKe__Ren02 = false;

    }

    private IEnumerator WaitUKe__Ren01()
    {

        UKe__Ren01 = true;
        yield return null;
        UKe__Ren01 = false;

    }
    //�����܂ŉ���
}
