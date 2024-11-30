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

        //�V�[���J�ڗp�̏���
        if (Kato_Status_P.NowHP <= 0)
        {
            timer += Time.deltaTime;

            if (timer >= WaitTime)
            {
                SceneManager.LoadScene(SceneName);
            }
        }
    }

    //HP�ɉ�����Image��ύX����
    void UpdateHpImage()
    {

        if (Kato_Status_P.NowHP >= 0 && Kato_Status_P.NowHP < hpSprites.Length)
        {
            image.sprite = hpSprites[Kato_Status_P.NowHP];
        }
    }
}
