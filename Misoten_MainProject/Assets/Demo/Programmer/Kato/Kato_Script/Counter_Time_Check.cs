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

    public float Check_Time;

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
        //�G�X�e�[�g
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

            Check_Current_Time = 0;
            //UnityEditor.EditorApplication.isPaused = true;
        }

        //�G�X�e�[�g
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren01"))
        {
            if (Miburo_State._Uke_Input)
            {
                Debug.Log(Check_Current_Time);
                UnityEditor.EditorApplication.isPaused = true;
            }
            Check_Current_Time += Time.deltaTime;
        }

        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren1"))
        {
            Debug.Log(Check_Current_Time);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time = 0;
        }

        //�G�X�e�[�g
        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren02"))
        {
            if (Miburo_State._Uke_Input)
            {
                Debug.Log(Check_Current_Time);
                UnityEditor.EditorApplication.isPaused = true;
            }
            Check_Current_Time += Time.deltaTime;
        }

        if (Enemy01_Animator.GetCurrentAnimatorStateInfo(0).IsName("Ren2"))
        {
            Debug.Log(Check_Current_Time);
            //UnityEditor.EditorApplication.isPaused = true;
            Check_Current_Time = 0;
        }
    }
}