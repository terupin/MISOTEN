using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_EffectMove : MonoBehaviour
{
    public float MoveTime = 1.5f;
    private float CurrentTime=0.0f;
    public

    // Start is called before the first frame update
    void Start()
    {
        if(Kato_a_Player_Anim.Katana_Direction==0 || Kato_a_Player_Anim.Katana_Direction == 1 || Kato_a_Player_Anim.Katana_Direction == 2|| Kato_a_Player_Anim.Katana_Direction == 7)
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 240.0f, 0.0f);
        }
        else if (Kato_a_Player_Anim.Katana_Direction == 4 || Kato_a_Player_Anim.Katana_Direction == 5 || Kato_a_Player_Anim.Katana_Direction == 6 || Kato_a_Player_Anim.Katana_Direction == 3)
        {
            gameObject.transform.rotation = Quaternion.Euler(0.0f, 300.0f, 0.0f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentTime>=MoveTime)
        {
            Destroy(gameObject);
        }

        gameObject.transform.position += gameObject.transform.right * 15 * Time.deltaTime;
        CurrentTime += Time.deltaTime;
    }
}
