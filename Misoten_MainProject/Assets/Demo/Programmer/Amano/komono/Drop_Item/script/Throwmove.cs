using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwmove : MonoBehaviour
{
    private Rigidbody rb;
    private bool isMoving = false; // �ړ����t���O

    [SerializeField, Header("�ҋ@����")]
    public float waitTime = 2.0f;

    [SerializeField, Header("��΂�����")]
    public float Tall;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndMove());   // �R���[�`���őҋ@��Ɉړ����J�n

        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(waitTime);  //�w�莞�ԑҋ@

        Decide_Parabola();

        isMoving = true;
    }

    //�x�W�F�Ȑ���p���ĕ�������`��
    public void Decide_Parabola()
    {
        //���_�����߂�
        Vector3 StartPoint =transform.position;
        Vector3 EndPoint =Vector3.zero;
        Vector3 CenterPoint = StartPoint - EndPoint;
    }

    //�x�W�F�Ȑ��̕��
    Vector3 CalcLerpPoint(Vector3 start, Vector3 center,Vector3 end)
    {
        Vector3 a = Vector3.Lerp(start, center, Time.deltaTime);
        Vector3 b = Vector3.Lerp(center,end,Time.deltaTime);
        return Vector3.Lerp(a, b, Time.deltaTime);
    }
}
