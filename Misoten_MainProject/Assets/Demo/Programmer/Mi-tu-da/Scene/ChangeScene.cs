using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //�V�[���؂�ւ��ɕK�v�Ȗ��O���

public class ChangeScene : MonoBehaviour
{
    public string SceneName; // �؂�ւ���̃V�[����

    public void ButtonClick()
    {
        // �w�肳�ꂽ�V�[���ɐ؂�ւ�
        if ((Input.GetKeyDown(KeyCode.Return)) || (Input.GetKeyDown(KeyCode.JoystickButton0))){

            SceneManager.LoadScene(SceneName);
        }
    }
}
