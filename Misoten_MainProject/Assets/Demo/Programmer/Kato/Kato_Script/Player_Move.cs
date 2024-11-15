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

    public GameObject Target;

    public GameObject Player;

    private float Player_Rotate;

    private float Timea;

    // Start is called before the first frame update
    void Start()
    {
        Player_Rotate = Player.transform.rotation.y;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {



        if (HandleMovementInput())
        {

            //MoveCharacter();
        }

    }


    // 入力デバイス管理
    private bool HandleMovementInput()
    {

        Timea += Time.deltaTime;
        float moveX = Input.GetAxis("Vertical");
        float RotateY = Input.GetAxis("Horizontal");

        float degree = Mathf.Atan2(RotateY,moveX) * Mathf.Rad2Deg;
        

        //if (degree < 0)
        //{
        //    degree += 360;
        //}

        Debug.Log((int)degree);
        //p_rotation =  degree+transform.rotation.y;

        

        //if (p_rotation >= 360)
        //{
        //    p_rotation -= 360;
        //}
        if(Timea>=0.5f)
        { 
        }
        Player_Rotate = degree;
        Player.transform.rotation = Quaternion.Euler(new Vector3(0, Player_Rotate, 0));

        Player_Rotate=gameObject.transform.rotation.y;
        if (MathF.Abs(moveX)+ MathF.Abs(RotateY)>=0.1f && MathF.Abs(moveX) + MathF.Abs(RotateY) < 1.0f)
        {
     
           
        }


        if (MathF.Abs(moveX) >= 0.05f || MathF.Abs(RotateY) >= 0.05f)
        {
            RUN_FLG = true;


            transform.position += transform.forward * Move_Speed * Time.deltaTime;
            // gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, gameObject.transform.rotation.y + degree*Time.deltaTime, 0)); //gameObject.transform.rotation; (new Vector3(0, degree * Time.deltaTime, 0));
            //gameObject.transform.rotation = gameObject.transform.rotation + new Vector3(transform.rotation.x, gameObject.transform.rotation.y + p_rotation, transform.rotation.z)
            //transform.position += transform.forward * Move_Speed * Time.deltaTime;
        }
        else
        {
            RUN_FLG = false;
        }

        degree = 0;

       // gameObject.transform.Rotate(new Vector3(0, RotateY, 0) * Time.deltaTime * Rotate_Speed);
       //transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, gameObject.transform.rotation.y + p_rotation, transform.rotation.z));

        //gameObject.transform.Rotate(new Vector3(0, degree, 0) * Time.deltaTime * Rotate_Speed);

        //// ゲームパッドの処理
        //if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        //{

        //    GamePadUpdate();
        //    return true;
        //}

        ////ジョイスティック右押し込み時カメラ
        //if (UnityEngine.Input.GetKeyDown("joystick button 9"))
        //{
        //    gameObject.transform.LookAt(Target.transform);
        //}



        return false;
    }

    // ゲームパッドの処理
    void GamePadUpdate()
    {

        float moveX = Input.GetAxis("Vertical");
        float RotateY = Input.GetAxis("Horizontal");
    
        if (MathF.Abs(moveX) >= 0.05f)
        {
            RUN_FLG = true;         
        }
        else
        {
            RUN_FLG = false;
        }

        transform.position += transform.forward * moveX* Move_Speed* Time.deltaTime;
        gameObject.transform.Rotate(new Vector3(0, RotateY , 0) * Time.deltaTime * Rotate_Speed);


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
