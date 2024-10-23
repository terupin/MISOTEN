using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class Kato_Hittest : MonoBehaviour
{
    public GameObject obj;

    private bool P_G_flg=Kato_Player_Anim.G_Flg;
    private bool P_A_flg = Kato_Player_Anim.A_Flg;

    public GameObject S_Effect;  

    public static bool Ukenagashi_Flg;//�󂯗����t���O 

    private bool Hitflg=false;

    public GameObject PkatanaHitbox;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = obj.transform.position;
        gameObject.transform.rotation = obj.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        P_G_flg = Kato_Player_Anim.G_Flg;
        P_A_flg = Kato_Player_Anim.A_Flg;

        gameObject.transform.position= obj.transform.position;
        gameObject.transform.rotation = obj.transform.rotation;

        if (gameObject.name == "Enemy_HitBox" && Hitflg)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(PkatanaHitbox.transform.position.x, PkatanaHitbox.transform.position.y, transform.position.z) + PkatanaHitbox.transform.right * 2, 20 * Time.deltaTime);
        }
        else
        {
            gameObject.transform.position = obj.transform.position;
            gameObject.transform.rotation = obj.transform.rotation;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogFormat("{1}�� {0}�ɓ�������", collision.gameObject.name, gameObject.name); // �Ԃ���������̖��O���擾

        //���̃X�N���v�g�̎�����ɂ���ĕ���
        if(gameObject.name== "Player_HitBox")
        {
            if(collision.gameObject.name== "P_Enemy" && P_A_flg)
            {
                Debug.LogFormat("{1}�� {0}�Ƀ_���[�W��^�����I", collision.gameObject.name, gameObject.name);
            }
            if (collision.gameObject.name == "Enemy_HitBox" && P_G_flg)
            {
                Debug.LogFormat("�󂯗��������I");
                Ukenagashi_Flg = true;

                Instantiate(S_Effect);
                S_Effect.transform.position = gameObject.transform.position;
            }
 

        }

        if (gameObject.name == "Enemy_HitBox")
        {
            if (collision.gameObject.name == "Proto_Player" && !P_G_flg)
            {
                Debug.LogFormat("{1}�� {0}�Ƀ_���[�W��^�����I", collision.gameObject.name, gameObject.name);
            }
            if (collision.gameObject.name == "Player_HitBox" && P_G_flg)
            {
                Debug.LogFormat("�Ռ��g����!");
                //UnityEditor.EditorApplication.isPaused = true;
                Hitflg = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (gameObject.name == "Enemy_HitBox" && collision.gameObject.name == "Player_HitBox")
        {
            Ukenagashi_Flg = false;
            Hitflg = false;
            //UnityEditor.EditorApplication.isPaused = true;
        }
    }
}
