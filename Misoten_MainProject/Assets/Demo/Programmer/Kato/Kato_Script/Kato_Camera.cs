using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Camera : MonoBehaviour
{
    public GameObject player;   //プレイヤー情報格納用
    private Vector3 offset;      //相対距離取得用
    public GameObject enemy;
    private bool PushFlg_A = false;//Rスティック押下フラグ
    private bool PushFlg_RTr = false;//Lスティック押下フラグ

    // Start is called before the first frame update
    void Start()
    {
        //カメラの向きを変更する
        transform.rotation = player.transform.rotation;
        transform.position = player.transform.position - transform.forward * 3;
    }

    // Update is called once per frame
    void Update()
    {
        //Lを押した時に押し込みフラグをTRUEにする
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            PushFlg_A = true;
        }
        //Lを離した時に押し込みフラグをfalseにする
        if (UnityEngine.Input.GetKeyUp("joystick button 0"))
        {
            PushFlg_A = false;

        }

        if (UnityEngine.Input.GetKeyDown("joystick button 9"))
        {
            PushFlg_RTr = true;
        }
        //Lを離した時に押し込みフラグをfalseにする
        if (UnityEngine.Input.GetKeyUp("joystick button 9"))
        {
            PushFlg_RTr = false;

        }



        GameObject obj = GameObject.Find("sword_test(Clone)");

        if (obj != null)
        {
            if (PushFlg_A)
            {
                transform.rotation = enemy.transform.rotation;
                transform.Rotate(30, 30, 0);
                transform.position = enemy.transform.position - enemy.transform.forward * 4;
                transform.position = transform.position - enemy.transform.right * 3;
                transform.position = transform.position + enemy.transform.up * 4;
            }
            else
            {
                transform.position = obj.transform.position - transform.forward * 5;
                transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
            }
        }
        else
        {
           

            if (PushFlg_RTr)
            {
                transform.rotation = player.transform.rotation;
                transform.position = player.transform.position - transform.forward * 3;
            }

              
            transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
        }
    }
}
