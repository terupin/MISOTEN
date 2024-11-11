using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Flash : MonoBehaviour
{
   private float MaxTime = 1;   // “_–ÅŽüŠú
    private GameObject ChildObj ;

    private float FlashTime = 0.0f;

    // Use this for initialization
    void Start()
    {
        FlashTime = 0.0f;
        ChildObj = gameObject.transform.FindChild("Miburo").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.time > nextTime)
        //{
        //    gameObject.SetActive(true); 

        //    nextTime += interval;
        //}
        FlashTime += Time.deltaTime*6;

        if((int)FlashTime%2==0)
        {
            ChildObj.SetActive(true);
        }
        else if ((int)FlashTime % 2 == 1)
        {
            ChildObj.SetActive(false);
        }

        if(FlashTime> MaxTime*6)
        {
            Destroy(this);
        }
    }
}

