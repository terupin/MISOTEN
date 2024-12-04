using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Miburo_State : MonoBehaviour
{
    //フラグ
    private bool _Step;
    static public bool _Parry;
    static public bool _Attack01;
    static public bool _Attack02;
    private bool _Run;
    private bool _Ren11;
    private bool _Ren22;

    private bool _wait;


    static public bool _Stick_Input;

    static public bool _Uke_Input;//受け流し入力

    //刀の方向
    static public int _Katana_Direction;

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

    [SerializeField, Header("斬撃エフェクト(debugテスト用)")]
    public GameObject S_Effect;

    [SerializeField, Header("モデル(debugテスト用)")]
    public GameObject Test;

    [SerializeField, Header("マテリアル(debugテスト用)")]
    public Material TestMat;

    public Material Reset;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        _Katana_Direction = -1;

         Test.AddComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //HP0以下ならゲームオーバー
        if (Kato_Status_P.NowHP <= 0)
        {
            Miburo_Animator.SetBool("GameOver", true);
            return;
        }
        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("Battou"))
        {
            return;
        }

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
            //StartCoroutine(Miburo_Parry());
            _Parry = true;
            return;
        }
        else
        {
          
        }

        //Aボタン押下
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            StartCoroutine(Miburo_Step());
        }

        //if (_Parry)//受け流し入力可能時に受け流し入力
        //{

        //    StartCoroutine(Miburo_Stick());

        //}






        Player_Run_Input();//走行入力がされているかのフラグ

        if (_Attack01 ||_Attack02|| _Stick_Input)
        {         
        }
        else
        {
            if(_Run)
            {
                Player_Run();//走行処理
            }      
        }


        ////判定をアニメーターへ

        Miburo_Animator.SetBool("Run", _Run);
        //Miburo_Animator.SetInteger("KatanaD", _Katana_Direction);


        //if (_Uke_Input)
        //{
        //    if (_Katana_Direction == 0 || _Katana_Direction == 1 || _Katana_Direction == 2 || _Katana_Direction == 7)
        //    {
        //        Miburo_Animator.SetBool("UkenagashiL", true);
        //    }
        //    else if (_Katana_Direction == 3 || _Katana_Direction == 4 || _Katana_Direction == 5 || _Katana_Direction == 6)
        //    {
        //        Miburo_Animator.SetBool("UkenagashiR", true);
        //    }
        //    else
        //    {
        //        Miburo_Animator.SetBool("UkenagashiL", false);
        //        Miburo_Animator.SetBool("UkenagashiR", false);
        //    }

        //}
        //else
        //{
        //    Miburo_Animator.SetBool("UkenagashiL", false);
        //    Miburo_Animator.SetBool("UkenagashiR", false);
        //}

        //if(Kato_Matsunaga_Enemy_State.UkeL)
        //{
        //    Miburo_Animator.SetBool("UkenagashiL", true);
        //}
        //else
        //{
        //    Miburo_Animator.SetBool("UkenagashiL", false);
        //}

        //if (Kato_Matsunaga_Enemy_State.UkeR)
        //{
        //    Miburo_Animator.SetBool("UkenagashiR", true);
        //}
        //else
        //{
        //    Miburo_Animator.SetBool("UkenagashiR", false);
        //}

        //if (_Katana_Direction == -1)
        //{
        //    StartCoroutine(Miburo_Parry_Wait());
        //}
        //else
        //{
        //    _Parry = false;
        //    Debug.Log("このむき　" + _Katana_Direction);
        //    UnityEditor.EditorApplication.isPaused = true;
        //}


        //if (Kato_Matsunaga_Enemy_State.UKe__Ren01)
        //{
        //    if(!_Ren11)
        //    {
        //        Miburo_Animator.SetTrigger("Ren11");
        //        _Ren11= true;        
        //    }  
        //}
        //else
        //{
        //    _Ren11 = false;
        //}
        //if (Kato_Matsunaga_Enemy_State.UKe__Ren02)
        //{
        //    if(!_Ren22)
        //    {
        //        Miburo_Animator.SetTrigger("Ren22");
        //        _Ren22= true;
        //    }

        //}
        //else
        //{
        //    _Ren22 = false;
        //}



        if (K_Matsunaga_Enemy_State.UKe__Ren01 &&_Parry)
        {
            if (!_Ren11)
            {
                _Ren11 = true;
                StartCoroutine(Miburo_Stick());
               //UnityEditor.EditorApplication.isPaused = true;
            }

        }
        else if(!K_Matsunaga_Enemy_State.UKe__Ren01 && _Parry)
        {
            //Debug.Log("判定　タイムオーバー2");
            StartCoroutine(Miburo_Parry_Wait());
            //UnityEditor.EditorApplication.isPaused = true;
        }

        if (K_Matsunaga_Enemy_State.UKe__Ren02 && _Parry)
        {
            if (!_Ren22)
            {
                _Ren22 = true;
                StartCoroutine(Miburo_Stick());
                //UnityEditor.EditorApplication.isPaused = true;
            }
        }
        else if (!K_Matsunaga_Enemy_State.UKe__Ren02 && _Parry)
        {
            Debug.Log("判定　タイムオーバー2");
            StartCoroutine(Miburo_Parry_Wait());
        }

        if (_Stick_Input)
        {
             GetKatana_Direction();
        }

        //Miburo_Animator.SetBool("Ren01", Kato_Matsunaga_Enemy_State.UKe__Ren01);
        //Miburo_Animator.SetBool("Ren02", Kato_Matsunaga_Enemy_State.UKe__Ren02);  

        ////縦切り受け流し左(みぶろポジション前３右0.5)
        //if (UnityEngine.Input.GetKeyDown(KeyCode.J))
        //{
        //    Miburo_Animator.SetTrigger("UkenagashiL");
        //}

        GetCurrentAnimationStateName();//ステート取得して
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

        }
        else
        {
            Debug.Log("待ち時間です。入力は反映されません。");
        }

    }

    //コルーチン(Stick)
    private IEnumerator Miburo_Stick()
    {
        if (!_Stick_Input)
        {
            _Stick_Input = true;
            Debug.Log("スティック");
            Miburo_Animator.SetBool("Gurd", _Stick_Input);
            yield return new WaitForSeconds(Parry_WaitTime);
            Debug.Log("スティック待ち時間終了");
            Input_Check();
            _Stick_Input = false;
            Miburo_Animator.SetBool("Gurd", _Stick_Input);
            //UnityEditor.EditorApplication.isPaused = true;

        }
        else
        {
            Debug.Log("待ち時間です。入力は反映されません。");
        }

    }

    //コルーチン(構えウェイト)
    private IEnumerator Miburo_Parry_Wait()
    {
        Test.GetComponent<MeshRenderer>().material = TestMat;
        //UnityEditor.EditorApplication.isPaused = true;
        yield return new WaitForSeconds(0.4f);//24フレーム(.4秒)
        Test.GetComponent<MeshRenderer>().material = Reset;

        _Parry = false;
        _Ren11 = false;
        _Ren22 = false;
        _Parry = false;
    }

    private IEnumerator Counter_Timing_Input()
    {
        if (!_Uke_Input)
        {
            _Uke_Input = true;
            Debug.Log("受け開始");
            yield return new WaitForSeconds(1);
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
    void GetKatana_Direction()
    {

        float h = UnityEngine.Input.GetAxis("Horizontal2");
        float v = UnityEngine.Input.GetAxis("Vertical2");

        float degree = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

        Debug.Log("判定　スティック");
        //UnityEditor.EditorApplication.isPaused = true;
        if (degree < 0)
        {
            degree += 360;
        }

        //if (Katana_Direction == -1)
        {

            if (MathF.Abs(v) <= 0.15f || MathF.Abs(h) <= 0.15f)
            {
                _Katana_Direction = -1;
            }
            else
            {
                if (degree < 22.5f) { _Katana_Direction = 0; }
                else if (degree < 67.5f) { _Katana_Direction = 1; }
                else if (degree < 112.5f) { _Katana_Direction = 2; }
                else if (degree < 157.5f) { _Katana_Direction = 3; }
                else if (degree < 202.5f) { _Katana_Direction = 4; }
                else if (degree < 247.5f) { _Katana_Direction = 5; }
                else if (degree < 292.5f) { _Katana_Direction = 6; }
                else if (degree < 337.5f) { _Katana_Direction = 7; }
                else { _Katana_Direction = 0; }
            }        
        }
    }

    //移動
    public void Player_Run()
    {
        float moveX = Input.GetAxis("Vertical");
        float RotateY = Input.GetAxis("Horizontal");
        float degree = Mathf.Atan2(RotateY, moveX) * Mathf.Rad2Deg;//コントローラー角度取得

        Rigidbody rb = GetComponent<Rigidbody>();

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, Camera_o.transform.localEulerAngles.y + degree, 0));
        rb.position+=gameObject.transform.forward * Move_Speed * Time.deltaTime;
        //gameObject.transform.position += gameObject.transform.forward * Move_Speed * Time.deltaTime;
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

    //入力成功確認
    public void Input_Check()
    {
        if(_Katana_Direction==-1)
        {
            Debug.Log("判定　失敗");
            if(!_wait)
            {
                StartCoroutine(Miburo_Parry_Wait());
                _wait = true;
            }
           
            //UnityEditor.EditorApplication.isPaused = true;
        }
        else
        {
            Debug.Log("判定　成功");
            //UnityEditor.EditorApplication.isPaused = true;
            if(_Katana_Direction==0|| _Katana_Direction == 1 || _Katana_Direction == 2 || _Katana_Direction == 7 )
            {
                //ここに受け流し
                Debug.Log("判定　右");
                //UnityEditor.EditorApplication.isPaused = true;
            }
            else if(_Katana_Direction == 3 || _Katana_Direction == 4 || _Katana_Direction == 5 || _Katana_Direction == 6)
            {
                //ここに受け流し
                Debug.Log("判定　左");
                //UnityEditor.EditorApplication.isPaused = true;
            }
        }

    
    }

    //アニメーターからステート名を取得
    void GetCurrentAnimationStateName()
    {
        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("UKE"))
        {
            gameObject.transform.position += gameObject.transform.forward * Time.deltaTime*5.5f;
            gameObject.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        }
        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("UKE2"))
        {
            gameObject.transform.position += gameObject.transform.forward * Time.deltaTime * 2.5f;
            gameObject.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        }
        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("UKE3"))
        {
            gameObject.transform.position -= gameObject.transform.forward * Time.deltaTime * 0.1f;
            gameObject.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        }
  
    }

    //受け流し成功時の位置調整
    void CounterPosSet()
    {
        gameObject.transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z);
    }

    //ゲームオーバー
    private IEnumerator Gameover()
    {

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("OverScene");

    }

    //当たり判定
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EWeapon")
        {
            GameObject Miburo_Box = GameObject.Find("Player");
            if (Miburo_Box && K_Matsunaga_Enemy_State.Attack)
            {
                gameObject.AddComponent<Damage_Flash>();
                Miburo_Animator.SetTrigger("Damage");
                //gameObject.transform.position -= gameObject.transform.forward*2.5f;
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.AddForce(Target.transform.forward*5);            
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision. == "EWeapon")
    }
}
