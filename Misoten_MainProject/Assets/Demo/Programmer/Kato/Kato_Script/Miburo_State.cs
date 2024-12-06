using System;
using System.Collections;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Miburo_State : MonoBehaviour
{
    //�t���O
    private bool _Step;
    private bool _StepMuteki;
    private bool _CoolDown;
    static public bool _Parry;
    static public bool _Parry_Timing;//�p���C���͂����u��
    static public bool _Attack01;
    static public bool _Attack02;

    static public bool _CounterL;
    static public bool _CounterR;
    static public bool _RenCounter01;
    static public bool _RenCounter02;
    private bool _Ren11;
    private bool _Ren22;

    static public bool _wait;

    private bool StickL;//�X�e�B�b�N���X�e�B�b�N
    private bool StickR;//�X�e�B�b�N�E�X�e�B�b�N

    private bool _KnockBack;//�m�b�N�o�b�N
    private bool _Muteki;//���G
    Vector3 dir;//�m�b�N�o�b�N�Ŏg�p����


    [SerializeField, Header("�m�b�N�o�b�N�X�s�[�h")]
    public float KnockBack_Speed;

    [SerializeField, Header("�m�b�N�o�b�N����(�b)")]
    public float KnockBack_Time;

    [SerializeField, Header("�X�e�b�v�X�s�[�h")]
    public float Step_Speed;
    [SerializeField, Header("�X�e�b�v����(�b)")]
    public float Step_Time;
    [SerializeField, Header("�҂�����(�X�e�b�v�I����)")]
    public float Step_WaitTime;

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

    [SerializeField, Header("�҂�����(�_���[�W�㖳�G)")]
    public float Damage_MutekiTime;

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

    [SerializeField, Header("�����蔻��{�b�N�X")]
     public GameObject Miburo_HitBox;
    private GameObject M_HitBox;

    [SerializeField, Header("�U���G�t�F�N�g")]
    public ParticleSystem slash_effect;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        _Katana_Direction = -1;

        Test.AddComponent<MeshRenderer>();

        //UI
        M_UkenagasiIcon.SetFloat("_CoolDown", 1.0f);
        M_StepIcon.SetFloat("_CoolDown", 1.0f);
        M_AttackIcon.SetFloat("_CoolDown", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {


        M_HitBox= GameObject.Find("Player");
        if ((M_HitBox))
        {
            //UnityEditor.EditorApplication.isPaused = true;
        }
        else
        {
            //UnityEditor.EditorApplication.isPaused = true;
        }

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


        //if(Kato_Status_P.instance.NowHP==3)
        //{
        //    UnityEditor.EditorApplication.isPaused = true;
        //}

        if(_KnockBack ||_StepMuteki)
        {

        }
        else
        {
            gameObject.transform.LookAt(Target.transform);
        }

       

        //R1�{�^������(�U��)
        if (UnityEngine.Input.GetKeyDown("joystick button 5"))
        {
            if (_Attack01)
            {
                StartCoroutine(Miburo_Attack02());
                Instantiate(slash_effect, transform.position, transform.rotation);
            }
            else
            {
                StartCoroutine(Miburo_Attack01());
                Instantiate(slash_effect, transform.position, transform.rotation);
            }
        }

        //L1�{�^������//�K�[�h
        if (UnityEngine.Input.GetKeyDown("joystick button 4"))
        {
            _Parry_Timing = true;
            StartCoroutine(Miburo_Parry());
        }
        else
        {
            _Parry_Timing = false;
        }

        //A�{�^������(�X�e�b�v)
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            StartCoroutine(Miburo_Step());
        }

        //�m�b�N�o�b�N
        if (_KnockBack)
        {
            Miburo_HitBox.SetActive(false);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.position -= dir * KnockBack_Speed * Time.deltaTime;
        }
        else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.position = Vector3.zero;
            Miburo_HitBox.SetActive(true);
        }

        if(_StepMuteki)
        {
            Miburo_HitBox.SetActive(false);
        }
        else
        {
                Miburo_HitBox.SetActive(true);                     
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

        ////������A�j���[�^�[��
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



        if (IsAnimationFinished("Attack02"))
        {

            StartCoroutine(ChangeCoolDown(M_AttackIcon, 0.0f, 1.0f, Attack01_WaitTime));
        }
       

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
            _StepMuteki = true;
            Miburo_HitBox.SetActive(false);
            Debug.Log("�X�e�b�v�J�n");
            StartCoroutine(ChangeCoolDown(M_StepIcon, 0.0f, 1.0f, Step_WaitTime+Step_Time));
            yield return new WaitForSeconds(Step_Time);
            Debug.Log("�X�e�b�v�҂����ԏI��");
            _StepMuteki = false;
            Miburo_HitBox.SetActive(true);
            yield return new WaitForSeconds(Step_WaitTime);
            _Step = false;
        }
        else
        {
            Debug.Log("�҂����Ԃł��B���͔͂��f����܂���B");
        }
    }

    // �w��A�j���[�V�������I�����Ă��邩�𔻒�
    private bool IsAnimationFinished(string animationName)
    {
        return Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) && Miburo_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f;
    }


    //�R���g���[���[����a���̕������擾
    void GetKatana_Direction()
    {

        float h = UnityEngine.Input.GetAxis("Horizontal2");
        float v = UnityEngine.Input.GetAxis("Vertical2");

        float degree = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }

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


    //�A�j���[�^�[����X�e�[�g�����擾
    void GetCurrentAnimationStateName()
    {
        if (Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _RenCounter01 = false;
            _RenCounter02 = false;
            _CounterL = false;
            _CounterR = false;
        }

    }


    //�Q�[���I�[�o�[
    private IEnumerator Gameover()
    {

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneName);

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
    IEnumerator aChangeCoolDown(Material _material, float startValue, float endValue, float duration)
    {
        if (!_CoolDown)
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

    //�����蔻��
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EWeapon")
        {
            //Miburo_HitBox =GameObject.Find("Player");
            if (M_HitBox && Matsunaga_Enemy01_State.Attack)
            {

                if (!Miburo_Animator.GetCurrentAnimatorStateInfo(0).IsName("Battou"))
                {
                    Rigidbody rb = GetComponent<Rigidbody>();
                    dir = (Target.transform.position - rb.position).normalized;
                    Miburo_Animator.SetTrigger("Damage");
                    Kato_Status_P.instance.Damage(1);
                    StartCoroutine(KnockBack());
                }
            }
        }
    }

    private IEnumerator Damage_Muteki()
    {
        //Miburo_HitBox.SetActive(false);
        yield return new WaitForSeconds(Damage_MutekiTime);
        //Miburo_HitBox.SetActive(true);

    }

    private IEnumerator Muteki()
    {
        if (!_Muteki)
        {


            _Muteki = true;
            yield return new WaitForSeconds(Damage_MutekiTime);
            _Muteki = false;
        }
    }
}



