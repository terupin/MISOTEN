using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Kato_Ukenagashi_Nagare1 : MonoBehaviour
{
    private bool Hitflg;

    public GameObject obj;
    private GameObject obj2;

    public float Speed = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        Hitflg=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hitflg)
        {
          //obj2=obj.transform.Find("Point0").gameObject;
          //  Debug.Log(obj2.transform.position);

            //transform.position = Quaternion.identity();

            //new Vector3(transform.position.x, transform.position.y, -1.0f)

            //if (Vector3.Distance(transform.position, new Vector3(obj2.transform.position.x, obj2.transform.position.y, transform.position.z)) >1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(obj2.transform.position.x, obj2.transform.position.y, transform.position.z)+ obj2.transform.right*2, Speed * Time.deltaTime);
            }




            Vector3.Distance(transform.position, obj2.transform.position);
            Debug.Log(Vector3.Distance(transform.position, obj2.transform.position));
            //this.transform.position-= (this.transform.position -obj.transform.position)*Time.deltaTime;
        }
        else
        {
            //gameObject.transform.position += gameObject.transform.forward*Time.deltaTime* Speed;

            this.transform.RotateAround(new Vector3(0.0f, 3.0f, -0.5f), gameObject.transform.right,  Speed*3 * Time.deltaTime);
        }

      
    }

    private void OnCollisionEnter(Collision collision)
    {
        Hitflg = true;
        obj2 = obj.transform.Find("Point0").gameObject;
        Debug.Log(obj2.transform.position);
        Debug.Log("“–‚½‚è”»’è");
        if (collision.gameObject.name=="") 
        {
            Hitflg = true;

        }
    }
}
