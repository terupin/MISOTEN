using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Camera : MonoBehaviour
{
    public GameObject player;   //�v���C���[���i�[�p
    private Vector3 offset;      //���΋����擾�p
    public GameObject enemy;
    private bool PushFlg_A = false;//R�X�e�B�b�N�����t���O
    private bool PushFlg_RTr = false;//L�X�e�B�b�N�����t���O

    // Start is called before the first frame update
    void Start()
    {
        //�J�����̌�����ύX����
        transform.rotation = player.transform.rotation;
        transform.position = player.transform.position - transform.forward * 3;
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

        if (UnityEngine.Input.GetKeyDown("joystick button 9"))
        {
            PushFlg_RTr = true;
        }
        //L�𗣂������ɉ������݃t���O��false�ɂ���
        if (UnityEngine.Input.GetKeyUp("joystick button 9"))
        {
            PushFlg_RTr = false;

        }



        GameObject obj = GameObject.Find("sword_test(Clone)");

        if (obj != null)
        {
            if (PushFlg_A)
            {
                transform.rotation = enemy.transform.rotation;
                transform.Rotate(30, 30, 0);
                transform.position = enemy.transform.position - enemy.transform.forward * 4;
                transform.position = transform.position - enemy.transform.right * 3;
                transform.position = transform.position + enemy.transform.up * 4;
            }
            else
            {
                transform.position = obj.transform.position - transform.forward * 5;
                transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
            }
        }
        else
        {
           

            if (PushFlg_RTr)
            {
                transform.rotation = player.transform.rotation;
                transform.position = player.transform.position - transform.forward * 3;
            }

              
            transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));
        }
    }
}
