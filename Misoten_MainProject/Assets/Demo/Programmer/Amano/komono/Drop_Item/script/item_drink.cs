using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_drink : MonoBehaviour
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
            Debug.Log("“–‚½‚Á‚½");

            Kato_Status_P Armor_meny = player_obj.gameObject.GetComponent<Kato_Status_P>();

            Armor_meny.Armor++;

            Destroy(this.gameObject);
        }
    }
}
