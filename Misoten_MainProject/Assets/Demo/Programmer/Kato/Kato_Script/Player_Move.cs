using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player_Move : MonoBehaviour
{
    //[SerializeField]
    //public float Move_Speed ;
    //static public bool RUN_FLG;

    //[SerializeField, Header("敵プレハブ")]
    //public GameObject Target;

    //[SerializeField, Header("プレイヤープレハブ")]
    //public GameObject Player;

    //[SerializeField, Header("カメラ")]
    //public GameObject Camera_o;

    void Start()
    {
     
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.N))
        {
            transform.position += transform.right   * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.M))
        {
            transform.position -= transform.right * Time.deltaTime;
        }

        //Player_Run();
    }

    // public void Player_Run()
    //{
    //    float moveX = Input.GetAxis("Vertical");
    //    float RotateY = Input.GetAxis("Horizontal");

    //    float degree = Mathf.Atan2(RotateY, moveX) * Mathf.Rad2Deg;//コントローラー角度取得

    //    if (MathF.Abs(moveX) >= 0.05f || MathF.Abs(RotateY) >= 0.05f)
    //    {
    //        if (Kato_Status_P.NowHP > 0)
    //        {
    //            Player.transform.rotation = Quaternion.Euler(new Vector3(0, Camera_o.transform.localEulerAngles.y + degree, 0));
    //            transform.position += transform.forward * Move_Speed * Time.deltaTime;

    //            RUN_FLG = true;
    //        }
    //    }
    //    else
    //    {
    //        RUN_FLG = false;
    //    }
    //}


}
