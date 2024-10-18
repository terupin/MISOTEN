using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kato_Ukenagashi_Nagare : MonoBehaviour
{
    private bool Hitflg=false;

    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Hitflg)
        {
          //obj=this.transform.Find("Point0").gameObject;
          //  Debug.Log(obj.transform.position);

          //  this.transform.position+= (this.transform.position -obj.transform.position)*Time.deltaTime;
        }
        else
        {
            //gameObject.transform.position += gameObject.transform.forward*Time.deltaTime;
        }

      
    }

    private void OnCollisionEnter(Collision collision)
    {
        Hitflg = true;
        Debug.Log("“–‚½‚è”»’è");
        if (collision.gameObject.name=="") 
        {
            Hitflg = true;

        }
    }
}
