using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class cameratest : MonoBehaviour
{
    private GameObject player;       //プレイヤー格納用
   
   
    // Use this for initialization
    void Start()
    {
        //unitychanをplayerに格納
        player = GameObject.Find("unitychan");
    }

    // Update is called once per frame
    void Update()
    {
        float axisx = Input.GetAxis("Horizontal");

        //左シフトが押されている時
        if (axisx < 0)
        {
            //ユニティちゃんを中心に-5f度回転
            transform.RotateAround(player.transform.position, Vector3.up, -5f);
        }
        //右シフトが押されている時
        else if (axisx > 0)
            {
            //ユニティちゃんを中心に5f度回転
            transform.RotateAround(player.transform.position, Vector3.up, 5f);
        }

    }
}

