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
    
    //�c�؂� ���͗P�\ 1.7�b

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
                UnityEditor.EditorApplication.isPaused = true;
            
        }


    }
}