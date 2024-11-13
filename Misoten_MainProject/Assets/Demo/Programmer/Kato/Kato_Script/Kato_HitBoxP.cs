using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_HitBoxP : MonoBehaviour
{
    [SerializeField, Header("����")]
    public GameObject WeponPoint;

    [SerializeField, Header("�����{")]
    public GameObject WeponRoot;

    private bool P_G_flg = Kato_a_Player_Anim.G_Flg;
    private bool P_A_flg = Kato_a_Player_Anim.A_Flg;

    public static bool Tubazeri_Flg;//�󂯗����t���O 

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
        if (collision.gameObject.name == "P_Enemy" && P_A_flg)
        {
            Debug.LogFormat("{1}�� {0}�Ƀ_���[�W��^�����I", collision.gameObject.name, gameObject.name);
        }
        if (collision.gameObject.name == "Enemy_HitBox" && P_G_flg)
        {
            Debug.LogFormat("�󂯗��������I");
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
