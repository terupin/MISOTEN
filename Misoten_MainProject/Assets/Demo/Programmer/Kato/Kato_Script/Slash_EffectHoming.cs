using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash_EffectHoming : MonoBehaviour
{
    public Transform TgtTransform;
    public Rigidbody HomingRigidbody;

    public float Speed; // 追従速度
    public float MaxForce; // 最大の力
    public float Kp; // P項係数
    public float Ki; // I項係数
    public float Kd; // D項係数

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
        Vector3 diffDir = (tgtPos - transform.position).normalized; // ターゲットの方向
        Vector3 tgtSpeed = diffDir * Speed;
        Vector3 speedErr = tgtSpeed - HomingRigidbody.velocity;
        SpeedErrInteg += speedErr * dt;
        Vector3 prevSpeedErr = PresentSpeedErr;
        PresentSpeedErr = speedErr;
        Vector3 speedErrDiff = (PresentSpeedErr - prevSpeedErr) / dt;
        Vector3 force = Kp * speedErr + Ki * SpeedErrInteg + Kd * speedErrDiff; // PID制御
        float forceMagnitude = force.magnitude;


        if (forceMagnitude > MaxForce)
        {
            force = force / forceMagnitude * MaxForce; // 力を最大値にする
        }

        HomingRigidbody.AddForce(force, ForceMode.Force);
    }
}
