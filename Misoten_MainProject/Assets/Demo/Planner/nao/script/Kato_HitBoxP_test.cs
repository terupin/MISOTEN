using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Kato_HitBoxP_test : MonoBehaviour
{
    [SerializeField, Header("剣先")]
    public GameObject WeponPoint;

    [SerializeField, Header("剣根本")]
    public GameObject WeponRoot;

    public GameObject Enemy_Model;

    private bool P_G_flg = Kato_a_Player_Anim_test.G_Flg;
    private bool P_A_flg = Kato_a_Player_Anim_test.A_Flg;

    public static bool Tubazeri_Flg;//受け流しフラグ 

    //カメラ切り替え用変数
    public static bool changecamstartL = false;
    public static bool changecamstartR = false;
    private float camchangecount = 0.0f;
    public CinemachineVirtualCamera ukenagasiLCam;
    public CinemachineVirtualCamera ukenagasiRCam;

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
        P_A_flg = Kato_a_Player_Anim_test.A_Flg;

        if (Tubazeri_Flg && P_A_flg)
        {
            //UnityEditor.EditorApplication.isPaused = true;
        }

        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;

        //if(changecamstartL == true)
        //{
        //    camchangecount++;
        //    ukenagasiLCam.Priority = 15;

        //}
        //else if(camchangecount >= 5)
        //{
        //    changecamstartL = false;
        //    camchangecount = 0;
        //    ukenagasiLCam.Priority = 1;
        //}
        //if (changecamstartR == true)
        //{
        //    camchangecount++;
        //    ukenagasiRCam.Priority = 15;

        //}
        //else if (camchangecount >= 5)
        //{
        //    changecamstartR = false;
        //    camchangecount = 0;
        //    ukenagasiRCam.Priority = 2;
        //}

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy" && P_A_flg)
        {
            Debug.LogFormat("{1}は {0}にダメージを与えた！", collision.gameObject.name, gameObject.name);
            Enemy_Model.AddComponent<Enemy_Damage>();
            //UnityEditor.EditorApplication.isPaused = true;
        }
        if (collision.gameObject.name == "Enemy" && Kato_HitBoxE_test.Ukenagashi_Flg)
        {
            Enemy_Model.AddComponent<UkenagashiDamage>();
            Debug.LogFormat("受け流し成功！");
            Tubazeri_Flg = true;
            //if(Input.GetAxis("Horizontal2") < -0.01)
            //{
            //    changecamstartL = true;
            //}
            //else if(Input.GetAxis("Horizontal2") > 0.01)
            //{
            //    changecamstartR = true;
            //}
            
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
