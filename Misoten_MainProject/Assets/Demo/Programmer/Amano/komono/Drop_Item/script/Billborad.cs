using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
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

        // LookAt�̑���ɐ��ʂ𒲐�
        Vector3 direction = (p - transform.position).normalized;
        transform.forward = -direction; // Quad�̌����ɍ��킹�邽�ߔ��]
    }
}
