using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHp : MonoBehaviour
{
    // 最大HPと現在のHP。
    private int MaxHp = Kato_Status_E.MaxHP;
    private float CurrentHp;
    private float TargetHp; // ダメージ後の目標HP
    public float DamageUiSpeed = 0.1f; // ダメージが減るスピード

    //public string SceneName;
    public float WaitTime = 2.0f;
    private float timer = 0.0f;
    
    public Slider slider;

    void Start()
    {
        MaxHp = Kato_Status_E.MaxHP;
        slider.minValue = 0;
        slider.maxValue = MaxHp;
        slider.value = MaxHp;

        // 現在のHPを最大HPと同じに。
        CurrentHp = MaxHp;
        TargetHp = MaxHp;
    }

    private void Update()
    {
        TargetHp = Kato_Status_E.NowHP;

        Debug.Log(TargetHp);
        //slider.value = (float)TargetHp;

        // 徐々にCurrentHpがTargetHpに向かって減少
        if (CurrentHp > TargetHp)
        {
            CurrentHp -= DamageUiSpeed * Time.deltaTime;
            CurrentHp = Mathf.Max(CurrentHp, TargetHp); // TargetHp以下にはならない
            slider.value = CurrentHp;
        }

        // Dキーが押されたときにダメージを与える(テスト用)
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Kato_Status_E.NowHP = Kato_Status_E.NowHP- 1000;
        //    //ApplyDamage(1000); // 1ダメージを与える
        //}

        if(CurrentHp <= 0)
        {
            slider.gameObject.SetActive(false);
        }

        //シーン遷移用の処理
        //if (Kato_Status_E.NowHP <= 0)
        //{
        //    timer += Time.deltaTime;

        //    if (timer >= WaitTime)
        //    {
        //        SceneManager.LoadScene(SceneName);
        //    }
        //}
    }

    // ダメージを適用する
    private void ApplyDamage(int damage)
    {
        TargetHp = Mathf.Max(TargetHp - damage, 0); // TargetHpを減少
    }
}
