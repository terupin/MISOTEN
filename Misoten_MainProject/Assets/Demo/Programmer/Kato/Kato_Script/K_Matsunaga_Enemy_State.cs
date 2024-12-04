using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class K_Matsunaga_Enemy_State : MonoBehaviour
{
    [SerializeField, Header("テストに使うオブジェクト")]
    public GameObject Testobj;//テストに使うオブジェクト

    //縦切り 最大入力猶予 1.7秒
    //連撃1 最大入力猶予 1.2秒
    //連撃2 最大入力猶予 0.5秒
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

    static public bool UkeL;
    static public bool UkeR;

    static public bool UKe__Ren01;
    static public bool UKe__Ren02;
    static public bool Attack;//攻撃　当たり判定に使う

    private bool P_Input;//パリイ入力されたかどうか

    // 敵の状態を表す列挙型
    public enum Enemy_State_
    {
        Idle,       // 待機状態
        Walk,       // 移動状態
        Tategiri,   // 縦切り攻撃状態
        RenGeki,    // 連撃攻撃状態
        Stagger,    // ひるみ状態
        Cooldown,   // クールダウン状態
        Kaihou      // 耐久フィールド展開状態
    };

    [SerializeField, Header("デバックモード")]
    public bool debug_switch ; //デバッグ用の処理のスイッチ

    private Enemy_State_ E_State; // 現在の敵の状態を格納

    [SerializeField, Header("ターゲットとなるプレイヤー")]
    public GameObject Target_P; // 敵がターゲットするプレイヤーオブジェクト

    [SerializeField, Header("サーチ射程(10)")]
    public float SearchLength = 100; // 敵がプレイヤーを探知できる距離

    [SerializeField, Header("攻撃射程(3.5)")]
    public float AttackLength = 3.5f; // 敵が攻撃可能な距離

    [SerializeField, Header("移動スピード(12)")]
    public float MoveSpeed = 12; // 敵が移動する速度

    [SerializeField, Header("縦切り攻撃確率(%)"), Range(0, 100)]
    public int TategiriChance = 60; // 縦切り攻撃を選択する確率

    [SerializeField, Header("連撃攻撃確率(%)"), Range(0, 100)]
    public int RenGekiChance = 40; // 連撃攻撃を選択する確率

    private float P_E_Length; // プレイヤーと敵との距離を保持

    public Animator E01Anim; // 敵のアニメーションを制御するAnimator

    public float StateTime = 2.5f; // 状態ごとの持続時間
    private float StateCurrentTime; // 現在の状態が開始してからの経過時間

    [SerializeField, Header("クールダウン時間")]
    private float CooldownTime = 2.5f; // クールダウン状態の持続時間

    [SerializeField, Header("ひるむ時間")]
    private float StaggerTime = 1.0f; // ひるみ状態の持続時間

    [Header("耐久フィールドのオブジェクト")]
    public GameObject durabilityFieldPrefab; // 耐久フィールドのプレハブオブジェクト

    [Header("耐久フィールドを生成する座標")]
    public Vector3[] fieldPositions; // 耐久フィールドを生成する座標

    [Header("耐久フィールドのスケール")]
    [SerializeField]
    public Vector3 fieldScale = new Vector3(1, 1, 1); // 耐久フィールドのスケール（デフォルト値: 1, 1, 1）

    private float currentHP; // 敵の現在のHP
    private bool hasUsedDurabilityField75 = false; // HP75%で耐久フィールドを生成済みかを管理
    private bool hasUsedDurabilityField50 = false; // HP50%で耐久フィールドを生成済みかを管理
    private bool hasUsedDurabilityField25 = false; // HP25%で耐久フィールドを生成済みかを管理

    [Header("バリアオブジェクト")]
    public GameObject barrierPrefab; // バリアのプレハブオブジェクト

    [Header("バリアを生成する座標")]
    public Vector3[] barrierPosition; // バリアを生成する座標の配列

    [Header("バリアのスケール")]
    public Vector3 barrierScale = new Vector3(1, 1, 1); // バリアのスケール（デフォルト値: 1, 1, 1）

    private float elapsedTime = 0f; // 経過時間を記録

    private void Start()
    {
        Application.targetFrameRate = 60;
        // 初期状態を設定
        E_State = Enemy_State_.Idle;
        StateCurrentTime = 0.0f; // 経過時間を初期化
        currentHP = Matsunaga_Status_E.NowHP / Matsunaga_Status_E.MaxHP; // 初期HPを設定
        elapsedTime = 0f; // 経過時間を初期化
        E01Anim.SetBool("Idle", true); // Idleアニメーションを初期状態に設定
    }

    private void Update()
    {
        if (Kato_Status_E.NowHP <= 0)
        {

            return;
        }

        if (Miburo_State._Parry_Timing)
        {
            if (!P_Input)
            {
                //UnityEditor.EditorApplication.isPaused = true;
                P_Input = true;
            }

        }

        // プレイヤーが設定されている場合のみ方向を向く処理を実行
        if (Target_P != null)
        {
            LookAtPlayer(); // プレイヤーを向く処理を呼び出し
        }

        //デバッグ用プログラム
        if (debug_switch)
        {
            // 1キーが押されたらHPが順番に変化
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Debug.Log("HPを75%に設定しました！");
                if (currentHP == 1.0f) // 現在HPが100%なら
                {
                    currentHP = 0.75f;  // HPを75%に設定
                    Debug.Log($"dc1-1 HPを75%に設定しました！: {currentHP} / 1.0f");
                }
                else if (currentHP == 0.75f) // 現在HPが75%なら
                {
                    currentHP = 0.50f;  // HPを50%に設定
                    Debug.Log($"dc1-2 HPを50%に設定しました！: {currentHP} / 1.0f");
                }
                else if (currentHP == 0.50f) // 現在HPが50%なら
                {
                    currentHP = 0.25f;  // HPを25%に設定
                    Debug.Log($"dc1-3 HPを25%に設定しました！: {currentHP} / 1.0f");
                }
                else if (currentHP == 0.25f) // 現在HPが25%なら
                {
                    currentHP = 0f;  // HPを0%に設定
                    Debug.Log($"dc1-4 HPを0%に設定しました！: {currentHP} / 1.0f");
                }
                else if (currentHP == 0f) // 現在HPが0%なら
                {
                    currentHP = 1.0f;  // HPを100%に設定
                    Debug.Log($"dc1-5 HPを100%に設定しました: {currentHP} / 1.0f");
                }
            }

            // 2キーが押されたら縦切りステートを実行
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("dc2: 縦切りステートを実行します");
                SetState(Enemy_State_.Tategiri);
            }

            // 3キーが押されたら連撃ステートを実行
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("dc3: 連撃ステートを実行します");
                SetState(Enemy_State_.RenGeki);
            }

            // 4キーが押されたら怯みステートを実行
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("dc4: 怯みステートを実行します");
                SetState(Enemy_State_.Stagger);
            }

            // 5キーが押されたら歩行ステートを実行
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5))
            {
                Debug.Log("dc5: 歩行ステートを実行します");
                SetState(Enemy_State_.Walk);
            }

            // 6キーが押されたらidleステートを実行
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6))
            {
                Debug.Log("dc6: idleステートを実行します");

                // Idle状態に遷移
                SetState(Enemy_State_.Idle);
            }

            // 7キーが押されたら解放ステートを実行
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha7))
            {
                Debug.Log("dc7: 解放ステートを実行します");
                SetState(Enemy_State_.Kaihou);
                E01Anim.SetBool("Kaihou", true); // 解放アニメーションのフラグを設定
            }
        }
        else
        {
            currentHP = Matsunaga_Status_E.NowHP / Matsunaga_Status_E.MaxHP;
        }

        if (Target_P == null)
        {
            Debug.LogWarning("Target_P が設定されていません！");
            return;
        }

        // プレイヤーと敵の距離を計算
        P_E_Length = Vector3.Distance(Target_P.transform.position, gameObject.transform.position);
        Debug.Log($"プレイヤーとの距離: {P_E_Length}");

        // 状態ごとの経過時間を更新
        StateCurrentTime += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        // 各状態に応じた処理を呼び出し
        if (E_State == Enemy_State_.Cooldown)
        {
            HandleCooldown();
        }
        else if (E_State == Enemy_State_.Idle || E_State == Enemy_State_.Walk)
        {
            HandleMovementAndState();
        }
        else if (E_State == Enemy_State_.RenGeki)
        {
            HandleRenGeki();
        }
       else  if (E_State == Enemy_State_.Tategiri)
        {
            HandleTategiri();
        }
        else if (E_State == Enemy_State_.Stagger)
        {
            HandleStagger();
        }
        else if (E_State == Enemy_State_.Kaihou)
        {
            HandleKaihou();
        }

        // HPに応じて耐久フィールドを生成
        HandleDurabilityField();

        // 状態に応じてアニメーションを更新
        UpdateAnimations();
    }

    // 新しい状態を設定し経過時間をリセット
    private void SetState(Enemy_State_ newState)
    {
        if (E_State == newState) return; // 同じ状態への遷移を防ぐ
        E_State = newState;
        StateCurrentTime = 0.0f;
        Debug.Log($"状態が {newState} に変更されました");
    }

    // バリアを生成する
    private void SpawnBarrier()
    {
        foreach (var position in barrierPosition)
        {
            // バリアを生成
            GameObject barrier = Instantiate(barrierPrefab, position, Quaternion.identity);

            // スケールを適用
            barrier.transform.localScale = barrierScale;

            Debug.Log($"バリアを生成: {position}, スケール: {barrierScale}");
        }
    }

    // 耐久フィールドを生成する
    private void SpawnDurabilityField()
    {
        foreach (var position in fieldPositions)
        {
            // 耐久フィールドを生成
            GameObject field = Instantiate(durabilityFieldPrefab, position, Quaternion.identity);

            // スケールを適用
            field.transform.localScale = fieldScale;

            Debug.Log($"耐久フィールドを生成: {position}, スケール: {fieldScale}");
        }
    }

    private void HandleMovementAndState()
    {
        // Kaihou状態中は移動処理を無効化
        if (E_State == Enemy_State_.Kaihou)
        {
            Debug.Log("解放中のため移動処理をスキップします。");
            return;
        }

        // それ以外の通常の移動処理
        if (debug_switch)
        {
            Debug.Log("デバッグモード中のため移動処理は実行されません。");
            return; // 処理を中断
        }

        if (StateCurrentTime >= StateTime)
        {

            StateCurrentTime = 0.0f;
            if (P_E_Length <= AttackLength)
            {
                Debug.Log("攻撃範囲に入ったので攻撃を開始！");

                DecideAttackType();
            }
            else if (P_E_Length < SearchLength)
            {
                Debug.Log("プレイヤーがサーチ範囲内にいますが攻撃範囲外です。移動を開始します。");
                SetState(Enemy_State_.Walk);
            }
            else
            {
                Debug.Log("プレイヤーが範囲外です。待機状態に戻ります。");
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
            SetState(Enemy_State_.Tategiri);
            //UnityEditor.EditorApplication.isPaused = true;
        }
        else
        {
            // 連撃攻撃を選択
            Debug.Log("連撃攻撃を選択しました！");
            SetState(Enemy_State_.RenGeki);
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }

    // 縦切り攻撃の処理
    private void HandleTategiri()
    {
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            // 縦切り攻撃完了後、クールダウンに遷移
            Debug.Log("縦切り攻撃が完了しました。Cooldown 状態に遷移します。");
            E01Anim.SetBool("Tategiri", false); // アニメーションをリセット
            SetState(Enemy_State_.Cooldown);
        }
    }

    // 連撃攻撃の処理
    private void HandleRenGeki()
    {
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren2") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            // 連撃攻撃完了後、クールダウンに遷移
            Debug.Log("連撃攻撃が完了しました。Cooldown 状態に遷移します。");
            E01Anim.SetBool("RenGeki", false); // アニメーションをリセット
            SetState(Enemy_State_.Cooldown);
        }
    }

    // ひるみ状態の処理
    private void HandleStagger()
    {
        if (IsAnimationFinished("Enemy01_Hirumi"))
        {
            // ひるみ状態終了後、待機状態に遷移
            Debug.Log("ひるみ状態が終了しました。Idle 状態に遷移します。");
            E01Anim.SetBool("Hiruimi", false); // ひるみアニメーションのフラグをリセット
            SetState(Enemy_State_.Idle);
        }
    }

    private void HandleKaihou()
    {
        // 全アニメーションを強制終了して解放アニメーションに遷移
        //E01Anim.CrossFade("Enemy01_Idling", 0.01f);
        //E01Anim.CrossFade("Enemy01_Kaihou", 0.1f); // "Enemy01_Kaihou" は解放アニメーションの状態名

        E01Anim.Play("Enemy01_Kaihou", 0, 0f);
        // 必要なら他の状態処理も実行
        SetState(Enemy_State_.Kaihou);

        if (IsAnimationFinished("Enemy01_Kaihou"))
        {
            Debug.Log("解放アニメーションが完了しました。Idle 状態に遷移します。");
            SetState(Enemy_State_.Idle);
        }
    }


    // クールダウン状態の処理
    private void HandleCooldown()
    {
        if (StateCurrentTime >= CooldownTime)
        {
            // クールダウン終了後、待機状態に遷移
            Debug.Log("クールダウンが終了しました。Idle 状態に遷移します。");
            SetState(Enemy_State_.Idle);
        }
    }

    // HPに応じた耐久フィールドの生成
    private void HandleDurabilityField()
    {
        if (currentHP <= 0.75f && !hasUsedDurabilityField75)
        {
            SpawnDurabilityField();
            StartCoroutine(DelayedBarrierSpawn());
            hasUsedDurabilityField75 = true;
            SetState(Enemy_State_.Kaihou);
        }

        if (currentHP <= 0.50f && !hasUsedDurabilityField50)
        {
            SpawnDurabilityField();
            StartCoroutine(DelayedBarrierSpawn());
            hasUsedDurabilityField50 = true;
            SetState(Enemy_State_.Kaihou);
        }

        if (currentHP <= 0.25f && !hasUsedDurabilityField25)
        {
            SpawnDurabilityField();
            StartCoroutine(DelayedBarrierSpawn());
            hasUsedDurabilityField25 = true;
            SetState(Enemy_State_.Kaihou);
        }
    }

    // バリア生成を遅延するコルーチン
    private IEnumerator DelayedBarrierSpawn()
    {
        yield return new WaitForSeconds(2f); // 2秒待機
        SpawnBarrier();
    }

    private IEnumerator WaitForKaihouAnimation()
    {
        yield return new WaitUntil(() => IsAnimationFinished("Enemy01_Kaihou"));

        // 解放アニメーションが終了したら、フラグをリセットし状態をIdleに遷移
        E01Anim.SetBool("Kaihou", false);
        Debug.Log("解放アニメーションが完了しました");
        SetState(Enemy_State_.Idle);
    }

    // プレイヤーを向く処理
    private void LookAtPlayer()
    {
        // プレイヤーの方向を計算
        Vector3 direction = (Target_P.transform.position - transform.position).normalized;

        // Y軸方向の回転のみ適用
        direction.y = 0;

        // プレイヤー方向を向く回転を計算
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // スムーズに回転させる
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed);
    }

    // 指定アニメーションが終了しているかを判定
    private bool IsAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = E01Anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }

    // 状態に応じてアニメーションを更新
    private void UpdateAnimations()
    {
        // 状態ごとのアニメーションフラグを更新
        E01Anim.SetBool("Idle", E_State == Enemy_State_.Idle);
        E01Anim.SetBool("Walk", E_State == Enemy_State_.Walk);
        E01Anim.SetBool("Tategiri", E_State == Enemy_State_.Tategiri);
        E01Anim.SetBool("Rengeki", E_State == Enemy_State_.RenGeki);
        E01Anim.SetBool("Hirumi", E_State == Enemy_State_.Stagger);
        E01Anim.SetBool("Kaihou", E_State == Enemy_State_.Kaihou);
        KatoUpdateAnim();
    }

    private void KatoUpdateAnim()
    {


        //縦切り振り上げ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            //UnityEditor.EditorApplication.isPaused = true;
            if (P_Input)
            {
                UnityEditor.EditorApplication.isPaused = true;
                if (Check_Current_Time0 > 0.0f && Check_Time0 >= Check_Current_Time0)
                {
                    Debug.Log("aaaaaaaa" + Check_Current_Time0);
                    //受け流し成功
                    Debug.Log(Check_Current_Time0);
                    if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        UkeL = true;
                        E01Anim.SetBool("UkeL", true);
                        Debug.Log("判定　成功0L");
                        UnityEditor.EditorApplication.isPaused = true;
                    }
                    else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        UkeR = true;
                        E01Anim.SetBool("UkeR", true);
                        Debug.Log("判定　成功0R");
                        UnityEditor.EditorApplication.isPaused = true;
                    }
                }
                else
                {
                    Debug.Log("判定　時間切れ　" + Check_Current_Time0);
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
            //UnityEditor.EditorApplication.isPaused = true;
        }

        //連撃1振り上げ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren01"))
        {
            //UnityEditor.EditorApplication.isPaused = true;
            if (P_Input)
            {
                if (Check_Current_Time1 > 0.0f && Check_Time1 > Check_Current_Time1)
                {
                    if (!UKe__Ren01)
                    {
                        Debug.Log("判定　成功1");
                        Debug.Log("iiiiiiiii" + Check_Current_Time1);
                        UnityEditor.EditorApplication.isPaused = true;
                        StartCoroutine(WaitUKe__Ren01());

                    }

                }
                else
                {
                    Debug.Log("判定　時間切れ1　" + Check_Current_Time1);
                    UnityEditor.EditorApplication.isPaused = true;
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
                    if (!UKe__Ren02)
                    {
                        StartCoroutine(WaitUKe__Ren02());
                        UnityEditor.EditorApplication.isPaused = true;
                        Debug.Log("ききき　" + Check_Current_Time2);
                        Debug.Log("ききき　" + Miburo_State._Katana_Direction);
                    }


                }
                else
                {
                    Debug.Log("判定　時間切れ2" + Check_Current_Time2);
                }

            }
            else
            {
                Check_Current_Time2 += Time.deltaTime;
            }
        }
        else
        {

        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereL") || E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereR"))
        {
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
            SetState(Enemy_State_.Stagger);
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            //P_Input = false;
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
            Testobj.transform.localScale += Vector3.one*Time.deltaTime;
        }
        else
        {
            Testobj.transform.localScale = Vector3.one;
            Testobj.SetActive(false);
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
}