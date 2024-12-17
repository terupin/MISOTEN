using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
   public static HitStop instance;
    [SerializeField, Header("ヒットストップ時間")]
    public float HitStopTime;
    [SerializeField, Header("ヒットストップスピード(スローモーションにもできるよ)") ,Range(0, 1)]
    public float HitStopSpeed;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator HitStop_()
    {
        Time.timeScale = HitStopSpeed;
        yield return new WaitForSecondsRealtime(HitStopTime);
        Time.timeScale = 1f;
    }
}
