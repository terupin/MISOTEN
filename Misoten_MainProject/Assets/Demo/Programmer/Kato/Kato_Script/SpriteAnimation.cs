using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour
{
    public Image _image;
    public Sprite[] AnimationImage;
    private float AnimCurrenttime;
    static public SpriteAnimation Instance;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _image.sprite = AnimationImage[0];
    }

    // Update is called once per frame
    void Update()
    {

        _image.sprite = AnimationImage[0];
    }

    public void AnimStart(float NowTime ,float Maxtime)
    {
        if(NowTime /Maxtime<=0.125f) {_image.sprite = AnimationImage[1];}
        else if (NowTime / Maxtime <= 0.25f) { _image.sprite = AnimationImage[2]; }
        else if (NowTime / Maxtime <= 0.375f) { _image.sprite = AnimationImage[3]; }
        else if (NowTime / Maxtime <= 0.5f) { _image.sprite = AnimationImage[4]; }
        else if (NowTime / Maxtime <= 0.625f) { _image.sprite = AnimationImage[5]; }
        else if (NowTime / Maxtime <= 0.75f) { _image.sprite = AnimationImage[6]; }
        else if (NowTime / Maxtime <= 0.875f) { _image.sprite = AnimationImage[7]; }
        else if (NowTime / Maxtime <= 1.0f) { _image.sprite = AnimationImage[8]; }

    }
}
