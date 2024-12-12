using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //�V�[���؂�ւ��ɕK�v�Ȗ��O���

public class ChangeScene : MonoBehaviour
{
    public string SceneName; // �؂�ւ���̃V�[����
    public Button StartButton; // �A�^�b�`����{�^��

    private void Start()
    {
        // �{�^���ɃN���b�N�C�x���g��ǉ�
        StartButton.onClick.AddListener(ButtonClick);
    }

    public void ButtonClick()
    {
        // �w�肳�ꂽ�V�[���ɐ؂�ւ�
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            SceneManager.LoadScene(SceneName);
        }
    }

    private void Update()
    {
        // Xbox�R���g���[���[��A�{�^���ijoystick button 0�j�����o
        
    }
}
