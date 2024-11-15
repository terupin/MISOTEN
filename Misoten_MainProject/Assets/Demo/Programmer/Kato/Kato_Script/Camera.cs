using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class Camera : MonoBehaviour
{
    [SerializeField, Header("プレイヤーデータ")]
    public GameObject player;   //プレイヤー情報格納用
    [SerializeField, Header("カメラフカン時に使う敵データ")]
    public GameObject enemy;
    private bool PushFlg_A = false;//L押下フラグ
    private bool PushFlg_L = false;//L押下フラグ

    [SerializeField, Header("カメラフカン角度 (30,30,0)")]
    public Vector3 CameraRot= new Vector3(30.0f,30.0f,0.0f);
    [SerializeField, Header("カメラフカン距離 (-3,4,-4)")]
    public Vector3 CameraPos = new Vector3(-3.0f, 4.0f, -4.0f);

    [SerializeField, Header("カメラ移動スピード")]
    public float CamSpd=100;

    [SerializeField, Header("カメラ移動Y上限値")]
    public float MaxX = 20.0f;

    [SerializeField, Header("カメラ移動Y下限値")]
    public float MinX = -20.0f;

    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        //カメラの向きを変更する
        transform.rotation = player.transform.rotation;

        transform.position = player.transform.position - transform.forward * 3;
        transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
        transform.rotation = player.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //Aボタンを押した時に押し込みフラグをTRUEにする
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            PushFlg_A = true;
        }
        //Aボタンを離した時に押し込みフラグをfalseにする
        if (UnityEngine.Input.GetKeyUp("joystick button 0"))
        {
            PushFlg_A = false;
        }
        //Lを押した時に押し込みフラグをTRUEにする
        if (UnityEngine.Input.GetKeyDown("joystick button 4"))
        {
            PushFlg_L = true;
        }
        //Lを離した時に押し込みフラグをfalseにする
        if (UnityEngine.Input.GetKeyUp("joystick button 4"))
        {
            PushFlg_L = false;
            //Debug.Log("L離れた");
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
           

                if(h!=0)
                {
                    transform.LookAt(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f));
                }

                if (MathF.Abs(h) > MathF.Abs(v))
                {
                    if (h > 0)
                    {
                        if (cam.transform.rotation.x <= MaxX)
                        {
                            if (transform.rotation.y > 90 && transform.rotation.y < 270)
                            {
                                transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.right, -Time.deltaTime * CamSpd);
                            }
                            else
                            {
                                transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.right, -Time.deltaTime * CamSpd);
                            }

                        }


                    }
                    else if (h < 0)
                    {

                        if (cam.transform.rotation.x >= MinX)
                        {
                            if (transform.rotation.y > 90 && transform.rotation.y < 270)
                            {
                                transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.right, Time.deltaTime * CamSpd);
                            }
                            else
                            {
                                transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.right, Time.deltaTime * CamSpd);
                            }
                        }


                    }
                }
                else
                {
                    if (v > 0)
                    {
                        transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.up, Time.deltaTime * CamSpd);

                    }
                    else if (v < 0)
                    {
                        transform.RotateAround(player.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.up, -Time.deltaTime * CamSpd);
                    }
                }








                if (h == 0 && v == 0)
                {

                    Cam_Spd = CamSpd;
                    transform.position = player.transform.position - transform.forward * 3;

                    transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
                    transform.rotation = player.transform.rotation;
                }


            }
        }

    }
}
