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

        gameObject.transform.position = new Vector3(EnemyKatanaBox.transform.localPosition.x, 0.0f, EnemyKatanaBox.transform.localPosition.z);

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

        //if (!Seach_Flg)
        //{
        //    StartCoroutine(Homing_Search());
        //    Seach_Flg = true;
        //}

        //if (Target != null)
        //{
        //    float HomingDistance = Vector3.Distance(Target.transform.position, gameObject.transform.position);




        //    if (HomingDistance <= Homing_Start_Dis && HomingDistance >= Homing_End_Dis)
        //    {
        //        // �Ώە��Ǝ������g�̍��W����x�N�g�����Z�o����Quaternion(��]�l)���擾
        //        Vector3 HomingVector = Target.transform.position - this.transform.position;
        //        // �����㉺�����̉�]�͂��Ȃ��悤�ɂ�������Έȉ��̂悤�ɂ���B
        //        HomingVector.y = 0f;

        //        // Quaternion(��]�l)���擾
        //        Quaternion quaternion = Quaternion.LookRotation(HomingVector);
        //        // �擾������]�l�����̃Q�[���I�u�W�F�N�g��rotation�ɑ��
        //        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(HomingVector), RotateSpeed * Time.deltaTime);

        //    }

        //    if (HomingDistance <= Homing_End_Dis && !Seach_END_Flg)
        //    {
        //        gameObject.transform.LookAt(new Vector3(Target.transform.position.x, gameObject.transform.position.y, Target.transform.position.z));
        //        Seach_END_Flg = true;
        //    }


        //}

        if(HomingFlg)
        {
          
        }


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

    //�z�[�~���O�Ώۂ��T�[�`
    private IEnumerator Homing_Search()
    {

        //�f���`�N�Ɛؒf�\�^�O�̃Q�[���I�u�W�F�N�g�����ׂĎ擾����
        _Denchiku = GameObject.FindGameObjectsWithTag("Denchiku");
        _Cut = GameObject.FindGameObjectsWithTag("Cut");

        //�ЂƂ܂Ƃ߂ɂ���
        _HomingList = new GameObject[_Denchiku.Length + _Cut.Length];

        _HomingDistance = new float[_HomingList.Length];

        Debug.Log(_HomingList.Length);

        int ListCount = 0;
        foreach (GameObject obj in _HomingList)
        {

            if (ListCount < _Denchiku.Length)
            {
                _HomingList[ListCount] = _Denchiku[ListCount];
            }
            else
            {
                _HomingList[ListCount] = _Cut[ListCount - _Denchiku.Length];
            }

            _HomingDistance[ListCount] = Vector3.Distance(_HomingList[ListCount].transform.position, gameObject.transform.position);
            Debug.LogFormat("�I�u�W�F�N�g���@{0}\n�����@{1}", _HomingList[ListCount].name, _HomingDistance[ListCount]);
            Debug.Log(_HomingList[ListCount].name);
            Debug.Log(_HomingDistance[ListCount]);
            ListCount++;
        }
        int DistanceCount = 0;

        //if (SearchPriority)
        {
            foreach (float dis in _HomingDistance)
            {
                if (dis == _HomingDistance.Min())
                {
                    Target = _HomingList[DistanceCount];
                }
                DistanceCount++;
            }

            //Debug.LogFormat("��ԋ߂��I�u�W�F�N�g���@{0}\n�����@{1}", Target.name, Vector3.Distance(Target.transform.position, gameObject.transform.position));

        }
        //else
        //{
        //    foreach (float dis in _HomingDistance)
        //    {
        //        if (dis == _HomingDistance.Max())
        //        {
        //            Target = _HomingList[DistanceCount];
        //        }
        //        DistanceCount++;
        //    }
        //    //Debug.LogFormat("��ԉ����I�u�W�F�N�g���@{0}\n�����@{1}", Target.name, Vector3.Distance(Target.transform.position, gameObject.transform.position));

        //}
        gameObject.transform.LookAt(new Vector3(Target.transform.position.x, gameObject.transform.position.y, Target.transform.position.z));

        yield break;
    }

}
