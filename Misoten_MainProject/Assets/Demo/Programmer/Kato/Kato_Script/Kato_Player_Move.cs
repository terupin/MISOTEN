using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Kato_Player_MOve : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField]
    public float Move_Speed = 1f;
    public float Rotate_Speed = 1f;
    //private float RotateY;
    private bool PushFlg_LTr = false;//Lスティック押下フラグ


    public GameObject Target; 

    private void Update()
    {
        HandleMovementInput();
    }


    // 入力デバイス管理
    private bool HandleMovementInput()
    {

        // キーボードの処理
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
        {

            KeyboardUpdate();
            return true;
        }

        // ゲームパッドの処理
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            GamePadUpdate();
            return true;
        }

        //ジョイスティック右押し込み時カメラを敵方向に向ける
        if (UnityEngine.Input.GetKeyDown("joystick button 9"))
        {
            gameObject.transform.LookAt(Target.transform);
        }



        return false;
    }

    // ゲームパッドの処理
    void GamePadUpdate()
    {
        var h = UnityEngine.Input.GetAxis("Horizontal");
        var v = UnityEngine.Input.GetAxis("Vertical");

        float degree = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }

        if (v == 0 && h == 0)
        {
            degree = 0;
        }

        //左スティック角度で方向転換・前進
        gameObject.transform.eulerAngles = new Vector3(0f, degree, 10f);

        if (UnityEngine.Input.GetKeyDown("joystick button 8"))
        {
            PushFlg_LTr = true;
        }
        //Lを離した時に押し込みフラグをfalseにする
        if (UnityEngine.Input.GetKeyUp("joystick button 8"))
        {
            PushFlg_LTr = false;
        }

        if (PushFlg_LTr)
        {
            transform.position += transform.forward * Move_Speed*Time.deltaTime;
        }



        //if (degree < 22.5f) { Katana_Direction = 0; }
        //else if (degree < 157.5f) { Katana_Direction = 3; }
        //else if (degree < 202.5f) { Katana_Direction = 4; }
        //else if (degree < 247.5f) { Katana_Direction = 5; }
        //else if (degree < 292.5f) { Katana_Direction = 6; }
        //else if (degree < 337.5f) { Katana_Direction = 7; }
        //else { Katana_Direction = 0; }

            //float moveX = Input.GetAxis("Vertical");
            //float RotateY = Input.GetAxis("Horizontal");

            //transform.position += transform.forward * moveX* Move_Speed* Time.deltaTime;

            //gameObject.transform.Rotate(new Vector3(0, RotateY , 0) * Time.deltaTime * Rotate_Speed);
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
