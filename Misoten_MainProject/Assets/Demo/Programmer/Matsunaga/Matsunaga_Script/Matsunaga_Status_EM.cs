using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsunaga_Status_EM : MonoBehaviour
{
    static public int MaxHP = 10000;
    static public int NowHP =MaxHP;

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
