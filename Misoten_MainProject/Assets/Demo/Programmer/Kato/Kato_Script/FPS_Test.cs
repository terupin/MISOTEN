using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_Test : MonoBehaviour
{
    public Text _test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _test.text = string.Format("現在のFPS：{0}\n最大FPS：{1}",(int)(1.0f/Time.deltaTime), Application.targetFrameRate);
    }
}
