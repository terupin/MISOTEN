using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Kato_MainCamera : MonoBehaviour
{
    //通常時とロックオン時のカメラ周りに使用する変数宣言
    public bool isLockOn = false; //ロックオン用カメラ切り替えフラグ
    [SerializeField] Transform loocOnTarget = null;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineVirtualCamera LockOnCamera;
    //受け流し用カメラ切り替え用変数
    public static bool changecamstartL = false;
    public static bool changecamstartR = false;
    private float camchangecount = 0.0f;
    public CinemachineVirtualCamera ukenagasiLCam;
    public CinemachineVirtualCamera ukenagasiRCam;
    private int C_ukenagashi = Kato_a_Player_Anim.Katana_Direction;
    [SerializeField, Header("受け流しカメラ切り替えフレーム数")]
    private float camchangecountlimit ;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Katoukenagashicam();
        LockOncamerachange();
    }

    void KatoCameraChenge()
    {
        if (isLockOn == true)
        {
            LockOnCamera.Priority = 15;

        }
        else
        {
            LockOnCamera.Priority = 3;
        }

    }
    void LockOncamerachange()
    {
        //R3を押したときにロックオンフラグをTRUEにする
        if (UnityEngine.Input.GetKeyUp("joystick button 9") && isLockOn == false)
        {
            isLockOn = true;
            KatoCameraChenge();
        }
        //R3を離したときにロックオンフラグをfalseにする
        else if (UnityEngine.Input.GetKeyUp("joystick button 9") && isLockOn == true)
        {
            isLockOn = false;
            KatoCameraChenge();
        }
        if (UnityEngine.Input.GetKeyUp("joystick button 9"))
        {
            Debug.Log("R3押し込み確認");
            Debug.Log(isLockOn);
        }
    }
    void Katoukenagashicam()
    {
        C_ukenagashi = Kato_a_Player_Anim.Katana_Direction;

        //if(Kato_HitBoxE.Ukenagashi_Flg)
        {
            if (C_ukenagashi == 0 || C_ukenagashi == 1 || C_ukenagashi == 2 || C_ukenagashi == 7)
            {
                changecamstartR = true;
                Debug.Log(changecamstartR);
            }
            else if (C_ukenagashi == 3 || C_ukenagashi == 4 || C_ukenagashi == 5 || C_ukenagashi == 6)
            {
                changecamstartL = true;
                Debug.Log(changecamstartL);
            }
        }

        //テスト
        if (UnityEngine.Input.GetKeyDown(KeyCode.N))
        {
            changecamstartL = true;
            Debug.Log(changecamstartL);
        }

        //テスト
        if (UnityEngine.Input.GetKeyDown(KeyCode.M))
        {
            changecamstartR = true;
            Debug.Log(changecamstartR);

        }



        if (changecamstartL == true)
        {
            camchangecount = camchangecount + 0.1f;
            ukenagasiLCam.Priority = 15;

        }
        else if (changecamstartR == true)
        {
            camchangecount = camchangecount + 0.1f;
            ukenagasiRCam.Priority = 15;

        }

        if (camchangecount >= camchangecountlimit)
        {
            changecamstartL = false; 
            changecamstartR = false;
            camchangecount = 0;
            ukenagasiLCam.Priority = 1;
            ukenagasiRCam.Priority = 2;
            
        }
        Debug.Log(camchangecount);
        Debug.Log(ukenagasiLCam.Priority);
        Debug.Log(ukenagasiRCam.Priority);
        Debug.Log(C_ukenagashi);
    }

}
