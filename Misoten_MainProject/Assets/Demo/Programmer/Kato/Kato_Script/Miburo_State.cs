using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class Miburo_State : MonoBehaviour
{
    //フラグ
    private bool _Step;
    private bool _Parry;
    private bool _Attack01;
    private bool _Attack02;
    private bool _Run;
    private bool _Ukenagashi_R;
    private bool _Ukenagashi_L;

    [SerializeField, Header("取得したいアニメーターのステート名")]
    public string StateName;
    private string currentStateName;

    //刀の方向
    private int _Katana_Direction;

    [SerializeField, Header("移動スピード")]
    public float Move_Speed;
    

    [SerializeField, Header("敵プレハブ")]
    public GameObject Target;

    [SerializeField, Header("プレイヤープレハブ")]
    public GameObject Player;

    [SerializeField, Header("カメラ")]
    public GameObject Camera_o;

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

    [SerializeField, Header("ボーン")]
    public GameObject _Born;

    [SerializeField, Header("斬撃エフェクト(テスト用)")]
    public GameObject S_Effect;

    [SerializeField, Header("")]
    static public bool _Uke_Input;

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
                StartCoroutine(Counter_Timing_Input());
            }
        }
        else
        {
            _Katana_Direction = -1;
        }

        Player_Run_Input();//走行入力がされているかのフラグ

        if (_Attack01 ||_Attack02)
        {         
        }
        else
        {
            if(_Run)
            {
                Player_Run();//走行処理
            }      
        }

        if(_Parry)
        {
            if(_Katana_Direction==0|| _Katana_Direction == 1 || _Katana_Direction == 2 || _Katana_Direction == 7 )
            {
                Miburo_Animator.SetTrigger("UkenagashiL");

            }
            else if(_Katana_Direction == 0 || _Katana_Direction == 1 || _Katana_Direction == 2 || _Katana_Direction == 7)
            {
                Miburo_Animator.SetTrigger("UkenagashiR");
            }
            
        }

        //判定をアニメーターへ
        Miburo_Animator.SetBool("Gurd", _Parry);
        Miburo_Animator.SetBool("Run", _Run);
        Miburo_Animator.SetInteger("KatanaD", _Katana_Direction);

        //HP0以下ならゲームオーバー
        if(Kato_Status_P.NowHP<=0)
        {
            Miburo_Animator.SetBool("GameOver", true);
        }


        //以下テスト用

        //ダメージ
        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
        {
            Miburo_Animator.SetTrigger("Damage");
        }

        //縦切り受け流し左(みぶろポジション前３右0.5)
        if (UnityEngine.Input.GetKeyDown(KeyCode.J))
        {
            Miburo_Animator.SetTrigger("UkenagashiL");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.V))
        {
            Instantiate(S_Effect);
        }

        //縦切り受け流し左(みぶろポジション前３右1)
        if (UnityEngine.Input.GetKeyDown(KeyCode.K))
        {
            Miburo_Animator.SetTrigger("UkenagashiR");
        }

        //連撃受け流し1回目(みぶろポジション前３右0.5)
        if (UnityEngine.Input.GetKeyDown(KeyCode.M))
        {
            Miburo_Animator.SetTrigger("UkenagashiR");
        }

        //連撃受け流し2回目(みぶろポジション前３右0.5)
        if (UnityEngine.Input.GetKeyDown(KeyCode.N))
        {
            Miburo_Animator.SetTrigger("Rengeki02");
            Miburo_Animator.SetBool("Counter",true);
        }
        Vector3 a = gameObject.transform.position;

        //gameObject.transform.position =  new Vector3(a.x+_Born.transform.position.x, 0, a.z+ _Born.transform.position.z);
        GetCurrentAnimationStateName();
    }

    //コルーチン(攻撃1)
    private IEnumerator Miburo_Attack01()
    {
        if (!_Attack01)
        {
            _Attack01 = true;
            Debug.Log("攻撃1開始");
            Miburo_Animator.SetBool("Attack01",true);
            yield return new WaitForSeconds(Attack01_WaitTime);
            Debug.Log("攻撃1待ち時間終了");
            Miburo_Animator.SetBool("Attack01", false);
            _Attack01 = false;
        }
        else
        {
            Debug.Log("待ち時間です。入力は反映されません。");
        }
    }

    //コルーチン(攻撃2)
    private IEnumerator Miburo_Attack02()
    {

        if (!_Attack02)
        {
            _Attack02 = true;
            Debug.Log("攻撃2開始");
            Miburo_Animator.SetBool("Attack02", true);
            yield return new WaitForSeconds(Attack02_WaitTime);
            Debug.Log("攻撃2待ち時間終了");
            Miburo_Animator.SetBool("Attack02", false);
            _Attack02 = false;
        }
        else
        {
            Debug.Log("待ち時間です。入力は反映されません。");
        }
    }

    //コルーチン(受け流し構え)
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

    private IEnumerator Counter_Timing_Input()
    {
        if (!_Uke_Input)
        {
            _Uke_Input = true;
            Debug.Log("受け開始");
            yield return new WaitForSeconds(3);
            Debug.Log("受け待ち時間終了");
            _Uke_Input = false;
        }
        else
        {
            Debug.Log("待ち時間です。入力は反映されません。");
        }

    }

    //コルーチン(ステップ)
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

    //移動
    public void Player_Run()
    {
        float moveX = Input.GetAxis("Vertical");
        float RotateY = Input.GetAxis("Horizontal");
        float degree = Mathf.Atan2(RotateY, moveX) * Mathf.Rad2Deg;//コントローラー角度取得

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, Camera_o.transform.localEulerAngles.y + degree, 0));
        gameObject.transform.position += gameObject.transform.forward * Move_Speed * Time.deltaTime;
    }

    //移動入力
    public void Player_Run_Input()
    {
        float moveX = Input.GetAxis("Vertical");
        float RotateY = Input.GetAxis("Horizontal"); 

        if (MathF.Abs(moveX) >= 0.05f || MathF.Abs(RotateY) >= 0.05f)
        {
            if (Kato_Status_P.NowHP > 0)
            {
                _Run = true;
            }
        }
        else
        {
            _Run = false;
        }
    }



    //アニメーターからステート名を取得
    void GetCurrentAnimationStateName()
    {
        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("UKE"))
        {
            gameObject.transform.position += gameObject.transform.forward * Time.deltaTime*5.5f;
            //UnityEditor.EditorApplication.isPaused = true;
            //currentStateName = "Idle";
            gameObject.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        }
        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("UKE2"))
        {
            gameObject.transform.position += gameObject.transform.forward * Time.deltaTime * 2.5f;
            //UnityEditor.EditorApplication.isPaused = true;
            //currentStateName = "Idle";
            gameObject.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        }
        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("UKE3"))
        {
            gameObject.transform.position -= gameObject.transform.forward * Time.deltaTime * 0.1f;
            //UnityEditor.EditorApplication.isPaused = true;
            //currentStateName = "Idle";
            gameObject.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        }
  

        //else
        {

        }
        //if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName(StateName))
        //{
        //    currentStateName = "Run";
        //}
        //Debug.Log(currentStateName);
    }
}
