using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Flash : MonoBehaviour
{
   private float MaxTime = 6.0f;   //被ダメージ後無敵時間
   private GameObject Miburo_Box;//ダメージ当たり判定

    private float FlashTime = 0.0f;

    // Use this for initialization
    void Start()
    {

        FlashTime = 0.0f;
        //GameObject Miburo_Box = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Miburo_Box)
        {
            Miburo_Box.SetActive(false);
        }

       FlashTime += Time.deltaTime;

        if(FlashTime> MaxTime)
        {

                Miburo_Box.SetActive(true);
            
          
            Destroy(this);
        }
    }
}

