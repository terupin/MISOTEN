using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
   public static HitStop instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator HitStop_(float StopTime)
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(StopTime);
        Time.timeScale = 1f;
    }

    public IEnumerator SlowMotion_(float SlowSpeed, float SlowTime)
    {
        Time.timeScale = SlowSpeed;
        yield return new WaitForSeconds(SlowTime);
        Time.timeScale = 1f;
    }
}
