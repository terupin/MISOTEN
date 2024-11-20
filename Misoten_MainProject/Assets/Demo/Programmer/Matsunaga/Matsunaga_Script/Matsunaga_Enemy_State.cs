using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsunaga_Enemy_State : MonoBehaviour
{
    // 敵の状態を管理する列挙型
    public enum Enemy_State_
    {
        Idle,              // 待機状態
        Walk,              // 移動状態
        Tategiri,          // 縦切り攻撃
        RenGeki_First,     // 連撃1回目
        RenGeki_Second,    // 連撃2回目
        Ukenagasare,       // 受け流し
        Cooldown           // クールダウン状態
    };

    private Enemy_State_ E_State; // 現在の敵の状態

    [SerializeField, Header("ターゲットとなるプレイヤー")]
    public GameObject Target_P;  // 対象となるプレイヤー

    [SerializeField, Header("サーチ射程(10)")]
    public float SearchLength = 10; // プレイヤーを検出する範囲

    [SerializeField, Header("攻撃射程(3.5)")]
    public float AttackLength = 3.5f; // 攻撃が届く範囲

    [SerializeField, Header("移動スピード(12)")]
    public float MoveSpeed = 12;  // 敵の移動スピード

    [SerializeField, Header("縦切り攻撃確率(%)"), Range(0, 100)]
    public int TategiriChance = 60;  // 縦切り攻撃をする確率

    [SerializeField, Header("連撃攻撃確率(%)"), Range(0, 100)]
    public int RenGekiChance = 40;  // 連撃攻撃をする確率

    private float P_E_Length;  // プレイヤーとの距離

    public Animator E01Anim;  // 敵のアニメーター

    private float StateTime = 2.5f;  // 状態の継続時間
    private float StateCurrentTime;  // 現在の状態時間

    [SerializeField, Header("クールダウン時間")]
    private float CooldownTime = 2.5f;  // クールダウン時間

    [Header("耐久フィールドのオブジェクト")]
    public GameObject durabilityFieldPrefab;  // インスペクタで指定する耐久フィールドのオブジェクト

    [Header("耐久フィールドを生成する座標")]
    public Vector3[] fieldPositions; // 耐久フィールドを生成する座標の配列

    private float currentHP;  // 現在のHP
    private bool hasUsedDurabilityField75 = false;  // HPが75%で耐久フィールドを使ったか
    private bool hasUsedDurabilityField50 = false;  // HPが50%で耐久フィールドを使ったか
    private bool hasUsedDurabilityField25 = false;  // HPが25%で耐久フィールドを使ったか

    private void Start()
    {
        E_State = Enemy_State_.Idle;  // 初期状態はIdle
        StateCurrentTime = 0.0f;  // 状態のカウンターを初期化
        currentHP = 75f;  // 初期HPを75%に設定
        E01Anim.SetBool("Idle", true);  // アニメーターのIdle状態を設定
    }

    private void Update()
    {
        // ターゲットプレイヤーが設定されていない場合の警告
        if (Target_P == null)
        {
            Debug.LogWarning("Target_P が設定されていません！");
            return;
        }

        // プレイヤーとの距離を計算
        P_E_Length = Vector3.Distance(Target_P.transform.position, gameObject.transform.position);
        Debug.Log($"プレイヤーとの距離: {P_E_Length}");

        // 状態ごとの経過時間を増加
        StateCurrentTime += Time.deltaTime;

        // クールダウン状態の場合、時間が経過したらIdle状態に戻す
        if (E_State == Enemy_State_.Cooldown && StateCurrentTime >= StateTime)
        {
            SetState(Enemy_State_.Idle);
        }

        // IdleまたはWalk状態の場合、移動処理を行う
        if (E_State == Enemy_State_.Idle || E_State == Enemy_State_.Walk)
        {
            HandleMovementAndState();
        }

        // 連撃攻撃の処理
        if (E_State == Enemy_State_.RenGeki_First || E_State == Enemy_State_.RenGeki_Second)
        {
            HandleRenGeki();
        }

        // 耐久フィールドの使用判定
        HandleDurabilityField();

        // アニメーションの更新
        UpdateAnimations();
    }

    // 敵の状態を変更する
    private void SetState(Enemy_State_ newState)
    {
        E_State = newState;
        StateCurrentTime = 0.0f;  // 状態遷移時にカウンターをリセット
    }

    // 耐久フィールドの使用判定処理
    private void HandleDurabilityField()
    {
        // HPが75%以下で、まだ耐久フィールドを使用していない場合
        if (currentHP <= 75f && !hasUsedDurabilityField75)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField75 = true;  // フラグを立てる
        }
        // HPが50%以下で、まだ耐久フィールドを使用していない場合
        else if (currentHP <= 50f && !hasUsedDurabilityField50)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField50 = true;  // フラグを立てる
        }
        // HPが25%以下で、まだ耐久フィールドを使用していない場合
        else if (currentHP <= 25f && !hasUsedDurabilityField25)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField25 = true;  // フラグを立てる
        }
    }

    // 耐久フィールドを指定された位置に生成する
    private void SpawnDurabilityField()
    {
        foreach (var position in fieldPositions)
        {
            Instantiate(durabilityFieldPrefab, position, Quaternion.identity);  // 指定座標に耐久フィールドを生成
            Debug.Log($"耐久フィールドを生成: {position}");
        }
    }

    // 移動と状態遷移の処理
    private void HandleMovementAndState()
    {
        if (StateCurrentTime >= StateTime)  // 経過時間が状態の持続時間を超えた場合
        {
            StateCurrentTime = 0.0f;  // カウンターリセット

            // プレイヤーが攻撃範囲内にいる場合、攻撃を開始
            if (P_E_Length < AttackLength)
            {
                Debug.Log("攻撃範囲に入ったので攻撃を開始！");
                DecideAttackType();  // 攻撃タイプを決定
            }
            // プレイヤーがサーチ範囲内にいるが攻撃範囲外にいる場合、移動を開始
            else if (P_E_Length < SearchLength)
            {
                Debug.Log("プレイヤーがサーチ範囲内にいますが攻撃範囲外です。移動を開始します。");
                SetState(Enemy_State_.Walk);
            }
            // プレイヤーが範囲外にいる場合、待機状態に戻る
            else
            {
                Debug.Log("プレイヤーが範囲外です。待機状態に戻ります。");
                SetState(Enemy_State_.Idle);
            }
        }

        // 移動状態の処理
        if (E_State == Enemy_State_.Walk)
        {
            if (P_E_Length > AttackLength && P_E_Length < SearchLength)
            {
                Vector3 direction = (Target_P.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);  // プレイヤーに向けて回転
                transform.position += direction * MoveSpeed * Time.deltaTime;  // プレイヤーに向かって移動
            }
        }
    }

    // 攻撃タイプを決定する
    private void DecideAttackType()
    {
        int randomValue = Random.Range(0, 100);  // 0から100までのランダム値
        if (randomValue < TategiriChance)
        {
            SetState(Enemy_State_.Tategiri);  // 縦切り攻撃
        }
        else
        {
            SetState(Enemy_State_.RenGeki_First);  // 連撃攻撃
        }
    }

    // 連撃攻撃の処理
    private void HandleRenGeki()
    {
        if (E_State == Enemy_State_.RenGeki_First)
        {
            if (IsAnimationFinished("RenGeki_First"))
            {
                SetState(Enemy_State_.RenGeki_Second);  // 1回目の連撃が終わったら2回目に進む
            }
        }
        else if (E_State == Enemy_State_.RenGeki_Second)
        {
            if (IsAnimationFinished("RenGeki_Second"))
            {
                SetState(Enemy_State_.Cooldown);  // 連撃が終わったらクールダウン状態に移行
                StateTime = CooldownTime;  // クールダウン時間を設定
            }
        }
    }

    // アニメーションが終了したかを確認する
    private bool IsAnimationFinished(string stateName)
    {
        AnimatorStateInfo stateInfo = E01Anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f;  // アニメーションが終了したか確認
    }

    // アニメーションを更新する
    private void UpdateAnimations()
    {
        E01Anim.SetBool("Walk", E_State == Enemy_State_.Walk);  // 移動状態のアニメーション
        E01Anim.SetBool("Idle", E_State == Enemy_State_.Idle);  // 待機状態のアニメーション
        E01Anim.SetBool("Tategiri", E_State == Enemy_State_.Tategiri);  // 縦切り攻撃のアニメーション
        E01Anim.SetBool("RenGeki", E_State == Enemy_State_.RenGeki_First || E_State == Enemy_State_.RenGeki_Second);  // 連撃攻撃のアニメーション
    }
}
