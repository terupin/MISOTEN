using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //シーン切り替えに必要な名前空間

public class ChangeScene : MonoBehaviour
{
    public string SceneName; // 切り替え先のシーン名

    public void ButtonClick()
    {
        // 指定されたシーンに切り替え
        if ((Input.GetKeyDown(KeyCode.Return)) || (Input.GetKeyDown(KeyCode.JoystickButton0))){

            SceneManager.LoadScene(SceneName);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Main_Stage02");
        }
    }
}
