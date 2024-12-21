using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Matsunaga_HitBoxEM : MonoBehaviour
{
    [SerializeField, Header("剣先")]
    public GameObject WeponPoint;
    [SerializeField, Header("剣根本")]
    public GameObject WeponRoot;
    private GameObject Clone_Effect;//エフェクトのクローン

    [SerializeField, Header("プレイヤーモデル")]
    public GameObject Player_Model;

    [SerializeField, Header("敵モデル")]
    public GameObject Enemy_Model;

    private bool P_G_flg ;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

}
