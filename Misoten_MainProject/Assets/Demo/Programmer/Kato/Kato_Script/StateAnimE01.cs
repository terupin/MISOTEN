using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class StateAnimE01 : MonoBehaviour
{
    [SerializeField, Header("ÉeÉXÉgÇ…égÇ§ÉIÉuÉWÉFÉNÉg")]
    public GameObject Testobj;//ÉeÉXÉgÇ…égÇ§ÉIÉuÉWÉFÉNÉg

    //ècêÿÇË ç≈ëÂì¸óÕóPó\ 1.7ïb
    //òAåÇ1 ç≈ëÂì¸óÕóPó\ 1.2ïb
    //òAåÇ2 ç≈ëÂì¸óÕóPó\ 0.5ïb
    [SerializeField, Header("ècêÿÇË ç≈ëÂì¸óÕóPó\ 1.7ïb")]
    public float Check_Time0;
    [SerializeField, Header("òAåÇ1 ç≈ëÂì¸óÕóPó\ 1.2ïb")]
    public float Check_Time1;
    [SerializeField, Header("òAåÇ2 ç≈ëÂì¸óÕóPó\ 0.75ïb")]
    public float Check_Time2;

    private float Check_Current_Time0;//ì¸óÕäJénÇ©ÇÁåoâﬂÇµÇΩéûä‘
    private float Check_Current_Time1;//ì¸óÕäJénÇ©ÇÁåoâﬂÇµÇΩéûä‘
    private float Check_Current_Time2;//ì¸óÕäJénÇ©ÇÁåoâﬂÇµÇΩéûä‘

    [SerializeField, Header("éaåÇÉGÉtÉFÉNÉg")]
    public GameObject S_Effect;
    public bool Effectflg;//ñ≥ë Ç»ÉGÉtÉFÉNÉgÇ™èoÇ»Ç¢ÇÊÇ§Ç…Ç∑ÇÈ

    private GameObject Clone_Effect;

    static public bool UkeL;
    static public bool UkeR;

    static public bool UKe__Ren01;
    static public bool UKe__Ren02;
    static public bool Attack;//çUåÇÅ@ìñÇΩÇËîªíËÇ…égÇ§

    private bool P_Input;//ÉpÉäÉCì¸óÕÇ≥ÇÍÇΩÇ©Ç«Ç§Ç©

    [SerializeField, Header("ÉAÉjÉÅÅ[É^Å[")]
    public Animator _AnimatorE01;

    private Matsunaga_Enemy01_State.Mai_State_ getState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (getState==Matsunaga_Enemy01_State.Mai_State_.Idle)
        {
            //_Animator.SetBool(,);
        }
        //else if (getState == Matsunaga_Enemy01_State.Mai_State_.Spin)
        {
            //_Animator.SetBool(,);
            //_Animator.SetBool(,);
        }
        //else if (getState == Matsunaga_Enemy01_State.Mai_State_.Goto)
        {
            //_Animator.SetBool(,);
        }
        //else if (getState == Matsunaga_Enemy01_State.Mai_State_.Attack)
        {
            //_Animator.SetBool(,);
        }
        //else if (getState == Matsunaga_Enemy01_State.Mai_State_.Jumpback)
        {
            //_Animator.SetBool(,);
        }
        //else if (getState == Matsunaga_Enemy01_State.Mai_State_.Kaihou)
        {
            //_Animator.SetBool(,);
        }


        //_Animator.SetBool(,);
        //_Animator.SetTrigger();
    }

    private void KatoUpdateAnim()
    {

            //ècêÿÇËêUÇËè„Ç∞
            if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            if (P_Input)
            {
                if (Check_Current_Time0 > 0.0f && Check_Time0 >= Check_Current_Time0)
                {
                    if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        UkeL = true;
                        _AnimatorE01.SetBool("UkeL", true);
                        UkeR = false;
                        _AnimatorE01.SetBool("UkeR", false);
                    }
                    else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        UkeL = false;
                        _AnimatorE01.SetBool("UkeL", false);
                        UkeR = true;
                        _AnimatorE01.SetBool("UkeR", true);
                    }
                    else
                    {
                        UkeL = false;
                        UkeR = false;
                    }
                }
            }
            else
            {
                Check_Current_Time0 += Time.deltaTime;
            }
        }
        else
        {
            _AnimatorE01.SetBool("UkeL", false);
            _AnimatorE01.SetBool("UkeR", false);
            Check_Current_Time0 = 0;
        }

        //ècêÿÇËêUÇËÇ®ÇÎÇµ
        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
        {
            Debug.Log(Check_Current_Time0);
            Check_Current_Time0 = 0;
        }

        //òAåÇ1êUÇËè„Ç∞
        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Ren01"))
        {
            if (P_Input)
            {
                if (Check_Current_Time1 > 0.0f && Check_Time1 > Check_Current_Time1)
                {
                    if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        if (!UKe__Ren01)
                        {
                            UKe__Ren01 = true;
                            _AnimatorE01.SetBool("RenUke01", true);

                        }
                    }
                    else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        UKe__Ren01 = false;
                        _AnimatorE01.SetBool("RenUke01", false);
                    }
                }
            }
            else
            {
                Check_Current_Time1 += Time.deltaTime;
            }
        }
        else
        {
        }

        //òAåÇ1êUÇËÇ®ÇÎÇµ
        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Ren1")){Check_Current_Time1 = 0;}

        //òAåÇ2êUÇËè„Ç∞
        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Ren02"))
        {
            if (P_Input)
            {
                if (Check_Current_Time2 > 0.0f && Check_Time2 > Check_Current_Time2)
                {
                    if (Miburo_State._Katana_Direction == 0 || Miburo_State._Katana_Direction == 1 || Miburo_State._Katana_Direction == 2 || Miburo_State._Katana_Direction == 7)
                    {
                        UKe__Ren02 = false;
                        _AnimatorE01.SetBool("RenUke02", false);
                    }
                    else if (Miburo_State._Katana_Direction == 3 || Miburo_State._Katana_Direction == 4 || Miburo_State._Katana_Direction == 5 || Miburo_State._Katana_Direction == 6)
                    {
                        if (!UKe__Ren02)
                        {
                            UKe__Ren02 = true;
                            _AnimatorE01.SetBool("RenUke02", true);
                        }
                    }
                }
            }
            else
            {
                Check_Current_Time2 += Time.deltaTime;
            }
        }

        //òAåÇ2êUÇËÇ®ÇÎÇµ         
        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Ren2")){Check_Current_Time2 = 0;}

        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            UkeL = false;
            UkeR = false;
            P_Input = false;
            Effectflg = false;
        }

        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("NagasereR") || _AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("NagasereL"))
        {
            Clone_Effect = GameObject.Find("Slash_Effect(Clone)");

            if (Clone_Effect == null && !Effectflg)
            {
                Instantiate(S_Effect);
                Effectflg = true;
            }
            UkeL = false;
            UkeR = false;
            Check_Current_Time0 = 0;
        }

        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("RtoNagasare"))
        {
            Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
            if (Clone_Effect == null && !Effectflg)
            {
                Instantiate(S_Effect);
                Effectflg = true;
            }
            UKe__Ren01 = false;
            Check_Current_Time1 = 0;
            _AnimatorE01.SetBool("RenUke01", false);
        }

        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("RtoLtoNagasare"))
        {
            Clone_Effect = GameObject.Find("Slash_Effect(Clone)");
            if (Clone_Effect == null && !Effectflg)
            {
                Instantiate(S_Effect);
                Effectflg = true;
            }
            _AnimatorE01.SetBool("RenUke02", false);
            UKe__Ren02 = false;
            Check_Current_Time2 = 0;
        }

        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Ren1") || _AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Ren2") || _AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Tategiri 0"))
        {
            Attack = true;
            Effectflg = false;
        }
        else
        {
            Attack = false;
        }

        if (_AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Ren01") || _AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Ren02") || _AnimatorE01.GetCurrentAnimatorStateInfo(0).IsName("Tategiri"))
        {
            Testobj.SetActive(true);
            Testobj.transform.localScale += Vector3.one * Time.deltaTime;
        }
        else
        {
            Testobj.transform.localScale = Vector3.one;
            Testobj.SetActive(false);
        }
    }
}
