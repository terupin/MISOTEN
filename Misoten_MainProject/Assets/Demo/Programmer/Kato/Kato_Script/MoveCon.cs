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

    // ���͕ێ��p
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
        // �L�[���͂��擾

        inputDirection.z = Input.GetAxis("Horizontal");
        inputDirection.x = Input.GetAxis("Vertical");

        // ���C���J�����̌����ɂ���ē��͂𒲐�
        //Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
        //inputDirection = cameraForward * inputDirection.x + mainCamera.transform.right * inputDirection.z;

        moveDirection = inputDirection * maxspeed;
        // �����ꂩ�̕����ɓ��͂�����ꍇ�B
        if (inputDirection != Vector3.zero)
        {
            // ��]�I
            lookingDirection = inputDirection;
        }
        else
        {
        }

        // �����]������(�X���[�Y�ɉ�]����悤�A�኱�f�B���C�������Ă��܂�)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookingDirection), (rotatespeed * Time.deltaTime));
        //controller.Move(moveDirection * Time.deltaTime);
    }
}

