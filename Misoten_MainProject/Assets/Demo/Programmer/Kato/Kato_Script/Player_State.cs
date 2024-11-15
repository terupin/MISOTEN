using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State : MonoBehaviour
{
    public Animator Player_Animator;

    private bool PushFlg_L = false;//L押下フラグ
    private bool PushFlg_R = false;//R押下フラグ
    static public int Katana_Direction = -1;

    [SerializeField, Header("連撃タイム(0.5)")]
    public float RengekiTime;//連撃タイム
    [SerializeField, Header("連撃最大カウント(2)")]
    public int RengekiMaxCount;
    private int RengekiCount;//連撃カウント
    private bool RengekiFlg;//連撃フラグ
    private float RengekiCurrentTime = 0.0f;//連撃カレントタイム

    [SerializeField, Header("受け流しタイム(0.5)")]
    public float Uke_Time;//受けタイム
    private bool Uke_Input_Flg;//受け入力フラグ
    private float Uke_CurrentTime = 0.0f;//受けカレントタイム

    [SerializeField, Header("カウンタータイム(0.5)")]
    public float Counter_Time;//カウンタータイム
    private bool Counter_Input_Flg;//カウンター入力フラグ
    private float Counter_CurrentTime = 0.0f;//カウンターカレントタイム

    private bool Counter_Flg;//カウンター成功フラグ

    public static bool G_Flg;//ガードフラグ
    public static bool A_Flg;//アタックフラグ

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
        //アイドル
        if (m_State == MIburo_State.Idle)
        {
            if (UnityEngine.Input.GetKeyDown("joystick button 5"))
            {
                m_State = MIburo_State.Attack01;
            }
        }

        //ガード(受け流し入力待ち)
        if (m_State == MIburo_State.Gurd)
        {

        }

        //移動
        if (m_State == MIburo_State.Run)
        {

        }

        //通常攻撃1段目
        if (m_State == MIburo_State.Attack01)
        {
            if (UnityEngine.Input.GetKeyDown("joystick button 5"))
            {
                m_State = MIburo_State.Attack01;
            }
        }

        //通常攻撃2段目
        if (m_State == MIburo_State.Attack02)
        {

        }

        //カウンター1段目
        if (m_State == MIburo_State.Counter01)
        {

        }

        //カウンター1段目
        if (m_State == MIburo_State.Counter02)
        {

        }
        //ダメージ
        if (m_State == MIburo_State.Damage)
        {

        }
        //ステップ
        if (m_State == MIburo_State.Step)
        {

        }



    }


}
