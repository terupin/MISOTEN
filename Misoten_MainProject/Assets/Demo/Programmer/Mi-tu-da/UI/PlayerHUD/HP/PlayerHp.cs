using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour
{
    public Image image;/*HP�\���p��Image*/
    public Sprite[] hpSprites;/*HP���Ƃ̉摜������z��*/
    private int Hp = 5;

    //Start is called before the first frame update
    void Start()
    {
        UpdateHpImage();/*����HP�̉摜��\��*/
    }

    //Update is called once per frame
    void Update()
    {
        UpdateHpImage();
        //�e�X�g�p��HP�ύX����
        if (Input.GetKeyDown(KeyCode.E))
        {
            Hp = Mathf.Max(0, Hp - 1);/*HP�����炷���A0�ȉ��ɂ͂Ȃ�Ȃ�*/

        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Hp = Mathf.Min(hpSprites.Length - 1, Hp + 1);/*HP�𑝂₷���A�摜���𒴂��Ȃ��悤��*/
            UpdateHpImage();
        }
    }

    //HP�ɉ�����Image��ύX����
    void UpdateHpImage()
    {
        Kato_Status_P Hp_UI = GameObject.Find("Player_Miburo").GetComponent<Kato_Status_P>();


        if (Hp_UI.NowHP >= 0 && Hp_UI.NowHP < hpSprites.Length)
        {
            image.sprite = hpSprites[Hp_UI.NowHP];
        }
    }
}
