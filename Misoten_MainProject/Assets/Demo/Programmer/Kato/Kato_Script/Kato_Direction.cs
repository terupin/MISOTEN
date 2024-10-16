using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Kato_Direction : MonoBehaviour
{
    //private int randomValue = 0; /* �����_���Ȓl���i�[���� */
    //private float CurrentTime = 0.0f;/* ���Ԍv�� */
    //public float WaitTime = 3.0f;/* �҂����� */
    //public float LifeTime = 5.0f;/* �\���\���� */

    public Image[] Image_=new Image[8];

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < Image_.Length; i++)
        {
            Image_[i].color = new Vector4(1.0f, 1.0f, 1.0f, 0.1f);
        }

        if(Kato_Enemy_Anim.randomValue>=0)
        {
            Image_[Kato_Enemy_Anim.randomValue].color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}
