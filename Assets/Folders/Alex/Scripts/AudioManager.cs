using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip backgroundAudio;
    [SerializeField] private AudioSource backGroundAudioSource;

    [Header("AudioMixers")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup backgroundGround;
    [SerializeField] private AudioMixerGroup soundEffectsGroup;

    private float bgSliderValue, effectSliderValue;

    private void Awake()
    {
        if (backgroundAudio)
        {
            backGroundAudioSource.clip = backgroundAudio;
            backGroundAudioSource.Play();
        }

        bgSliderValue = PlayerPrefs.GetFloat("BackGroundSliderValue");
        effectSliderValue = PlayerPrefs.GetFloat("SoundEffectsSliderValue");
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("BackGroundSliderValue") || !PlayerPrefs.HasKey("SoundEffectsSliderValue"))
        {
            PlayerPrefs.SetFloat("BackGroundSliderValue", 1);
            PlayerPrefs.SetFloat("SoundEffectsSliderValue", 1);
        }

        audioMixer.SetFloat("BackgroundVolume", Mathf.Log10(bgSliderValue) * 20);
        audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(effectSliderValue) * 20);
    }

    public void SetVolumeBackGround(float sliderValue)
    {
        audioMixer.SetFloat("BackgroundVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetVolumeEffects(float sliderValue)
    {
        audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(sliderValue) * 20);
    }
}
