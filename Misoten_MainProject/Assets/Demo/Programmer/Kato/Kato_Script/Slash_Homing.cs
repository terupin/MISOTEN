using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Slash_Homing : MonoBehaviour
{
    [SerializeField, Header("ホーミング旋回速度")]
    public float RotateSpeed;
    [SerializeField, Header("衝撃波速度")]
    public float MoveSpeed; // 敵の移動速度

    [SerializeField, Header("ホーミング開始距離")]
    public float Homing_Start_Dis;
    [SerializeField, Header("ホーミング終了距離")]
    public float Homing_End_Dis;

    [SerializeField, Header("trueなら最も近いオブジェクトをfalseなら最も遠いオブジェクト")]
    public bool SearchPriority;

    [SerializeField, Header("衝撃波が生成されている時間(秒)")]
    public float MoveTime ;
    private float CurrentTime = 0.0f;

    private GameObject Target; // プレイヤーオブジェクトのTransform

    //デンチクと切断可能タグのゲームオブジェクトをすべて取得する
    private GameObject[] _Denchiku ;
    private GameObject[] _Cut ;

    //ひとまとめにする
    private GameObject[] _HomingList ;

    private float[] _HomingDistance ;//距離

    private GameObject EnemyObj;
    private GameObject EnemyKatanaBox;

    // Start is called before the first frame update
    void Start()
    {
        EnemyObj = GameObject.FindWithTag("Enemy");
        EnemyKatanaBox = GameObject.Find("Enemy_HitBox");

        gameObject.transform.position = new Vector3(EnemyKatanaBox.transform.localPosition.x, 0.0f, EnemyKatanaBox.transform.localPosition.z);

        if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, EnemyObj.transform.localEulerAngles.y -30, 0.0f);
        }
        else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, EnemyObj.transform.localEulerAngles.y + 30, 0.0f);
        }

        //gameObject.transform.rotation = Quaternion.Euler(0.0f, EnemyObj.transform.localEulerAngles.y , 0.0f);

        ////オブジェクト生成時に探索
        StartCoroutine(Homing_Search());
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            float HomingDistance= Vector3.Distance(Target.transform.position, gameObject.transform.position);


            if(HomingDistance<=Homing_Start_Dis && HomingDistance >=Homing_End_Dis)
            {
                // 対象物と自分自身の座標からベクトルを算出してQuaternion(回転値)を取得
                Vector3 HomingVector = Target.transform.position - this.transform.position;
                // もし上下方向の回転はしないようにしたければ以下のようにする。
                HomingVector.y = 0f;

                // Quaternion(回転値)を取得
                Quaternion quaternion = Quaternion.LookRotation(HomingVector);
                // 取得した回転値をこのゲームオブジェクトのrotationに代入
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(HomingVector), RotateSpeed * Time.deltaTime);

            }

            if (HomingDistance <= Homing_End_Dis)
            {
                gameObject.transform.LookAt(new Vector3(Target.transform.position.x,gameObject.transform.position.y, Target.transform.position.z));
            }


        }

        //  衝撃波はホーミング時以外は直進します。
        gameObject.transform.position += gameObject.transform.forward * MoveSpeed * Time.deltaTime;

        if (CurrentTime >= MoveTime)
        {
            //UnityEditor.EditorApplication.isPaused = true;
            Destroy(gameObject);
        }
        CurrentTime += Time.deltaTime;
    }

    //ホーミング対象をサーチ
    private IEnumerator Homing_Search()
    {

        //デンチクと切断可能タグのゲームオブジェクトをすべて取得する
        _Denchiku = GameObject.FindGameObjectsWithTag("Denchiku");
        _Cut = GameObject.FindGameObjectsWithTag("Cut");

        //ひとまとめにする
        _HomingList = new GameObject[_Denchiku.Length + _Cut.Length];

        _HomingDistance = new float[_HomingList.Length];

        Debug.Log(_HomingList.Length);

        int ListCount = 0;
        foreach (GameObject obj in _HomingList)
        {

            if (ListCount < _Denchiku.Length)
            {
                _HomingList[ListCount] = _Denchiku[ListCount];
            }
            else
            {
                _HomingList[ListCount] = _Cut[ListCount - _Denchiku.Length];
            }

            _HomingDistance[ListCount] = Vector3.Distance(_HomingList[ListCount].transform.position, gameObject.transform.position);
            Debug.LogFormat("オブジェクト名　{0}\n距離　{1}",_HomingList[ListCount].name, _HomingDistance[ListCount]);
            Debug.Log(_HomingList[ListCount].name);
            Debug.Log(_HomingDistance[ListCount]);
            ListCount++;
        }
        int DistanceCount = 0;

        if (SearchPriority)
        {
            foreach (float dis in _HomingDistance)
            {
                if (dis == _HomingDistance.Min())
                {
                    Target=  _HomingList[DistanceCount];
                }
                DistanceCount++;
            }

            Debug.LogFormat("一番近いオブジェクト名　{0}\n距離　{1}", Target.name, Vector3.Distance(Target.transform.position, gameObject.transform.position));

        }
        else
        {
            foreach (float dis in _HomingDistance)
            {
                if (dis == _HomingDistance.Max())
                {
                    Target = _HomingList[DistanceCount];
                }
                DistanceCount++;
            }
            Debug.LogFormat("一番遠いオブジェクト名　{0}\n距離　{1}", Target.name, Vector3.Distance(Target.transform.position, gameObject.transform.position));

        }


        yield break;
    }

}
