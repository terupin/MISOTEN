using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dropitem_cutmaterial : MonoBehaviour
{
    [SerializeField, Header("�f�ʂ̃}�e���A��")]
    public Material cut_materal;

    [SerializeField, Header("�h���b�v����A�C�e��")]
    GameObject droping_item;

    [SerializeField,Header("�����G�t�F�N�g")]
    public ParticleSystem item_particle;

    [SerializeField, Header("�G�t�F�N�g�̗�������")]
    public Quaternion vector_particle;

    public bool itemdrop;

    // Start is called before the first frame update
    void Start()
    {
     }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Set_itemdrop()
    {
        itemdrop = true;
    }


    public void CreateItem()
    {
        if (droping_item != null && itemdrop)
        {
            GameObject prefab = Resources.Load<GameObject>(droping_item.name);
            GameObject drop_new = Instantiate(droping_item, new Vector3(this.transform.position.x, this.transform.position.y + 3.0f, this.transform.position.z), Quaternion.identity, gameObject.transform.parent);

            Instantiate(item_particle, transform.position, vector_particle);
            itemdrop = false;

            Debug.Log(droping_item.name);

        }
    }
}
