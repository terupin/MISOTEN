using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //シーン切り替えに必要な名前空間


public class ChangeScene : MonoBehaviour
{
    public string SceneName;

    public void ButtonClick()
    {
        SceneManager.LoadScene(SceneName);
    }
}
