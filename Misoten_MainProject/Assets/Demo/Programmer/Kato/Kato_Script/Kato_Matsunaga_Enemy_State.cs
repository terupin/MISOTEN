using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Matsunaga_Enemy_State : MonoBehaviour
{
    // 敵の状態を表す列挙型
    public enum Enemy_State_
    {
        Idle,       // 待機状態
        Walk,       // 移動状態
        Tategiri,   // 縦切り攻撃状態
        RenGeki,    // 連撃攻撃状態
        Stagger,    // ひるみ状態
        Cooldown    // クールダウン状態
    };

    private Enemy_State_ E_State; // 現在の敵の状態を格納

    [SerializeField, Header("ターゲットとなるプレイヤー")]
    public GameObject Target_P; // 敵がターゲットするプレイヤーオブジェクト

    [SerializeField, Header("サーチ射程(10)")]
    public float SearchLength = 10; // 敵がプレイヤーを探知できる距離

    [SerializeField, Header("攻撃射程(3.5)")]
    public float AttackLength = 3.5f; // 敵が攻撃可能な距離

    [SerializeField, Header("移動スピード(12)")]
    public float MoveSpeed = 12; // 敵が移動する速度

    [SerializeField, Header("縦切り攻撃確率(%)"), Range(0, 100)]
    public int TategiriChance ; // 縦切り攻撃を選択する確率

    [SerializeField, Header("連撃攻撃確率(%)"), Range(0, 100)]
    public int RenGekiChance ; // 連撃攻撃を選択する確率

    private float P_E_Length; // プレイヤーと敵との距離を保持

    public Animator E01Anim; // 敵のアニメーションを制御するAnimator

    private float StateTime = 2.5f; // 状態ごとの持続時間
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

    private float elapsedTime = 0f; // 経過時間を記録

    private void Start()
    {
        // 初期状態を設定
        E_State = Enemy_State_.Idle;
        StateCurrentTime = 0.0f; // 経過時間を初期化
        currentHP = Matsunaga_Status_E.NowHP / Matsunaga_Status_E.MaxHP; // 初期HPを設定
        elapsedTime = 0f; // 経過時間を初期化
        E01Anim.SetBool("Idle", true); // Idleアニメーションを初期状態に設定
    }

    private void Update()
    {
        Debug.Log($"currentHP: {currentHP}");

        // 1キーが押されたらHPを75%に設定
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentHP = 0.75f * Matsunaga_Status_E.MaxHP;  // HPを75%に設定
            Debug.Log("HPを75%に設定しました！");
        }


        currentHP = Matsunaga_Status_E.NowHP / Matsunaga_Status_E.MaxHP;
        // ターゲットが設定されていない場合は警告を表示し処理を中断
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
        else if (E_State == Enemy_State_.Tategiri)
        {
            HandleTategiri();
        }
        else if (E_State == Enemy_State_.Stagger)
        {
            HandleStagger();
        }

        // HPに応じて耐久フィールドを生成
        HandleDurabilityField();

        // 状態に応じてアニメーションを更新
        UpdateAnimations();
    }

    // 新しい状態を設定し経過時間をリセット
    private void SetState(Enemy_State_ newState)
    {
        E_State = newState;
        StateCurrentTime = 0.0f;
    }

    // バリアを生成する
    private void SpawnBarrier()
    {
        Instantiate(barrierPrefab, barrierPosition[0], Quaternion.identity); // 現在の位置にバリアを生成
        Debug.Log("バリアを生成しました");
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

    // 待機または移動状態での処理
    private void HandleMovementAndState()
    {
        if (StateCurrentTime >= StateTime)
        {
            // 状態遷移タイミングをリセット
            StateCurrentTime = 0.0f;

            if (P_E_Length < AttackLength)
            {
                // 攻撃範囲内にプレイヤーがいる場合、攻撃を開始
                Debug.Log("攻撃範囲に入ったので攻撃を開始！");
                DecideAttackType();
                //UnityEditor.EditorApplication.isPaused = true;
            }
            else if (P_E_Length < SearchLength)
            {
                // サーチ範囲内にプレイヤーがいる場合、移動を開始
                Debug.Log("プレイヤーがサーチ範囲内にいますが攻撃範囲外です。移動を開始します。");
                SetState(Enemy_State_.Walk);
            }
            else
            {
                // プレイヤーが範囲外の場合、待機状態に戻る
                Debug.Log("プレイヤーが範囲外です。待機状態に戻ります。");
                SetState(Enemy_State_.Idle);
            }
        }

        // Idle状態の場合でもプレイヤーの方向を向く
        if (E_State == Enemy_State_.Idle && P_E_Length < SearchLength)
        {
            Vector3 direction = (Target_P.transform.position - transform.position).normalized; // プレイヤー方向
            direction.y = 0; // Y軸回転を抑制
            Quaternion targetRotation = Quaternion.LookRotation(direction); // プレイヤー方向を向く回転
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed); // スムーズな回転
        }

        // Walk状態の場合、プレイヤーに向かって移動
        if (E_State == Enemy_State_.Walk)
        {
            if (P_E_Length > AttackLength && P_E_Length < SearchLength)
            {
                Vector3 direction = (Target_P.transform.position - transform.position).normalized; // プレイヤー方向
                direction.y = 0; // Y軸回転を抑制
                Quaternion targetRotation = Quaternion.LookRotation(direction); // プレイヤー方向を向く回転
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed); // スムーズな回転

                transform.position += direction * MoveSpeed * Time.deltaTime; // プレイヤーに向かって移動
            }
        }
    }


    // 攻撃タイプを決定する
    private void DecideAttackType()
    {
        int randomValue = Random.Range(0, 100); // 0～100のランダム値を生成
        Debug.Log($"DecideAttackType: Random Value = {randomValue}, TategiriChance = {TategiriChance}");

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
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }

    // 連撃攻撃の処理
    private void HandleRenGeki()
    {
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren2") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            // 連撃攻撃完了後、クールダウンに遷移
            Debug.Log("連撃攻撃が完了しました。Cooldown 状態に遷移します。");
            E01Anim.SetBool("Rengeki", false); // アニメーションをリセット
            SetState(Enemy_State_.Cooldown);
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }

    // ひるみ状態の処理
    private void HandleStagger()
    {
        if (StateCurrentTime >= StaggerTime)
        {
            // ひるみ状態終了後、待機状態に遷移
            Debug.Log("ひるみ状態が終了しました。Idle 状態に遷移します。");
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
            hasUsedDurabilityField75 = true;
        }
        if (currentHP <= 0.50f && !hasUsedDurabilityField50)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField50 = true;
        }
        if (currentHP <= 0.25f && !hasUsedDurabilityField25)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField25 = true;
        }
    }

    // 指定アニメーションが終了しているかを判定
    private bool IsAnimationFinished(string animationName)
    {
        return //E01Anim.GetCurrentAnimatorStateInfo(0).IsName(animationName)
            /*&&*/ E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
    }

    // 状態に応じてアニメーションを更新
    private void UpdateAnimations()
    {
        // 状態ごとのアニメーションフラグを更新
        E01Anim.SetBool("Idle", E_State == Enemy_State_.Idle || E_State == Enemy_State_.Cooldown);
        E01Anim.SetBool("Walk", E_State == Enemy_State_.Walk);
        E01Anim.SetBool("Tategiri", E_State == Enemy_State_.Tategiri);
        E01Anim.SetBool("Rengeki", E_State == Enemy_State_.RenGeki);
        KatoUpdateAnim();
    }
    private float Check_Current_Time;//入力開始から経過した時間

    //縦切り 最大入力猶予 1.7秒
    //連撃1 最大入力猶予 1.2秒
    //連撃2 最大入力猶予 0.5秒
    [SerializeField, Header("縦切り 最大入力猶予 1.7秒")]
    public float Check_Time0;
    [SerializeField, Header("連撃1 最大入力猶予 1.2秒")]
    public float Check_Time1;
    [SerializeField, Header("連撃2 最大入力猶予 0.5秒")]
    public float Check_Time2;

    static public bool UkeL;
    static public bool UkeR;
    static public bool RenUke01;
    static public bool RenUke02;

    private void KatoUpdateAnim()
    {
        //縦切り振り上げ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            //UnityEditor.EditorApplication.isPaused = true;
            if (Miburo_State._Uke_Input)
            {

                if (Check_Current_Time > 0.0f && Check_Time0 >= Check_Current_Time)
                {
                    Debug.Log("aaaaaaaa" + Check_Current_Time);
                   // UnityEditor.EditorApplication.isPaused = true;
                    //受け流し成功
                    Debug.Log(Check_Current_Time);
                    if (Miburo_State._Katana_Direction==0|| Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        UkeL = true;
                        E01Anim.SetBool("UkeL", true);
                        UnityEditor.EditorApplication.isPaused = true;
                    }
                    else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        UkeR = true;
                        E01Anim.SetBool("UkeR", true);
                        UnityEditor.EditorApplication.isPaused = true;
                    }
                }
            }
            else
            {

                Check_Current_Time += Time.deltaTime;
            }
        }
        else
        {
            E01Anim.SetBool("UkeL", false);
            E01Anim.SetBool("UkeR", false);
            Check_Current_Time = 0;
        }

        //縦切り振りおろし
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
        {
            Debug.Log(Check_Current_Time);


            Check_Current_Time = 0;
            //UnityEditor.EditorApplication.isPaused = true;
        }

        //連撃1振り上げ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren01"))
        {
            if (Miburo_State._Uke_Input)
            {
                Debug.Log("iiiiiiiii" + Check_Current_Time);
                UnityEditor.EditorApplication.isPaused = true;
                if (Check_Current_Time > 0.0f && Check_Time1 >= Check_Current_Time)
                {
                    //受け流し成功
                    Debug.Log(Check_Current_Time);
                    UnityEditor.EditorApplication.isPaused = true;
                    //Enemy01_Animator.SetBool("RenUke01", true);
                    RenUke01 = true;
                    E01Anim.SetBool("RenUke01", true);
                }
            }
            else
            {
                Check_Current_Time += Time.deltaTime;
            }
        }
        else
        {
            RenUke01 = false;
            E01Anim.SetBool("RenUke01", false);
            //Enemy01_Animator.SetBool("RenUke01", false);
            //Check_Current_Time = 0;
        }

        //連撃1振りおろし
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
        {
            Debug.Log(Check_Current_Time);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time = 0;
        }

        //連撃2振り上げ
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren02"))
        {
            if (Miburo_State._Uke_Input)
            {
                Debug.Log("uuuuuuuu" + Check_Current_Time);
                UnityEditor.EditorApplication.isPaused = true;
                if (Check_Current_Time > 0.0f && Check_Time2 >= Check_Current_Time)
                {
                    //受け流し成功
                    RenUke02 = true;
                    E01Anim.SetBool("RenUke02", true);
                    Debug.Log(Check_Current_Time);
                    //UnityEditor.EditorApplication.isPaused = true;
                }
            }
            else
            {
                Check_Current_Time += Time.deltaTime;
            }
        }
        else
        {
            RenUke02 = false;
            E01Anim.SetBool("RenUke02", false);
            //Check_Current_Time = 0;
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Check_Current_Time = 0;
        }

         //連撃2振りおろし         
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren2"))
        {

            Debug.Log(Check_Current_Time);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time = 0;
        }
    }
}