using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Kato_a_Player_Anim_camcheck : MonoBehaviour
{

    public string R_Anim_bool = "R_counter";  // �I�������m�������A�j���[�V�����̖��O
    public string L_Anim_bool = "L_counter";  // �I�������m�������A�j���[�V�����̖��O
    public string Gard_Anim_bool = "Gard";  // �I�������m�������A�j���[�V�����̖��O

    public string R_Anim_name = "ukeR04_mcp";  // �I�������m�������A�j���[�V�����̖��O
    public string L_Anim_name = "uke06_mcp";  // �I�������m�������A�j���[�V�����̖��O
    public string Grad_Anim_name = "uketome07_mcp";  // �I�������m�������A�j���[�V�����̖��O

    public string RUN_bool = "Run";  //

    public Animator Player_Animator;

    private bool PushFlg_L = false;//L�����t���O
    private bool PushFlg_R = false;//R�����t���O
    static public int Katana_Direction = -1;

    [SerializeField, Header("�A���^�C��(0.5)")]
    public float RengekiTime;//�A���^�C��
    [SerializeField, Header("�A���ő�J�E���g(2)")]
    public int RengekiMaxCount;
    private int RengekiCount;//�A���J�E���g
    private bool RengekiFlg;//�A���t���O
    private float RengekiCurrentTime = 0.0f;//�A���J�����g�^�C��

    [SerializeField, Header("�󂯗����^�C��(0.5)")]
    public float Uke_Time;//�󂯃^�C��
    private bool Uke_Input_Flg;//�󂯓��̓t���O
    private float Uke_CurrentTime = 0.0f;//�󂯃J�����g�^�C��

    [SerializeField, Header("�J�E���^�[�^�C��(0.5)")]
    public float Counter_Time;//�J�E���^�[�^�C��
    private bool Counter_Input_Flg;//�J�E���^�[���̓t���O
    private float Counter_CurrentTime = 0.0f;//�J�E���^�[�J�����g�^�C��

    private bool Counter_Flg;//�J�E���^�[�����t���O

    public static bool G_Flg;//�K�[�h�t���O
    public static bool A_Flg;//�A�^�b�N�t���O

    public MainCamera mainCamerascript;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //L�����������ɉ������݃t���O��TRUE�ɂ���
        if (UnityEngine.Input.GetKeyDown("joystick button 4"))
        {
            PushFlg_L = true;
            Uke_Input_Flg = true;
        }
        //L�𗣂������ɉ������݃t���O��false�ɂ���
        if (UnityEngine.Input.GetKeyUp("joystick button 4"))
        {
            PushFlg_L = false;
            Uke_Input_Flg = false;
            Uke_CurrentTime = 0;
        }

        //R�����������ɉ������݃t���O��TRUE�ɂ���
        if (UnityEngine.Input.GetKeyDown("joystick button 5"))
        {
            Player_Animator.SetTrigger("Attack");
            RengekiFlg = true;
            PushFlg_R = true;
        }
        //R�𗣂������ɉ������݃t���O��false�ɂ���
        if (UnityEngine.Input.GetKeyUp("joystick button 5"))
        {
            PushFlg_R = false;
        }
        


        Player_Animator.SetBool(RUN_bool, Player_Move.RUN_FLG);


        AnimatorStateInfo animatorStateInfo = Player_Animator.GetCurrentAnimatorStateInfo(0);

        if (Uke_Input_Flg)
        {
            Player_Animator.SetBool(Gard_Anim_bool, true);

            Kato_a_GetKatana_Direction();
            if (Kato_HitBoxP_camcheck.Tubazeri_Flg)
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
            Player_Animator.SetBool(R_Anim_bool, false);
            Player_Animator.SetBool(L_Anim_bool, false);
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
                Katana_Direction = -1;
            }
        }

        //G_Flg = PushFlg_L;
        A_Flg = RengekiFlg;

        //�A��
        if (RengekiFlg && !Uke_Input_Flg && !Counter_Input_Flg)
        {
            RengekiCurrentTime += Time.deltaTime;

            if (UnityEngine.Input.GetKeyDown("joystick button 5"))
            {
                RengekiCount++;
                Debug.Log(RengekiCount);
                RengekiCurrentTime = 0;
            }

            if (RengekiCurrentTime >= RengekiTime)
            {
                RengekiCurrentTime = 0;
                RengekiCount = 0;
                RengekiFlg = false;
                Debug.Log("�A���^�C���I��");
               // UnityEditor.EditorApplication.isPaused = true;
            }

            if (RengekiCount >= RengekiMaxCount)
            {

                RengekiCount = 0;
                RengekiCurrentTime = 0;
                RengekiFlg = false;
                Debug.Log("�A������ɒB���܂����B");
                //UnityEditor.EditorApplication.isPaused = true;
            }
        }
        if (Uke_CurrentTime == 0.0f)
        {
            //Katana_Direction = -1;
        }

        //�󂯗���
        if (Uke_Input_Flg && !Counter_Input_Flg)
        {
            
            Uke_CurrentTime += Time.deltaTime;

            if (Katana_Direction != -1)
            {
                Debug.Log("�������͊���");
                G_Flg = true;
                Debug.Log("�J�E���^�[���̓^�C���J�n");
                //UnityEditor.EditorApplication.isPaused = true;
                Counter_Input_Flg = true;
                Uke_CurrentTime = 0;
                Uke_Input_Flg = false;
                //�J�����؂�ւ����������(�֐�����ē����)
            }

            if (Uke_CurrentTime >= Uke_Time)
            {
                Uke_CurrentTime = 0;
                Uke_Input_Flg = false;
                Debug.Log("�󂯗����^�C���I��");
                G_Flg = false;
                //UnityEditor.EditorApplication.isPaused = true;
            }
        }

        Player_Animator.SetBool("Gurd", Kato_HitBoxE.Ukenagashi_Flg);

        Player_Animator.SetInteger("KatanaD", Katana_Direction);


        //�J�E���^�[
        if (Counter_Input_Flg)
        {
            //Katana_Direction = -1;
            Counter_CurrentTime += Time.deltaTime;

            if (UnityEngine.Input.GetKeyDown("joystick button 5") && Kato_HitBoxP_camcheck.Tubazeri_Flg)
            {
                G_Flg = false;
                Counter_Input_Flg = false;
                Counter_Flg = true;
                Debug.Log("�J�E���^�[���͐���");
                //UnityEditor.EditorApplication.isPaused = true;
            }


            if (Counter_CurrentTime >= Counter_Time)
            {
                G_Flg = false;
                Counter_Input_Flg = false;
                Uke_CurrentTime = 0;
                Debug.Log("�J�E���^�[���̓^�C���I��");
                //UnityEditor.EditorApplication.isPaused = true;
            }
        }


       
    }

    //�R���g���[���[����a���̕������擾
    void Kato_a_GetKatana_Direction()
    {

        if (Uke_Input_Flg)
        {
            float h = UnityEngine.Input.GetAxis("Horizontal2");
            float v = UnityEngine.Input.GetAxis("Vertical2");

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

                    if (MathF.Abs(v) <= 0.1f || MathF.Abs(h) <= 0.1f)
                    {
                        Katana_Direction = -1;
                        //PushFlg_L = false;
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
