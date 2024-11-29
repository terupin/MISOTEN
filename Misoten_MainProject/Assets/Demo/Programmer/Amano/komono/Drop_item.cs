using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_item : MonoBehaviour
{
    [SerializeField, Header("ドロップするアイテム")]
    GameObject droping_item;

    [System.NonSerialized]
    public bool drop_switch; //ドロップするタイミング

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
        Instantiate(droping_item,this.transform.position, Quaternion.identity);
        droping_item.AddComponent<Billborad>(); 
    }
}
