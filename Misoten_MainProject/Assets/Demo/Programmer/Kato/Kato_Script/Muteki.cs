using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muteki : MonoBehaviour
{
    private GameObject Miburo_Box;

    // Start is called before the first frame update
    void Start()
    {
        Miburo_Box = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
       

        if (Kato_a_Player_Anim.Katana_Direction != -1)
        {
            Miburo_Box.SetActive(false);
            
        }
        else
        {
            Miburo_Box.SetActive(true);
        }
    }
}
