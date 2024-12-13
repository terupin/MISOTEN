using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwmove : MonoBehaviour
{
    private Rigidbody rb;
    private bool isMoving = false; // �ړ����t���O

    [SerializeField, Header("�ҋ@����")]
    public float waitTime = 2.0f;

    [SerializeField, Header("��������")]
    public float itemspeed = 5;

    [SerializeField, Header("��΂�����")]
    public float Tall;

    private Vector3 StartPoint;
    private Vector3 EndPoint;
    private float CenterPoint;

    private float movestart;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(WaitAndMove());   // �R���[�`���őҋ@��Ɉړ����J�n
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            rb.velocity = Vector3.zero;

            float elapsedTime = Time.time - movestart;
            float progress = elapsedTime * itemspeed;

            if (progress > 1.0f) // �I������
            {
                isMoving = false;
                return;
            }

            // XZ���W�̒����ړ�
            Vector3 currentXZ = Vector3.Lerp(StartPoint, EndPoint, progress);

            float height = Mathf.Sin(progress * Mathf.PI) * Tall;  //���������߂�

            if (height < EndPoint.y && progress > 0.5f)
                height = EndPoint.y;

            //�V�����ʒu��ݒ�
            transform.position = new Vector3(currentXZ.x,height,currentXZ.z);

        }

    }

    IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(waitTime);  //�w�莞�ԑҋ@

        isMoving = true;

        StartPoint = transform.parent.position;
        EndPoint = Vector3.zero;

        StartPoint.y = transform.position.y * 2;
        EndPoint.y = transform.position.y * 2;

        Debug.Log(StartPoint);
        Debug.Log(EndPoint);

        movestart = Time.time;
    }

}
