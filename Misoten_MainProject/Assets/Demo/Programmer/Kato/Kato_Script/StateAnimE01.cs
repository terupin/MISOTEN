using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class StateAnimE01 : MonoBehaviour
{
    [SerializeField, Header("�e�X�g�Ɏg���I�u�W�F�N�g")]
    public GameObject Testobj;//�e�X�g�Ɏg���I�u�W�F�N�g
    [SerializeField, Header("�e�X�g�Ɏg���I�u�W�F�N�g�̃}�e���A���ʏ�")]
    public Material Defultmat;
    [SerializeField, Header("�e�X�g�Ɏg���I�u�W�F�N�g�̃}�e���A������")]
    public Material Testobjmat;


    //�c�؂� �ő���͗P�\ 1.7�b
    //�A��1 �ő���͗P�\ 1.2�b
    //�A��2 �ő���͗P�\ 0.5�b
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

    static public bool UkeL;
    static public bool UkeR;

    static public bool UKe__Ren01;
    static public bool UKe__Ren02;
    static public bool Attack;//�U���@�����蔻��Ɏg��

    private bool P_Input;//�p���C���͂��ꂽ���ǂ���

    [SerializeField, Header("�A�j���[�^�[")]
    public Animator _AnimatorE01;

    //private Matsunaga_Enemy01_State.Mai_State_ getState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (getState==Matsunaga_Enemy01_State.Mai_State_.Idle)
        {
            //_Animator.SetBool(,);
        }
        //else if (getState == Matsunaga_Enemy01_State.Mai_State_.Spin)
        {
            //_Animator.SetBool(,);
            //_Animator.SetBool(,);
        }
        //else if (getState == Matsunaga_Enemy01_State.Mai_State_.Goto)
        {
            //_Animator.SetBool(,);
        }
        //else if (getState == Matsunaga_Enemy01_State.Mai_State_.Attack)
        {
            //_Animator.SetBool(,);
        }
        //else if (getState == Matsunaga_Enemy01_State.Mai_State_.Jumpback)
        {
            //_Animator.SetBool(,);
        }
        //else if (getState == Matsunaga_Enemy01_State.Mai_State_.Kaihou)
        {
            //_Animator.SetBool(,);
        }


        //_Animator.SetBool(,);
        //_Animator.SetTrigger();
    }

    ////��������������͔��菈��
    //private void KatoUpdateAnim()
    //{
    //    if (E_State == Enemy_State_.Goto)
    //    {
    //        Testobj.SetActive(true);
    //        Testobj.transform.localScale += Vector3.one * Time.deltaTime * 0.1f;
    //    }

    //    //�c�؂�U��グ
    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
    //    {
    //        //UnityEditor.EditorApplication.isPaused = true;
    //        if (P_Input)
    //        {
    //            if (Check_Current_Time0 > 0.0f && Check_Time0 >= Check_Current_Time0)
    //            {
    //                Debug.Log("aaaaaaaa" + Check_Current_Time0);
    //                //�󂯗�������
    //                Debug.Log(Check_Current_Time0);
    //                //UnityEditor.EditorApplication.isPaused = true;
    //                Debug.Log("����" + Miburo_State._Katana_Direction);
    //                if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
    //                {
    //                    UkeL = true;
    //                    E01Anim.SetBool("UkeL", true);
    //                    UkeR = false;
    //                    E01Anim.SetBool("UkeR", false);
    //                    Debug.Log("����@����0L");
    //                    //UnityEditor.EditorApplication.isPaused = true;

    //                    E_State = Enemy_State_.Ukenagasare;
    //                    //E_State = Enemy_State_.Idle;

    //                }
    //                else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
    //                {
    //                    UkeL = false;
    //                    E01Anim.SetBool("UkeL", false);
    //                    UkeR = true;
    //                    E01Anim.SetBool("UkeR", true);
    //                    Debug.Log("����@����0R");
    //                    //UnityEditor.EditorApplication.isPaused = true;

    //                    E_State = Enemy_State_.Ukenagasare;
    //                    //E_State = Enemy_State_.Idle;
    //                }
    //                else
    //                {
    //                    UkeL = false;
    //                    UkeR = false;
    //                    //E_State = Enemy_State_.Jumpback;
    //                    //E_State = Enemy_State_.Idle;
    //                }
    //            }
    //            else
    //            {
    //                Debug.Log("����@���Ԑ؂�@" + Check_Current_Time0);
    //            }
    //        }
    //        else
    //        {
    //            Check_Current_Time0 += Time.deltaTime;
    //        }

    //        if (Check_Current_Time0 > 0.0f && Check_Time0 >= Check_Current_Time0)
    //        {
    //            Testobj.GetComponent<MeshRenderer>().material = Testobjmat;
    //        }
    //    }
    //    else
    //    {

    //        E01Anim.SetBool("UkeL", false);
    //        E01Anim.SetBool("UkeR", false);
    //        Check_Current_Time0 = 0;
    //    }

    //    //�c�؂�U�肨�낵
    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
    //    {
    //        Debug.Log(Check_Current_Time0);


    //        Check_Current_Time0 = 0;
    //        //UnityEditor.EditorApplication.isPaused = true;
    //    }

    //    //�A��1�U��グ
    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren01"))
    //    {
    //        //UnityEditor.EditorApplication.isPaused = true;
    //        if (P_Input)
    //        {
    //            if (Check_Current_Time1 > 0.0f && Check_Time1 > Check_Current_Time1)
    //            {
    //                if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
    //                {
    //                    if (!UKe__Ren01)
    //                    {
    //                        Debug.Log("����@����1");
    //                        Debug.Log("iiiiiiiii" + Check_Current_Time1);
    //                        //UnityEditor.EditorApplication.isPaused = true;
    //                        UKe__Ren01 = true;
    //                        E01Anim.SetBool("RenUke01", true);
    //                        E_State = Enemy_State_.Ukenagasare;
    //                        //E_State = Enemy_State_.Idle;

    //                    }
    //                }
    //                else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
    //                {
    //                    UKe__Ren01 = false;
    //                    E01Anim.SetBool("RenUke01", false);
    //                }

    //            }
    //            else
    //            {
    //                Debug.Log("����@���Ԑ؂�1�@" + Check_Current_Time1);
    //                //UnityEditor.EditorApplication.isPaused = true;
    //            }
    //        }
    //        else
    //        {
    //            Check_Current_Time1 += Time.deltaTime;
    //        }

    //        if (Check_Current_Time0 > 0.0f && Check_Time0 >= Check_Current_Time0)
    //        {
    //            Testobj.GetComponent<MeshRenderer>().material = Testobjmat;
    //        }

    //    }
    //    else
    //    {

    //    }

    //    //�A��1�U�肨�낵
    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
    //    {
    //        Debug.Log(Check_Current_Time1);
    //        //UnityEditor.EditorApplication.isPaused = true;
    //        Check_Current_Time1 = 0;


    //    }

    //    //�A��2�U��グ
    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren02"))
    //    {
    //        if (P_Input)
    //        {
    //            if (Check_Current_Time2 > 0.0f && Check_Time2 > Check_Current_Time2)
    //            {
    //                if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
    //                {
    //                    UKe__Ren02 = false;
    //                    E01Anim.SetBool("RenUke02", false);
    //                }
    //                else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
    //                {
    //                    if (!UKe__Ren02)
    //                    {
    //                        UKe__Ren02 = true;
    //                        E01Anim.SetBool("RenUke02", true);
    //                        UnityEditor.EditorApplication.isPaused = true;
    //                        Debug.Log("����@����2");
    //                    }
    //                }



    //            }
    //            else
    //            {
    //                Debug.Log("����@���Ԑ؂�2 " + Check_Current_Time2);

    //            }

    //        }
    //        else
    //        {
    //            Check_Current_Time2 += Time.deltaTime;
    //        }

    //        if (Check_Current_Time0 > 0.0f && Check_Time0 >= Check_Current_Time0)
    //        {
    //            Testobj.GetComponent<MeshRenderer>().material = Testobjmat;
    //        }
    //    }
    //    else
    //    {

    //    }

    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereL") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereR"))
    //    {
    //        UkeL = false;
    //        UkeR = false;
    //        Check_Current_Time0 = 0;
    //        Debug.Log("asd" + Check_Current_Time0);
    //        //UnityEditor.EditorApplication.isPaused = true;
    //    }

    //    //�A��2�U�肨�낵         
    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
    //    {

    //        Debug.Log(Check_Current_Time1);
    //        //UnityEditor.EditorApplication.isPaused = true;
    //        Check_Current_Time1 = 0;
    //    }

    //    //�A��2�U�肨�낵         
    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren2"))
    //    {

    //        Debug.Log(Check_Current_Time2);
    //        //UnityEditor.EditorApplication.isPaused = true;
    //        Check_Current_Time2 = 0;
    //    }

    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Hirumi"))
    //    {
    //        SetState(Enemy_State_.Stagger);
    //    }

    //    if (E_State == Enemy_State_.Spin)
    //    {
    //        UkeL = false;
    //        UkeR = false;
    //        P_Input = false;
    //        Effectflg = false;
    //    }

    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereR") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereL"))
    //    {

    //        Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
    //        if (Clone_Effect == null && !Effectflg)
    //        {
    //            Instantiate(S_Effect);
    //            Effectflg = true;
    //        }
    //    }

    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("RtoNagasare"))
    //    {
    //        Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
    //        if (Clone_Effect == null && !Effectflg)
    //        {
    //            Instantiate(S_Effect);

    //            Effectflg = true;
    //        }

    //        UKe__Ren01 = false;
    //        Check_Current_Time1 = 0;
    //        E01Anim.SetBool("RenUke01", false);
    //    }

    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("RtoLtoNagasare"))
    //    {
    //        Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
    //        if (Clone_Effect == null && !Effectflg)
    //        {
    //            Instantiate(S_Effect);
    //            Effectflg = true;
    //            //UnityEditor.EditorApplication.isPaused = true;
    //        }

    //        E01Anim.SetBool("RenUke02", false);
    //        UKe__Ren02 = false;
    //        Check_Current_Time2 = 0;
    //    }

    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren2") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
    //    {
    //        Attack = true;
    //        //UnityEditor.EditorApplication.isPaused = true;
    //        Effectflg = false;
    //    }
    //    else
    //    {
    //        Attack = false;
    //    }

    //    if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren01") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren02") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
    //    {
    //        Testobj.SetActive(true);
    //        //Testobj.transform.localScale += Vector3.one * Time.deltaTime;
    //    }
    //    else
    //    {
    //        Testobj.GetComponent<MeshRenderer>().material = Defultmat;
    //        Testobj.transform.localScale = Vector3.one;
    //        Testobj.SetActive(false);
    //        //Testobj.transform.localScale = Vector3.one;
    //        //Testobj.SetActive(false);
    //    }
    //}
    ////�����܂ŉ���
}
}
