using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_EffectMove : MonoBehaviour
{
    public float MoveTime = 1.5f;
    private float CurrentTime=0.0f;

    private GameObject EnemyObj;
    private GameObject EnemyKatanaBox;

    private float Enemy_Roty;
    

    // Start is called before the first frame update
    void Start()
    {
        //EnemyObj = GameObject.FindWithTag("Enemy");
        //EnemyKatanaBox = GameObject.Find("Enemy_HitBox");

        //gameObject.transform.position = new Vector3(EnemyKatanaBox.transform.localPosition.x,2.0f, EnemyKatanaBox.transform.localPosition.z);

        //if (Kato_a_Player_Anim.Katana_Direction == 0 || Kato_a_Player_Anim.Katana_Direction == 1 || Kato_a_Player_Anim.Katana_Direction == 2 || Kato_a_Player_Anim.Katana_Direction == 7)
        //{
        //    gameObject.transform.rotation = Quaternion.Euler(0.0f, EnemyObj.transform.localEulerAngles.y - 120, 0.0f);
        //}
        //else if (Kato_a_Player_Anim.Katana_Direction == 4 || Kato_a_Player_Anim.Katana_Direction == 5 || Kato_a_Player_Anim.Katana_Direction == 6 || Kato_a_Player_Anim.Katana_Direction == 3)
        //{
        //    gameObject.transform.rotation = Quaternion.Euler(0.0f, EnemyObj.transform.localEulerAngles.y - 60, 0.0f);
        //}
    }

    // Update is called once per frame
    void Update()
    {


        //Vector3 to = target.transform.position - transform.position;
        //var angle = Vector3.SignedAngle(transform.forward, to, Vector3.up);





        if (CurrentTime>=MoveTime)
        {
            //UnityEditor.EditorApplication.isPaused = true;
            //Destroy(gameObject);
        }

        gameObject.transform.position += gameObject.transform.forward * 15 * Time.deltaTime;
        CurrentTime += Time.deltaTime;
    }
}
