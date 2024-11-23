using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSet : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider BgmSlider;
    [SerializeField] private Slider SeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("BgmVolume"))
        {
            LoadVolume();
        }
        else
        {
            BgmSlider.value = 1.0f; // èâä˙íl
            SeSlider.value = 1.0f; // èâä˙íl

            SetBgmVolume();
            SetSeVolume();
        }
    }

    public void SetBgmVolume()
    {
        float volume = BgmSlider.value;
        mixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BgmVolume", volume);
    }
    
    public void SetSeVolume()
    {
        float volume = SeSlider.value;
        mixer.SetFloat("SE", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SeVolume", volume);
    }

    private void LoadVolume()
    {
        BgmSlider.value = PlayerPrefs.GetFloat("BgmVolume");
        SeSlider.value = PlayerPrefs.GetFloat("SeVolume");

        SetBgmVolume();
        SetSeVolume(); 
    }
}


