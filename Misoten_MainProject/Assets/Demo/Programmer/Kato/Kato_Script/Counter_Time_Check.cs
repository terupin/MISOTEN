using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter_Time_Check : MonoBehaviour
{
    [SerializeField, Header("敵01アニメーター")]
    public Animator Enemy01_Animator;

    [SerializeField, Header("みぶろアニメーター")]
    public Animator Miburo_Animator;

    private float Check_Current_Time;//入力開始から経過した時間
    
    //縦切り 入力猶予 1.7秒

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        //敵ステート
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            if (Miburo_State._Uke_Input)
            {

                Debug.Log(Check_Current_Time);
                UnityEditor.EditorApplication.isPaused = true;
            }

            Check_Current_Time += Time.deltaTime;
        }

        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
        {
                Debug.Log(Check_Current_Time);
                UnityEditor.EditorApplication.isPaused = true;
            
        }


    }
}