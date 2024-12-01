using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Flash : MonoBehaviour
{
   private float MaxTime = 5;   // “_–ÅŽüŠú
    private GameObject ChildObj ;
    private GameObject Miburo_Box;

    private float FlashTime = 0.0f;

    // Use this for initialization
    void Start()
    {
        Kato_Status_P.NowHP--;
        FlashTime = 0.0f;
        ChildObj = GameObject.Find("Char_Miburo_noRoot");
        Miburo_Box = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Miburo_Box)
        {
            Miburo_Box.SetActive(false);
        }

       FlashTime += Time.deltaTime*6;

        //if((int)FlashTime%2==0)
        //{
        //    ChildObj.SetActive(true);
        //}
        //else if ((int)FlashTime % 2 == 1)
        //{
        //    ChildObj.SetActive(false);
        //}

        if(FlashTime> MaxTime*6)
        {
            if(!Miburo_Box)
            {
                Miburo_Box.SetActive(true);
            }
          
            Destroy(this);
        }
    }
}

