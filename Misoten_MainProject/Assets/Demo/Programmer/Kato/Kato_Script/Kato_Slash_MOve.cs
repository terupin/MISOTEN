using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Slash_MOve : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyUp("joystick button 5"))
        {
            gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward;
        }
    }
}
