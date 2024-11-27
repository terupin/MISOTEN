using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class Miburo_State : MonoBehaviour
{
    //�t���O
    private bool _Step;
    private bool _Parry;
    private bool _Attack01;
    private bool _Attack02;
    private bool _Run;
    private bool _Ukenagashi_R;
    private bool _Ukenagashi_L;

    //���̕���
    private int _Katana_Direction;

    [SerializeField, Header("�ړ��X�s�[�h")]
    public float Move_Speed;
    

    [SerializeField, Header("�G�v���n�u")]
    public GameObject Target;

    [SerializeField, Header("�v���C���[�v���n�u")]
    public GameObject Player;

    [SerializeField, Header("�J����")]
    public GameObject Camera_o;

    [SerializeField, Header("�݂Ԃ�A�j���[�^�[")]
    public Animator Miburo_Animator;

    [SerializeField, Header("�҂�����(�X�e�b�v)")]
    public float Step_WaitTime;

    [SerializeField, Header("�҂�����(�p���B)")]
    public float Parry_WaitTime;

    [SerializeField, Header("�҂�����(�U��1�i��)")]
    public float Attack01_WaitTime;

    [SerializeField, Header("�҂�����(�U��2�i��)")]
    public float Attack02_WaitTime;

    [SerializeField, Header("�҂�����(�󂯗��������Z�b�g)")]
    public float Katana_DirectionSet_WaitTime;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //R1�{�^������
        if (UnityEngine.Input.GetKeyDown("joystick button 5"))
        {
            if (_Attack01)
            {
                StartCoroutine(Miburo_Attack02());

            }
            else
            {
                StartCoroutine(Miburo_Attack01());
            }

        }
        //L1�{�^������
        if (UnityEngine.Input.GetKeyDown("joystick button 4"))
        {
            StartCoroutine(Miburo_Parry());
        }

        //A�{�^������
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            StartCoroutine(Miburo_Step());
        }

        if (_Parry)//�󂯗������͉\���Ɏ󂯗�������
        {
            if (_Katana_Direction == -1)
            {
                _Katana_Direction = GetKatana_Direction();
            }
            else
            {
                Debug.Log("���͊���\n���͕����@" + _Katana_Direction);
            }
        }
        else
        {
            _Katana_Direction = -1;
        }

        Player_Run_Input();//���s���͂�����Ă��邩�̃t���O

        if (_Attack01 ||_Attack02)
        {
            
        }
        else
        {
            if(_Run)
            {
                Player_Run();//���s����
            }
          
        }

        if(_Parry)
        {
            if(_Katana_Direction==0|| _Katana_Direction == 1 || _Katana_Direction == 2 || _Katana_Direction == 7 )
            {
                Miburo_Animator.SetTrigger("UkenagashiL");
            }
            else if(_Katana_Direction == 0 || _Katana_Direction == 1 || _Katana_Direction == 2 || _Katana_Direction == 7)
            {
                Miburo_Animator.SetTrigger("UkenagashiR");
            }
            
        }


        Miburo_Animator.SetBool("Gurd", _Parry);
        Miburo_Animator.SetBool("Run", _Run);
        Miburo_Animator.SetInteger("KatanaD", _Katana_Direction);

        //HP0�ȉ��Ȃ�Q�[���I�[�o�[
        if(Kato_Status_P.NowHP<=0)
        {
            Miburo_Animator.SetBool("GameOver", true);
        }


        //�e�X�g�p
        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
        {
            Miburo_Animator.SetTrigger("Damage");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.J))
        {
            Miburo_Animator.SetTrigger("UkenagashiL");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.K))
        {
            Miburo_Animator.SetTrigger("UkenagashiR");
        }
    }

    //�R���[�`��()
    private IEnumerator Miburo_Attack01()
    {
        if (!_Attack01)
        {
            _Attack01 = true;
            Debug.Log("�U��1�J�n");
            Miburo_Animator.SetBool("Attack01",true);
            yield return new WaitForSeconds(Attack01_WaitTime);
            Debug.Log("�U��1�҂����ԏI��");
            Miburo_Animator.SetBool("Attack01", false);
            _Attack01 = false;
        }
        else
        {
            Debug.Log("�҂����Ԃł��B���͔͂��f����܂���B");
        }
    }

    //�R���[�`��()
    private IEnumerator Miburo_Attack02()
    {

        if (!_Attack02)
        {
            _Attack02 = true;
            Debug.Log("�U��2�J�n");
            Miburo_Animator.SetBool("Attack02", true);
            yield return new WaitForSeconds(Attack02_WaitTime);
            Debug.Log("�U��2�҂����ԏI��");
            Miburo_Animator.SetBool("Attack02", false);
            _Attack02 = false;
        }
        else
        {
            Debug.Log("�҂����Ԃł��B���͔͂��f����܂���B");
        }
    }

    //�R���[�`��()
    private IEnumerator Miburo_Parry()
    {
        if (!_Parry)
        {
            _Parry = true;
            Debug.Log("�p���C�J�n");
            yield return new WaitForSeconds(Parry_WaitTime);
            Debug.Log("�p���C�҂����ԏI��");
            _Parry = false;
        }
        else
        {
            Debug.Log("�҂����Ԃł��B���͔͂��f����܂���B");
        }

    }

    //�R���[�`��()
    private IEnumerator Miburo_Step()
    {
        if (!_Step)
        {
            _Step = true;
            Debug.Log("�X�e�b�v�J�n");
            yield return new WaitForSeconds(Step_WaitTime);
            Debug.Log("�X�e�b�v�҂����ԏI��");
            _Step = false;
        }
        else
        {
            Debug.Log("�҂����Ԃł��B���͔͂��f����܂���B");
        }
    }

    // �w��A�j���[�V�������I�����Ă��邩�𔻒�
    private bool AnimationFinished(string animationName)
    {
        return !Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)
            || Miburo_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
    }

    //�R���g���[���[����a���̕������擾
    int GetKatana_Direction()
    {
        int Katana_Direction = -1;
        float h = UnityEngine.Input.GetAxis("Horizontal2");
        float v = UnityEngine.Input.GetAxis("Vertical2");

        float degree = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }

        if (Katana_Direction == -1)
        {

            if (MathF.Abs(v) <= 0.15f || MathF.Abs(h) <= 0.15f)
            {
                Katana_Direction = -1;
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

        return Katana_Direction;
    }

    //�ړ�
    public void Player_Run()
    {
        float moveX = Input.GetAxis("Vertical");
        float RotateY = Input.GetAxis("Horizontal");
        float degree = Mathf.Atan2(RotateY, moveX) * Mathf.Rad2Deg;//�R���g���[���[�p�x�擾

        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, Camera_o.transform.localEulerAngles.y + degree, 0));
        gameObject.transform.position += gameObject.transform.forward * Move_Speed * Time.deltaTime;
    }

    //�ړ�����
    public void Player_Run_Input()
    {
        float moveX = Input.GetAxis("Vertical");
        float RotateY = Input.GetAxis("Horizontal"); 

        if (MathF.Abs(moveX) >= 0.05f || MathF.Abs(RotateY) >= 0.05f)
        {
            if (Kato_Status_P.NowHP > 0)
            {
                _Run = true;
            }
        }
        else
        {
            _Run = false;
        }
    }
}
