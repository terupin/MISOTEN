using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_drink : MonoBehaviour
{
    private GameObject player_obj;
    private bool first_get;

    // Start is called before the first frame update
    void Start()
    {
        player_obj = GameObject.FindWithTag("Player");
        first_get = true;
    }

    // Update is called once per frame
    void Update()
    {
        float itempos = this.GetComponent<Throwmove>().progress;

        if (itempos > 1.0f && first_get)
        {
            GetItem();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ItemSenser"&&first_get)
        {
            GetItem();
            Destroy(this.gameObject);
        }
    }
    private void GetItem()
    {
        first_get = false;

        Debug.Log("“–‚½‚Á‚½");

        Kato_Status_P Armor_meny = player_obj.gameObject.GetComponent<Kato_Status_P>();

        Armor_meny.Armor++;
    }
}
