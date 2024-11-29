using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter_Time_Check : MonoBehaviour
{
    [SerializeField, Header("�G01�A�j���[�^�[")]
    public Animator Enemy01_Animator;

    [SerializeField, Header("�݂Ԃ�A�j���[�^�[")]
    public Animator Miburo_Animator;

    private float Check_Current_Time;//���͊J�n����o�߂�������

    [SerializeField, Header("�c�؂� �ő���͗P�\ 1.7�b")]
    public float Check_Time0;
    [SerializeField, Header("�A��1 �ő���͗P�\ 1.2�b")]
    public float Check_Time1;
    [SerializeField, Header("�A��2 �ő���͗P�\ 0.5�b")]
    public float Check_Time2;

    //�c�؂� �ő���͗P�\ 1.7�b
    //�A��1 �ő���͗P�\ 1.2�b
    //�A��2 �ő���͗P�\ 0.5�b

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //�c�؂�U��グ
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            if (Miburo_State._Uke_Input)
            {
                if(Check_Current_Time > 0.0f &&Check_Time0>=Check_Current_Time)
                {
                    //�󂯗�������
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

        //�c�؂�U�肨�낵
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
        {
            Debug.Log(Check_Current_Time);


            Check_Current_Time = 0;
            //UnityEditor.EditorApplication.isPaused = true;
        }

        //�A��1�U��グ
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren01"))
        {
            if (Miburo_State._Uke_Input)
            {
                if (Check_Current_Time > 0.0f && Check_Time1 >= Check_Current_Time)
                {
                    //�󂯗�������
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

        //�A��1�U�肨�낵
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
        {           
            Debug.Log(Check_Current_Time);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time = 0;
        }

        //�A��2�U��グ
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren02"))
        {
            if (Miburo_State._Uke_Input)
            {
                if (Check_Current_Time > 0.0f && Check_Time2 >= Check_Current_Time)
                {
                    //�󂯗�������
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

        //�A��2�U�肨�낵
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren2"))
        {
            
            Debug.Log(Check_Current_Time);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time = 0;
        }
    }
}