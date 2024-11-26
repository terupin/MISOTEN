using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miburo_State : MonoBehaviour
{
    private bool _Step;
    private bool _Parry;
    private bool _Attack01;
    private bool _Attack02;


    private int _Katana_Direction;

    [SerializeField, Header("みぶろアニメーター")]
    public Animator Miburo_Animator;

    [SerializeField, Header("待ち時間(ステップ)")]
    public float Step_WaitTime;

    [SerializeField, Header("待ち時間(パリィ)")]
    public float Parry_WaitTime;

    [SerializeField, Header("待ち時間(攻撃1段目)")]
    public float Attack01_WaitTime;

    [SerializeField, Header("待ち時間(攻撃2段目)")]
    public float Attack02_WaitTime;

    [SerializeField, Header("待ち時間(受け流し方向セット)")]
    public float Katana_DirectionSet_WaitTime;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //R1ボタン押下
        if (UnityEngine.Input.GetKeyDown("joystick button 5"))
        {
            if (_Attack01)
            {
                StartCoroutine(Miburo_Attack02());

            }
            else
            {
                StartCoroutine(Miburo_Attack01());
            }

        }
        //L1ボタン押下
        if (UnityEngine.Input.GetKeyDown("joystick button 4"))
        {
            StartCoroutine(Miburo_Parry());
        }

        //Aボタン押下
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            StartCoroutine(Miburo_Step());
        }

        if (_Parry)//受け流し入力可能時に受け流し入力
        {
            if (_Katana_Direction == -1)
            {
                _Katana_Direction = GetKatana_Direction();
            }
            else
            {
                Debug.Log("入力完了\n入力方向　" + _Katana_Direction);
            }
        }
        else
        {
            _Katana_Direction = -1;
        }

        Miburo_Animator.SetBool("Gard",_Parry);
    }

    //コルーチン()
    private IEnumerator Miburo_Attack01()
    {
        if (!_Attack01)
        {
            _Attack01 = true;
            Debug.Log("攻撃1開始");
            yield return new WaitForSeconds(Attack01_WaitTime);
            Debug.Log("攻撃1待ち時間終了");
            _Attack01 = false;
        }
        else
        {
            Debug.Log("待ち時間です。入力は反映されません。");
        }
    }

    //コルーチン()
    private IEnumerator Miburo_Attack02()
    {

        if (!_Attack02)
        {
            _Attack02 = true;
            Debug.Log("攻撃2開始");
            yield return new WaitForSeconds(Attack02_WaitTime);
            Debug.Log("攻撃2待ち時間終了");
            _Attack02 = false;
        }
        else
        {
            Debug.Log("待ち時間です。入力は反映されません。");
        }
    }

    //コルーチン()
    private IEnumerator Miburo_Parry()
    {
        if (!_Parry)
        {
            _Parry = true;
            Debug.Log("パリイ開始");
            yield return new WaitForSeconds(Parry_WaitTime);
            Debug.Log("パリイ待ち時間終了");
            _Parry = false;
        }
        else
        {
            Debug.Log("待ち時間です。入力は反映されません。");
        }

    }

    //コルーチン()
    private IEnumerator Miburo_Step()
    {
        if (!_Step)
        {
            _Step = true;
            Debug.Log("ステップ開始");
            yield return new WaitForSeconds(Step_WaitTime);
            Debug.Log("ステップ待ち時間終了");
            _Step = false;
        }
        else
        {
            Debug.Log("待ち時間です。入力は反映されません。");
        }
    }

    // 指定アニメーションが終了しているかを判定
    private bool AnimationFinished(string animationName)
    {
        return !Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)
            || Miburo_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
    }

    //コントローラーから斬撃の方向を取得
    int GetKatana_Direction()
    {
        int Katana_Direction = -1;
        float h = UnityEngine.Input.GetAxis("Horizontal2");
        float v = UnityEngine.Input.GetAxis("Vertical2");

        float degree = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }

        if (Katana_Direction == -1)
        {

            if (MathF.Abs(v) <= 0.15f || MathF.Abs(h) <= 0.15f)
            {
                Katana_Direction = -1;
            }
            else
            {
                if (degree < 22.5f) { Katana_Direction = 0; }
                else if (degree < 67.5f) { Katana_Direction = 1; }
                else if (degree < 112.5f) { Katana_Direction = 2; }
                else if (degree < 157.5f) { Katana_Direction = 3; }
                else if (degree < 202.5f) { Katana_Direction = 4; }
                else if (degree < 247.5f) { Katana_Direction = 5; }
                else if (degree < 292.5f) { Katana_Direction = 6; }
                else if (degree < 337.5f) { Katana_Direction = 7; }
                else { Katana_Direction = 0; }
            }
        }

        return Katana_Direction;
    }
}
