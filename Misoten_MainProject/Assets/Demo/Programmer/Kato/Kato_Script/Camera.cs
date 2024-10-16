using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Camera : MonoBehaviour
{
    public GameObject player;   //プレイヤー情報格納用
    private Vector3 offset;      //相対距離取得用

    // Start is called before the first frame update
    void Start()
    {

        // MainCamera(自分自身)とplayerとの相対距離を求める
        offset = transform.position - player.transform.position;

        //カメラの向きを変更する
        transform.rotation = player.transform.rotation;
    }



    // Update is called once per frame
    void Update()
    {

        //if (UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.D))
        //{
        //}
        //else
        //{
        //    //カメラの向きを変更する
        //    transform.rotation = player.transform.rotation;
        //}


        //新しいトランスフォームの値を代入する
        //transform.position = player.transform.position - transform.forward*10;
        //transform.position = (new Vector3(transform.position.x, 5.0f, transform.position.z));

        transform.position = player.transform.position - transform.forward*3 ;
        transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));



    }
}
