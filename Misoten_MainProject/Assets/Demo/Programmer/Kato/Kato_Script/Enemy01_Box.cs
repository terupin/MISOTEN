using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy01_Box : MonoBehaviour
{
    //public TagValueType TagName;
    [SerializeField, Header("ダメージ時に再生する音声")]
    public AudioClip AudioClip_E01;
    private AudioSource audioSource_E;
    [SerializeField, Header("ダメージ時に再生するエフェクト")]
    public ParticleSystem Attack_effect;
    // Start is called before the first frame update
    void Start()
    {
        audioSource_E = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Miburo_State._RenCounter01)
        //{
        //    UnityEditor.EditorApplication.isPaused = true;
        //}
        //else if (Miburo_State._RenCounter02)
        //{
        //    UnityEditor.EditorApplication.isPaused = true;
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PWeapon")
        {
            GameObject Enemy_Box = GameObject.Find("Enemy");
            if (Enemy_Box)
            {
                if (Miburo_State._Attack02 || Miburo_State._CounterR || Miburo_State._CounterL)
                {
                    gameObject.AddComponent<Enemy_Damage2>();
                    audioSource_E.PlayOneShot(AudioClip_E01);
                    Instantiate(Attack_effect, transform.localPosition + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);

                }
                else if (Miburo_State._Attack01 )
                {
                    gameObject.AddComponent<Enemy_Damage>();
                    audioSource_E.PlayOneShot(AudioClip_E01);
                    Instantiate(Attack_effect, transform.localPosition + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
                }
                else if (Miburo_State._RenCounter01)
                {
                    gameObject.AddComponent<Enemy_Damage3>();
                    audioSource_E.PlayOneShot(AudioClip_E01);
                    //UnityEditor.EditorApplication.isPaused = true;
                    Instantiate(Attack_effect, transform.localPosition + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
                }
                else if (Miburo_State._RenCounter02)
                {
                    gameObject.AddComponent<Enemy_Damage4>();
                    audioSource_E.PlayOneShot(AudioClip_E01);
                    //UnityEditor.EditorApplication.isPaused = true;
                    Instantiate(Attack_effect, transform.localPosition+new Vector3(0.0f,1.0f,0.0f), Quaternion.identity);
                }
            }
        }
    }
}
