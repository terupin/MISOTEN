using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy01_Box : MonoBehaviour
{
    //public TagValueType TagName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PWeapon")
        {
            GameObject Enemy_Box = GameObject.Find("Enemy");
            if (Enemy_Box)
            {
                if (Miburo_State._Attack02)
                {
                    gameObject.AddComponent<Enemy_Damage2>();
                    gameObject.transform.position -= gameObject.transform.forward * 1.5f;
                }
                else if (Miburo_State._Attack01)
                {
                    gameObject.AddComponent<Enemy_Damage>();
                    gameObject.transform.position -= gameObject.transform.forward * 1.5f;
                }
                else if (Miburo_State._Attack01&& Kato_Matsunaga_Enemy_State.UKe__Ren02/*&& Miburo_State._Attack01*/)
                {
                    gameObject.AddComponent<Enemy_Damage3>();
                    gameObject.transform.position -= gameObject.transform.forward * 1.5f;
                }
                else if (Miburo_State._Attack01&& Kato_Matsunaga_Enemy_State.UKe__Ren02/* && Miburo_State._Attack02*/)
                {
                    gameObject.AddComponent<Enemy_Damage4>();
                    gameObject.transform.position -= gameObject.transform.forward * 1.5f;
                }
            }
        }
    }
}
