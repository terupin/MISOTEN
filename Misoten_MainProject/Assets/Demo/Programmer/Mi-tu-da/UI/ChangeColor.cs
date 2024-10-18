using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour{

    // �ϐ��錾
    private Image image; /* Image�R���|�[�l���g�ւ̎Q�Ƃ�ێ� */
    private int randomValue = 0; /* �����_���Ȓl���i�[���� */
    private float CurrentTime = 0.0f;/* ���Ԍv�� */
    public int Imagenumber = 0; /* �摜��ԍ��ŊǗ� */
    public float WaitTime = 5.0f;/* �҂����� */
    public float LifeTime = 3.0f;/* �\���\���� */

    void SetAlpha(Image img, float alphaValue){

        Color color = img.color;
        color.a = alphaValue; /*�A���t�@�l��ݒ�*/
        img.color = color; /*�ݒ肵���J���[��UI�C���[�W�ɔ��f�A���t�@�l��ݒ�*/
    }

    // Start is called before the first frame update
    void Start(){

        image = GetComponent<Image>(); /*Image�R���|�[�l���g���擾*/
        SetAlpha(image, 0.0f); /*�\�����Ȃ�*/
    }

    // Update is called once per frame
    void Update(){

        CurrentTime += Time.deltaTime;
        Debug.Log(CurrentTime);

        // �����_����1�`8�̐����𐶐����ĉ摜��I��
        randomValue = Random.Range(1, 9);

        //�����b�̑҂�
        if (CurrentTime >= WaitTime){

            CurrentTime = 0.0f;
            ChangeImage(randomValue);
        }
    }

    void ChangeImage(int value){

        if (value == Imagenumber){
            SetAlpha(image, 1.0f); /* �\�� */

            //�����b�ԕ\��
            if (CurrentTime >= LifeTime)
            {
                SetAlpha(image, 0.0f); /* ��\�� */
                CurrentTime = 0.0f; /* ���Ԃ����Z�b�g */
                randomValue = 0;
            }
        }
        else if(value != Imagenumber)
        {
            SetAlpha(image, 0.0f); /* ��\�� */
        }
    }
}
