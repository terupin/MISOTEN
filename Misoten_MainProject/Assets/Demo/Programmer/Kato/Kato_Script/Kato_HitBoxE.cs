using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Kato_HitBoxE : MonoBehaviour
{
    [SerializeField, Header("����")]
    public GameObject WeponPoint;
    [SerializeField, Header("�����{")]
    public GameObject WeponRoot;
    private GameObject Clone_Effect;//�G�t�F�N�g�̃N���[��

    [SerializeField, Header("�a���G�t�F�N�g")]
    public GameObject S_Effect;

    [SerializeField, Header("�v���C���[���f��")]
    public GameObject Player_Model;

    [SerializeField, Header("�G���f��")]
    public GameObject Enemy_Model;

    public static bool Damage_Flg;//�󂯗����t���O 
    public static bool Ukenagashi_Flg;//�󂯗����t���O 

    private bool P_G_flg = Kato_Player_Anim.G_Flg;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
        Damage_Flg = false;
    }

    // Update is called once per frame
    void Update()
    {
        P_G_flg = Kato_a_Player_Anim.G_Flg;

        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.LogFormat("{1}�� {0}�ɓ�������", collision.gameObject.name, gameObject.name); // �Ԃ���������̖��O���擾

        if (collision.gameObject.name == "Player" && Enemy_State.E_AttackFlg)
        {
            //Debug.LogFormat("{1}�� {0}�Ƀ_���[�W��^�����I", collision.gameObject.name, gameObject.name);

   
            
           
            Player_Model.AddComponent<Damage_Flash>();
            Damage_Flg = true;
            //UnityEditor.EditorApplication.isPaused = true;
        }
        //else
        //{
        //    Damage_Flg = false;
        //}
        if (collision.gameObject.name == "Player_HitBox" && Kato_a_Player_Anim.Katana_Direction!=-1 && Kato_a_Player_Anim.G_Flg)
        {
            //Debug.LogFormat("�Ռ��g����!");
            Ukenagashi_Flg = true;
            Clone_Effect = GameObject.Find("sword_test(Clone)");
            if (Clone_Effect == null )
            {

                Instantiate(S_Effect);
               

            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (Kato_a_Player_Anim.G_Flg==false)
        {
            Ukenagashi_Flg = false;
        }

        Damage_Flg = false;

        if (gameObject.name == "Enemy_HitBox" && collision.gameObject.name == "Player_HitBox")
        {
            Ukenagashi_Flg = false;
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }
}
