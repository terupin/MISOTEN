using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class cameratest : MonoBehaviour
{
    private GameObject player;       //�v���C���[�i�[�p
   
   
    // Use this for initialization
    void Start()
    {
        //unitychan��player�Ɋi�[
        player = GameObject.Find("unitychan");
    }

    // Update is called once per frame
    void Update()
    {
        float axisx = Input.GetAxis("Horizontal");

        //���V�t�g��������Ă��鎞
        if (axisx < 0)
        {
            //���j�e�B�����𒆐S��-5f�x��]
            transform.RotateAround(player.transform.position, Vector3.up, -5f);
        }
        //�E�V�t�g��������Ă��鎞
        else if (axisx > 0)
            {
            //���j�e�B�����𒆐S��5f�x��]
            transform.RotateAround(player.transform.position, Vector3.up, 5f);
        }

    }
}

