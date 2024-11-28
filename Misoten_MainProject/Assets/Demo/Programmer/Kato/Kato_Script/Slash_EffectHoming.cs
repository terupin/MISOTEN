using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash_EffectHoming : MonoBehaviour
{
    public Transform TgtTransform;
    public Rigidbody HomingRigidbody;

    public float Speed; // �Ǐ]���x
    public float MaxForce; // �ő�̗�
    public float Kp; // P���W��
    public float Ki; // I���W��
    public float Kd; // D���W��

    Vector3 SpeedErrInteg;
    Vector3 PresentSpeedErr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.fixedDeltaTime;
        Vector3 tgtPos = TgtTransform.position;
        Vector3 diffDir = (tgtPos - transform.position).normalized; // �^�[�Q�b�g�̕���
        Vector3 tgtSpeed = diffDir * Speed;
        Vector3 speedErr = tgtSpeed - HomingRigidbody.velocity;
        SpeedErrInteg += speedErr * dt;
        Vector3 prevSpeedErr = PresentSpeedErr;
        PresentSpeedErr = speedErr;
        Vector3 speedErrDiff = (PresentSpeedErr - prevSpeedErr) / dt;
        Vector3 force = Kp * speedErr + Ki * SpeedErrInteg + Kd * speedErrDiff; // PID����
        float forceMagnitude = force.magnitude;


        if (forceMagnitude > MaxForce)
        {
            force = force / forceMagnitude * MaxForce; // �͂��ő�l�ɂ���
        }

        HomingRigidbody.AddForce(force, ForceMode.Force);
    }
}
