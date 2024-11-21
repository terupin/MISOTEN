using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsunaga_Enemy_State : MonoBehaviour
{
    public enum Enemy_State_
    {
        Idle,
        Walk,
        Tategiri,
        RenGeki_First,
        RenGeki_Second,
        Ukenagasare,
        Cooldown
    };

    private Enemy_State_ E_State;

    [SerializeField, Header("ターゲットとなるプレイヤー")]
    public GameObject Target_P;

    [SerializeField, Header("サーチ射程(10)")]
    public float SearchLength = 10;

    [SerializeField, Header("攻撃射程(3.5)")]
    public float AttackLength = 3.5f;

    [SerializeField, Header("移動スピード(12)")]
    public float MoveSpeed = 12;

    [SerializeField, Header("縦切り攻撃確率(%)"), Range(0, 100)]
    public int TategiriChance = 60;

    [SerializeField, Header("連撃攻撃確率(%)"), Range(0, 100)]
    public int RenGekiChance = 40;

    private float P_E_Length;

    public Animator E01Anim;

    private float StateTime = 2.5f;
    private float StateCurrentTime;

    [SerializeField, Header("クールダウン時間")]
    private float CooldownTime = 2.5f;

    [Header("耐久フィールドのオブジェクト")]
    public GameObject durabilityFieldPrefab;  // インスペクタで指定する耐久フィールドのオブジェクト

    [Header("耐久フィールドを生成する座標")]
    public Vector3[] fieldPositions; // 座標を指定する配列

    private float currentHP;
    private bool hasUsedDurabilityField75 = false;
    private bool hasUsedDurabilityField50 = false;
    private bool hasUsedDurabilityField25 = false;

    // バリアのオブジェクト
    [Header("バリアオブジェクト")]
    public GameObject barrierPrefab;

    private float elapsedTime = 0f;  // 経過時間を追跡するための変数

    private void Start()
    {
        E_State = Enemy_State_.Idle;
        StateCurrentTime = 0.0f;
        currentHP = 100f;  // 初期HPを100に設定
        elapsedTime = 0f;  // 経過時間をリセット
        E01Anim.SetBool("Idle", true);
    }

    private void Update()
    {
        if (Target_P == null)
        {
            Debug.LogWarning("Target_P が設定されていません！");
            return;
        }

        P_E_Length = Vector3.Distance(Target_P.transform.position, gameObject.transform.position);
        Debug.Log($"プレイヤーとの距離: {P_E_Length}");

        StateCurrentTime += Time.deltaTime;
        elapsedTime += Time.deltaTime;  // 修正された部分

        if (E_State == Enemy_State_.Cooldown && StateCurrentTime >= StateTime)
        {
            SetState(Enemy_State_.Idle);
        }

        // 30秒経過後に体力を75に設定
        if (elapsedTime >= 30f && currentHP == 100f)
        {
            currentHP = 75f;
            Debug.Log("30秒経過後、体力が75に設定されました");
        }

        if (E_State == Enemy_State_.Idle || E_State == Enemy_State_.Walk)
        {
            HandleMovementAndState();
        }

        if (E_State == Enemy_State_.RenGeki_First || E_State == Enemy_State_.RenGeki_Second)
        {
            HandleRenGeki();
        }

        HandleDurabilityField();  // 修正された部分

        CheckDenchikuAndSpawnBarrier();  // denchikuがあればバリアを出す

        UpdateAnimations();
    }

    private void SetState(Enemy_State_ newState)
    {
        E_State = newState;
        StateCurrentTime = 0.0f;
    }

    // denchikuが1つ以上あればバリアを出す
    private void CheckDenchikuAndSpawnBarrier()
    {
        // シーン内に存在する「denchiku」タグのオブジェクトを検索
        GameObject[] denchikuObjects = GameObject.FindGameObjectsWithTag("denchiku");

        if (denchikuObjects.Length > 0)  // denchikuが1つ以上存在する場合
        {
            // denchikuが生成されたタイミングでバリアを生成
            SpawnBarrier();  // バリアを生成
        }
    }

    // バリアを生成する
    private void SpawnBarrier()
    {
        // バリアを指定した位置に生成
        Instantiate(barrierPrefab, transform.position, Quaternion.identity);
        Debug.Log("バリアを生成しました");
    }

    private void SpawnDurabilityField()
    {
        foreach (var position in fieldPositions)
        {
            Instantiate(durabilityFieldPrefab, position, Quaternion.identity);  // 指定座標に耐久フィールドを生成
            Debug.Log($"耐久フィールドを生成: {position}");
        }
    }

    // 移動と状態の更新
    private void HandleMovementAndState()
    {
        if (StateCurrentTime >= StateTime)
        {
            StateCurrentTime = 0.0f;

            if (P_E_Length < AttackLength)
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

        if (E_State == Enemy_State_.Walk)
        {
            if (P_E_Length > AttackLength && P_E_Length < SearchLength)
            {
                Vector3 direction = (Target_P.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);
                transform.position += direction * MoveSpeed * Time.deltaTime;
            }
        }
    }

    private void DecideAttackType()
    {
        int randomValue = Random.Range(0, 100);
        if (randomValue < TategiriChance)
        {
            SetState(Enemy_State_.Tategiri);
        }
        else
        {
            SetState(Enemy_State_.RenGeki_First);
        }
    }

    private void HandleRenGeki()
    {
        if (E_State == Enemy_State_.RenGeki_First)
        {
            if (IsAnimationFinished("RenGeki_First"))
            {
                SetState(Enemy_State_.RenGeki_Second);
            }
        }
        else if (E_State == Enemy_State_.RenGeki_Second)
        {
            if (IsAnimationFinished("RenGeki_Second"))
            {
                SetState(Enemy_State_.Cooldown);
                StateTime = CooldownTime;
            }
        }
    }

    // 既存のコードの続き

    // 耐久フィールドを管理するためのメソッド
    private void HandleDurabilityField()
    {
        // 体力によって耐久フィールドを使用するかどうか判断
        if (currentHP <= 75f && !hasUsedDurabilityField75)
        {
            SpawnDurabilityField();  // 耐久フィールドを生成
            hasUsedDurabilityField75 = true;  // 75%で発動したらフラグを立てる
        }
        else if (currentHP <= 50f && !hasUsedDurabilityField50)
        {
            SpawnDurabilityField();  // 耐久フィールドを生成
            hasUsedDurabilityField50 = true;  // 50%で発動したらフラグを立てる
        }
        else if (currentHP <= 25f && !hasUsedDurabilityField25)
        {
            SpawnDurabilityField();  // 耐久フィールドを生成
            hasUsedDurabilityField25 = true;  // 25%で発動したらフラグを立てる
        }
    }



    private bool IsAnimationFinished(string stateName)
    {
        AnimatorStateInfo stateInfo = E01Anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName) && stateInfo.normalizedTime >= 1.0f;
    }

    private void UpdateAnimations()
    {
        E01Anim.SetBool("Walk", E_State == Enemy_State_.Walk);
        E01Anim.SetBool("Idle", E_State == Enemy_State_.Idle);
        E01Anim.SetBool("Tategiri", E_State == Enemy_State_.Tategiri);
        E01Anim.SetBool("RenGeki", E_State == Enemy_State_.RenGeki_First || E_State == Enemy_State_.RenGeki_Second);
    }
}
