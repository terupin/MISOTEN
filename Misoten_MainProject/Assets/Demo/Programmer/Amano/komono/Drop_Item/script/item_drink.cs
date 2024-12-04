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

    private void OnTriggerEnter(Collider other)
    {
        Kato_Status_P Armor_meny = other.gameObject.GetComponent<Kato_Status_P>(); //ƒvƒŒƒCƒ„[‚ÌHP—Ê‚Ìæ“¾
        Armor_meny.Armor++;
        Destroy(this.gameObject);
    }



}
