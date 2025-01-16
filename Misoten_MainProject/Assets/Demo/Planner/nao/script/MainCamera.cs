using Cinemachine;
using UnityEngine;

public class MainCamera : MonoBehaviour
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
    private float camchangecountlimit;
    public CinemachineInputProvider MainCamLock;
    public bool LDownflg = false;

    public CinemachineVirtualCamera LockOnCamera2;
    public CinemachineTargetGroup targetGroup;
    public Transform player;
    private Transform currentTargetEnemy;

    // Start is called before the first frame update
    void Start()
    {
        // 初期設定: プレイヤーをターゲットグループに追加
        targetGroup.AddMember(player, 1.0f, 2.0f); // Weight=1, Radius=2
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ukenagashicam();
        LockOncamerachange();
        maincamlock();
    }

    void CameraChenge()
    {
        if (isLockOn == true)
        {
            LockOnCamera.Priority = 15;
            Transform newTargetEnemy = FindClosestEnemy();

            if (currentTargetEnemy != null)
            {
                // 既存の敵をターゲットグループから削除
                targetGroup.RemoveMember(currentTargetEnemy);
            }

            if (newTargetEnemy != null)
            {
                // 新しい敵をターゲットグループに追加
                targetGroup.AddMember(newTargetEnemy, 1.0f, 2.0f); // Weight=1, Radius=2
                currentTargetEnemy = newTargetEnemy;
            }

        }
        else
        {
            LockOnCamera.Priority = 3;
        }

    }
    void LockOncamerachange()
    {
        //R3を押したときにロックオンフラグをTRUEにする
        //if (unityengine.input.getkeyup("joystick button 9") && islockon == false)
        //{
        //    islockon = true;
        //    camerachenge();
        //}
        //r3を離したときにロックオンフラグをfalseにする
        //else if (unityengine.input.getkeyup("joystick button 9") && islockon == true)
        //{
        //    islockon = false;
        //    camerachenge();
        //}
        //if (unityengine.input.getkeyup("joystick button 9"))
        //{
        //    debug.log("r3押し込み確認");
        //    debug.log(islockon);
        //}
    }
    void ukenagashicam()
    {
        
        C_ukenagashi = Miburo_State._Katana_Direction;

        if (C_ukenagashi == 0 || C_ukenagashi == 1 || C_ukenagashi == 2 || C_ukenagashi == 7)
        {
            if (Matsunaga_Enemy01_State.UkeL)
            {
                changecamstartR = true;
                //Debug.Log(changecamstartL);
            }
            else if(Matsunaga_Enemy01_State.UKe__Ren01 || Matsunaga_Enemy02_State.UKe__Ren01)
            {
                changecamstartR = true;
                //Debug.Log(changecamstartL);
            }

        }
        else if (C_ukenagashi == 3 || C_ukenagashi == 4 || C_ukenagashi == 5 || C_ukenagashi == 6)
        {
            if (Matsunaga_Enemy01_State.UkeR)
            {
                changecamstartL = true;
                //Debug.Log(changecamstartR);
            }
            else if(Matsunaga_Enemy01_State.UKe__Ren02 || Matsunaga_Enemy02_State.UKe__Ren02)
            {
                changecamstartL = true;
            }
        }



        if (changecamstartL == true)
        {
            camchangecount = camchangecount + Time.deltaTime;
            ukenagasiLCam.Priority = 15;

        }
        else if (changecamstartR == true)
        {
            camchangecount = camchangecount + Time.deltaTime;
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
        //Debug.Log(camchangecount);
        //Debug.Log(ukenagasiLCam.Priority);
        //Debug.Log(ukenagasiRCam.Priority);
        //Debug.Log(C_ukenagashi);
    }

    void maincamlock()
    {
        if (UnityEngine.Input.GetKey("joystick button 4"))
        {
            LDownflg = true;
        }
        else 
        {
            LDownflg = false;
            MainCamLock.enabled = true;
        }

        if (LDownflg == true)
        {
            MainCamLock.enabled = false;
        }

        //if (unityengine.input.getkeyup("joystick button 4"))
        //{
        //    ldownflg = false;
        //}
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }

   
}
