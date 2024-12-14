using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_ramen : MonoBehaviour
{
    private GameObject player_obj;

    // Start is called before the first frame update
    void Start()
    {
        player_obj = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ItemSenser")
        {
            Debug.Log("ìñÇΩÇ¡ÇΩ");

            Kato_Status_P Hp_meny = player_obj.gameObject.GetComponent<Kato_Status_P>(); //ÉvÉåÉCÉÑÅ[ÇÃHPó ÇÃéÊìæ

            if (Hp_meny.NowHP < Hp_meny.MaxHP)
            {
                Hp_meny.NowHP++;
            }

            Destroy(this.gameObject);
        }
    }
}
