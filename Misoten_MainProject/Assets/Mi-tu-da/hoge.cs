using UnityEngine;
using UnityEngine.UI;
using System.Collections;  // IEnumeratorを使うために必要

public class hoge : MonoBehaviour
{
    public Image upImage;    // Up画像
    public Image leftImage;  // Left画像
    public Image rightImage; // Right画像
    public Image bottomImage; // Bottom画像
    public float coolTime = 10.0f; // クールタイムの長さ（秒）
    public float hideAfter = 10.0f; // 表示した後に非表示にする秒数
    private bool isCoolingDown = false; // クールタイム中かどうかのフラグ
    private float currentCoolTime = 0.0f; // 残りクールタイム
    private int randomValue = 0;

    // アルファ値を設定するメソッド
    void SetAlpha(Image image, float alphaValue)
    {
        Color color = image.color;
        color.a = alphaValue; // アルファ値を設定
        image.color = color;  // 設定したカラーをUIイメージに反映
    }

    private void Start()
    {
        // 初期状態で全ての画像のアルファ値を0にリセット（非表示）
        SetAlpha(upImage, 0.0f);
        SetAlpha(leftImage, 0.0f);
        SetAlpha(rightImage, 0.0f);
        SetAlpha(bottomImage, 0.0f);

        // クールタイムを開始
        StartCoroutine(WaitAndShowRandomImage());
    }

    // クールタイム後にランダムな画像を表示する
    IEnumerator WaitAndShowRandomImage()
    {
        while (true)
        {
            randomValue = 0;

            if (!isCoolingDown)
            {
                // クールタイム開始
                isCoolingDown = true;
                currentCoolTime = coolTime;

                // ランダムで1～4の整数を生成して画像を選択
                randomValue = Random.Range(1, 5);
                ChangeAlpha(randomValue);

                // 表示後に非表示にする処理を開始
                yield return new WaitForSeconds(hideAfter);
                HideImages();

                // クールタイムを待つ
                yield return new WaitForSeconds(currentCoolTime);

                // 次の表示の準備
                isCoolingDown = false;
            }
        }
    }

    // すべての画像を非表示にする
    private void HideImages()
    {
        SetAlpha(upImage, 0.0f);
        SetAlpha(leftImage, 0.0f);
        SetAlpha(rightImage, 0.0f);
        SetAlpha(bottomImage, 0.0f);
    }

    // 数値を受け取って対応する画像のアルファ値を変更
    private void ChangeAlpha(int direction)
    {
        // 受け取った数値に応じて、特定の画像のアルファを1.0に設定（表示）
        if (direction == 1)
        {
            SetAlpha(upImage, 1.0f); // 1ならUpの画像のアルファを1.0に
        }
        else if (direction == 2)
        {
            SetAlpha(leftImage, 1.0f); // 2ならLeftの画像
        }
        else if (direction == 3)
        {
            SetAlpha(rightImage, 1.0f); // 3ならRightの画像
        }
        else if (direction == 4)
        {
            SetAlpha(bottomImage, 1.0f); // 4ならBottomの画像
        }
    }
}
