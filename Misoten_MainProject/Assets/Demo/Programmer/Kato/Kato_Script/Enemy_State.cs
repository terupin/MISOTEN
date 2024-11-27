using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy_State : MonoBehaviour
{
     public enum Enemy_State_
    {
        Idle,
        Walk,
        RtoL,
        Hirumi,
        Tategiri,
        Ukenagasare,
        CoolDown,
    };

    private Enemy_State_ E_State;

    static public bool E_AttackFlg;

    [SerializeField, Header("ターゲットとなるプレイヤー(プレイヤーみぶろ)")]
    public GameObject Target_P;

    [SerializeField, Header("サーチ射程(10)")]
    public float SearchLength;

    [SerializeField, Header("攻撃射程(4.5)")]
    public float    AttackLength;

    [SerializeField, Header("移動スピード(12)")]
    public float MoveSpeed;

    private float P_E_Length;//プレイヤーと敵の距離

    [SerializeField, Header("アニメーター")]
    public Animator E01Anim;

    private float StateTime=2.5f;
    private float StateCurrentTime;

    // Start is called before the first frame update
    void Start()
    {
        E_State = Enemy_State_.Idle;
        StateCurrentTime = 0.0f;
        E01Anim.SetBool("Idle",true);
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.J))
        {
            E01Anim.SetTrigger("UkeL");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.K))
        {
            E01Anim.SetTrigger("UkeR");
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.N))
        {
            E01Anim.SetTrigger("UkeR");
        }

        //
        if (UnityEngine.Input.GetKeyDown(KeyCode.M))
        {
            E01Anim.SetTrigger("UkeR");
        }

        P_E_Length = Vector3.Distance(Target_P.transform.position, gameObject.transform.position);
       // Debug.Log(P_E_Length);

        //StateCurrentTime += Time.deltaTime;

        //if(StateCurrentTime>=StateTime)
        //{
        //    StateCurrentTime = 0.0f;

        //    if (E_State == Enemy_State_.Idle)
        //    {
        //        if(P_E_Length <AttackLength)
        //        {
        //            E_State = Enemy_State_.Tategiri;
        //        }
        //        else if (P_E_Length < SearchLength)
        //        {
        //            E_State = Enemy_State_.Walk;
        //        }
        //    }
        //    else if(E_State == Enemy_State_.Walk)
        //    {
        //        if (P_E_Length < AttackLength)
        //        {
        //            E_State = Enemy_State_.Tategiri;
        //        }
        //        else
        //        {
        //            E_State = Enemy_State_.Idle;
        //        }
        //    }
        //    else
        //    {
        //        E_State = Enemy_State_.Idle;
        //    }


        //}

        //if (E_State == Enemy_State_.Walk)
        //{
        //    if(P_E_Length > AttackLength && P_E_Length<SearchLength)
        //    {
        //        gameObject.transform.LookAt(Target_P.transform.position);
        //        transform.position = transform.position + transform.forward * MoveSpeed * Time.deltaTime;
        //    }
        //    else if(P_E_Length < AttackLength)
        //    {
        //        StateCurrentTime = 0.0f;
        //        E_State = Enemy_State_.Tategiri;
        //    }
        //    else if (P_E_Length > SearchLength)
        //    {
        //        StateCurrentTime = 0.0f;
        //        E_State = Enemy_State_.Idle;
        //    }

        //}

        //if (E_State == Enemy_State_.Tategiri)
        //{
        //    if(Kato_HitBoxE.Ukenagashi_Flg )
        //    {
        //        StateCurrentTime = 0.0f;
        //        E_State = Enemy_State_.Ukenagasare;


        //    }
        //}

        //if (E_State == Enemy_State_.Ukenagasare)
        //{
        //    if (Kato_a_Player_Anim.Katana_Direction == 0 || Kato_a_Player_Anim.Katana_Direction == 1 || Kato_a_Player_Anim.Katana_Direction == 2 || Kato_a_Player_Anim.Katana_Direction == 7)
        //    {
        //        E01Anim.SetTrigger("UkeR");
        //    }
        //    else if (Kato_a_Player_Anim.Katana_Direction == 3 || Kato_a_Player_Anim.Katana_Direction == 4 || Kato_a_Player_Anim.Katana_Direction == 5 || Kato_a_Player_Anim.Katana_Direction == 6)
        //    {
        //        E01Anim.SetTrigger("UkeL");
        //    }
        //}


            E01Anim.SetBool("Walk", E_State == Enemy_State_.Walk);
        E01Anim.SetBool("Idle", E_State == Enemy_State_.Idle);
        E01Anim.SetBool("Tategiri", E_State == Enemy_State_.Tategiri);

        if(E_State == Enemy_State_.Tategiri|| E_State == Enemy_State_.Tategiri)
        {
            E_AttackFlg = true;
        }
        else
        {
            E_AttackFlg = false;
        }

    }
}
