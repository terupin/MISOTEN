using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Enemy_Anim : MonoBehaviour
{
    public Animator Enemy_Animator;

    public string Enemy_Anim_bool ;  // �I�������m�������A�j���[�V�����̖��O

    public string Enemy_Anim_name;  // �I�������m�������A�j���[�V�����̖��O

    public static int randomValue = -1; /* �����_���Ȓl���i�[���� */

    public GameObject PkatanaHitbox;
    public GameObject Ejoint;

    private int Katana_Direction = -1;

    private bool U_Ukenagasi_Flg = false;//�󂯗����t���O


    public float CullTime;
    private float CurrentTime=0.0f;

    private float AnimCurrentTime = 0.0f;

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


        //�v���C���[�̍���]��
        //�������火
        if (Kato_Hittest.Ukenagashi_Flg && AnimCurrentTime < 0.5f)
        {
            Enemy_Animator.SetFloat("E_AnimSpeed", 0.0f); // �ꎞ��~
            Enemy_Animator.enabled = false;
            Ejoint.transform.RotateAround(Ejoint.transform.position, Vector3.up, -180.0f * Time.deltaTime);
            AnimCurrentTime += Time.deltaTime;

        }
        else if (AnimCurrentTime  >0.5f)
        {
            //UnityEditor.EditorApplication.isPaused = true;
        }
        else 
        {

        }
        //�����܂�



        if (Enemy_Animator.enabled)
        {
            if (CurrentTime > CullTime)
            {
                Enemy_Animator.SetBool(Enemy_Anim_bool, false);

                randomValue = -1;
                CurrentTime = 0.0f;
            }
            else
            {
                Enemy_Animator.SetBool(Enemy_Anim_bool, true);
            }



            if (animatorStateInfo.IsName(Enemy_Anim_name))
            {
                if (animatorStateInfo.normalizedTime >= 0.9f && !animatorStateInfo.loop)
                {
                    Enemy_Animator.SetBool(Enemy_Anim_bool, false);
                    AnimCurrentTime = 0.0f;
                }
            }

            if (CurrentTime == 0.0f)
            {
                randomValue = UnityEngine.Random.Range(0, 8);
            }

            CurrentTime += Time.deltaTime;
        }
    }



}
