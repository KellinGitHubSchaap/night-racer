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

        if (isBackGroundSlider)
            slider.value = PlayerPrefs.GetFloat("BackGroundSliderValue");
        else
            slider.value = PlayerPrefs.GetFloat("SoundEffectsSliderValue");
    }

    /// <summary>
    /// This updates the current audio slider value
    /// </summary>
    public void UpdateSoundVolume()
    {
        if (isBackGroundSlider)
            PlayerPrefs.SetFloat("BackGroundSliderValue", slider.value);
        else
            PlayerPrefs.SetFloat("SoundEffectsSliderValue", slider.value);
    }
}
