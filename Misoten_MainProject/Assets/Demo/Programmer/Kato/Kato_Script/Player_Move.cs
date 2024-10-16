using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MOve : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField]
    public float Move_Speed = 1f;
    public float Rotate_Speed = 1f;
    //private float RotateY;

    private void Update()
    {
        //if (Input.GetKey("Y-Axis"))
        //{

        //}


        //    if (Input.anyKey)
        //{
        //    var velocity = Vector3.zero;

        //    if (Input.GetKey(KeyCode.W))
        //    {
        //        transform.position += transform.forward * Time.deltaTime;
        //    }
        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        RotateY = -Rotate_Speed;
        //    }
        //    if (Input.GetKey(KeyCode.S))
        //    {
        //        transform.position -= transform.forward * Time.deltaTime;
        //    }
        //    if (Input.GetKey(KeyCode.D))
        //    {
        //        RotateY = Rotate_Speed;
        //    }
        //    //if (velocity.x != 0 || velocity.z != 0)
        //    //{
        //    //    transform.position += transform.rotation * velocity;
        //    //}


        //    gameObject.transform.Rotate(new Vector3(0, RotateY, 0) * Time.deltaTime);

        //   RotateY = 0;
        //}


        if (HandleMovementInput())
        {

            //MoveCharacter();
        }

    }


    // ���̓f�o�C�X�Ǘ�
    private bool HandleMovementInput()
    {

        // �L�[�{�[�h�̏���
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
        {

            KeyboardUpdate();
            return true;
        }

        // �Q�[���p�b�h�̏���
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {

            GamePadUpdate();
            return true;
        }

        return false;
    }

    // �Q�[���p�b�h�̏���
    void GamePadUpdate()
    {

        float moveX = Input.GetAxis("Vertical");
        float RotateY = Input.GetAxis("Horizontal");

        transform.position += transform.forward * moveX* Move_Speed* Time.deltaTime;
        gameObject.transform.Rotate(new Vector3(0, RotateY , 0) * Time.deltaTime * Rotate_Speed);


        //Debug.Log("GamePad�g�p��");
    }

    // �L�[�{�[�h�̏���
    void KeyboardUpdate()
    {

        //float moveX = 0f;
        //float moveZ = 0f;

        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveZ = 1f;
        //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveZ = -1f;
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1f;

        //movementInput = new Vector3(moveX, 0, moveZ).normalized;

        ////Debug.Log("Keyboard�g�p��");
    }


}
