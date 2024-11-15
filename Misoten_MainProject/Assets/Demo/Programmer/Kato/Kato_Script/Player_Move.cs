using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player_Move : MonoBehaviour
{
    [SerializeField]
    public float Move_Speed = 1f;
    public float Rotate_Speed = 1f;
    //private float RotateY;
    static public bool RUN_FLG;

    [SerializeField, Header("敵プレハブ")]
    public GameObject Target;

    [SerializeField, Header("プレイヤープレハブ")]
    public GameObject Player;

    //private float Player_Rotate;
    //private float Now_Rotate;


    //private float Timea;

    //private bool D_FLG;

    private float resetrot;

    // Start is called before the first frame update
    void Start()
    {
       // Player_Rotate = Player.transform.rotation.y;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Vertical");
        float RotateY = Input.GetAxis("Horizontal");

        float degree = Mathf.Atan2(RotateY, moveX) * Mathf.Rad2Deg;

        if (MathF.Abs(moveX) >= 0.05f || MathF.Abs(RotateY) >= 0.05f)
        {
            Player.transform.rotation = Quaternion.Euler(new Vector3(0, degree, 0));
            transform.position += transform.forward * Move_Speed * Time.deltaTime;

            RUN_FLG = true;
        }
        else
        {
            RUN_FLG = false;
        }

    }


    // 入力デバイス管理
    private bool HandleMovementInput()
    {

    

        //if (MathF.Abs(moveX) >= 0.05f || MathF.Abs(RotateY) >= 0.05f)
        //{
        //    Player.transform.rotation = Quaternion.Euler(new Vector3(0, Player_Rotate + degree, 0));

        //    resetrot = Player_Rotate + degree;

        //    transform.position += transform.forward * Move_Speed * Time.deltaTime;

        //    RUN_FLG = true;
        //}
        //else
        //{
        //    Player_Rotate = resetrot;
        //    Player.transform.rotation = Quaternion.Euler(new Vector3(0, resetrot, 0));

        //    RUN_FLG = false;

        //}

        //degree = 0;

        return false;
    }

    // ゲームパッドの処理
    void GamePadUpdate()
    {

        //float moveX = Input.GetAxis("Vertical");
        //float RotateY = Input.GetAxis("Horizontal");
    
        //if (MathF.Abs(moveX) >= 0.05f)
        //{
        //    RUN_FLG = true;         
        //}
        //else
        //{
        //    RUN_FLG = false;
        //}

        //transform.position += transform.forward * moveX* Move_Speed* Time.deltaTime;
        //gameObject.transform.Rotate(new Vector3(0, RotateY , 0) * Time.deltaTime * Rotate_Speed);


        //Debug.Log("GamePad使用中");
    }

    // キーボードの処理
    void KeyboardUpdate()
    {

        //float moveX = 0f;
        //float moveZ = 0f;

        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveZ = 1f;
        //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveZ = -1f;
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1f;

        //movementInput = new Vector3(moveX, 0, moveZ).normalized;

        ////Debug.Log("Keyboard使用中");
    }


}
