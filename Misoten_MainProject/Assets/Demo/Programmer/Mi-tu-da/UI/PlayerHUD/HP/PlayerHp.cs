using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
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
    }

    //HPに応じてImageを変更する
    void UpdateHpImage()
    {
        Kato_Status_P Hp_UI = GameObject.Find("Player_Miburo").GetComponent<Kato_Status_P>();


        if (Hp_UI.NowHP >= 0 && Hp_UI.NowHP < hpSprites.Length)
        {
            image.sprite = hpSprites[Hp_UI.NowHP];
        }
    }
}
