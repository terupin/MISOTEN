using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Flash : MonoBehaviour
{
   private float MaxTime = 6.0f;   //��_���[�W�㖳�G����
   private GameObject Miburo_Box;//�_���[�W�����蔻��

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

