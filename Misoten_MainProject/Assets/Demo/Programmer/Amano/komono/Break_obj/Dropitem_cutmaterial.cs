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
            Instantiate(droping_item, this.transform.position, Quaternion.identity);
            droping_item.AddComponent<Billboard>();
        }

    }
}
