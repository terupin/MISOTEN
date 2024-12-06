using System;
using System.Collections;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Miburo_State : MonoBehaviour
{
    //�t���O
    private bool _Step;
    private bool _CoolDown;
    static public bool _Parry;
    static public bool _Parry_Timing;//�p���C���͂����u��
    static public bool _Attack01;
    static public bool _Attack02;

    static public bool _CounterL;
    static public bool _CounterR;
    static public bool _RenCounter01;
    static public bool _RenCounter02;
    private bool _Run;
    private bool _Ren11;
    private bool _Ren22;

    static public bool _wait;

    public bool StickL;
    public bool StickR;



    private bool _KnockBack;
    Vector3 dir;

    //Rigidbody rb;

    [SerializeField, Header("�m�b�N�o�b�N�X�s�[�h")]
    public float KnockBack_Speed;

    [SerializeField, Header("�m�b�N�o�b�N����(�b)")]
    public float KnockBack_Time;

    [SerializeField, Header("�X�e�b�v�X�s�[�h")]
    public float Step_Speed;

    [SerializeField, Header("�X�e�b�v����(�b)")]
    public float Step_Time;

    static public bool _Stick_Input;

    static public bool _Uke_Input;//�󂯗�������

    //���̕���
    static public int _Katana_Direction;

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

    [SerializeField, Header("�a���G�t�F�N�g(debug�e�X�g�p)")]
    public GameObject S_Effect;

    [SerializeField, Header("���f��(debug�e�X�g�p)")]
    public GameObject Test;

    [SerializeField, Header("�}�e���A��(debug�e�X�g�p)")]
    public Material TestMat;
    public Material M_UkenagasiIcon;
    public Material M_StepIcon;
    public Material M_AttackIcon;

    public Material Reset;

    [SerializeField, Header("�Q�[���I�[�o�[�V�[����")]
    public string SceneName;

    GameObject Miburo_Box;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        _Katana_Direction = -1;

        Test.AddComponent<MeshRenderer>();
        Miburo_Box = GameObject.Find("Player");

        //UI
        M_UkenagasiIcon.SetFloat("_CoolDown", 1.0f);
        M_StepIcon.SetFloat("_CoolDown", 1.0f);
        M_AttackIcon.SetFloat("_CoolDown", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //HP0�ȉ��Ȃ�Q�[���I�[�o�[
        if (Kato_Status_P.instance.NowHP <= 0)
        {
            Miburo_Animator.SetBool("GameOver", true);
            StartCoroutine(Gameover());
            return;
        }

        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("Battou"))
        {
            return;
        }

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
            _Parry_Timing = true;
            StartCoroutine(Miburo_Parry());
        }
        else
        {
            _Parry_Timing = false;
        }

        //A�{�^������
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            StartCoroutine(Miburo_Step());
        }





        if (UnityEngine.Input.GetKey("joystick button 0"))
        {
            StartCoroutine(Miburo_Step());

        }

        Miburo_Box.SetActive(!_Step);


        //�m�b�N�o�b�N
        if (_KnockBack)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.position -= dir * KnockBack_Speed * Time.deltaTime;
        }
        else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.position = Vector3.zero;
        }

        if (_Parry)
        {

        }

        GetKatana_Direction();

        if (_Katana_Direction == 0 || _Katana_Direction == 1 || _Katana_Direction == 2 || _Katana_Direction == 7)
        {
            StickR = true;
            StickL = false;
        }
        else if (_Katana_Direction == 3 || _Katana_Direction == 4 || _Katana_Direction == 5 || _Katana_Direction == 6)
        {
            StickL = true;
            StickR = false;
        }
        else
        {
            StickR = false;
            StickL = false;
        }

        Miburo_Animator.SetBool("StickR", StickR);
        Miburo_Animator.SetBool("StickL", StickL);

        if (Enemy01_State.UKe__Ren01 || Enemy01_State.UKe__Ren02 || Enemy01_State.UkeL || Enemy01_State.UkeR)
        {


        }
        else
        {
            if (_Parry)
            {

            }

        }
        if (Enemy01_State.P_Wait)
        {
            if (!_wait)
            {
                StartCoroutine(Miburo_Parry_Wait());
                Test.GetComponent<MeshRenderer>().material = TestMat;

                
                _wait = true;
            }
        }
        else
        {
            Test.GetComponent<MeshRenderer>().material = Reset;

            _wait = false;
        }


        //Player_Run_Input();//���s���͂�����Ă��邩�̃t���O

        //if (_Attack01 || _Attack02 || _Stick_Input || _Parry || _wait)
        //{
        //}
        //else
        //{
        //    if (_Run)
        //    {
        //        Player_Run();//���s����
        //    }
        //}


        ////������A�j���[�^�[��

        //Miburo_Animator.SetBool("Run", _Run);
        Miburo_Animator.SetBool("Gurd", _Parry);


        if (Enemy01_State.UkeL)
        {
            Miburo_Animator.SetBool("UkenagashiL", true);
            _CounterL = true;
        }
        else
        {
            Miburo_Animator.SetBool("UkenagashiL", false);
        }

        if (Enemy01_State.UkeR)
        {
            Miburo_Animator.SetBool("UkenagashiR", true);
            _CounterR = true;
        }
        else
        {
            Miburo_Animator.SetBool("UkenagashiR", false);
        }



        if (Enemy01_State.UKe__Ren01)
        {
            if (!_Ren11)
            {
                Miburo_Animator.SetTrigger("Ren11");
                _Ren11 = true;
                _RenCounter01 = true;
            }
        }
        else
        {
            _Ren11 = false;
        }

        if (Enemy01_State.UKe__Ren02)
        {
            if (!_Ren22)
            {
                Miburo_Animator.SetTrigger("Ren22");
                _Ren22 = true;
                _RenCounter02 = true;
            }

        }
        else
        {
            _Ren22 = false;
        }

        gameObject.transform.LookAt(Target.transform);


        //if (_Stick_Input)
        //{
        //     GetKatana_Direction();
        //} 


        GetCurrentAnimationStateName();//�X�e�[�g�擾����
    }

    //�R���[�`��(�U��1)
    private IEnumerator Miburo_Attack01()
    {
        if (!_Attack01)
        {
            _Attack01 = true;
            Debug.Log("�U��1�J�n");
            Miburo_Animator.SetBool("Attack01", true);
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

    //�R���[�`��(�U��2)
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

    //�R���[�`��(�󂯗����\��)
    private IEnumerator Miburo_Parry()
    {
        if (!_Parry)
        {
            _Parry = true;
            Debug.Log("�p���C�J�n");
            yield return new WaitForSeconds(Parry_WaitTime);

            M_UkenagasiIcon.SetFloat("_CoolDown", 0.0f);

            // �V�����ǉ��FM_UkenagasiIcon �̕ύX���J�n
            StartCoroutine(ChangeCoolDown(M_UkenagasiIcon,0.0f, 1.0f, 1.0f));

            Debug.Log("�p���C�҂����ԏI��");
            _Parry = false;

        }
        else
        {
            Debug.Log("�҂����Ԃł��B���͔͂��f����܂���B");
        }

    }

    //�R���[�`��(Stick)
    private IEnumerator Miburo_Stick()
    {
        if (!_Stick_Input)
        {
            _Stick_Input = true;
            Debug.Log("�X�e�B�b�N");
            //Miburo_Animator.SetBool("Gurd", _Stick_Input);
            yield return new WaitForSeconds(Parry_WaitTime);
            Debug.Log("�X�e�B�b�N�҂����ԏI��");
            Input_Check();
            _Stick_Input = false;
            //Miburo_Animator.SetBool("Gurd", _Stick_Input);
            //UnityEditor.EditorApplication.isPaused = true;

        }
        else
        {
            Debug.Log("�҂����Ԃł��B���͔͂��f����܂���B");
        }

    }

    //�R���[�`��(�\���E�F�C�g)
    private IEnumerator Miburo_Parry_Wait()
    {
        //Test.GetComponent<MeshRenderer>().material = TestMat;
        //UnityEditor.EditorApplication.isPaused = true;
        yield return new WaitForSeconds(0.4f);//24�t���[��(.4�b)
        //Test.GetComponent<MeshRenderer>().material = Reset;

        _Parry = false;
        _Ren11 = false;
        _Ren22 = false;
        _Parry = false;
    }

    private IEnumerator Counter_Timing_Input()
    {
        if (!_Uke_Input)
        {
            _Uke_Input = true;
            Debug.Log("�󂯊J�n");
            yield return new WaitForSeconds(1);
            Debug.Log("�󂯑҂����ԏI��");
            _Uke_Input = false;
        }
        else
        {
            Debug.Log("�҂����Ԃł��B���͔͂��f����܂���B");
        }
    }

    //�R���[�`��(�X�e�b�v)
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
    void GetKatana_Direction()
    {

        float h = UnityEngine.Input.GetAxis("Horizontal2");
        float v = UnityEngine.Input.GetAxis("Vertical2");

        float degree = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

        //UnityEditor.EditorApplication.isPaused = true;
        if (degree < 0)
        {
            degree += 360;
        }

        //if (Katana_Direction == -1)
        {

            if (MathF.Abs(v) <= 0.15f || MathF.Abs(h) <= 0.15f)
            {
                _Katana_Direction = -1;
            }
            else
            {
                if (degree < 22.5f) { _Katana_Direction = 0; }
                else if (degree < 67.5f) { _Katana_Direction = 1; }
                else if (degree < 112.5f) { _Katana_Direction = 2; }
                else if (degree < 157.5f) { _Katana_Direction = 3; }
                else if (degree < 202.5f) { _Katana_Direction = 4; }
                else if (degree < 247.5f) { _Katana_Direction = 5; }
                else if (degree < 292.5f) { _Katana_Direction = 6; }
                else if (degree < 337.5f) { _Katana_Direction = 7; }
                else { _Katana_Direction = 0; }
            }
        }
    }

    ////�ړ�
    //public void Player_Run()
    //{
    //    float moveX = Input.GetAxis("Vertical");
    //    float RotateY = Input.GetAxis("Horizontal");
    //    float degree = Mathf.Atan2(RotateY, moveX) * Mathf.Rad2Deg;//�R���g���[���[�p�x�擾

    //    Rigidbody rb = GetComponent<Rigidbody>();

    //    gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, Camera_o.transform.localEulerAngles.y + degree, 0));
    //    rb.position+=gameObject.transform.forward * Move_Speed * Time.deltaTime;
    //    //gameObject.transform.position += gameObject.transform.forward * Move_Speed * Time.deltaTime;
    //}

    ////�ړ�����
    //public void Player_Run_Input()
    //{
    //    float moveX = Input.GetAxis("Vertical");
    //    float RotateY = Input.GetAxis("Horizontal"); 

    //    if (MathF.Abs(moveX) >= 0.05f || MathF.Abs(RotateY) >= 0.05f)
    //    {
    //        if (Kato_Status_P.instance.NowHP > 0)
    //        {
    //            _Run = true;
    //        }
    //    }
    //    else
    //    {
    //        _Run = false;
    //    }
    //}

    //���͐����m�F
    public void Input_Check()
    {
        if (_Katana_Direction == -1)
        {
            Debug.Log("����@���s");
            if (!_wait)
            {
                //StartCoroutine(Miburo_Parry_Wait());
                _wait = true;
            }

            //UnityEditor.EditorApplication.isPaused = true;
        }
        else
        {
            Debug.Log("����@����");
            //UnityEditor.EditorApplication.isPaused = true;
            if (_Katana_Direction == 0 || _Katana_Direction == 1 || _Katana_Direction == 2 || _Katana_Direction == 7)
            {
                //�����Ɏ󂯗���
                Debug.Log("����@�E");
                //UnityEditor.EditorApplication.isPaused = true;
            }
            else if (_Katana_Direction == 3 || _Katana_Direction == 4 || _Katana_Direction == 5 || _Katana_Direction == 6)
            {
                //�����Ɏ󂯗���
                Debug.Log("����@��");
                //UnityEditor.EditorApplication.isPaused = true;
            }
        }
    }

    //�A�j���[�^�[����X�e�[�g�����擾
    void GetCurrentAnimationStateName()
    {
        //if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("UKE"))
        //{
        //    gameObject.transform.position += gameObject.transform.forward * Time.deltaTime*5.5f;
        //    gameObject.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        //}
        //if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("UKE2"))
        //{
        //    gameObject.transform.position += gameObject.transform.forward * Time.deltaTime * 2.5f;
        //    gameObject.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        //}
        //if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("UKE3"))
        //{
        //    gameObject.transform.position -= gameObject.transform.forward * Time.deltaTime * 0.1f;
        //    gameObject.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        //}

        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _RenCounter01 = false;
            _RenCounter02 = false;
            _CounterL = false;
            _CounterR = false;
        }

    }

    //�󂯗����������̈ʒu����
    void CounterPosSet()
    {
        gameObject.transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z);
    }

    //�Q�[���I�[�o�[
    private IEnumerator Gameover()
    {

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneName);

    }

    //�����蔻��
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EWeapon")
        {
            GameObject Miburo_Box = GameObject.Find("Player");
            if (Miburo_Box && Enemy01_State.Attack)
            {
                if (!Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("Battou"))
                {
                    Rigidbody rb = GetComponent<Rigidbody>();
                    dir = (Target.transform.position - rb.position).normalized;
                    Miburo_Animator.SetTrigger("Damage");
                    gameObject.AddComponent<Damage_Flash>();

                    StartCoroutine(KnockBack());
                }


            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision. == "EWeapon")
    }

    private IEnumerator KnockBack()
    {
        if (!_KnockBack)
        {


            _KnockBack = true;
            yield return new WaitForSeconds(KnockBack_Time);
            _KnockBack = false;
        }
    }

    // UI�ɔ��f�����邽�߂̃R���[�`��
    IEnumerator ChangeCoolDown(Material _material,float startValue, float endValue, float duration)
    {
        if(!_CoolDown)
        {
            _CoolDown = true;
            float time = 0.0f;

            // ���Ԍo�߂� M_UkenagasiIcon �� CoolDown �l�����X�ɕύX
            while (time < duration)
            {
                time += Time.deltaTime;
                float value = Mathf.Lerp(startValue, endValue, time / duration);
                _material.SetFloat("_CoolDown", value); // M_UkenagasiIcon �݂̂𑀍�
                yield return null;
            }

            // �ŏI�l���m��
            _material.SetFloat("_CoolDown", endValue);
            _CoolDown = false;
        }
        else
        {

        }

    }

}



