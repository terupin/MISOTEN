using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Camera : MonoBehaviour
{
    public GameObject player;   //プレイヤー情報格納用
    private Vector3 offset;      //相対距離取得用

   public GameObject enemy;

    private bool PushFlg_A = false;//L押下フラグ

    // Start is called before the first frame update
    void Start()
    {

        //カメラの向きを変更する
        transform.rotation = player.transform.rotation;
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
            //Debug.Log("L離れた");
        }

        GameObject obj = GameObject.Find("sword_test(Clone)");

        if (obj != null) 
        {
            if (PushFlg_A)
            {

                //{
                //    //transform.position = player.transform.position;
                //    transform.position = (new Vector3(transform.position.x, 2.5f, transform.position.z));
                //    //transform.Rotate(0, -360.0f * Time.deltaTime, 0);
                //    transform.RotateAround(new Vector3((player.transform.position.x+ enemy.transform.position.x)/2, 3.0f, (player.transform.position.z + enemy.transform.position.z) / 2), -transform.up, 540 * Time.deltaTime);
                //}
                //if (transform.rotation.y < enemy.transform.rotation.y)
                {
                    transform.rotation = enemy.transform.rotation;
                    transform.Rotate(30, 30, 0);
                    transform.position = enemy.transform.position - enemy.transform.forward * 4;
                    transform.position = transform.position - enemy.transform.right * 3;
                    transform.position = transform.position + enemy.transform.up * 4;
                    //transform.position = (new Vector3(enemy.transform.position.x, 4.5f, enemy.transform.position.z - enemy.transform.forward * 4));
                }



            

            }
            else
            {
                transform.position = obj.transform.position - transform.forward * 5;
                transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
            }

            //UnityEditor.EditorApplication.isPaused = true;



        }
        else
        {
            transform.rotation = player.transform.rotation;

            transform.position = player.transform.position - transform.forward * 3;
            transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
        }





    }
}
