using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Step : MonoBehaviour
{
    [SerializeField, Header("ステップ待ちタイム(1.0)")]
    public float StepWaitTime;//連撃タイム
    [SerializeField, Header("ステップ距離( 3.5)")]
    public float StepRength;//ステップ距離算出

    private float StepCurrentTime = 0.0f;
    private bool StepFlg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            if (!StepFlg)
            {
                gameObject.transform.position += gameObject.transform.forward * StepRength;
                StepFlg = true;
            }

        }

        if (StepFlg)
        {
            StepCurrentTime += Time.deltaTime;
            if (StepCurrentTime >= StepWaitTime)
            {
                StepCurrentTime = 0.0f;
                StepFlg = false;
                Debug.LogFormat("ステップ待ち終了");
            }

        }
    }
}
