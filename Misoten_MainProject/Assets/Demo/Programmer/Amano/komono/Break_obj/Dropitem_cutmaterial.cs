using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropitem_cutmaterial : MonoBehaviour
{
    [SerializeField, Header("�f�ʂ̃}�e���A��")]
    public Material cut_materal;

    [SerializeField, Header("�h���b�v����A�C�e��")]
    GameObject droping_item;

    

    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (droping_item != null)
        {
            GameObject prefab = Resources.Load<GameObject>(droping_item.name);
            GameObject drop_new = Instantiate(droping_item, this.transform.position, Quaternion.identity);
            drop_new.AddComponent<Billboard>();

            Debug.Log(droping_item.name);

        }

    }
}
