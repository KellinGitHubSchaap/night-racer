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

    private void Start()
    {
        if (PlayerPrefs.GetFloat("BackgroundVolume") != backGroundAudioSource.volume)
        {
            backGroundAudioSource.volume = PlayerPrefs.GetFloat("BackgroundVolume");
        }

        StartCoroutine(UpdateSong());
        backGroundAudioSource.volume = PlayerPrefs.GetFloat("BackgroundVolume");

    }

    private void Update()
    {
        if (!backGroundAudioSource.isPlaying)
        {
            StartCoroutine(UpdateSong());
        }

        if (PlayerPrefs.GetFloat("MusicVolume") != backGroundAudioSource.volume)
        {
            backGroundAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        }
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
