using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Status_P : MonoBehaviour
{
    public int MaxHP=5;
    public int NowHP;
    public int Armor;
    private float ArmorTime=30.0f;
    private float ArmorCurrentTime;
    public static Kato_Status_P instance;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        NowHP = MaxHP;
        Armor= 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Armor!=0)
        {
            ArmorCurrentTime += Time.deltaTime;

            if(ArmorCurrentTime>=ArmorTime)
            {
                Armor = 0;
                ArmorCurrentTime = 0;
            }
        }
        else
        {
            ArmorCurrentTime = 0;
        }
        
    }

    public void Damage(int  Damage)
    {
        NowHP= NowHP - Damage;
        Debug.LogFormat("Žc‚èHP‚Í {0}", NowHP);
    }
}
