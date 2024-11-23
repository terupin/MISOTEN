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
    public GameObject durabilityFieldPrefab;

    [Header("耐久フィールドを生成する座標")]
    public Vector3[] fieldPositions;

    private float currentHP;
    private bool hasUsedDurabilityField75 = false;
    private bool hasUsedDurabilityField50 = false;
    private bool hasUsedDurabilityField25 = false;

    [Header("バリアオブジェクト")]
    public GameObject barrierPrefab;

    private float elapsedTime = 0f;

    private void Start()
    {
        E_State = Enemy_State_.Idle;
        StateCurrentTime = 0.0f;
        currentHP = 100f;
        elapsedTime = 0f;
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
        elapsedTime += Time.deltaTime;

        if (E_State == Enemy_State_.Cooldown)
        {
            HandleCooldown();
        }

        if (E_State == Enemy_State_.Idle || E_State == Enemy_State_.Walk)
        {
            HandleMovementAndState();
        }

        if (E_State == Enemy_State_.RenGeki_First || E_State == Enemy_State_.RenGeki_Second)
        {
            HandleRenGeki();
        }

        if (E_State == Enemy_State_.Tategiri)
        {
            HandleTategiri();
        }

        HandleDurabilityField();

        UpdateAnimations();
    }


    private void SetState(Enemy_State_ newState)
    {
        E_State = newState;
        StateCurrentTime = 0.0f;
    }

    private void SpawnBarrier()
    {
        Instantiate(barrierPrefab, transform.position, Quaternion.identity);
        Debug.Log("バリアを生成しました");
    }

    private void SpawnDurabilityField()
    {
        foreach (var position in fieldPositions)
        {
            Instantiate(durabilityFieldPrefab, position, Quaternion.identity);
            Debug.Log($"耐久フィールドを生成: {position}");
        }
    }

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
        Debug.Log($"DecideAttackType: Random Value = {randomValue}, TategiriChance = {TategiriChance}");

        if (randomValue < TategiriChance)
        {
            Debug.Log("縦切り攻撃を選択しました！");
            SetState(Enemy_State_.Tategiri);
        }
        else
        {
            Debug.Log("連撃攻撃を選択しました！");
            SetState(Enemy_State_.RenGeki_First);
        }
    }

    private void HandleTategiri()
    {
        if (E_State == Enemy_State_.Tategiri)
        {
            if (IsAnimationFinished("Tategiri"))
            {
                Debug.Log("縦切り攻撃が完了しました。Cooldown 状態に遷移します。");
                E01Anim.SetBool("Tategiri", false); // アニメーションパラメータをリセット
                SetState(Enemy_State_.Cooldown);    // クールダウン状態へ
            }
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

    private void HandleCooldown()
    {
        if (StateCurrentTime < CooldownTime)
        {
            return;
        }

        SetState(Enemy_State_.Idle);
        Debug.Log("クールダウンが終了し、Idle状態に遷移しました。");
    }

    private void HandleDurabilityField()
    {
        if (currentHP <= 75f && !hasUsedDurabilityField75)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField75 = true;
        }
        else if (currentHP <= 50f && !hasUsedDurabilityField50)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField50 = true;
        }
        else if (currentHP <= 25f && !hasUsedDurabilityField25)
        {
            SpawnDurabilityField();
            hasUsedDurabilityField25 = true;
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
