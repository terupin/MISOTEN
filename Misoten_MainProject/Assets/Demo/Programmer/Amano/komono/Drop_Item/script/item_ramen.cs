using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_ramen : MonoBehaviour
{
    [SerializeField, Header("�񕜎��G�t�F�N�g")]
    public ParticleSystem hell;

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
            Debug.Log("��������");

            Kato_Status_P Hp_meny = player_obj.gameObject.GetComponent<Kato_Status_P>(); //�v���C���[��HP�ʂ̎擾

            if (Hp_meny.NowHP < Hp_meny.MaxHP)
            {
                Hp_meny.NowHP++;
                Instantiate(hell,new Vector3(player_obj.transform.position.x,0.01f,player_obj.transform.position.z),Quaternion.identity);
            }

            Destroy(this.gameObject);
        }
    }
}
