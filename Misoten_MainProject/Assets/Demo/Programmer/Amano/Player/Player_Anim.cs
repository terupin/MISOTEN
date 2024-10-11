using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Anim : MonoBehaviour
{
    public Animator Player_Animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Player_Animator.SetTrigger("counter");
            Left_or_Right();
        }



    }

    private void Left_or_Right()
    {
     
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Player_Animator.SetTrigger("R_counter");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Player_Animator.SetTrigger("L_counter");
            }
        }
    }

}
