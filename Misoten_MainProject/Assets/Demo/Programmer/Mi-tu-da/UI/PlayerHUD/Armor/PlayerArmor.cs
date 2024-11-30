using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerArmor : MonoBehaviour
{
    public Image image;/*Armor�\���p��Image*/
    public Sprite[] ArmorSprites;/*Armor���Ƃ̉摜������z��*/
    private int Armor = 0;

    //Start is called before the first frame update
    void Start()
    {
        UpdateArmorImage();/*����Armor�̉摜��\��*/
    }

    //Update is called once per frame
    void Update()
    {
        //�e�X�g�p��Armor�ύX����
        if (Input.GetKeyDown(KeyCode.T))
        {
            Armor = Mathf.Max(0, Armor - 1);/*Armor�����炷���A0�ȉ��ɂ͂Ȃ�Ȃ�*/
            UpdateArmorImage();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            Armor = Mathf.Min(ArmorSprites.Length - 1, Armor + 1);/*Armor�𑝂₷���A�摜���𒴂��Ȃ��悤��*/
            UpdateArmorImage();
        }
    }

    //Armor�ɉ�����Image��ύX����
    void UpdateArmorImage()
    {
        if (Armor >= 0 && Armor < ArmorSprites.Length)
        {
            image.sprite = ArmorSprites[Armor];
        }

        if (Armor == 0)
        {
            image.enabled = false;/*��\��*/
        }
        else
        {
            image.enabled = true; /*Image��\��*/
        }
    }
}


