using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSet : MonoBehaviour{
    
    private bool UISetflag;

    void Start(){

        GetComponent<Canvas>().enabled = false;/*非表示*/
    }

    void Update(){

        //UIの表示・非表示
        if (UISetflag == true){

            if (Input.GetKeyDown(KeyCode.Escape)){/*非表示*/

                GetComponent<Canvas>().enabled = false;
                ResumeGame();
                UISetflag = false;
            }
        }
        else{

            if (Input.GetKeyDown(KeyCode.Escape)){/*表示*/

                GetComponent<Canvas>().enabled = true;
                PauseGame();/*Time.deltatime無いと一時停止できない*/
                UISetflag = true;
            }
        }
    }

    // ゲームの時間を停止
    void PauseGame()
    {
        Time.timeScale = 0;
    }

    // ゲームの時間を再開
    void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
