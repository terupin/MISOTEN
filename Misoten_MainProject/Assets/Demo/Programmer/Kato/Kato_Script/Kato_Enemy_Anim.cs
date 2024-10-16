using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Enemy_Anim : MonoBehaviour
{
    public Animator Enemy_Animator;

    public string R_Anim_bool ;  // 終了を検知したいアニメーションの名前
    //public string L_Anim_bool = "L_counter";  // 終了を検知したいアニメーションの名前
    //public string Gard_Anim_bool = "L_counter";  // 終了を検知したいアニメーションの名前

    public string R_Anim_name;  // 終了を検知したいアニメーションの名前
    //public string L_Anim_name;  // 終了を検知したいアニメーションの名前
    //public string Grad_Anim_name;  // 終了を検知したいアニメーションの名前

    public static int randomValue = -1; /* ランダムな値を格納する */




    private int Katana_Direction = -1;

    private bool Ukenagasi_Flg = false;//受け流しフラグ


    public float CullTime;
    private float CurrentTime=0.0f;

    private float AttackTime = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        randomValue = -1;
        CurrentTime = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animatorStateInfo = Enemy_Animator.GetCurrentAnimatorStateInfo(0);



        if (CurrentTime > CullTime)
        {
            Enemy_Animator.SetBool(R_Anim_bool, false);

            randomValue = -1;
            //randomValue = UnityEngine.Random.Range(0, 8);
            CurrentTime = 0.0f;
        }
        else
        {
            Enemy_Animator.SetBool(R_Anim_bool, true);
        }





        ////Lを押した時に押し込みフラグをTRUEにする
        //if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        //{
        //    Enemy_Animator.SetBool(R_Anim_bool, true);
        //}




        if (animatorStateInfo.IsName(R_Anim_name))
        {
            if (animatorStateInfo.normalizedTime >= 0.9f && !animatorStateInfo.loop)
            {
                Enemy_Animator.SetBool(R_Anim_bool, false);
                //CurrentTime = 0;
                //randomValue = -1;
            }
        }

        if (CurrentTime==0.0f)
        {
            randomValue = UnityEngine.Random.Range(0, 8);
        }

        CurrentTime += Time.deltaTime;
    }



}
