using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_ramen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Kato_Status_P Hp_meny = other.gameObject.GetComponent<Kato_Status_P>(); //ƒvƒŒƒCƒ„[‚ÌHP—Ê‚Ìæ“¾

            //if (Hp_meny.NowHP < Hp_meny.MaxHP)
            //{
            //    Hp_meny.NowHP++;
            //}

            Destroy(this.gameObject);
        }
    }
}
