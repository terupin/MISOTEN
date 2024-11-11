using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Status_P : MonoBehaviour
{
    public int MaxHP=5;
    static public int NowHP;
    public static Kato_Status_P instance;

    // Start is called before the first frame update
    void Start()
    {
        NowHP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int  Damage)
    {
        NowHP= NowHP - Damage;
        Debug.LogFormat("Žc‚èHP‚Í {0}", NowHP);
    }
}
