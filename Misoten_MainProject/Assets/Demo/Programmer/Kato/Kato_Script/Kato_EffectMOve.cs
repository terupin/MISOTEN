using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_EffectMOve : MonoBehaviour
{
    public float MoveTime = 0.5f;
    private float CurrentTime=0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentTime>=MoveTime)
        {
            Destroy(gameObject); 
        }

        gameObject.transform.position += gameObject.transform.up * 0.5f * Time.deltaTime;
        gameObject.transform.position += gameObject.transform.right * 10 * Time.deltaTime;
        CurrentTime += Time.deltaTime;
    }
}
