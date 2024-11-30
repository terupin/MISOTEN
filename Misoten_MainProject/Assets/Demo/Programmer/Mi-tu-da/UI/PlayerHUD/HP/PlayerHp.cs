using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHp : MonoBehaviour
{
    public string SceneName;
    public float WaitTime = 2.0f;
    private float timer = 0.0f;

    public Image image;/*HP表示用のImage*/
    public Sprite[] hpSprites;/*HPごとの画像を入れる配列*/
    
    private int Hp = 5;

    //Start is called before the first frame update
    void Start()
    {
        UpdateHpImage();/*初期HPの画像を表示*/
    }

    //Update is called once per frame
    void Update()
    {
        UpdateHpImage();

        //テスト用のHP変更処理
        if (Input.GetKeyDown(KeyCode.E))
        {
            Hp = Mathf.Max(0, Hp - 1);/*HPを減らすが、0以下にはならない*/

        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Hp = Mathf.Min(hpSprites.Length - 1, Hp + 1);/*HPを増やすが、画像数を超えないように*/
            UpdateHpImage();
        }

        //シーン遷移用の処理
        if (Kato_Status_P.NowHP <= 0)
        {
            timer += Time.deltaTime;

            if (timer >= WaitTime)
            {
                SceneManager.LoadScene(SceneName);
            }
        }
    }

    //HPに応じてImageを変更する
    void UpdateHpImage()
    {

        if (Kato_Status_P.NowHP >= 0 && Kato_Status_P.NowHP < hpSprites.Length)
        {
            image.sprite = hpSprites[Kato_Status_P.NowHP];
        }
    }
}
