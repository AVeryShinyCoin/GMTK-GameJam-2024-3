using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundVolumeSetting : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SfxVolume";
    public void UpdateMusicMixer()
    {
        float value = (musicSlider.value);
        if (value == 0)
        {
            value = 0.00001f;
        }
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20f);
    }
    public void UpdateSfxMixer()
    {
        float value = (sfxSlider.value);
        if (value == 0)
        {
            value = 0.00001f;
        }
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20f);
    }

    public void ToggleMusic(bool toggle)
    {
        if (!toggle)
        {
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(0.00001f) * 20f);
        }
        else
        {
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(1) * 20f);
        }
    }
}
