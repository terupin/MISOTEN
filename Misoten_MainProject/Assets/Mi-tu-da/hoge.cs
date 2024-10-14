using UnityEngine;
using UnityEngine.UI;
using System.Collections;  // IEnumerator���g�����߂ɕK�v

public class hoge : MonoBehaviour
{
    public Image upImage;    // Up�摜
    public Image leftImage;  // Left�摜
    public Image rightImage; // Right�摜
    public Image bottomImage; // Bottom�摜
    public float coolTime = 10.0f; // �N�[���^�C���̒����i�b�j
    public float hideAfter = 10.0f; // �\��������ɔ�\���ɂ���b��
    private bool isCoolingDown = false; // �N�[���^�C�������ǂ����̃t���O
    private float currentCoolTime = 0.0f; // �c��N�[���^�C��
    private int randomValue = 0;

    // �A���t�@�l��ݒ肷�郁�\�b�h
    void SetAlpha(Image image, float alphaValue)
    {
        Color color = image.color;
        color.a = alphaValue; // �A���t�@�l��ݒ�
        image.color = color;  // �ݒ肵���J���[��UI�C���[�W�ɔ��f
    }

    private void Start()
    {
        // ������ԂőS�Ẳ摜�̃A���t�@�l��0�Ƀ��Z�b�g�i��\���j
        SetAlpha(upImage, 0.0f);
        SetAlpha(leftImage, 0.0f);
        SetAlpha(rightImage, 0.0f);
        SetAlpha(bottomImage, 0.0f);

        // �N�[���^�C�����J�n
        StartCoroutine(WaitAndShowRandomImage());
    }

    // �N�[���^�C����Ƀ����_���ȉ摜��\������
    IEnumerator WaitAndShowRandomImage()
    {
        while (true)
        {
            randomValue = 0;

            if (!isCoolingDown)
            {
                // �N�[���^�C���J�n
                isCoolingDown = true;
                currentCoolTime = coolTime;

                // �����_����1�`4�̐����𐶐����ĉ摜��I��
                randomValue = Random.Range(1, 5);
                ChangeAlpha(randomValue);

                // �\����ɔ�\���ɂ��鏈�����J�n
                yield return new WaitForSeconds(hideAfter);
                HideImages();

                // �N�[���^�C����҂�
                yield return new WaitForSeconds(currentCoolTime);

                // ���̕\���̏���
                isCoolingDown = false;
            }
        }
    }

    // ���ׂẲ摜���\���ɂ���
    private void HideImages()
    {
        SetAlpha(upImage, 0.0f);
        SetAlpha(leftImage, 0.0f);
        SetAlpha(rightImage, 0.0f);
        SetAlpha(bottomImage, 0.0f);
    }

    // ���l���󂯎���đΉ�����摜�̃A���t�@�l��ύX
    private void ChangeAlpha(int direction)
    {
        // �󂯎�������l�ɉ����āA����̉摜�̃A���t�@��1.0�ɐݒ�i�\���j
        if (direction == 1)
        {
            SetAlpha(upImage, 1.0f); // 1�Ȃ�Up�̉摜�̃A���t�@��1.0��
        }
        else if (direction == 2)
        {
            SetAlpha(leftImage, 1.0f); // 2�Ȃ�Left�̉摜
        }
        else if (direction == 3)
        {
            SetAlpha(rightImage, 1.0f); // 3�Ȃ�Right�̉摜
        }
        else if (direction == 4)
        {
            SetAlpha(bottomImage, 1.0f); // 4�Ȃ�Bottom�̉摜
        }
    }
}
