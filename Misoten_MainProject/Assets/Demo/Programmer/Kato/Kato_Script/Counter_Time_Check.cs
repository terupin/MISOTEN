using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter_Time_Check : MonoBehaviour
{
    [SerializeField, Header("ìG01ÉAÉjÉÅÅ[É^Å[")]
    public Animator Enemy01_Animator;

    [SerializeField, Header("Ç›Ç‘ÇÎÉAÉjÉÅÅ[É^Å[")]
    public Animator Miburo_Animator;

    private float Check_Current_Time;//ì¸óÕäJénÇ©ÇÁåoâﬂÇµÇΩéûä‘

    [SerializeField, Header("ècêÿÇË ç≈ëÂì¸óÕóPó\ 1.7ïb")]
    public float Check_Time0;
    [SerializeField, Header("òAåÇ1 ç≈ëÂì¸óÕóPó\ 1.2ïb")]
    public float Check_Time1;
    [SerializeField, Header("òAåÇ2 ç≈ëÂì¸óÕóPó\ 0.5ïb")]
    public float Check_Time2;

    //ècêÿÇË ç≈ëÂì¸óÕóPó\ 1.7ïb
    //òAåÇ1 ç≈ëÂì¸óÕóPó\ 1.2ïb
    //òAåÇ2 ç≈ëÂì¸óÕóPó\ 0.5ïb

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //ècêÿÇËêUÇËè„Ç∞
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            if (Miburo_State._Uke_Input)
            {
                if(Check_Current_Time > 0.0f &&Check_Time0>=Check_Current_Time)
                {
                    //éÛÇØó¨Çµê¨å˜
                    Debug.Log(Check_Current_Time);
                    if(Miburo_State._Ukenagashi_L)
                    {
                        Enemy01_Animator.SetBool("UkeL",true);
                    }
                    else if (Miburo_State._Ukenagashi_R)
                    {
                        Enemy01_Animator.SetBool("UkeR", true);
                    }
                    //UnityEditor.EditorApplication.isPaused = true;
                }
            }
            Check_Current_Time += Time.deltaTime;
        }
        else
        {
            Enemy01_Animator.SetBool("UkeL", false);
            Enemy01_Animator.SetBool("UkeR", false);
            Check_Current_Time = 0;
        }

        //ècêÿÇËêUÇËÇ®ÇÎÇµ
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
        {
            Debug.Log(Check_Current_Time);


            Check_Current_Time = 0;
            //UnityEditor.EditorApplication.isPaused = true;
        }

        //òAåÇ1êUÇËè„Ç∞
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren01"))
        {
            if (Miburo_State._Uke_Input)
            {
                if (Check_Current_Time > 0.0f && Check_Time1 >= Check_Current_Time)
                {
                    //éÛÇØó¨Çµê¨å˜
                    Debug.Log(Check_Current_Time);
                    //UnityEditor.EditorApplication.isPaused = true;
                    Enemy01_Animator.SetBool("RenUke01", true);
                }
            }
            Check_Current_Time += Time.deltaTime;
        }
        else
        {
            Enemy01_Animator.SetBool("RenUke01", false);
            //Check_Current_Time = 0;
        }

        //òAåÇ1êUÇËÇ®ÇÎÇµ
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
        {           
            Debug.Log(Check_Current_Time);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time = 0;
        }

        //òAåÇ2êUÇËè„Ç∞
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren02"))
        {
            if (Miburo_State._Uke_Input)
            {
                if (Check_Current_Time > 0.0f && Check_Time2 >= Check_Current_Time)
                {
                    //éÛÇØó¨Çµê¨å˜
                    Enemy01_Animator.SetBool("RenUke02", true);
                    Debug.Log(Check_Current_Time);
                    //UnityEditor.EditorApplication.isPaused = true;
                }
            }
            Check_Current_Time += Time.deltaTime;
        }
        else
        {
            Enemy01_Animator.SetBool("RenUke02", false);
            //Check_Current_Time = 0;
        }

        //òAåÇ2êUÇËÇ®ÇÎÇµ
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren2"))
        {
            
            Debug.Log(Check_Current_Time);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time = 0;
        }
    }
}