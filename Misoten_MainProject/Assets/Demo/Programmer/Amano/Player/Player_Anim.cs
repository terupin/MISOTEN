using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Anim : MonoBehaviour
{
    public Animator Player_Animator;

    public string R_Anim_bool = "R_counter";  // �I�������m�������A�j���[�V�����̖��O
    public string L_Anim_bool = "L_counter";  // �I�������m�������A�j���[�V�����̖��O

    public string R_Anim_name;  // �I�������m�������A�j���[�V�����̖��O
    public string L_Anim_name;  // �I�������m�������A�j���[�V�����̖��O


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animatorStateInfo = Player_Animator.GetCurrentAnimatorStateInfo(0);



        if (Input.GetKeyDown(KeyCode.R))
        {
            Player_Animator.SetBool(R_Anim_bool, true);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Player_Animator.SetBool(L_Anim_bool, true);
        }

        if (animatorStateInfo.IsName(R_Anim_name) || animatorStateInfo.IsName(L_Anim_name))
        {
            if (animatorStateInfo.normalizedTime >= 0.9f && !animatorStateInfo.loop)
            {
                Player_Animator.SetBool(R_Anim_bool, false);
                Player_Animator.SetBool(L_Anim_bool, false);
            }
        }
    }
}
