using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderAssign : MonoBehaviour
{
    [SerializeField] bool assignMusicSlider;
    [SerializeField] bool assignSfxSlider;

    private void OnEnable()
    {
        SoundVolumeSetting settings = FindAnyObjectByType<SoundVolumeSetting>();
        if (settings != null)
        {
            if (assignMusicSlider)
            {
                settings.musicSlider = GetComponent<Slider>();
            }
            else if (assignSfxSlider)
            {
                settings.sfxSlider = GetComponent<Slider>();
            }
        }
        else
        {
            Debug.Log("triggered");
        }
    }
}
