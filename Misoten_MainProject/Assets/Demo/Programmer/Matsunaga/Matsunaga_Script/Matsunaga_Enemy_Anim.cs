using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsunaga_Enemy_Anim : MonoBehaviour
{
    // アニメーターおよびアニメーション管理用の変数
    public Animator Enemy_Animator;
    public string Enemy_Anim_bool; // 終了を検知したいアニメーションのフラグ
    public string Enemy_Anim_name; // 終了を検知したいアニメーションの名前
    public static int randomValue = -1; // ランダムな値を格納する

    // キャラクターのステータス関係
    [Header("体力")]
    public int HitPoint;
    [Header("振り下ろし時の攻撃力")]
    public int Attack_point_hit_to_down;
    [Header("連撃時の攻撃力")]
    public int Attack_point_double_attack;

    // 攻撃関連の変数とフラグ
    public GameObject PkatanaHitbox;
    public GameObject Ejoint;
    private int Katana_Direction = -1;
    private bool U_Ukenagasi_Flg = false; // 受け流しフラグ
    public float CullTime;
    private float CurrentTime = 0.0f;
    private float AnimCurrentTime = 0.0f;
    private float AttackTime = 3.0f;

    // プレイヤーと敵の剣の角度取得用の変数
    public GameObject playersword;
    public GameObject enemysword;

    // ターゲットに向かって移動・向くための変数
    public Transform target; // 目標のオブジェクト
    public float maintainDistance = 5.0f;  // 保つべき距離
    public float moveSpeed = 2.0f; // 移動速度

    // 攻撃の種類を定義するenum
    enum attack_part
    {
        hit_to_down,    // 振り被り攻撃
        double_attack,  // 連撃
        registans_field // 耐久フィールド
    }

    // 攻撃パターンの構造体を定義
    struct attack_patern
    {
        public attack_part one, two, three, four, five, six;

        // 各攻撃アクションを受け取るコンストラクタ
        public attack_patern(attack_part one, attack_part two, attack_part three, attack_part four, attack_part five, attack_part six)
        {
            this.one = one;
            this.two = two;
            this.three = three;
            this.four = four;
            this.five = five;
            this.six = six;
        }
    }

    [Header("振り上げの発生確率")]
    [Range(0.0f, 1.0f)] public float hitToDownProbability = 0.3f;
    [Header("連撃の発生確率")]
    [Range(0.0f, 1.0f)] public float doubleAttackProbability = 0.5f;

    private attack_patern[] routine; // 攻撃パターンの配列
    private Coroutine attackRoutineCoroutine; // 実行中の攻撃ルーチンの参照
    private bool isExecutingFieldAction = false; // 耐久フィールド展開中かどうかのフラグ

    void Start()
    {
        // 初期化
        randomValue = -1;
        CurrentTime = 10.0f;

        HitPoint = 10000;
        Attack_point_hit_to_down = 1;
        Attack_point_double_attack = 1;

        // 3つの攻撃パターンを初期化
        routine = new attack_patern[3];
        routine[0] = new attack_patern();
        routine[1] = new attack_patern();
        routine[2] = new attack_patern();
    }

    void Update()
    {
        AnimatorStateInfo animatorStateInfo = Enemy_Animator.GetCurrentAnimatorStateInfo(0);

        if (playersword != null) Enemy_Animator.SetFloat("playersword_angul", playersword.transform.eulerAngles.y);
        if (enemysword != null) Enemy_Animator.SetFloat("enemysword_angul", enemysword.transform.eulerAngles.y);

        // 攻撃ルーチンを開始
        if (attackRoutineCoroutine == null)
        {
            attackRoutineCoroutine = StartCoroutine(ExecuteAttackRoutineWithDelay(SelectRandomRoutine()));
        }

        if (target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;
            directionToTarget.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);

            float distanceToTarget = directionToTarget.magnitude;
            if (distanceToTarget > maintainDistance)
            {
                Vector3 moveDirection = directionToTarget.normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
        }

        if ((HitPoint <= 7500 && HitPoint > 5000) ||
            (HitPoint <= 5000 && HitPoint > 2500) ||
            (HitPoint <= 2500 && HitPoint > 0))
        {
            if (!isExecutingFieldAction)
            {
                if (attackRoutineCoroutine != null)
                {
                    StopCoroutine(attackRoutineCoroutine);
                }
                attackRoutineCoroutine = StartCoroutine(ExecuteResistFieldAndResumeRoutine());
            }
        }

        if (Enemy_Animator.enabled)
        {
            if (CurrentTime > CullTime)
            {
                Enemy_Animator.SetBool(Enemy_Anim_bool, false);
                randomValue = -1;
                CurrentTime = 0.0f;
            }
            else
            {
                Enemy_Animator.SetBool(Enemy_Anim_bool, true);
            }

            if (animatorStateInfo.IsName(Enemy_Anim_name) && animatorStateInfo.normalizedTime >= 0.9f && !animatorStateInfo.loop)
            {
                Enemy_Animator.SetBool(Enemy_Anim_bool, false);
                AnimCurrentTime = 0.0f;
            }

            if (CurrentTime == 0.0f)
            {
                randomValue = UnityEngine.Random.Range(0, 8);
            }

            CurrentTime += Time.deltaTime;
        }
    }

    private attack_patern SelectRandomRoutine()
    {
        float random = UnityEngine.Random.Range(0f, 1f);
        if (random < hitToDownProbability)
        {
            return routine[0];
        }
        else if (random < hitToDownProbability + doubleAttackProbability)
        {
            return routine[1];
        }
        else
        {
            return routine[2];
        }
    }

    IEnumerator ExecuteResistFieldAndResumeRoutine()
    {
        isExecutingFieldAction = true;
        ProcessAttack(attack_part.registans_field);
        yield return new WaitForSeconds(0.5f);

        int nextRoutineIndex = UnityEngine.Random.Range(0, routine.Length);
        attackRoutineCoroutine = StartCoroutine(ExecuteAttackRoutineWithDelay(routine[nextRoutineIndex]));
        isExecutingFieldAction = false;
    }

    IEnumerator ExecuteAttackRoutineWithDelay(attack_patern selectedRoutine)
    {
        ProcessAttack(selectedRoutine.one);
        yield return new WaitForSeconds(0.1f);

        ProcessAttack(selectedRoutine.two);
        yield return new WaitForSeconds(0.1f);

        ProcessAttack(selectedRoutine.three);
        yield return new WaitForSeconds(0.1f);

        ProcessAttack(selectedRoutine.four);
        yield return new WaitForSeconds(0.1f);

        ProcessAttack(selectedRoutine.five);
        yield return new WaitForSeconds(0.1f);

        ProcessAttack(selectedRoutine.six);

        yield return new WaitForSeconds(0.5f);
        attackRoutineCoroutine = StartCoroutine(ExecuteAttackRoutineWithDelay(SelectRandomRoutine()));
    }

    void ProcessAttack(attack_part attack)
    {
        switch (attack)
        {
            case attack_part.hit_to_down:
                Debug.Log("振り下ろし攻撃");
                break;
            case attack_part.double_attack:
                Debug.Log("連撃");
                break;
            case attack_part.registans_field:
                Debug.Log("耐久フィールド展開");
                break;
        }
    }
}
