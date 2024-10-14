using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour{

    // 変数宣言
    private Image image; /* Imageコンポーネントへの参照を保持 */
    private int randomValue = 0; /* ランダムな値を格納する */
    private float CurrentTime = 0.0f;/* 時間計測 */
    public int Imagenumber = 0; /* 画像を番号で管理 */
    public float WaitTime = 5.0f;/* 待ち時間 */
    public float LifeTime = 3.0f;/* 表示可能時間 */

    void SetAlpha(Image img, float alphaValue){

        Color color = img.color;
        color.a = alphaValue; /*アルファ値を設定*/
        img.color = color; /*設定したカラーをUIイメージに反映アルファ値を設定*/
    }

    // Start is called before the first frame update
    void Start(){

        image = GetComponent<Image>(); /*Imageコンポーネントを取得*/
        SetAlpha(image, 0.0f); /*表示しない*/
    }

    // Update is called once per frame
    void Update(){

        CurrentTime += Time.deltaTime;
        Debug.Log(CurrentTime);

        // ランダムで1～8の整数を生成して画像を選択
        randomValue = Random.Range(1, 9);

        //○○秒の待ち
        if (CurrentTime >= WaitTime){

            CurrentTime = 0.0f;
            ChangeImage(randomValue);
        }
    }

    void ChangeImage(int value){

        if (value == Imagenumber){
            SetAlpha(image, 1.0f); /* 表示 */

            //○○秒間表示
            if (CurrentTime >= LifeTime)
            {
                SetAlpha(image, 0.0f); /* 非表示 */
                CurrentTime = 0.0f; /* 時間をリセット */
                randomValue = 0;
            }
        }
        else if(value != Imagenumber)
        {
            SetAlpha(image, 0.0f); /* 非表示 */
        }
    }
}
