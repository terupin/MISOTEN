using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State : MonoBehaviour
{
    public Animator Player_Animator;

    private bool PushFlg_L = false;//L�����t���O
    private bool PushFlg_R = false;//R�����t���O
    static public int Katana_Direction = -1;

    [SerializeField, Header("�A���^�C��(0.5)")]
    public float RengekiTime;//�A���^�C��
    [SerializeField, Header("�A���ő�J�E���g(2)")]
    public int RengekiMaxCount;
    private int RengekiCount;//�A���J�E���g
    private bool RengekiFlg;//�A���t���O
    private float RengekiCurrentTime = 0.0f;//�A���J�����g�^�C��

    [SerializeField, Header("�󂯗����^�C��(0.5)")]
    public float Uke_Time;//�󂯃^�C��
    private bool Uke_Input_Flg;//�󂯓��̓t���O
    private float Uke_CurrentTime = 0.0f;//�󂯃J�����g�^�C��

    [SerializeField, Header("�J�E���^�[�^�C��(0.5)")]
    public float Counter_Time;//�J�E���^�[�^�C��
    private bool Counter_Input_Flg;//�J�E���^�[���̓t���O
    private float Counter_CurrentTime = 0.0f;//�J�E���^�[�J�����g�^�C��

    private bool Counter_Flg;//�J�E���^�[�����t���O

    public static bool G_Flg;//�K�[�h�t���O
    public static bool A_Flg;//�A�^�b�N�t���O

    public enum MIburo_State
    {
        Idle,
        Run,
        Gurd,
        Attack01,
        Attack02,
        Counter01,
        Counter02,
        Damage,
        Step,

    };

    private MIburo_State m_State;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�A�C�h��
        if (m_State == MIburo_State.Idle)
        {
            if (UnityEngine.Input.GetKeyDown("joystick button 5"))
            {
                m_State = MIburo_State.Attack01;
            }
        }

        //�K�[�h(�󂯗������͑҂�)
        if (m_State == MIburo_State.Gurd)
        {

        }

        //�ړ�
        if (m_State == MIburo_State.Run)
        {

        }

        //�ʏ�U��1�i��
        if (m_State == MIburo_State.Attack01)
        {
            if (UnityEngine.Input.GetKeyDown("joystick button 5"))
            {
                m_State = MIburo_State.Attack01;
            }
        }

        //�ʏ�U��2�i��
        if (m_State == MIburo_State.Attack02)
        {

        }

        //�J�E���^�[1�i��
        if (m_State == MIburo_State.Counter01)
        {

        }

        //�J�E���^�[1�i��
        if (m_State == MIburo_State.Counter02)
        {

        }
        //�_���[�W
        if (m_State == MIburo_State.Damage)
        {

        }
        //�X�e�b�v
        if (m_State == MIburo_State.Step)
        {

        }



    }


}
