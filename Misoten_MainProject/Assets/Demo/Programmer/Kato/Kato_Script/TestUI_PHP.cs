using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI_PHP : MonoBehaviour
{
    public Text PHP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PHP.text = string.Format("{0}", Kato_Status_P.NowHP);
    }
}
