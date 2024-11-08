using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_HitBoxE : MonoBehaviour
{
    [SerializeField, Header("剣先")]
    public GameObject WeponPoint;
    private GameObject Clone_Effect;//エフェクトのクローン

    [SerializeField, Header("斬撃エフェクト")]
    public GameObject S_Effect;

    private bool Hitflg = false;

    public static bool Ukenagashi_Flg;//受け流しフラグ 

    private bool P_G_flg = Kato_Player_Anim.G_Flg;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        P_G_flg = Kato_a_Player_Anim.G_Flg;

        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Proto_Player" && !P_G_flg)
        {
            Debug.LogFormat("{1}は {0}にダメージを与えた！", collision.gameObject.name, gameObject.name);
        }
        if (collision.gameObject.name == "Player_HitBox" && P_G_flg)
        {
            Debug.LogFormat("衝撃波発生!");

            Clone_Effect = GameObject.Find("sword_test(Clone)");
            if (Clone_Effect == null && Kato_a_Player_Anim.Katana_Direction > -1)
            {
                Instantiate(S_Effect);
                S_Effect.transform.position = new Vector3(gameObject.transform.position.x, 2.0f, gameObject.transform.position.z);
                //UnityEditor.EditorApplication.isPaused = true;
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
