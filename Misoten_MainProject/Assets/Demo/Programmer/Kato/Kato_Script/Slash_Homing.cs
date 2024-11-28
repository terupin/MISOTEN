using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Slash_Homing : MonoBehaviour
{
    [SerializeField, Header("ホーミング回転速度")]
    public float RotateSpeed;
    [SerializeField, Header("衝撃波速度")]
    public float MoveSpeed; // 敵の移動速度

    [SerializeField, Header("trueなら近いオブジェクトをfalseなら遠いオブジェクト")]
    public bool SearchPriority;

    private GameObject Target; // プレイヤーオブジェクトのTransform

    //デンチクと切断可能タグのゲームオブジェクトをすべて取得する
    private GameObject[] _Denchiku ;
    private GameObject[] _Cut ;

    //ひとまとめにする
    private GameObject[] _HomingList ;

    private float[] _HomingDistance ;


    // Start is called before the first frame update
    void Start()
    {

        ////オブジェクト生成時に探索
        StartCoroutine(Homing_Search());
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            float HomingDistance= Vector3.Distance(Target.transform.position, gameObject.transform.position);

            // 対象物と自分自身の座標からベクトルを算出してQuaternion(回転値)を取得
            Vector3 HomingVector = Target.transform.position - this.transform.position;
            // もし上下方向の回転はしないようにしたければ以下のようにする。
            HomingVector.y = 0f;

            // Quaternion(回転値)を取得
            Quaternion quaternion = Quaternion.LookRotation(HomingVector);
            // 取得した回転値をこのゲームオブジェクトのrotationに代入
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(HomingVector), RotateSpeed*Time.deltaTime);

            // ターゲットに向かって移動
            gameObject.transform.position+= gameObject.transform.forward * MoveSpeed * Time.deltaTime;
        }
    }

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
