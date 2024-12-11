using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_drink : MonoBehaviour
{
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
        if (collision.gameObject.tag == "Player")
        {
            Kato_Status_P Armor_meny = collision.gameObject.GetComponent<Kato_Status_P>(); //ÉvÉåÉCÉÑÅ[ÇÃHPó ÇÃéÊìæ
                                                                                       //Armor_meny.Armor++;
            Destroy(this.gameObject);
        }
    }




}
