using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billborad : MonoBehaviour
{
    private Camera targetCamera;

    // Start is called before the first frame update
    void Start()
    {
        targetCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = targetCamera.transform.position;
        p.y = transform.position.y;
        transform.LookAt(p);
    }
}
