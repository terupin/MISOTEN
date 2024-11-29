using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsunaga_Status_E : MonoBehaviour
{
    static public float MaxHP = 10000;
    static public float NowHP =MaxHP;

    // Start is called before the first frame update
    void Start()
    {
        NowHP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
