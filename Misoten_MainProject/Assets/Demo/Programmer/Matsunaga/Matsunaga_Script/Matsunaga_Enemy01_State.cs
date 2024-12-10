using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Matsunaga_Enemy01_State : MonoBehaviour
{
    //ここから加藤
    [SerializeField, Header("テストに使うオブジェクト")]
    public GameObject Testobj;//テストに使うオブジェクト
    [SerializeField, Header("テストに使うオブジェクトのマテリアル通常")]
    public Material Defultmat;
    [SerializeField, Header("テストに使うオブジェクトのマテリアル発光")]
    public Material Testobjmat;

    [Header("開放時に隠す電竹")]
    public GameObject Den01; // 頂点に生成するオブジェクト
    public GameObject Den02; // 頂点に生成するオブジェクト
    public GameObject Den03; // 頂点に生成するオブジェクト
    public GameObject Den04; // 頂点に生成するオブジェクト
    public GameObject Den05; // 頂点に生成するオブジェクト
    public GameObject Den06; // 頂点に生成するオブジェクト

    //縦切り 最大入力猶予 1.7秒
    //連撃1 最大入力猶予 1.2秒
    //連撃2 最大入力猶予 0.5秒
    [SerializeField, Header("縦切り前ディレイ時間")]
    public float Check_TimeWait0;
    [SerializeField, Header("連撃1ディレイ時間")]
    public float Check_TimeWait1;
    [SerializeField, Header("連撃2ディレイ時間")]
    public float Check_TimeWait2;
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
    //ここまで加藤



    // 敵の状態を表す列挙型
    public enum Enemy_State_
    {
        Idle,       // 待機状態
        //Walk,       // 移動状態
        Spin,       //周回状態
        Goto,       //接近状態
        Attack,
        Tategiri,   // 縦切り攻撃状態
        RenGeki,    // 連撃攻撃状態
        Stagger,    // ひるみ状態
        Cooldown,   // クールダウン状態
        Jumpback,   //撤退状態
        Kaihou,     //耐久フィールド展開
        Ukenagasare,//受け流しが成功
    };

    //周回関係
    private enum Mai_State_
    {
        //Idle,       //デフォルト状態

        //Goto,       //接近状態
        //Attack,     //攻撃状態
        //Jumpback,   //撤退状態
        //Kaihou,     //耐久フィールド展開
        //Ukenagasare,//受け流しが成功
    };

    private Enemy_State_ E_State; // 現在の敵の状態を格納
    //private Mai_State_ M_state;

    [SerializeField, Header("デバックモード")]
    public bool debug_switch = false; //デバッグ用の処理のスイッチ



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

    private float currentHP; // 敵の現在のHP
    private bool hasUsedDurabilityField100 = false;
    private bool hasUsedDurabilityField75 = true; // HP75%で耐久フィールドを生成済みかを管理
    private bool hasUsedDurabilityField50 = true; // HP50%で耐久フィールドを生成済みかを管理
    private bool hasUsedDurabilityField25 = true; // HP25%で耐久フィールドを生成済みかを管理

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
    [Header("バリアと電竹の生成角度")]
    public float rotationAngle = 0.0f; //バリアと電竹の生成角度

    Vector3[] lowerVertices = new Vector3[6];
    Vector3[] upperVertices = new Vector3[6];



    private bool run_for_me = false;

    [SerializeField]
    private float spinRadius = 5f; // 円の半径（インスペクタで編集可能）

    [SerializeField]
    private float spinSpeed = 2f; // 周回速度（ラジアン/秒）

    private float currentAngle = 90.0f; // 現在の角度（ラジアン）

    private bool isReverse = false; // 逆方向かどうか

    private bool hasStartedSpin = false; // Spin開始済みかどうか

    // 攻撃ポイント（円を6等分した点）
    private Vector3[] attackPoints = new Vector3[6];

    // Goto状態の開始時の位置を記録
    private Vector3 gotoStartPosition;

    // Goto状態の移動速度
    [Header(" Goto状態の移動速度")]
    public float moveSpeed = 5f;

    // Goto状態で到達すべき中心からの半径
    private float targetRadius = 1f;

    // Jumpback状態で待機する時間（インスペクタで設定可能）
    [SerializeField]
    private float waitTime = 2f; // 待機時間

    // Jumpback状態で経過した時間
    private float jumpbackTimer = 0f;

    private bool UkeTestFlag = false;

    private string mySceneName; // 自身が配置されているシーン名

    private GameObject[] denthiku_num;

    private void Start()
    {
        // 初期状態を設定
        //E_State = Enemy_State_.Idle;
        E_State = Enemy_State_.Kaihou;
        StateCurrentTime = 0.0f; // 経過時間を初期化
        currentHP = Kato_Status_E.NowHP / Kato_Status_E.MaxHP; // 初期HPを設定
        elapsedTime = 0f; // 経過時間を初期化
        E01Anim.SetBool("Idle", true); // Idleアニメーションを初期状態に設定

        // 頂点を計算
        for (int i = 0; i < 6; i++)
        {
            float Hexaangle = Mathf.Deg2Rad * (60 * i + rotationAngle);
            float x = Mathf.Cos(Hexaangle) * radius;
            float z = Mathf.Sin(Hexaangle) * radius;

            lowerVertices[i] = new Vector3(x, 0, z) + centerOffset;
            upperVertices[i] = new Vector3(x, height, z) + centerOffset;
        }

        run_for_me = true;

        CalculateAttackPoints();

        //E_State = Enemy_State_.Spin;
    }

    private void Update()
    {
        //加藤(入力判定)  
        if (Miburo_State._Parry_Timing)
        {
            if (!P_Input)
            {
                P_Input = true;
            }

        }
        //加藤  

        // テスト用の入力
        if (Input.GetKeyDown(KeyCode.Space)) // スペースキーを押すとテストを実行
        {
            Debug.Log("受け流しが成功しました");
            UkeTestFlag = true;

            // 受け流し成功時の処理開始
            StartCoroutine(WaitForUke());
        }

        // プレイヤーが設定されている場合のみ方向を向く処理を実行
        if (Target_P != null)
        {
            LookAtPlayer(); // プレイヤーを向く処理を呼び出し
        }



        // run_for_meがtrueで、まだSpin開始処理をしていない場合
        if (run_for_me && !hasStartedSpin)
        {
            hasStartedSpin = true; // 処理を一度だけ実行するためのフラグ
            //StartCoroutine(StartSpinAfterDelay(2f)); // 2秒待ってSpin状態を開始
        }

        if (IsAnimationFinished("Kaihou"))
        {
            E_State = Enemy_State_.Idle;
            //UnityEditor.EditorApplication.isPaused = true;
        }

        KatoUpdateAnim();
        if (run_for_me)
        {
            switch (E_State)
            {
                case Enemy_State_.Idle:

                    E_State= Enemy_State_.Spin;

                    break;
                case Enemy_State_.Spin:

                    UpdateSpin();                  

                    break;

                case Enemy_State_.Goto:

                    UpdateGoto();

                    //UnityEditor.EditorApplication.isPaused = true;

                    break;

                case Enemy_State_.Attack:

                    DecideAttackType();

                    //Debug.Log("attack状態");
                    //UnityEditor.EditorApplication.isPaused = true;

                    break;

                case Enemy_State_.Tategiri:

                    HandleTategiri();

                    break;

                case Enemy_State_.RenGeki:

                    HandleRenGeki();

                    break;

                case Enemy_State_.Jumpback:

                    UpdateJumpback();

                    break;

                case Enemy_State_.Kaihou:

                    if (hasUsedDurabilityField25 == false)
                    {
                        GenerateObjectsAtVertices(lowerVertices);
                        StartCoroutine(DelayedBarrierSpawn());
                        hasUsedDurabilityField25 = true;
                        Debug.Log($"hasUsedDurabilityField25: {hasUsedDurabilityField25} ");
                    }
                    if (hasUsedDurabilityField50 == false)
                    {
                        GenerateObjectsAtVertices(lowerVertices);
                        StartCoroutine(DelayedBarrierSpawn());
                        hasUsedDurabilityField50 = true;
                        Debug.Log($"hasUsedDurabilityField50: {hasUsedDurabilityField50} ");
                    }
                    if (hasUsedDurabilityField75 == false)
                    {
                        GenerateObjectsAtVertices(lowerVertices);
                        StartCoroutine(DelayedBarrierSpawn());
                        hasUsedDurabilityField75 = true;
                        Debug.Log($"hasUsedDurabilityField75: {hasUsedDurabilityField75} ");
                    }
                    if (hasUsedDurabilityField100 == false)
                    {
                        GenerateObjectsAtVertices(lowerVertices);
                        StartCoroutine(DelayedBarrierSpawn());
                        hasUsedDurabilityField100 = true;
                        Debug.Log($"hasUsedDurabilityField100: {hasUsedDurabilityField100} ");
                    }
                    
                    break;

                case Enemy_State_.Ukenagasare:

                    //StartCoroutine(WaitForUke());
                    HandleNagasare();

                    break;
            }

            Debug.Log($"状態チェック: {E_State} ");
        }
        

        {
            currentHP = (float)Kato_Status_E.NowHP / (float)Kato_Status_E.MaxHP;
        }

        if (Target_P == null)
        {
            Debug.LogWarning("Target_P が設定されていません！");
            return;
        }

        // プレイヤーと敵の距離を計算
        //P_E_Length = Vector3.Distance(Target_P.transform.position, gameObject.transform.position);
        P_E_Length = Vector3.Distance(new Vector3(0, 0, 0), transform.position);
        Debug.Log($"プレイヤーとの距離: {P_E_Length}");

        // 状態ごとの経過時間を更新
        StateCurrentTime += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        // 各状態に応じた処理を呼び出し
        if (E_State == Enemy_State_.Cooldown)
        {
            HandleCooldown();
        }
        else if (E_State == Enemy_State_.Stagger)
        {
            HandleStagger();
        }





        // 状態に応じてアニメーションを更新
        UpdateAnimations();
    }

    private void UpdateSpin()
    {
        // 方向を変更（正方向または逆方向）
        float direction = isReverse ? -1f : 1f;

        // 角度を更新（時間経過に応じて進む）
        currentAngle += direction * spinSpeed * Time.deltaTime;

        // 円周上の位置を計算
        float x = Mathf.Cos(currentAngle) * spinRadius;
        float z = Mathf.Sin(currentAngle) * spinRadius;

        // オブジェクトの位置を更新
        transform.position = new Vector3(x, transform.position.y, z);

        // 6等分された攻撃ポイントを計算
        //CalculateAttackPoints();

        // 現在位置が攻撃ポイントに到達したらGoto状態に遷移
        CheckAttackPointReached(x, z);

        if(E_State == Enemy_State_.Spin)
        {
            if((currentHP == 1.0f) && !hasUsedDurabilityField100)
            {
                E_State = Enemy_State_.Kaihou;
                Debug.Log("100%時の解放実行");
            }
            else if ((currentHP <= 0.75f) && !hasUsedDurabilityField75)
            {
                E_State = Enemy_State_.Kaihou;
                hasUsedDurabilityField75 = false;
                Debug.Log("75%時の解放実行");
            }
            else if ((currentHP <= 0.5f) && !hasUsedDurabilityField50)
            {
                E_State = Enemy_State_.Kaihou;
                hasUsedDurabilityField50 = false;
                Debug.Log("50%時の解放実行");
            }
            else if ((currentHP <= 0.25f) && !hasUsedDurabilityField25)
            {
                E_State = Enemy_State_.Kaihou;
                hasUsedDurabilityField25 = false;
                Debug.Log("25%時の解放実行");
            }
        }

        Debug.Log($"Spin状態: 現在の方向 = {(isReverse ? "逆" : "正")}, 半径 = {spinRadius}, 位置 = ({x}, {z})");
    }

    /*
    private IEnumerator StartSpinAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 指定した時間だけ待つ

        // 50%の確率で逆方向を決定
        isReverse = Random.value > 0.5f;
        E_State = Enemy_State_.Spin; // Spin状態に遷移
        Debug.Log($"Spin状態開始: 現在の方向 = {(isReverse ? "逆" : "正")}");
    }
    */

    private void CalculateAttackPoints()
    {
        // 0度を開始として360度を6等分した攻撃ポイントを計算
        for (int i = 0; i < 6; i++)
        {
            float angleInRadians = Mathf.Deg2Rad * (i * 60+30); // 60度間隔で分割
            float attackX = Mathf.Cos(angleInRadians) * spinRadius;
            float attackZ = Mathf.Sin(angleInRadians) * spinRadius;

            attackPoints[i] = new Vector3(attackX, transform.position.y, attackZ);
        }
    }

    private void CheckAttackPointReached(float x, float z)
    {
        // 現在位置と攻撃ポイントが十分近ければGoto状態に遷移
        float threshold = 0.2f; // 近づく距離の閾値
        foreach (var point in attackPoints)
        {
            if (Vector3.Distance(new Vector3(x, transform.position.y, z), point) < threshold)
            {
                Debug.Log("攻撃ポイント到達!");
                E_State = Enemy_State_.Goto; // Goto状態に遷移
                gotoStartPosition = transform.position;
                //UnityEditor.EditorApplication.isPaused = true;
                break;
            }
        }
    }

    // 指定アニメーションが終了しているかを判定
    private bool IsAnimationFinished(string animationName)
    {
        return E01Anim.GetCurrentAnimatorStateInfo(0).IsName(animationName) && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f;
    }

    private void UpdateGoto()
    {
        // 0,0,0に向かって移動（円周上の速度を維持）
        Vector3 direction = Vector3.zero - transform.position; // 中心座標 (0,0,0) への方向
        direction.y = 0; // 高さは変えずにXY平面で移動

        // 目標半径（内側の円に到達したら攻撃状態に遷移）
        /*
        if (direction.magnitude <= AttackLength)
        {
            E_State = Enemy_State_.Attack; // Attack状態に遷移
            Debug.Log("Attack状態に遷移！");
            //UnityEditor.EditorApplication.isPaused = true;
        }
        */

        if (direction.magnitude >= AttackLength)
        {
            // 移動処理
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, step);
        }
        else
        {
            E_State = Enemy_State_.Attack; // Attack状態に遷移
            Debug.Log("Attack状態に遷移！");
            //UnityEditor.EditorApplication.isPaused = true;
        }

        /*
        // 移動処理
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, step);
        */

        Debug.Log($"Goto状態: 位置 = {transform.position}");
    }

    private void UpdateJumpback()
    {
        // Goto状態開始時に格納した位置に戻る処理
        Vector3 directionToGotoStart = gotoStartPosition - transform.position;
        directionToGotoStart.y = 0; // 高さは変えずにXY平面で移動

        
        // 到着したら指定の待機時間を待つ
        jumpbackTimer += Time.deltaTime;

        /*
        // 待機時間を経過したらSpin状態に遷移
        if (jumpbackTimer >= waitTime)
        {
            E_State = Enemy_State_.Spin; // Spin状態に遷移
            jumpbackTimer = 0f; // タイマーをリセット
            Debug.Log("Spin状態に遷移！");
            isReverse = Random.value > 0.5f;
        }
        */

        if (jumpbackTimer >= 1.0f)
        {
            // 目標半径（内側の円に到達したら攻撃状態に遷移）
            if (directionToGotoStart.magnitude <= 0.0f)
            {
                E_State = Enemy_State_.Spin; // Attack状態に遷移
                Debug.Log("Spin状態に遷移！");
                //UnityEditor.EditorApplication.isPaused = true;
                jumpbackTimer = 0f; // タイマーをリセット
            }
            else
            {
                // 移動処理
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, gotoStartPosition, step);
            }
        }
    }

    /*
    private void Waittostart()
    {
        // Goto状態開始時に格納した位置に戻る処理
        Vector3 directionToGotoStart = gotoStartPosition - transform.position;
        directionToGotoStart.y = 0; // 高さは変えずにXY平面で移動

        // 到着したら指定の待機時間を待つ
        jumpbackTimer += Time.deltaTime;

        // 待機時間を経過したらSpin状態に遷移
        if (jumpbackTimer >= waitTime)
        {
            E_State = Enemy_State_.Spin; // Spin状態に遷移
            jumpbackTimer = 0f; // タイマーをリセット
            Debug.Log("Spin状態に遷移！");
            isReverse = Random.value > 0.5f;
        }
    }
    */

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
        // それ以外の通常の移動処理
        if (debug_switch)
        {
            Debug.Log("デバッグモード中のため移動処理は実行されません。");
            return; // 処理を中断
        }
        
        if (E_State == Enemy_State_.Idle && P_E_Length < SearchLength)
        {
            Vector3 direction = (Target_P.transform.position - transform.position).normalized;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * MoveSpeed);
        }


        if (E_State == Enemy_State_.Goto)
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
        Debug.Log("handletategiri状態");
        //UnityEditor.EditorApplication.isPaused = true;

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            // 縦切り攻撃完了後、クールダウンに遷移
            Debug.Log("縦切り攻撃が完了しました。Cooldown 状態に遷移します。");
            E01Anim.SetBool("Tategiri", false); // アニメーションをリセット
            
            E_State = Enemy_State_.Jumpback;
            //UnityEditor.EditorApplication.isPaused = true;
        }

        if (IsAnimationFinished("NagasereL") || IsAnimationFinished("NagasereR"))
        {
            E_State = Enemy_State_.Jumpback;
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
            
            E_State = Enemy_State_.Jumpback;
        }

        if (IsAnimationFinished("RtoNagasare") || IsAnimationFinished("RtoLtoNagasare"))
        {
            E_State = Enemy_State_.Jumpback;
        }
    }

    private void HandleNagasare()
    {
        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereL") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            E_State = Enemy_State_.Jumpback;
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("NagasereR") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            E_State = Enemy_State_.Jumpback;
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("RtoLtoNagasare") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            E_State = Enemy_State_.Jumpback;
        }

        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("RtoNagasare") && E01Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            E_State = Enemy_State_.Jumpback;
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

    // クールダウン状態の処理
    private void HandleCooldown()
    {
        SetState(Enemy_State_.Idle);
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
        Debug.Log("解放アニメーションの待機を開始します");
        float waitTime = 8.0f;
        yield return new WaitForSeconds(waitTime);

        E01Anim.SetBool("Kaihou", false);
        Debug.Log("解放アニメーションが終了しました");

        //UnityEditor.EditorApplication.isPaused = true;
        E_State = Enemy_State_.Spin; // 次の状態に遷移

        Debug.Log($"状態がSpinに設定されました: {E_State}");
    }

    private IEnumerator WaitForUke()
    {
        Debug.Log("受け流しの待機を開始します");

        // 指定した秒数待機（例: 5秒）
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        
        E_State = Enemy_State_.Jumpback;
        Debug.Log($"待機が完了しました。M_state が Jumpback に設定されました: {E_State}");

        // 状態リセット（モックの状態変更）
        UkeTestFlag = false;
        Debug.Log("受け流し状態がリセットされました");
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

    // 状態に応じてアニメーションを更新
    private void UpdateAnimations()
    {
        // 状態ごとのアニメーションフラグを更新
        E01Anim.SetBool("Idle", E_State == Enemy_State_.Idle );
        E01Anim.SetBool("RunR", E_State == Enemy_State_.Spin && !isReverse);
        E01Anim.SetBool("RunL", E_State == Enemy_State_.Spin && isReverse);
        E01Anim.SetBool("Walk", E_State == Enemy_State_.Goto);
        E01Anim.SetBool("Tategiri", E_State == Enemy_State_.Tategiri);
        E01Anim.SetBool("Rengeki", E_State == Enemy_State_.RenGeki);
        E01Anim.SetBool("Hirumi", E_State == Enemy_State_.Stagger);
        E01Anim.SetBool("Kaihou", E_State == Enemy_State_.Kaihou);
        E01Anim.SetBool("BackStep", E_State == Enemy_State_.Jumpback);
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

        // 親を設定せず、ワールド空間に配置
        lineObject.transform.SetParent(null);
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

        // 親を設定せず、ワールド空間に配置
        meshObject.transform.SetParent(null);
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

            // 親を設定せず、ワールド空間に配置
            vertexObject.transform.SetParent(null);
        }
    }

    void Awake()
    {
        // 現在のシーン名を取得
        mySceneName = gameObject.scene.name;

        // シーンロードイベントに登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // シーンロードイベントを解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンが読み込まれたときに呼び出されるメソッド
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mySceneName)
        {
            Debug.Log($"シーン '{mySceneName}' が読み込まれました。オブジェクト: {gameObject.name}");
            HandleSceneLoaded(); // シーン読み込み時の処理
        }
    }

    // シーンが読み込まれたときの処理
    private void HandleSceneLoaded()
    {
        //初期位置の初期化
        transform.position = new Vector3(0, 0, 10);
        //耐久フィールド生成のフラグの初期化
        hasUsedDurabilityField100 = false;
        hasUsedDurabilityField75 = true;
        hasUsedDurabilityField50 = true;
        hasUsedDurabilityField25 = true;


        StartCoroutine(Waitwhenload());
    }

    private IEnumerator Waitwhenload()
    {
        // 指定した秒数待機（例: 5秒）
        float waitTime = 10.0f;
        yield return new WaitForSeconds(waitTime);

        // 待機終了後、M_state を Spin に変更
        E_State = Enemy_State_.Spin;
    }


    private void KatoUpdateAnim()
    {
        if (E_State == Enemy_State_.Goto)
        {
            Testobj.SetActive(true);
            Testobj.transform.localScale += Vector3.one * Time.deltaTime * 0.1f;
        }

        //確認用
        //if(Check_Current_Time0>=2.2f)
        //{
        //    UkeL = true;
        //    Miburo_State._Katana_Direction = 1;
        //    //UnityEditor.EditorApplication.isPaused = true;
        //}

        if (E_State == Enemy_State_.Tategiri)
        {
            if(Check_Current_Time0 > Check_TimeWait0 && Check_Time0+ Check_TimeWait0 >= Check_Current_Time0)
            {
                //UnityEditor.EditorApplication.isPaused = true;
                if (P_Input)
                {
                    if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        UkeL = true;
                        UkeR = false;
                    }
                    else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        UkeL = false;
                        UkeR = true;
                    }
                }

            }

           if( Check_Time0 + Check_TimeWait0 < Check_Current_Time0)
            {
                if (UkeR)
                {
                    E01Anim.SetBool("UkeR", true);
                    E_State = Enemy_State_.Ukenagasare;
                    Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
                    if (Clone_Effect == null && !Effectflg)
                    {
                        Instantiate(S_Effect);
                        Effectflg = true;
                        //UnityEditor.EditorApplication.isPaused = true;
                    }
                }
                else if (UkeL)
                {
                    E01Anim.SetBool("UkeL", true);
                    E_State = Enemy_State_.Ukenagasare;
                    Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
                    if (Clone_Effect == null && !Effectflg)
                    {
                        Instantiate(S_Effect);
                        Effectflg = true;
                        //UnityEditor.EditorApplication.isPaused = true;
                    }
                }
                else
                {
                    Attack = true;
                     //UnityEditor.EditorApplication.isPaused = true;
                }
            }

            Check_Current_Time0 += Time.deltaTime;
        }

        if (E_State == Enemy_State_.Ukenagasare)
        {
            UkeL = false;
            UkeR = false;
            UKe__Ren01 = false;
            UKe__Ren02 = false;
        }

            if (E_State == Enemy_State_.Jumpback)
        {
            UkeL = false;
            UkeR = false;
            P_Input = false;
            Effectflg = false;
            Attack = false;
            Check_Current_Time0 = 0.0f;
            Check_Current_Time1 = 0.0f;
            Check_Current_Time2 = 0.0f;
            E01Anim.SetBool("UkeL", false);
            E01Anim.SetBool("UkeR", false);
            E01Anim.SetBool("RenUke01", false);
            E01Anim.SetBool("RenUke02", false);
        }

        if (E_State == Enemy_State_.RenGeki)
        {
            if (Check_Current_Time1 > Check_TimeWait1 && Check_Time1 + Check_TimeWait1 > Check_Current_Time1 )
            {
                if (P_Input)  
                {
                if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        if (!UKe__Ren01)
                        {
                            Debug.Log("判定　成功1");
                            Debug.Log("iiiiiiiii" + Check_Current_Time1);
                            //UnityEditor.EditorApplication.isPaused = true;
                            UKe__Ren01 = true;
                        }
                    }

                }
            }

            if (Check_Time1+ Check_TimeWait1 < Check_Current_Time1)
            {
                if (UKe__Ren01)
                {
                    E01Anim.SetBool("RenUke01", true);
                    E_State = Enemy_State_.Ukenagasare;
                }
            }

            if (Check_Current_Time1 > Check_TimeWait2+ Check_Current_Time1 + Check_TimeWait1 && Check_Time1 + Check_TimeWait1 + Check_Time2 + Check_TimeWait2 >= Check_Current_Time1 )
            {
                if (P_Input)
                {
                    if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        if (!UKe__Ren02)
                        {
                            Debug.Log("判定　成功1");
                            Debug.Log("iiiiiiiii" + Check_Current_Time1);
                            //UnityEditor.EditorApplication.isPaused = true;
                            UKe__Ren02 = true;
                        }
                    }

                }
            }

            if (Check_Time1 + Check_TimeWait1 < Check_Current_Time1)
            {
                if (UKe__Ren01)
                {
                    E01Anim.SetBool("RenUke01", true);
                    Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
                    if (Clone_Effect == null && !Effectflg)
                    {
                        Instantiate(S_Effect);
                        Effectflg = true;
                        //UnityEditor.EditorApplication.isPaused = true;
                    }
                    E_State = Enemy_State_.Ukenagasare;
                }
                else
                {
                    Attack = true;
                    //UnityEditor.EditorApplication.isPaused = true;
                }
            }

            if (Check_Time1 + Check_TimeWait1 + Check_Time2 + Check_TimeWait2 < Check_Current_Time1)
            {
                if (UKe__Ren02)
                {
                    E01Anim.SetBool("RenUke02", true);
                    Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
                    if (Clone_Effect == null && !Effectflg)
                    {
                        Instantiate(S_Effect);
                        Effectflg = true;
                        //UnityEditor.EditorApplication.isPaused = true;
                    }
                    E_State = Enemy_State_.Ukenagasare;
                }
                else
                {
                    Attack = true;
                }
            }

            Check_Current_Time1 += Time.deltaTime;
        }



        if (E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Hirumi"))
        {
            SetState(Enemy_State_.Stagger);
        }

        if ( E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
        {
            Testobj.SetActive(true);
            Debug.Log("時間時間　" + Check_Current_Time1);
        }
        else
        {
            Testobj.GetComponent<MeshRenderer>().material = Defultmat;
            Testobj.transform.localScale = Vector3.one;
            Testobj.SetActive(false);
        }

       if( E01Anim.GetCurrentAnimatorStateInfo(0).IsName("Kaihou"))
        {
            Den01.SetActive(false);
            Den02.SetActive(false);
            Den03.SetActive(false);
            Den04.SetActive(false);
            Den05.SetActive(false);
            Den06.SetActive(false);
        }
       else
        {
            Den01.SetActive(true);
            Den02.SetActive(true);
            Den03.SetActive(true);
            Den04.SetActive(true);
            Den05.SetActive(true);
            Den06.SetActive(true);
        }
    }
}
