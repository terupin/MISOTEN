using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revival_komono : MonoBehaviour
{
    public GameObject prefab; // ê∂ê¨ÇµÇΩÇ¢PrefabÇInspectorÇ≈éwíË

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            GameObject revivaling = Instantiate(prefab, this.transform.position, Quaternion.identity, this.gameObject.transform);
        }
    }
}
