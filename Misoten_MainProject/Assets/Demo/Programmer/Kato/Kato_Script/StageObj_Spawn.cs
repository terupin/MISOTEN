using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObj_Spawn : MonoBehaviour
{
    [Header("ステージに設置するオブジェクト")]
    public GameObject[] SpawnObj;

    private Vector3[] SpawnPoints;

    [Header("ステージに設置するオブジェクトのスケール")]
    public Vector3 SpawnObjScale;

    [Header("円状に設置するオブジェクトの半径")]
    public float SpawnObjRadius ; // 円の半径（インスペクタで編集可能）

    [Header("配置開始角度ずらし")]
    public float SetobjRot; // 円の半径（インスペクタで編集可能）

    // Start is called before the first frame update
    void Start()
    {
        SpawnPoints = new Vector3[SpawnObj.Length];
        SpawnpointSet();
        ObjectSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //スポーン位置を設定
    private void SpawnpointSet()
    {
        // 0度を開始として360度を6等分した攻撃ポイントを計算
        for (int i = 0; i < 6; i++)
        {
            float angleInRadians = Mathf.Deg2Rad * (i * 60+ SetobjRot); // 60度間隔で分割
            float SetPosX = Mathf.Cos(angleInRadians) * SpawnObjRadius;
            float SetPosZ = Mathf.Sin(angleInRadians) * SpawnObjRadius;

            SpawnPoints[i] = new Vector3(SetPosX, 0.0f, SetPosZ);
        }
    }

    //設定したスポーン位置にオブジェクトを生成
    private void ObjectSpawn()
    {
        // 底面の頂点にオブジェクトを生成
        for (int i = 0; i < SpawnObj.Length; i++)
        {
            GameObject vertexObject = Instantiate(SpawnObj[i], SpawnPoints[i], Quaternion.identity, transform);
            vertexObject.transform.localScale = SpawnObjScale;

            // 親を設定せず、ワールド空間に配置
            vertexObject.transform.SetParent(null);

            vertexObject.transform.LookAt(Vector3.zero);
        }
    }
}
