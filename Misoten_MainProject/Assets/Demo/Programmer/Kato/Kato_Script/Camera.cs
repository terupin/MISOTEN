using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Camera : MonoBehaviour
{
    public GameObject player;   //�v���C���[���i�[�p
    private Vector3 offset;      //���΋����擾�p

    // Start is called before the first frame update
    void Start()
    {

        // MainCamera(�������g)��player�Ƃ̑��΋��������߂�
        offset = transform.position - player.transform.position;

        //�J�����̌�����ύX����
        transform.rotation = player.transform.rotation;
    }



    // Update is called once per frame
    void Update()
    {

        //if (UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.D))
        //{
        //}
        //else
        //{
        //    //�J�����̌�����ύX����
        //    transform.rotation = player.transform.rotation;
        //}


        //�V�����g�����X�t�H�[���̒l��������
        //transform.position = player.transform.position - transform.forward*10;
        //transform.position = (new Vector3(transform.position.x, 5.0f, transform.position.z));

        transform.position = player.transform.position - transform.forward*3 ;
        transform.position = (new Vector3(transform.position.x, 2.0f, transform.position.z));



    }
}
