using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Denchiku_Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�Ռ��g�Ɠ����������ɓd�|������
       if( collision.gameObject.tag=="ShockWave")
        {
            Destroy(gameObject);
        }
    }
}
