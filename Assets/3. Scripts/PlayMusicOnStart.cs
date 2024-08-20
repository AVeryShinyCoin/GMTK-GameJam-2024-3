using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnStart : MonoBehaviour
{
    [SerializeField] string music;

    private void Start()
    {
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayMusic(music);
    }
}
