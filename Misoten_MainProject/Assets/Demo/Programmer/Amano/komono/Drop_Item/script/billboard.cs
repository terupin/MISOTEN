using System.Collections;
using UnityEngine;

public class billboard : MonoBehaviour
{
    private Camera targetCamera;

    void Start()
    {
        targetCamera = Camera.main;
    }

    void Update()
    {
        Vector3 p = targetCamera.transform.position;
        p.y = transform.position.y;

        // Billborad‚ÌÀ‘•
        Vector3 direction = (p - transform.position).normalized;
        transform.forward = -direction; // Quad‚ÌŒü‚«‚É‡‚í‚¹‚é‚½‚ß”½“]
    }

}
