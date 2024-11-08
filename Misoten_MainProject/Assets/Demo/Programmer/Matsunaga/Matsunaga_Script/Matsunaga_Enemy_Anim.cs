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

    private attack_patern[] routine; // 攻撃パターンの配列
    private Coroutine attackRoutineCoroutine; // 実行中の攻撃ルーチンの参照

    void Start()
    {
        randomValue = -1;
        CurrentTime = 10.0f;

        // 3つの攻撃パターンを初期化
        routine = new attack_patern[3];
        routine[0] = new attack_patern(attack_part.hit_to_down, attack_part.hit_to_down, attack_part.hit_to_down, attack_part.hit_to_down, attack_part.hit_to_down, attack_part.hit_to_down);
        routine[1] = new attack_patern(attack_part.double_attack, attack_part.double_attack, attack_part.double_attack, attack_part.double_attack, attack_part.double_attack, attack_part.double_attack);
        routine[2] = new attack_patern(attack_part.registans_field, attack_part.registans_field, attack_part.registans_field, attack_part.registans_field, attack_part.registans_field, attack_part.registans_field);
    }

    void Update()
    {
        // アニメーターの現在の状態を取得
        AnimatorStateInfo animatorStateInfo = Enemy_Animator.GetCurrentAnimatorStateInfo(0);

        // プレイヤーと敵の剣の角度をアニメーターに設定
        if (playersword != null) Enemy_Animator.SetFloat("playersword_angul", playersword.transform.eulerAngles.y);
        if (enemysword != null) Enemy_Animator.SetFloat("enemysword_angul", enemysword.transform.eulerAngles.y);

        // 攻撃パターンをランダムに選択し、ルーチンが存在しない場合に実行
        int selectedRoutineIndex = UnityEngine.Random.Range(0, routine.Length);
        if (attackRoutineCoroutine == null)
        {
            attackRoutineCoroutine = StartCoroutine(ExecuteAttackRoutineWithDelay(routine[selectedRoutineIndex]));
        }

        // ターゲットが設定されている場合、ターゲットの方向を向き、指定距離を保ちながら移動
        if (target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;
            directionToTarget.y = 0; // 水平方向のみ向く
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);

            Debug.Log("方向転換を行いました。ターゲットに向いています。"); // 方向転換時のログ出力

            // 一定の距離を保ちながらターゲットに向かって移動
            float distanceToTarget = directionToTarget.magnitude;
            if (distanceToTarget > maintainDistance)
            {
                Vector3 moveDirection = directionToTarget.normalized;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                Debug.Log("移動を行っています。ターゲットとの距離を保っています。"); // 移動時のログ出力
            }
        }

        // アニメーション終了と攻撃パターンのリセット処理
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

            // アニメーション名が一致しており、完了していればリセット
            if (animatorStateInfo.IsName(Enemy_Anim_name) && animatorStateInfo.normalizedTime >= 0.9f && !animatorStateInfo.loop)
            {
                Enemy_Animator.SetBool(Enemy_Anim_bool, false);
                AnimCurrentTime = 0.0f;
                Debug.Log("アニメーションが完了しました: " + Enemy_Anim_name); // アニメーション完了時のログ出力
            }

            // ランダム値のリセット
            if (CurrentTime == 0.0f)
            {
                randomValue = UnityEngine.Random.Range(0, 8);
            }

            CurrentTime += Time.deltaTime;
        }
    }

    // 攻撃ルーチンを指定の遅延と共に実行
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

        // 次の攻撃パターンに移行
        int nextRoutineIndex = UnityEngine.Random.Range(0, routine.Length);
        yield return new WaitForSeconds(0.5f);

        attackRoutineCoroutine = StartCoroutine(ExecuteAttackRoutineWithDelay(routine[nextRoutineIndex]));
    }

    // 各攻撃パートに応じたアクションを実行
    void ProcessAttack(attack_part attack)
    {
        switch (attack)
        {
            case attack_part.hit_to_down:
                Enemy_Animator.SetTrigger("HitToDown");
                Debug.Log("アクション: 振り被り攻撃"); // 攻撃アクションログ
                break;

            case attack_part.double_attack:
                Enemy_Animator.SetTrigger("DoubleAttack");
                Debug.Log("アクション: 連撃"); // 攻撃アクションログ
                break;

            case attack_part.registans_field:
                Enemy_Animator.SetTrigger("RegistansField");
                Debug.Log("アクション: 耐久フィールド展開"); // 攻撃アクションログ
                break;
        }
    }
}