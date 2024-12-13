using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSet : MonoBehaviour{
    
    private bool UISetflag;

    void Start(){

        GetComponent<Canvas>().enabled = false;/*��\��*/
    }

    void Update(){

        //UI�̕\���E��\��
        if (UISetflag == true){

            if (Input.GetKeyDown(KeyCode.Escape)){/*��\��*/

                GetComponent<Canvas>().enabled = false;
                ResumeGame();
                UISetflag = false;
            }
        }
        else{

            if (Input.GetKeyDown(KeyCode.Escape)){/*�\��*/

                GetComponent<Canvas>().enabled = true;
                PauseGame();/*Time.deltatime�����ƈꎞ��~�ł��Ȃ�*/
                UISetflag = true;
            }
        }
    }

    // �Q�[���̎��Ԃ��~
    void PauseGame()
    {
        Time.timeScale = 0;
    }

    // �Q�[���̎��Ԃ��ĊJ
    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
