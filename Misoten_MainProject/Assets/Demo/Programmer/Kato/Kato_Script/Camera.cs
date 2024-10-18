using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Camera : MonoBehaviour
{
    public GameObject player;   //�v���C���[���i�[�p
    private Vector3 offset;      //���΋����擾�p

   public GameObject enemy;

    private bool PushFlg_A = false;//L�����t���O

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
            //Debug.Log("L���ꂽ");
        }

        GameObject obj = GameObject.Find("sword_test(Clone)");

        if (obj != null) 
        {
            if (PushFlg_A)
            {

                {
                    //transform.position = player.transform.position;
                    transform.position = (new Vector3(transform.position.x, 2.5f, transform.position.z));
                    //transform.Rotate(0, -360.0f * Time.deltaTime, 0);
                    transform.RotateAround(new Vector3((player.transform.position.x+ enemy.transform.position.x)/2, 3.0f, (player.transform.position.z + enemy.transform.position.z) / 2), -transform.up, 540 * Time.deltaTime);
                }
                if (transform.rotation.y < enemy.transform.rotation.y)
                {
                    transform.rotation = enemy.transform.rotation;
                    transform.position = enemy.transform.position - transform.forward * 6;
                    transform.position = (new Vector3(transform.position.x, 3.0f, transform.position.z));
                }



            

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
