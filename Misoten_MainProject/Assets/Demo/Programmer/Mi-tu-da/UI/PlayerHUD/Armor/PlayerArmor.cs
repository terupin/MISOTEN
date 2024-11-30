using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerArmor : MonoBehaviour
{
    public Image image;/*Armor表示用のImage*/
    public Sprite[] ArmorSprites;/*Armorごとの画像を入れる配列*/
    private int Armor = 0;

    //Start is called before the first frame update
    void Start()
    {
        UpdateArmorImage();/*初期Armorの画像を表示*/
    }

    //Update is called once per frame
    void Update()
    {
        //テスト用のArmor変更処理
        if (Input.GetKeyDown(KeyCode.T))
        {
            Armor = Mathf.Max(0, Armor - 1);/*Armorを減らすが、0以下にはならない*/
            UpdateArmorImage();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            Armor = Mathf.Min(ArmorSprites.Length - 1, Armor + 1);/*Armorを増やすが、画像数を超えないように*/
            UpdateArmorImage();
        }
    }

    //Armorに応じてImageを変更する
    void UpdateArmorImage()
    {
        if (Armor >= 0 && Armor < ArmorSprites.Length)
        {
            image.sprite = ArmorSprites[Armor];
        }

        if (Armor == 0)
        {
            image.enabled = false;/*非表示*/
        }
        else
        {
            image.enabled = true; /*Imageを表示*/
        }
    }
}


