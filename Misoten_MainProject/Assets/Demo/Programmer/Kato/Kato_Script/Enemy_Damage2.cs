using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Damage2 : MonoBehaviour
{
    private GameObject Enemy_Box;
    private float FlashTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject Baria_Obj = GameObject.Find("Baria");
        if (Baria_Obj)
        {
            Kato_Status_E.NowHP = Kato_Status_E.NowHP - Random.Range(125, 150) * 3 / 4;
        }
        else
        {
            Kato_Status_E.NowHP = Kato_Status_E.NowHP - Random.Range(125, 150);
        }
        Enemy_Box = GameObject.Find("Enemy");

    }

    // Update is called once per frame
    void Update()
    {

        Enemy_Box.SetActive(false);
        FlashTime += Time.deltaTime;

        //if ((int)FlashTime % 2 == 0)
        //{
        //    ChildObj.SetActive(true);
        //}
        //else if ((int)FlashTime % 2 == 1)
        //{
        //    ChildObj.SetActive(false);
        //}

        if (FlashTime > 0.2)
        {

            Enemy_Box.SetActive(true);
            Destroy(this);
        }
    }
}
