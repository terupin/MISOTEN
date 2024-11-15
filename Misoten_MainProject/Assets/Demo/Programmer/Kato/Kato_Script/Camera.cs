using System;
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
    private bool PushFlg_L = false;//L�����t���O

    [SerializeField, Header("�J�����t�J���p�x (30,30,0)")]
    public Vector3 CameraRot= new Vector3(30.0f,30.0f,0.0f);
    [SerializeField, Header("�J�����t�J������ (-3,4,-4)")]
    public Vector3 CameraPos = new Vector3(-3.0f, 4.0f, -4.0f);

    [SerializeField, Header("�J�����ړ��X�s�[�h")]
    public float CamSpd=200;

    private float Cam_Spd;

    [SerializeField, Header("�J�����ړ�Y����l")]
    public float MaxX = 20.0f;

    [SerializeField, Header("�J�����ړ�Y�����l")]
    public float MinX = -20.0f;

    public GameObject cam;




    // Start is called before the first frame update
    void Start()
    {

        Cam_Spd= CamSpd;
        //�J�����̌�����ύX����
        transform.rotation = player.transform.rotation;

        transform.position = player.transform.position - transform.forward * 3;
        transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
        transform.rotation = player.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //A�{�^�������������ɉ������݃t���O��TRUE�ɂ���
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            PushFlg_A = true;
        }
        //A�{�^���𗣂������ɉ������݃t���O��false�ɂ���
        if (UnityEngine.Input.GetKeyUp("joystick button 0"))
        {
            PushFlg_A = false;
        }
        //L�����������ɉ������݃t���O��TRUE�ɂ���
        if (UnityEngine.Input.GetKeyDown("joystick button 4"))
        {
            PushFlg_L = true;
        }
        //L�𗣂������ɉ������݃t���O��false�ɂ���
        if (UnityEngine.Input.GetKeyUp("joystick button 4"))
        {
            PushFlg_L = false;
            //Debug.Log("L���ꂽ");
        }


        GameObject obj = GameObject.Find("sword_test(Clone)");

        if (obj != null) 
        {
            if (PushFlg_A)
            {
                transform.position = obj.transform.position - transform.forward * 5;
                transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
            }
            else
            {

                if (Kato_a_Player_Anim.Katana_Direction == 0 || Kato_a_Player_Anim.Katana_Direction == 1 || Kato_a_Player_Anim.Katana_Direction == 2 || Kato_a_Player_Anim.Katana_Direction == 7)
                {
                    transform.rotation = enemy.transform.rotation;
                    transform.Rotate(new Vector3(CameraRot.x, -CameraRot.y, CameraRot.z));
                    transform.position = enemy.transform.position + enemy.transform.forward * CameraPos.z;
                    transform.position = transform.position - enemy.transform.right * CameraPos.x;
                    transform.position = transform.position + enemy.transform.up * CameraPos.y;
                    //UnityEditor.EditorApplication.isPaused = true;
                }
                else if (Kato_a_Player_Anim.Katana_Direction == 4 || Kato_a_Player_Anim.Katana_Direction == 5 || Kato_a_Player_Anim.Katana_Direction == 6 || Kato_a_Player_Anim.Katana_Direction == 3)
                {
                    transform.rotation = enemy.transform.rotation;
                    transform.Rotate(CameraRot);
                    transform.position = enemy.transform.position + enemy.transform.forward * CameraPos.z;
                    transform.position = transform.position + enemy.transform.right * CameraPos.x;
                    transform.position = transform.position + enemy.transform.up * CameraPos.y;
                    //UnityEditor.EditorApplication.isPaused = true;
                }
                else
                {
                    Debug.Log(Kato_a_Player_Anim.Katana_Direction);
                    
                    //UnityEditor.EditorApplication.isPaused = true;
                }
            }
        }
        else
        {

            if(PushFlg_L)
            {
                player.gameObject.transform.LookAt(enemy.transform.position);
                transform.rotation = player.transform.rotation;

            }
            else
            {
                float h = UnityEngine.Input.GetAxis("Vertical2");
                float v = UnityEngine.Input.GetAxis("Horizontal2");

                
                if (h!=0)
                {
                    transform.LookAt(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f));
                }



                {
                    if (MathF.Abs(h) > MathF.Abs(v))
                    {
                       
                        if (h > 0)
                        {
                            Cam_Spd -= 8;

                            if (Cam_Spd < 0)
                            {
                                Cam_Spd = 0;
                            }
                            if (cam.transform.rotation.x <= MaxX)
                            {


                                if (transform.rotation.y > 90 && transform.rotation.y < 270)
                                {
                                    transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.right, -Time.deltaTime * Cam_Spd);
                                }
                                else
                                {
                                    transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.right, -Time.deltaTime * Cam_Spd);
                                }

                            }


                        }
                        else if (h < 0)
                        {
                            Cam_Spd -= 20;

                            if (Cam_Spd < 0)
                            {
                                Cam_Spd = 0;
                            }
                            if (cam.transform.rotation.x >= MinX)
                            {
                                if (transform.rotation.y > 90 && transform.rotation.y < 270)
                                {
                                    transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.right, Time.deltaTime * Cam_Spd);
                                }
                                else
                                {
                                    transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.right, Time.deltaTime * Cam_Spd);
                                }
                            }


                        }
                    }
                    else
                    {


                        Cam_Spd-=2;

                        if (Cam_Spd < 0)
                        {
                            Cam_Spd = 0;
                        }

                        if (v > 0)
                        {

                                transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.up, Time.deltaTime * Cam_Spd);
                            


                        }
                        else if (v < 0)
                        {

                                transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.up, -Time.deltaTime * Cam_Spd);
                            

                        }
                    }
                }


                if (h == 0 && v == 0)
                {
                    Cam_Spd = CamSpd;
                    transform.position = player.transform.position - transform.forward * 5;
                    transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
                    transform.rotation = player.transform.rotation;
                }
                else
                {

                    if (Cam_Spd < 0)
                    {
                        Cam_Spd = 0;
                    }
                }

            }
        }

    }
}
