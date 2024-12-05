using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy01_State : MonoBehaviour
{
    //ここから加藤
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
    public bool debug_switch = false; //デバッグ用の処理のスイッチ

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

    public float StateTime = 0.1f; // 状態ごとの持続時間
    private float StateCurrentTime; // 現在の状態が開始してからの経過時間

    [SerializeField, Header("クールダウン時間")]
    private float CooldownTime = 2.5f; // クールダウン状態の持続時間

    [SerializeField, Header("ひるむ時間")]
    private float StaggerTime = 1.0f; // ひるみ状態の持続時間

    private float currentHP; // 敵の現在のHP
    private bool hasUsedDurabilityField75 = false; // HP75%で耐久フィールドを生成済みかを管理
    private bool hasUsedDurabilityField50 = false; // HP50%で耐久フィールドを生成済みかを管理
    private bool hasUsedDurabilityField25 = false; // HP25%で耐久フィールドを生成済みかを管理

    private float elapsedTime = 0f; // 経過時間を記録

    [Header("六角形の半径")]
    public float radius = 1.0f; // 六角形の半径
    [Header("六角柱の高さ")]
    public float height = 2.0f; // 六角柱の高さ
    [Header("六角柱の中央座標")]
    public Vector3 centerOffset = Vector3.zero; // 中央座標のオフセット
    [Header("辺部分のマテリアル")]
    public Material lineMaterial; // 線用のマテリアル
    [Header("面部分のマテリアル")]
    public Material faceMaterial; // 面用のマテリアル
    [Header("電竹のモデル")]
    public GameObject vertexObjectPrefab; // 頂点に生成するオブジェクト
    [Header("電竹のスケール")]
    public Vector3 vertexObjectScale = Vector3.one; // 頂点オブジェクトのスケール

    Vector3[] lowerVertices = new Vector3[6];
    Vector3[] upperVertices = new Vector3[6];

    public float maiclue_radius = 5.0f; //周回する円の半径
    private float maiclue_x;    //周回計算用のX座標
    private float maiclue_y;    //周回計算用のY座標
    private float maiclue_z;    //周回計算用のZ座標
    public float maiclue_speed; //周回スピード

    private bool run_for_me; //周回用のフラグ
     
    private float angle = 0.0f; //周回計算用の角度

    private float maiclue_attacktime; //周回時の攻撃間隔の時間(乱数格納用)
    public float maiclue_maxtime = 3.0f; //周回時の攻撃間隔の最大時間
    public float maiclue_mintime = 5.0f; //周回時の攻撃間隔の最小時間

    private float maiclue_starttime;
    private float maiclue_elapsedtime;

    private bool maiclue_iscount;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // 初期状態を設定
        E_State = Enemy_State_.Idle;
        StateCurrentTime = 0.0f; // 経過時間を初期化
        currentHP = Kato_Status_E.NowHP / Kato_Status_E.MaxHP; // 初期HPを設定
        elapsedTime = 0f; // 経過時間を初期化
        E01Anim.SetBool("Idle", true); // Idleアニメーションを初期状態に設定

        // 頂点を計算
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60 * i);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            lowerVertices[i] = new Vector3(x, 0, z) + centerOffset;
            upperVertices[i] = new Vector3(x, height, z) + centerOffset;
        }

        run_for_me = false;
    }

    private void Update()
    {
         //加藤  
        if (UnityEngine.Input.GetKeyDown(KeyCode.O))
        {
            Kato_Status_E.NowHP -= 500;
            audioSource.PlayOneShot(_Sound_Test[1]);
        }

        if (Miburo_State._Parry_Timing)
        {
            if (!P_Input)
            {
                //UnityEditor.EditorApplication.isPaused = true;
                P_Input = true;
            }

        }
        //加藤  

        // プレイヤーが設定されている場合のみ方向を向く処理を実行
        if (Target_P != null)
        {
            LookAtPlayer(); // プレイヤーを向く処理を呼び出し
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("dc5: 歩行ステートを実行します");

            run_for_me = true;

            //SetState(Enemy_State_.Walk);
        }

        if (run_for_me)
        {
            // 角度を更新（速度を考慮）
            angle += maiclue_speed * Time.deltaTime;

            // 円周上の位置を計算
            maiclue_x = Target_P.transform.position.x + Mathf.Cos(angle) * maiclue_radius;
            maiclue_z = Target_P.transform.position.z + Mathf.Sin(angle) * maiclue_radius;

            // オブジェクトを移動
            transform.position = new Vector3(maiclue_x, transform.position.y, maiclue_z);
            /*
            if (maiclue_iscount)
            {
                maiclue_starttime = Time.time;
            }

            maiclue_elapsedtime = Time.time - maiclue_starttime;

            

            

            maiclue_attacktime = Random.Range(maiclue_mintime, maiclue_maxtime);

            if (maiclue_elapsedtime <= maiclue_attacktime)
            {
                // 角度を更新（速度を考慮）
                angle += maiclue_speed * Time.deltaTime;

                // 円周上の位置を計算
                maiclue_x = Target_P.transform.position.x + Mathf.Cos(angle) * maiclue_radius;
                maiclue_z = Target_P.transform.position.z + Mathf.Sin(angle) * maiclue_radius;

                // オブジェクトを移動
                transform.position = new Vector3(maiclue_x, transform.position.y, maiclue_z);

            }
            */
        }

        //デバッグ用プログラム
        if (debug_switch)
        {
            // 1キーが押されたらHPが順番に変化
            if (Input.GetKeyDown(KeyCode.Alpha1))
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
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("dc2: 縦切りステートを実行します");
                SetState(Enemy_State_.Tategiri);
            }

            // 3キーが押されたら連撃ステートを実行
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("dc3: 連撃ステートを実行します");
                SetState(Enemy_State_.RenGeki);
            }

            // 4キーが押されたら怯みステートを実行
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("dc4: 怯みステートを実行します");
                SetState(Enemy_State_.Stagger);
            }

            // 5キーが押されたら歩行ステートを実行
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Debug.Log("dc5: 歩行ステートを実行します");

                run_for_me = true;
                
                //SetState(Enemy_State_.Walk);
            }

            // 6キーが押されたらidleステートを実行
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Debug.Log("dc6: idleステートを実行します");

                // Idle状態に遷移
                SetState(Enemy_State_.Idle);
            }

            // 7キーが押されたら解放ステートを実行
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Debug.Log("dc7: 解放ステートを実行します");
                SetState(Enemy_State_.Kaihou);
                E01Anim.SetBool("Kaihou", true); // 解放アニメーションのフラグを設定
            }
        }
        else
        {
            currentHP = (float)Kato_Status_E.NowHP / (float)Kato_Status_E.MaxHP;
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
        else if (E_State == Enemy_State_.Tategiri)
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

        if (P_E_Length < AttackLength)
        {
            Debug.Log("時間　"+ StateCurrentTime);
            //UnityEditor.EditorApplication.isPaused = true;
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

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("RtoLtoNagasareS") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
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
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Hirumi") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            // ひるみ状態終了後、待機状態に遷移
            Debug.Log("ひるみ状態が終了しました。Idle 状態に遷移します。");
            E01Anim.SetBool("Hiruimi", false); // ひるみアニメーションのフラグをリセット
            SetState(Enemy_State_.Idle);
        }
    }

    private void HandleKaihou()
    {

        E01Anim.Play("Enemy01_Kaihou", 0, 0f);
        // 必要なら他の状態処理も実行
        SetState(Enemy_State_.Kaihou);

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Kaihou") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
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
            //SpawnDurabilityField();
            // 底面の頂点にオブジェクトを生成
            GenerateObjectsAtVertices(lowerVertices);
            StartCoroutine(DelayedBarrierSpawn());
            hasUsedDurabilityField75 = true;
            SetState(Enemy_State_.Kaihou);
        }

        if (currentHP <= 0.50f && !hasUsedDurabilityField50)
        {
            //SpawnDurabilityField();
            GenerateObjectsAtVertices(lowerVertices);
            StartCoroutine(DelayedBarrierSpawn());
            hasUsedDurabilityField50 = true;
            SetState(Enemy_State_.Kaihou);
        }

        if (currentHP <= 0.25f && !hasUsedDurabilityField25)
        {
            //SpawnDurabilityField();
            GenerateObjectsAtVertices(lowerVertices);
            StartCoroutine(DelayedBarrierSpawn());
            hasUsedDurabilityField25 = true;
            SetState(Enemy_State_.Kaihou);
        }
    }

    // バリア生成を遅延するコルーチン
    private IEnumerator DelayedBarrierSpawn()
    {
        yield return new WaitForSeconds(2f); // 2秒待機
        //SpawnBarrier();

        // 辺を描画
        for (int i = 0; i < 6; i++)
        {
            // 水平辺 (下)
            DrawLine(lowerVertices[i], lowerVertices[(i + 1) % 6]);
            // 水平辺 (上)
            DrawLine(upperVertices[i], upperVertices[(i + 1) % 6]);
            // 垂直辺
            DrawLine(lowerVertices[i], upperVertices[i]);
        }

        // 面を描画
        CreateMesh(lowerVertices, upperVertices);
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

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObject = new GameObject("Line");
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] { start, end });
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    void CreateMesh(Vector3[] lowerVertices, Vector3[] upperVertices)
    {
        GameObject meshObject = new GameObject("HexagonalPrism");
        MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();


        // 側面
        for (int i = 0; i < 6; i++)
        {
            int next = (i + 1) % 6;

            vertices.Add(lowerVertices[i]);
            vertices.Add(upperVertices[i]);
            vertices.Add(upperVertices[next]);
            vertices.Add(lowerVertices[next]);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
        }

        // 頂点と三角形をメッシュに設定
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        // メッシュを設定
        meshFilter.mesh = mesh;
        meshRenderer.material = faceMaterial;
    }

    void GenerateObjectsAtVertices(Vector3[] lowerVertices)
    {
        if (vertexObjectPrefab == null)
        {
            Debug.LogWarning("Vertex object prefab is not assigned.");
            return;
        }

        // 底面の頂点にオブジェクトを生成
        for (int i = 0; i < lowerVertices.Length; i++)
        {
            GameObject vertexObject = Instantiate(vertexObjectPrefab, lowerVertices[i], Quaternion.identity, transform);
            vertexObject.transform.localScale = vertexObjectScale;
        }
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
                    if(!P_Wait)
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
            SetState(Enemy_State_.Stagger);
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
            P_Wait =false;
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
            else if(  Check_Current_Time0> Check_Time0)
            {
                Testobj.transform.localScale = Vector3.one;
                Testobj.SetActive(false);
            }

            if (Check_Current_Time1 > 0.0f && Check_Time1 > Check_Current_Time1)
            {
                Testobj.transform.localScale += Vector3.one * Time.deltaTime;
            }
            else if (  Check_Current_Time1> Check_Time1)
            {
                Testobj.transform.localScale = Vector3.one;
                Testobj.SetActive(false);
            }

            if (Check_Current_Time2 > 0.0f && Check_Time2 > Check_Current_Time2)
            {
                Testobj.transform.localScale += Vector3.one * Time.deltaTime;
            }
            else if (Check_Current_Time2> Check_Time2  )
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