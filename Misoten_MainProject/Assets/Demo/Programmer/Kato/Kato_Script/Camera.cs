using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class Camera : MonoBehaviour
{
    [SerializeField, Header("�v���C���[�f�[�^")]
    public GameObject player;   //�v���C���[���i�[�p
    [SerializeField, Header("�J�����t�J�����Ɏg���G�f�[�^")]
    public GameObject enemy;
    private bool PushFlg_A = false;//L�����t���O

    [SerializeField, Header("�J�����t�J���p�x (30,30,0)")]
    public Vector3 CameraRot= new Vector3(30.0f,30.0f,0.0f);
    [SerializeField, Header("�J�����t�J������ (-3,4,-4)")]
    public Vector3 CameraPos = new Vector3(-3.0f, 4.0f, -4.0f);

    // Start is called before the first frame update
    void Start()
    {
        //�J�����̌�����ύX����
        transform.rotation = player.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //L�����������ɉ������݃t���O��TRUE�ɂ���
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            PushFlg_A = true;
        }
        //L�𗣂������ɉ������݃t���O��false�ɂ���
        if (UnityEngine.Input.GetKeyUp("joystick button 0"))
        {
            PushFlg_A = false;
        }

        GameObject obj = GameObject.Find("sword_test(Clone)");

        if (obj != null) 
        {
            if (PushFlg_A)
            {
                    transform.rotation = enemy.transform.rotation;
                    transform.Rotate(CameraRot);
                    transform.position = enemy.transform.position + enemy.transform.forward * CameraPos.z;
                    transform.position = transform.position + enemy.transform.right * CameraPos.x;
                    transform.position = transform.position + enemy.transform.up * CameraPos.y;
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