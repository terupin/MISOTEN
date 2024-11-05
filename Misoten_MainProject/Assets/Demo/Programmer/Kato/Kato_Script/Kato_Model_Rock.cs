using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Model_Rock : MonoBehaviour
{
    public GameObject Object;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        //gameObject.transform.rotation = Quaternion.Euler(0.0f, Object.transform.rotation.y-180.0f, 0.0f);
        gameObject.transform.position= Object.transform.position;
    }
}
