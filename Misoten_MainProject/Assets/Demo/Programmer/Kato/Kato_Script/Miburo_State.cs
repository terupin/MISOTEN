using System;
using System.Collections;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

//UnityEditor.EditorApplication.isPaused = true;

public class Miburo_State : MonoBehaviour
{
    //フラグ
    private bool _Step;
    private bool _StepMuteki;
    private bool _CoolDown;
    static public bool _Parry;
    static public bool _Parry_Timing;//パリイ入力した瞬間
    static public bool _Attack01;
    static public bool _Attack02;

    static public bool _CounterL;
    static public bool _CounterR;
    static public bool _RenCounter01;
    static public bool _RenCounter02;
    private bool _Ren11;
    private bool _Ren22;

    static public bool _wait;

    private bool StickL;//スティック左スティック
    private bool StickR;//スティック右スティック

    private bool _KnockBack;//ノックバック
    private bool _Muteki;//無敵
    Vector3 dir;//ノックバックで使用する

    private bool sippai;//入力失敗時

   [SerializeField, Header("ノックバックスピード")]
    public float KnockBack_Speed;

    [SerializeField, Header("ノックバック時間(秒)")]
    public float KnockBack_Time;

    [SerializeField, Header("ステップスピード")]
    public float Step_Speed;
    [SerializeField, Header("ステップ時間(秒)")]
    public float Step_Time;
    [SerializeField, Header("待ち時間(ステップ終了後)")]
    public float Step_WaitTime;

    static public bool _Stick_Input;

    static public bool _Uke_Input;//受け流し入力

    //刀の方向
    static public int _Katana_Direction;

    [SerializeField, Header("敵プレハブ")]
    public GameObject Target;

    [SerializeField, Header("プレイヤープレハブ")]
    public GameObject Player;

    [SerializeField, Header("カメラ")]
    public GameObject Camera_o;

    [SerializeField, Header("みぶろアニメーター")]
    public Animator Miburo_Animator;

    [SerializeField, Header("待ち時間(ダメージ後無敵)")]
    public float Damage_MutekiTime;

    [SerializeField, Header("待ち時間(パリィ)")]
    public float Parry_WaitTime;

    [SerializeField, Header("攻撃時間(攻撃1段目)")]
    public float Attack01_Time;
    [SerializeField, Header("待ち時間(攻撃1段目)")]
    public float Attack01_WaitTime;

    [SerializeField, Header("攻撃時間(攻撃2段目)")]
    public float Attack02_Time;
    [SerializeField, Header("待ち時間(攻撃2段目)")]
    public float Attack02_WaitTime;

    [SerializeField, Header("受け流し方向セット時間")]
    public float Katana_DirectionSet_Time;
    [SerializeField, Header("待ち時間(受け流し方向セット失敗)")]
    public float Katana_DirectionSet_WaitTime;

    [SerializeField, Header("玉(debugテスト受け流し入力用)")]
    public GameObject Test;

    [SerializeField, Header("玉(debugテスト回避判定用玉)")]
    public GameObject _StepBoll;

    [SerializeField, Header("マテリアル(debugテスト用)")]
    public Material TestMat;
    public Material M_UkenagasiIcon;
    public Material M_StepIcon;
    public Material M_AttackIcon;

    public Material Reset;

    [SerializeField, Header("ゲームオーバーシーン名")]
    public string SceneName;

    [SerializeField, Header("当たり判定ボックス")]
     public GameObject Miburo_HitBox;
    private GameObject M_HitBox;

    [SerializeField, Header("攻撃エフェクト")]
    public ParticleSystem slash_effect;

    [SerializeField, Header("被ダメージ")]
    public AudioClip AudioClip00;

    private AudioSource audioSource_P;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        _Katana_Direction = -1;

        Test.AddComponent<MeshRenderer>();
        audioSource_P = GetComponent<AudioSource>();
        //UI
        M_UkenagasiIcon.SetFloat("_CoolDown", 1.0f);
        M_StepIcon.SetFloat("_CoolDown", 1.0f);
        M_AttackIcon.SetFloat("_CoolDown", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

        M_HitBox= GameObject.Find("Player");

        //HP0以下ならゲームオーバー
        if (Kato_Status_P.instance.NowHP <= 0)
        {
            Miburo_Animator.SetBool("GameOver", true);
            StartCoroutine(Gameover());
            return;
        }

        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("Battou"))
        {
            return;
        }

        if(_KnockBack ||_StepMuteki)
        {

        }
        else
        {
            gameObject.transform.LookAt(Target.transform);
        }





        //R1ボタン押下(攻撃)
        if (UnityEngine.Input.GetKeyDown("joystick button 5"))
        {
            if (_Attack01)
            {
                StartCoroutine(ChangeCoolDown(M_AttackIcon, 0.0f, 1.0f, Attack02_Time + Attack02_WaitTime));
                StartCoroutine(Miburo_Attack02());
                Instantiate(slash_effect, transform.position, transform.rotation);
            }
            else
            {
                StartCoroutine(ChangeCoolDown(M_AttackIcon, 0.0f, 1.0f, Attack01_Time + Attack01_WaitTime));
                StartCoroutine(Miburo_Attack01());
                Instantiate(slash_effect, transform.position, transform.rotation);
            }
        }

        //L1ボタン押下//ガード
        if (UnityEngine.Input.GetKeyDown("joystick button 4"))
        {
            _Parry_Timing = true;
            StartCoroutine(Miburo_Parry());
        }
        else
        {
            _Parry_Timing = false;
        }

        //Aボタン押下(ステップ)
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            StartCoroutine(Miburo_Step());
        }

        //ノックバック
        if (_KnockBack)
        {
            Miburo_HitBox.SetActive(false);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.position -= dir * KnockBack_Speed * Time.deltaTime;
        }
        else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.position = Vector3.zero;
            Miburo_HitBox.SetActive(true);
        }

        _StepBoll.SetActive(_StepMuteki);
        if (_StepMuteki)
        {
            Miburo_HitBox.SetActive(false);
        }
        else
        {
                Miburo_HitBox.SetActive(true);                     
        }

        GetKatana_Direction();

        if (_Katana_Direction == 0 || _Katana_Direction == 1 || _Katana_Direction == 2 || _Katana_Direction == 7)
        {
            StickR = true;
            StickL = false;
        }
        else if (_Katana_Direction == 3 || _Katana_Direction == 4 || _Katana_Direction == 5 || _Katana_Direction == 6)
        {
            StickL = true;
            StickR = false;
        }
        else
        {
            StickR = false;
            StickL = false;
        }

        Miburo_Animator.SetBool("StickR", StickR);
        Miburo_Animator.SetBool("StickL", StickL);

        if(sippai)
        {
            Test.GetComponent<MeshRenderer>().material = TestMat;
        }
        else
        {
            Test.GetComponent<MeshRenderer>().material = Reset;
        }

        ////判定をアニメーターへ
        Miburo_Animator.SetBool("Gurd", _Parry);


        if (Matsunaga_Enemy01_State.UkeL)
        {
            Miburo_Animator.SetBool("UkenagashiL", true);
            _CounterL = true;
        }
        else
        {
            Miburo_Animator.SetBool("UkenagashiL", false);
        }

        if (Matsunaga_Enemy01_State.UkeR)
        {
            Miburo_Animator.SetBool("UkenagashiR", true);
            _CounterR = true;
        }
        else
        {
            Miburo_Animator.SetBool("UkenagashiR", false);
        }

        GetCurrentAnimationStateName();//ステート取得して

        if (Matsunaga_Enemy01_State.UKe__Ren01)
        {
            if (!_Ren11)
            {
                Miburo_Animator.SetTrigger("Ren11");
                _Ren11 = true;
                _RenCounter01 = true;
            }
        }
        else
        {
            _Ren11 = false;
        }

        if (Matsunaga_Enemy01_State.UKe__Ren02)
        {
            if (!_Ren22)
            {
                //UnityEditor.EditorApplication.isPaused = true;
                Miburo_Animator.SetTrigger("Ren22");
                _Ren22 = true;
                _RenCounter02 = true;
            }

        }
        else
        {
            _Ren22 = false;
        }
    }

    //コルーチン(攻撃1)
    private IEnumerator Miburo_Attack01()
    {
        if (!_Attack01)
        {
            _Attack01 = true;
            Debug.Log("攻撃1開始");
            Miburo_Animator.SetBool("Attack01", true);
            //StartCoroutine(ChangeCoolDown(M_AttackIcon, 0.0f, 1.0f, Attack01_Time + Attack01_WaitTime));
            yield return new WaitForSeconds(Attack01_Time);
            Miburo_Animator.SetBool("Attack01", false);
            yield return new WaitForSeconds(Attack01_WaitTime);
            Debug.Log("攻撃1待ち時間終了");

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
            //StartCoroutine(ChangeCoolDown(M_AttackIcon, 0.0f, 1.0f, Attack02_Time + Attack02_WaitTime));
            yield return new WaitForSeconds(Attack02_Time);
            Debug.Log("攻撃2待ち時間終了");
            Miburo_Animator.SetBool("Attack02", false);
            yield return new WaitForSeconds(Attack02_WaitTime);
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
            yield return new WaitForSeconds(Katana_DirectionSet_Time);

            M_UkenagasiIcon.SetFloat("_CoolDown", 0.0f);

            // 新しく追加：M_UkenagasiIcon の変更を開始
            if(Matsunaga_Enemy01_State.UkeL|| Matsunaga_Enemy01_State.UkeR|| Matsunaga_Enemy01_State.UKe__Ren01 || Matsunaga_Enemy01_State.UKe__Ren02)
            {
                //UnityEditor.EditorApplication.isPaused = true;
                StartCoroutine(ChangeCoolDown(M_UkenagasiIcon, 0.0f, 1.0f, Parry_WaitTime));
                yield return new WaitForSeconds(Parry_WaitTime);
            }
            else 
            {
                sippai = true;
                StartCoroutine(ChangeCoolDown(M_UkenagasiIcon, 0.0f, 1.0f, Parry_WaitTime+Katana_DirectionSet_WaitTime));
                yield return new WaitForSeconds(Parry_WaitTime + Katana_DirectionSet_WaitTime);
                sippai = false;
            }

            Debug.Log("パリイ待ち時間終了");
            _Parry = false;

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
            _StepMuteki = true;
            Miburo_HitBox.SetActive(false);
            Debug.Log("ステップ開始");
            Miburo_Animator.SetTrigger("Step");
            StartCoroutine(ChangeCoolDown(M_StepIcon, 0.0f, 1.0f, Step_WaitTime+Step_Time));
            yield return new WaitForSeconds(Step_Time);
            Debug.Log("ステップ待ち時間終了");
            _StepMuteki = false;
            Miburo_HitBox.SetActive(true);
            yield return new WaitForSeconds(Step_WaitTime);
            _Step = false;
        }
        else
        {
            Debug.Log("待ち時間です。入力は反映されません。");
        }
    }

    // 指定アニメーションが終了しているかを判定
    private bool IsAnimationFinished(string animationName)
    {
        return Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) && Miburo_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f;
    }


    //コントローラーから斬撃の方向を取得
    void GetKatana_Direction()
    {

        float h = UnityEngine.Input.GetAxis("Horizontal2");
        float v = UnityEngine.Input.GetAxis("Vertical2");

        float degree = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }

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


    //アニメーターからステート名を取得
    void GetCurrentAnimationStateName()
    {
        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _RenCounter01 = false;
            _RenCounter02 = false;
            _CounterL = false;
            _CounterR = false;
        }
        else
        {

        }

    }


    //ゲームオーバー
    private IEnumerator Gameover()
    {

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneName);

    }  

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision. == "EWeapon")
    }

    private IEnumerator KnockBack()
    {
        if (!_KnockBack)
        {


            _KnockBack = true;
            yield return new WaitForSeconds(KnockBack_Time);
            _KnockBack = false;
        }
    }

    // UIに反映させるためのコルーチン
    IEnumerator aChangeCoolDown(Material _material, float startValue, float endValue, float duration)
    {
        if (!_CoolDown)
        {
            _CoolDown = true;
            float time = 0.0f;

            // 時間経過で M_UkenagasiIcon の CoolDown 値を徐々に変更
            while (time < duration)
            {
                time += Time.deltaTime;
                float value = Mathf.Lerp(startValue, endValue, time / duration);
                _material.SetFloat("_CoolDown", value); // M_UkenagasiIcon のみを操作
                yield return null;
            }

            // 最終値を確定
            _material.SetFloat("_CoolDown", endValue);
            _CoolDown = false;
        }
        else
        {
        }
    }

    // UIに反映させるためのコルーチン
    IEnumerator ChangeCoolDown(Material _material,float startValue, float endValue, float duration)
    {
        //if(!_CoolDown)
        //{
        //    _CoolDown = true;
            float time = 0.0f;

            // 時間経過で M_UkenagasiIcon の CoolDown 値を徐々に変更
            while (time < duration)
            {
                time += Time.deltaTime;
                float value = Mathf.Lerp(startValue, endValue, time / duration);
                _material.SetFloat("_CoolDown", value); // M_UkenagasiIcon のみを操作
                yield return null;
            }

            // 最終値を確定
            _material.SetFloat("_CoolDown", endValue);
        //    _CoolDown = false;
        //}
        //else
        //{
        //}
    }

    //当たり判定
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EWeapon")
        {
            //Miburo_HitBox =GameObject.Find("Player");
            if (M_HitBox && Matsunaga_Enemy01_State.Attack)
            {

                if (!Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("Battou"))
                {
                    Rigidbody rb = GetComponent<Rigidbody>();
                    dir = (Target.transform.position - rb.position).normalized;
                    Miburo_Animator.SetTrigger("Damage");
                    audioSource_P.PlayOneShot(AudioClip00);
                    Kato_Status_P.instance.Damage(1);
                    StartCoroutine(KnockBack());
                }
            }
        }
    }

    private IEnumerator Damage_Muteki()
    {
        //Miburo_HitBox.SetActive(false);
        yield return new WaitForSeconds(Damage_MutekiTime);
        //Miburo_HitBox.SetActive(true);

    }

    private IEnumerator Muteki()
    {
        if (!_Muteki)
        {


            _Muteki = true;
            yield return new WaitForSeconds(Damage_MutekiTime);
            _Muteki = false;
        }
    }
}



