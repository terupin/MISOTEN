using System.Collections;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera targetCamera;
    private GameObject goto_player;
    private Rigidbody rb;
    public float speeeed;
    public float waitTime = 2.0f; // �ҋ@����
    private bool isMoving = false; // �ړ����t���O

    void Start()
    {
        targetCamera = Camera.main;
        goto_player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody>();

        // �R���[�`���őҋ@��Ɉړ����J�n
        StartCoroutine(WaitAndMove());
    }

    void Update()
    {
        Vector3 p = targetCamera.transform.position;
        p.y = transform.position.y;

        if (isMoving) // �ړ����̂ݎ��s
        {
            Goto_player(goto_player);
        }

        // LookAt�̑���ɐ��ʂ𒲐�
        Vector3 direction = (p - transform.position).normalized;
        transform.forward = -direction; // Quad�̌����ɍ��킹�邽�ߔ��]
    }

    void Goto_player(GameObject play)
    {
        Vector3 force = new Vector3();
        Vector3 Length = this.transform.position - play.transform.position;

        // �i�ޕ���
        if (Length.x > 0) { force.x = -speeeed; } else { force.x = speeeed; }
        if (Length.z > 0) { force.z = -speeeed; } else { force.z = speeeed; }

        this.transform.position += force * Time.deltaTime; // Time.deltaTime�Ńt���[���ˑ������
    }

    IEnumerator WaitAndMove()
    {
        // �w�莞�ԑҋ@
        yield return new WaitForSeconds(waitTime);

        // �ړ����J�n
        isMoving = true;
    }
}
