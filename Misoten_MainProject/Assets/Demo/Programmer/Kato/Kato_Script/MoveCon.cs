using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCon : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    public float maxspeed = 3.0f;
    public float rotatespeed = 360.0f;
    private Camera mainCamera = null;

    // 入力保持用
    private Vector3 inputDirection;
    private Vector3 lookingDirection;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        lookingDirection = new Vector3(1, 0, 1);

    }

    // Update is called once per frame
    void Update()
    {
        // キー入力を取得

        inputDirection.z = Input.GetAxis("Horizontal");
        inputDirection.x = Input.GetAxis("Vertical");

        // メインカメラの向きによって入力を調整
        //Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        //inputDirection = cameraForward * inputDirection.x + mainCamera.transform.right * inputDirection.z;

        moveDirection = inputDirection * maxspeed;
        // いずれかの方向に入力がある場合。
        if (inputDirection != Vector3.zero)
        {
            // 回転！
            lookingDirection = inputDirection;
        }
        else
        {
        }

        // 方向転換処理(スムーズに回転するよう、若干ディレイをかけています)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookingDirection), (rotatespeed * Time.deltaTime));
        //controller.Move(moveDirection * Time.deltaTime);
    }
}

