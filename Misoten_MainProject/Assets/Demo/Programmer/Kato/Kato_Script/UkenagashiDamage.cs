using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UkenagashiDamage : MonoBehaviour
{
    private GameObject Enemy_Box;
    private float FlashTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Kato_Status_E.NowHP = Kato_Status_E.NowHP - Random.Range(175, 200);
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

        if (FlashTime > 0.5)
        {

            Enemy_Box.SetActive(true);
            Destroy(this);
        }
    }
}
