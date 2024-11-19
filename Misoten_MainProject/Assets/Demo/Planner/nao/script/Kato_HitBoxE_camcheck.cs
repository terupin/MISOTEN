using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Kato_HitBoxE_camcheck : MonoBehaviour
{
    [SerializeField, Header("剣先")]
    public GameObject WeponPoint;
    [SerializeField, Header("剣根本")]
    public GameObject WeponRoot;
    private GameObject Clone_Effect;//エフェクトのクローン

    [SerializeField, Header("斬撃エフェクト")]
    public GameObject S_Effect;

    [SerializeField, Header("プレイヤーモデル")]
    public GameObject Player_Model;

    [SerializeField, Header("敵モデル")]
    public GameObject Enemy_Model;

    private bool Hitflg = false;

    public static bool Ukenagashi_Flg;//受け流しフラグ 

    private bool P_G_flg = Kato_a_Player_Anim_test.G_Flg;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        P_G_flg = Kato_a_Player_Anim_test.G_Flg;

        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogFormat("{1}は {0}に当たった", collision.gameObject.name, gameObject.name); // ぶつかった相手の名前を取得

        if (collision.gameObject.name == "Player" )
        {
            Debug.LogFormat("{1}は {0}にダメージを与えた！", collision.gameObject.name, gameObject.name);
            Player_Model.AddComponent<Damage_Flash>();
            //UnityEditor.EditorApplication.isPaused = true;
        }
        if (collision.gameObject.name == "Player_HitBox" && Kato_a_Player_Anim_test.Katana_Direction!=-1)
        {
            Debug.LogFormat("衝撃波発生!");
            Ukenagashi_Flg = true;
            Clone_Effect = GameObject.Find("sword_test(Clone)");
            if (Clone_Effect == null )
            {

                Instantiate(S_Effect);
                S_Effect.transform.position = new Vector3(gameObject.transform.position.x, 2.0f, gameObject.transform.position.z);

            }

            Hitflg = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (gameObject.name == "Enemy_HitBox" && collision.gameObject.name == "Player_HitBox")
        {
            Ukenagashi_Flg = false;
            Hitflg = false;
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }
}
