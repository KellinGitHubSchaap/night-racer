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

    [Header("Music Settings")]
    public SongDataSO[] m_songData;
    public AudioClip m_currentSong;

    private void Awake()
    {
        if (backgroundAudio)
        {
            backGroundAudioSource.clip = backgroundAudio;
            backGroundAudioSource.Play();
        }
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("BackGroundSliderValue") || !PlayerPrefs.HasKey("SoundEffectsSliderValue"))
        {
            PlayerPrefs.SetFloat("BackGroundSliderValue", 1);
            PlayerPrefs.SetFloat("SoundEffectsSliderValue", 1);
        }
        
        if (PlayerPrefs.GetFloat("BackgroundVolume") != backGroundAudioSource.volume)
        {
            backGroundAudioSource.volume = PlayerPrefs.GetFloat("BackgroundVolume");
        }

        //audioMixer.SetFloat("BackgroundVolume", Mathf.Log10(bgSliderValue) * 20);
        //audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(effectSliderValue) * 20);

        StartCoroutine(UpdateSong());
        //backGroundAudioSource.volume = PlayerPrefs.GetFloat("BackGroundSliderValue");
    
    }

    private void Update()
    {
        if (!backGroundAudioSource.isPlaying)
        {
            StartCoroutine(UpdateSong());
        }

        if (PlayerPrefs.GetFloat("BackgroundVolume") != backGroundAudioSource.volume)
        {
            backGroundAudioSource.volume = PlayerPrefs.GetFloat("BackgroundVolume");
        }
    }

    public void SetVolumeBackGround(float sliderValue)
    {
        audioMixer.SetFloat("BackgroundVolume", sliderValue);
    }

    public void SetVolumeEffects(float sliderValue)
    {
        audioMixer.SetFloat("SoundEffectsVolume", sliderValue);
    }

    private IEnumerator UpdateSong()
    {
        if (m_songData != null) // Get a new song from the List this mustn't be a song that is currently played and update the whole songNameText
        {
            Debug.Log("Song has been updated");

            int randomValue = Random.Range(0, m_songData.Length);

            while (m_songData[randomValue].song == backgroundAudio)
            {
                // If the randomValue is the same as the last songs position, the function will be restarted
                randomValue = Random.Range(0, m_songData.Length);
            }

            backgroundAudio = m_songData[randomValue].song;

            if (!backGroundAudioSource.isPlaying)
            {
                backGroundAudioSource.clip = backgroundAudio;
                backGroundAudioSource.PlayOneShot(backgroundAudio);
            }
        }

        yield return null;
    }
}
