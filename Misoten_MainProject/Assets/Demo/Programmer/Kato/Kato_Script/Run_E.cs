using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run_E : MonoBehaviour
{
    //ここから加藤
    [SerializeField, Header("テストに使うオブジェクト")]
    public GameObject Testobj;//テストに使うオブジェクト

    //縦切り 最大入力猶予 1.7秒
    //連撃1 最大入力猶予 1.2秒
    //連撃2 最大入力猶予 0.5秒

    [SerializeField, Header("ターゲットとなるプレイヤー")]
    public GameObject Target_P; // 敵がターゲットするプレイヤーオブジェクト

    [SerializeField, Header("縦切り 最大入力猶予 1.7秒")]
    public float Check_Time0;
    [SerializeField, Header("連撃1 最大入力猶予 1.2秒")]
    public float Check_Time1;
    [SerializeField, Header("連撃2 最大入力猶予 0.75秒")]
    public float Check_Time2;

    private float Check_Current_Time0;//入力開始から経過した時間
    private float Check_Current_Time1;//入力開始から経過した時間
    private float Check_Current_Time2;//入力開始から経過した時間

    [SerializeField, Header("斬撃エフェクト")]
    public GameObject S_Effect;
    public bool Effectflg;//無駄なエフェクトが出ないようにする

    private GameObject Clone_Effect;

    static public bool P_Wait;

    static public bool UkeL;
    static public bool UkeR;

    static public bool UKe__Ren01;
    static public bool UKe__Ren02;
    static public bool Attack;//攻撃　当たり判定に使う

    private bool P_Input;//パリイ入力されたかどうか

    [SerializeField, Header("テストサウンド")]
    public AudioClip[] _Sound_Test;
    AudioSource audioSource;
    //ここまで加藤

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

    [SerializeField, Header("縦切り攻撃確率(%)"), Range(0, 100)]
    public int TategiriChance = 60; // 縦切り攻撃を選択する確率

    [SerializeField, Header("連撃攻撃確率(%)"), Range(0, 100)]
    public int RenGekiChance = 40; // 連撃攻撃を選択する確率

    [SerializeField, Header("攻撃射程(3.5)")]
    public float AttackLength = 3.5f; // 敵が攻撃可能な距離

    private float P_E_Length; // プレイヤーと敵との距離を保持

    [SerializeField, Header("移動スピード(12)")]
    public float MoveSpeed = 12; // 敵が移動する速度

    public float maiclue_radius = 5.0f; //周回する円の半径
    private float maiclue_x;    //周回計算用のX座標
    private float maiclue_y;    //周回計算用のY座標
    private float maiclue_z;    //周回計算用のZ座標
    public float maiclue_speed; //周回スピード

    private float maiclue_attacktime; //周回時の攻撃間隔の時間(乱数格納用)
    public float maiclue_maxtime = 5.0f; //周回時の攻撃間隔の最大時間
    public float maiclue_mintime = 3.0f; //周回時の攻撃間隔の最小時間

    private float maiclue_starttime;
    private float maiclue_elapsedtime;
    private Vector3 targetPoint;
    private bool maiclue_iscount = false;
    private Mai_State_ M_state;
    private int maiclue_spind; //時計回りか反時計回りか(乱数格納用)
    private float angle = 0.0f; //周回計算用の角度

    public Animator E01Anim; // 敵のアニメーションを制御するAnimator

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
        // プレイヤーが設定されている場合のみ方向を向く処理を実行
        if (Target_P != null)
        {
            LookAtPlayer(); // プレイヤーを向く処理を呼び出し
        }
        P_E_Length = Vector3.Distance(Vector3.zero, gameObject.transform.position);

        State();
        SetAnimation();//アニメーションセット
        KatoUpdateAnim();
    }

    // プレイヤーを向く処理
    private void LookAtPlayer()
    {
        // プレイヤーの方向を計算
        Vector3 direction = (Vector3.zero - transform.position).normalized;

        // Y軸方向の回転のみ適用
        direction.y = 0;

        // プレイヤー方向を向く回転を計算
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // スムーズに回転させる
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed);
    }


    // 攻撃タイプを決定する
    private void DecideAttackType()
    {
        int randomValue = Random.Range(0, 100); // 0～100のランダム値を生成
        Debug.Log($"DecideAttackType: Random Value = {randomValue}, TategiriChance = {TategiriChance}");

        //E01Anim.SetBool("Walk", false); // アニメーションをリセット
        Debug.Log($"穂{E01Anim.GetBool("Walk")}");

        if (randomValue < TategiriChance)
        {
            // 縦切り攻撃を選択
            Debug.Log("縦切り攻撃を選択しました！");
            M_state=Mai_State_.Tategiri;
            //UnityEditor.EditorApplication.isPaused = true;
        }
        else
        {
            // 連撃攻撃を選択
            Debug.Log("連撃攻撃を選択しました！");
            M_state = Mai_State_.Rengeki;
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }

    // 指定アニメーションが終了しているかを判定
    private bool IsAnimationFinished(string animationName)
    {
        return E01Anim.GetCurrentAnimatorStateInfo(0).IsName(animationName) && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f;
    }

    //ステート実行
    private void State()
    {
        switch (M_state)
        {
            //周回
            case Mai_State_.Spin:

                if (maiclue_iscount)
                {
                    maiclue_starttime = Time.time;
                    maiclue_attacktime = Random.Range(maiclue_mintime, maiclue_maxtime);
                    maiclue_iscount = !maiclue_iscount;
                    maiclue_spind = Random.Range(1, 3);
                }

                maiclue_elapsedtime = Time.time - maiclue_starttime;

                // 角度を更新（速度を考慮）
                angle += maiclue_speed * Time.deltaTime;

                // 円周上の位置を計算
                maiclue_x = 0.0f + Mathf.Cos(angle) * maiclue_radius;
                //時計回り
                if (maiclue_spind == 1)
                {
                    maiclue_z = 0.0f + Mathf.Sin(angle) * maiclue_radius;
                }
                //反時計周り
                else
                {
                    maiclue_z = 0.0f - Mathf.Sin(angle) * maiclue_radius;
                }


                // オブジェクトを移動
                transform.position = new Vector3(maiclue_x, transform.position.y, maiclue_z);

                if (!(maiclue_elapsedtime <= maiclue_attacktime))
                {
                    M_state = Mai_State_.Goto;
                }

                break;

            //接近
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

            //攻撃
            case Mai_State_.Attack:

                
                DecideAttackType();

                break;

            //攻撃
            case Mai_State_.Tategiri:

                //UnityEditor.EditorApplication.isPaused = true;
                break;

            //攻撃
            case Mai_State_.Rengeki:

                //UnityEditor.EditorApplication.isPaused = true;
                break;

            //元の場所に戻る
            case Mai_State_.Jumpback:

                //UnityEditor.EditorApplication.isPaused = true;
                transform.position = targetPoint;

                //WaitForSeconds(2.5f); //待機

                if (transform.position == targetPoint)
                {
                    maiclue_iscount = !maiclue_iscount;

                    M_state = Mai_State_.Spin;
                }

                break;
        }
    }

    //アニメーション
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

    //ここから加藤
    private void KatoUpdateAnim()
    {


        //縦切り振り上げ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            //UnityEditor.EditorApplication.isPaused = true;
            if (P_Input)
            {
                if (Check_Current_Time0 > 0.0f && Check_Time0 >= Check_Current_Time0)
                {
                    //受け流し成功
                    if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        UkeL = true;
                        E01Anim.SetBool("UkeL", true);
                        UkeR = false;
                        E01Anim.SetBool("UkeR", false);
                        Debug.Log("判定　成功0L");
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
                    Debug.Log("判定　時間切れ　" + Check_Current_Time0);
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

        //縦切り振りおろし
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
        {
            Debug.Log(Check_Current_Time0);
            Check_Current_Time0 = 0;
        }

        //連撃1振り上げ
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
                            Debug.Log("判定　成功1");
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
                    Debug.Log("判定　時間切れ1　" + Check_Current_Time1);
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

        //連撃1振りおろし
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
        {
            Debug.Log(Check_Current_Time1);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time1 = 0;


        }

        //連撃2振り上げ
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
                            Debug.Log("判定　成功2");
                            audioSource.PlayOneShot(_Sound_Test[0]);
                            P_Wait = false;
                        }
                    }



                }
                else
                {
                    Debug.Log("判定　時間切れ2 " + Check_Current_Time2);
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

        //連撃2振りおろし         
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
        {

            Debug.Log(Check_Current_Time1);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time1 = 0;
        }

        //連撃2振りおろし         
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
    //ここまで加藤
}
