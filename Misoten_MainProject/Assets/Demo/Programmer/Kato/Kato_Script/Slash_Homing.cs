using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Slash_Homing : MonoBehaviour
{
    [SerializeField, Header("�z�[�~���O��]���x")]
    public float RotateSpeed;
    [SerializeField, Header("�Ռ��g���x")]
    public float MoveSpeed; // �G�̈ړ����x

    [SerializeField, Header("true�Ȃ�߂��I�u�W�F�N�g��false�Ȃ牓���I�u�W�F�N�g")]
    public bool SearchPriority;

    private GameObject Target; // �v���C���[�I�u�W�F�N�g��Transform

    //�f���`�N�Ɛؒf�\�^�O�̃Q�[���I�u�W�F�N�g�����ׂĎ擾����
    private GameObject[] _Denchiku ;
    private GameObject[] _Cut ;

    //�ЂƂ܂Ƃ߂ɂ���
    private GameObject[] _HomingList ;

    private float[] _HomingDistance ;


    // Start is called before the first frame update
    void Start()
    {

        ////�I�u�W�F�N�g�������ɒT��
        StartCoroutine(Homing_Search());
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            float HomingDistance= Vector3.Distance(Target.transform.position, gameObject.transform.position);

            // �Ώە��Ǝ������g�̍��W����x�N�g�����Z�o����Quaternion(��]�l)���擾
            Vector3 HomingVector = Target.transform.position - this.transform.position;
            // �����㉺�����̉�]�͂��Ȃ��悤�ɂ�������Έȉ��̂悤�ɂ���B
            HomingVector.y = 0f;

            // Quaternion(��]�l)���擾
            Quaternion quaternion = Quaternion.LookRotation(HomingVector);
            // �擾������]�l�����̃Q�[���I�u�W�F�N�g��rotation�ɑ��
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(HomingVector), RotateSpeed*Time.deltaTime);

            // �^�[�Q�b�g�Ɍ������Ĉړ�
            gameObject.transform.position+= gameObject.transform.forward * MoveSpeed * Time.deltaTime;
        }
    }

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
            Debug.LogFormat("�I�u�W�F�N�g���@{0}\n�����@{1}",_HomingList[ListCount].name, _HomingDistance[ListCount]);
            Debug.Log(_HomingList[ListCount].name);
            Debug.Log(_HomingDistance[ListCount]);
            ListCount++;
        }
        int DistanceCount = 0;

        if (SearchPriority)
        {
            foreach (float dis in _HomingDistance)
            {
                if (dis == _HomingDistance.Min())
                {
                    Target=  _HomingList[DistanceCount];
                }
                DistanceCount++;
            }

            Debug.LogFormat("��ԋ߂��I�u�W�F�N�g���@{0}\n�����@{1}", Target.name, Vector3.Distance(Target.transform.position, gameObject.transform.position));

        }
        else
        {
            foreach (float dis in _HomingDistance)
            {
                if (dis == _HomingDistance.Max())
                {
                    Target = _HomingList[DistanceCount];
                }
                DistanceCount++;
            }
            Debug.LogFormat("��ԉ����I�u�W�F�N�g���@{0}\n�����@{1}", Target.name, Vector3.Distance(Target.transform.position, gameObject.transform.position));

        }


        yield break;
    }

}
