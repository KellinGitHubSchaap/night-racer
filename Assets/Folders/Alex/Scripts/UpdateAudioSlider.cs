using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UpdateAudioSlider : MonoBehaviour
{
    [SerializeField] private bool isBackGroundSlider;

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        //if (isBackGroundSlider)
        //    slider.value = PlayerPrefs.GetFloat("BackgroundVolume");
        //else
        //    slider.value = PlayerPrefs.GetFloat("BackgroundVolume");
    }

    /// <summary>
    /// This updates the current audio slider value
    /// </summary>
    public void UpdateSoundVolume()
    {
        if (isBackGroundSlider)
            PlayerPrefs.SetFloat("MusicVolume", slider.value);
        else
            PlayerPrefs.SetFloat("BackgroundVolume", slider.value);
    }
}
