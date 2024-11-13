using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_HitBoxP : MonoBehaviour
{
    [SerializeField, Header("剣先")]
    public GameObject WeponPoint;

    [SerializeField, Header("剣根本")]
    public GameObject WeponRoot;

    public GameObject Enemy_Model;

    private bool P_G_flg = Kato_a_Player_Anim.G_Flg;
    private bool P_A_flg = Kato_a_Player_Anim.A_Flg;

    public static bool Tubazeri_Flg;//受け流しフラグ 

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
        P_A_flg = Kato_a_Player_Anim.A_Flg;

        if (Tubazeri_Flg && P_A_flg)
        {
            //UnityEditor.EditorApplication.isPaused = true;
        }

        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy" && P_A_flg)
        {
            Debug.LogFormat("{1}は {0}にダメージを与えた！", collision.gameObject.name, gameObject.name);
            Enemy_Model.AddComponent<Enemy_Damage>();
            //UnityEditor.EditorApplication.isPaused = true;
        }
        if (collision.gameObject.name == "Enemy" && Kato_HitBoxE.Ukenagashi_Flg)
        {
            Enemy_Model.AddComponent<UkenagashiDamage>();
            Debug.LogFormat("受け流し成功！");
            Tubazeri_Flg = true;

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Enemy_HitBox")
        {
            Tubazeri_Flg = false;
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }
}
