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
                if (Miburo_State._Attack02 || Miburo_State._CounterR || Miburo_State._CounterL)
                {
                    gameObject.AddComponent<Enemy_Damage2>();
                    
                }
                else if (Miburo_State._Attack01 )
                {
                    gameObject.AddComponent<Enemy_Damage>();
                   
                }
                else if (Miburo_State._RenCounter01)
                {
                    gameObject.AddComponent<Enemy_Damage3>();
                    UnityEditor.EditorApplication.isPaused = true;
                }
                else if (Miburo_State._RenCounter02)
                {
                    gameObject.AddComponent<Enemy_Damage4>();
                    UnityEditor.EditorApplication.isPaused = true;
                }
            }
        }
    }
}
