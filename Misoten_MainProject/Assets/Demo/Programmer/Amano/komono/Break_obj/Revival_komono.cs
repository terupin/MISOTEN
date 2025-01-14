using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revival_komono : MonoBehaviour
{
    public GameObject prefab; // ¶¬‚µ‚½‚¢Prefab‚ðInspector‚ÅŽw’è
    Kato_Status_E get_HP;

    private bool seven_five = true;
    private bool five_one = true;
    private bool two_five = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float HP_Enemy = Kato_Status_E.NowHP;

        if (transform.childCount == 0 )
        {
            if(HP_Enemy > 7500.0f && HP_Enemy<5000.0f && seven_five)
            {
                seven_five = false;
                GameObject revivaling = Instantiate(prefab, this.transform.position, Quaternion.identity, this.gameObject.transform);
            }
            if (HP_Enemy > 5000.0f && HP_Enemy < 2500.0f && five_one) 
            {
                five_one = false;
                GameObject revivaling = Instantiate(prefab, this.transform.position, Quaternion.identity, this.gameObject.transform);
            }

            if (HP_Enemy > 2500.0f && two_five)
            {
                two_five = false;
                GameObject revivaling = Instantiate(prefab, this.transform.position, Quaternion.identity, this.gameObject.transform);
            }
            
        }
    }
}
