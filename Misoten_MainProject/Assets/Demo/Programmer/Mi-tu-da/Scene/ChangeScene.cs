using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //�V�[���؂�ւ��ɕK�v�Ȗ��O���


public class ChangeScene : MonoBehaviour
{
    public string SceneName;

    public void ButtonClick()
    {
        SceneManager.LoadScene(SceneName);
    }
}
