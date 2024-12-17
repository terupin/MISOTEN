using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Slash_Homing : MonoBehaviour
{
    //[SerializeField, Header("�z�[�~���O���񑬓x")]
    //public float RotateSpeed;
    [SerializeField, Header("�Ռ��g���x")]
    public float MoveSpeed; // �G�̈ړ����x

    //[SerializeField, Header("�z�[�~���O�J�n����")]
    //public float Homing_Start_Dis;
    //[SerializeField, Header("�z�[�~���O�I������")]
    //public float Homing_End_Dis;

    //[SerializeField, Header("true�Ȃ�ł��߂��I�u�W�F�N�g��false�Ȃ�ł������I�u�W�F�N�g")]
    //public bool SearchPriority;



    [SerializeField, Header("�Ռ��g����������Ă��鎞��(�b)")]
    public float MoveTime ;
    [SerializeField, Header("�Ռ��g�z�[�~���O�J�n����(�b)")]
    public float HomingStartTime;
    private float CurrentTime = 0.0f;

    private GameObject Target; // �v���C���[�I�u�W�F�N�g��Transform

    private bool HomingFlg;//�z�[�~���O�t���O

    //�f���`�N�Ɛؒf�\�^�O�̃Q�[���I�u�W�F�N�g�����ׂĎ擾����
    private GameObject[] _Denchiku ;
    private GameObject[] _Cut ;

    //�ЂƂ܂Ƃ߂ɂ���
    private GameObject[] _HomingList ;

    private float[] _HomingDistance ;//����

    private GameObject EnemyObj;
    private GameObject EnemyKatanaBox;

    private bool Seach_Flg;

    private bool Seach_END_Flg;

    // Start is called before the first frame update
    void Start()
    {
        EnemyObj = GameObject.FindWithTag("Enemy");
        EnemyKatanaBox = GameObject.Find("Enemy_HitBox");

        gameObject.transform.position = Vector3.zero;

        if ( Matsunaga_Enemy01_State.UKe__Ren01 || Matsunaga_Enemy01_State.UkeR)
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, EnemyObj.transform.localEulerAngles.y - 15, 0.0f);
        }
        else if (Matsunaga_Enemy01_State.UKe__Ren02 || Matsunaga_Enemy01_State.UkeL)
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, EnemyObj.transform.localEulerAngles.y + 15, 0.0f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //  �Ռ��g�̓z�[�~���O���ȊO�͒��i���܂��B
        gameObject.transform.position += gameObject.transform.forward * MoveSpeed * Time.deltaTime;


        if (CurrentTime >= HomingStartTime)
        {
            if (!HomingFlg)
            {
                StartCoroutine(Homing_Search());
                HomingFlg = true;
            }
                    
        }



        if (CurrentTime >= MoveTime )
        {
            Destroy(gameObject);
        }
        CurrentTime += Time.deltaTime;
    }

    //�z�[�~���O�p�x�␳
    private IEnumerator Homing_Search()
    {
        float roty= 0;

        if (gameObject.transform.localEulerAngles.y < 0) { gameObject.transform.rotation =  Quaternion.Euler(EnemyObj.transform.localEulerAngles.x, EnemyObj.transform.localEulerAngles.y+360, EnemyObj.transform.localEulerAngles.z); }


        if (30.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 0.0f) { roty = 15; }
        else if (60.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 30.0f) { roty = 45; }
        else if (90.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 60.0f) { roty = 75; }
        else if (120.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 90.0f) { roty = 105; }
        else if (150.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 120.0f) { roty = 135; }
        else if (180.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 150.0f) { roty = 165; }
        else if (210.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 180.0f) { roty = 195; }
        else if (240.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 210.0f) { roty = 225; }
        else if (270.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 240.0f) { roty = 255; }
        else if (300.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 270.0f) { roty = 285; }
        else if (330.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 300.0f) { roty = 315; }
        else if (360.0f > gameObject.transform.localEulerAngles.y && gameObject.transform.localEulerAngles.y > 330.0f) { roty = 345; }

        gameObject.transform.rotation = Quaternion.Euler(EnemyObj.transform.localEulerAngles.x, roty, EnemyObj.transform.localEulerAngles.z);



        yield break;
    }

}
