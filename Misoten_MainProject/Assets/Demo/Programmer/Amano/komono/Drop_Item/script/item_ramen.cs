using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class item_ramen : MonoBehaviour
{
    [SerializeField, Header("回復時エフェクト")]
    public GameObject heal;

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
        if (other.gameObject.tag == "ItemSenser" && first_get)
        {
            GetItem();
            Destroy(this.gameObject);
        }
    }

    private void GetItem()
    {
        Debug.Log("当たった");

        Kato_Status_P Hp_meny = player_obj.gameObject.GetComponent<Kato_Status_P>(); //プレイヤーのHP量の取得

        if (Hp_meny.NowHP < Hp_meny.MaxHP)
        {
            Hp_meny.NowHP++;
            Instantiate(heal, new Vector3(player_obj.transform.position.x, 0.01f, player_obj.transform.position.z), Quaternion.identity);
        }
    }


}
