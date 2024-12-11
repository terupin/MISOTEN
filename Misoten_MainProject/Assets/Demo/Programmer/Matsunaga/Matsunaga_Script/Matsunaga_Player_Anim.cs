using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsunaga_Player_Anim : MonoBehaviour
{
    public Animator Player_Animator;

    public string R_Anim_bool = "R_counter";  // 終了を検知したいアニメーションの名前
    public string L_Anim_bool = "L_counter";  // 終了を検知したいアニメーションの名前
    public string Gard_Anim_bool = "Gard";  // 終了を検知したいアニメーションの名前

    public string R_Anim_name = "Ukenagashi_R";  // 終了を検知したいアニメーションの名前
    public string L_Anim_name ="Ukenagashi_L";  // 終了を検知したいアニメーションの名前
    public string Grad_Anim_name = "Ukenagashi";  // 終了を検知したいアニメーションの名前


    private bool PushFlg_L = false;//L押下フラグ
    private bool PushFlg_R = false;//R押下フラグ
    private int Katana_Direction = -1;

    public GameObject W_HitBox;//武器当たり判定


    public static bool G_Flg;//ガードフラグ
    public static bool A_Flg;//アタックフラグ

    // Start is called before the first frame update
    void Start()
    {
        //W_HitBox.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //W_HitBox.SetActive(false);

        //Lを押した時に押し込みフラグをTRUEにする
        if (UnityEngine.Input.GetKeyDown("joystick button 4") || Input.GetKeyDown(KeyCode.Return))
        {
            PushFlg_L = true;
        }
        //Lを離した時に押し込みフラグをfalseにする
        if (UnityEngine.Input.GetKeyUp("joystick button 4") || Input.GetKeyUp(KeyCode.Return))
        {
            PushFlg_L = false;
            Debug.Log("L離れた");
        }

        //Lを押した時に押し込みフラグをTRUEにする
        if (UnityEngine.Input.GetKeyDown("joystick button 5"))
        {
            PushFlg_R = true;
        }
        //Lを離した時に押し込みフラグをfalseにする
        if (UnityEngine.Input.GetKeyUp("joystick button 5"))
        {
            PushFlg_R = false;
            //Debug.Log("R離れた");
        }


        AnimatorStateInfo animatorStateInfo = Player_Animator.GetCurrentAnimatorStateInfo(0);

        if(PushFlg_L)
        {
            //W_HitBox.SetActive(true);
            Player_Animator.SetBool(Gard_Anim_bool, true);

            Kato_GetKatana_Direction();
            //if(Kato_Hittest.Ukenagashi_Flg)
            {
                if (Katana_Direction == 0 || Katana_Direction == 1 || Katana_Direction == 2 || Katana_Direction == 7)
                {
                    Player_Animator.SetBool(R_Anim_bool, true);
                }
                else if (Katana_Direction == 3 || Katana_Direction == 4 || Katana_Direction == 5 || Katana_Direction == 6)
                {
                    Player_Animator.SetBool(L_Anim_bool, true);
                }
            }          

        }
        else
        {
            Player_Animator.SetBool(Gard_Anim_bool, false);
        }


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

                PushFlg_L = false;
                //W_HitBox.SetActive(false);
                Katana_Direction = -1;
            }
        }

        G_Flg = PushFlg_L;
        A_Flg = PushFlg_R;
    }

    //コントローラーから斬撃の方向を取得
    void Kato_GetKatana_Direction()
    {

        if (PushFlg_L)
        {
            var h = UnityEngine.Input.GetAxis("Horizontal2");
            var v = UnityEngine.Input.GetAxis("Vertical2");

            float degree = Mathf.Atan2(v, h) * Mathf.Rad2Deg;



            if (degree < 0)
            {
                degree += 360;
            }

            if (v == 0 && h == 0)
            {
                Katana_Direction = -1;
            }
            else
            {
                if (Katana_Direction == -1)
                {
                    if (v == 0 && h == 0)
                    {
                        Katana_Direction = -1;
                        PushFlg_L = false;
                    }
                    else
                    {
                        if (degree < 22.5f) { Katana_Direction = 0; }
                        else if (degree < 67.5f) { Katana_Direction = 1; }
                        else if (degree < 112.5f) { Katana_Direction = 2; }
                        else if (degree < 157.5f) { Katana_Direction = 3; }
                        else if (degree < 202.5f) { Katana_Direction = 4; }
                        else if (degree < 247.5f) { Katana_Direction = 5; }
                        else if (degree < 292.5f) { Katana_Direction = 6; }
                        else if (degree < 337.5f) { Katana_Direction = 7; }
                        else { Katana_Direction = 0; }
                    }
                }
            }

        }
        else
        {
            Katana_Direction = -1;
        }
    }


}
