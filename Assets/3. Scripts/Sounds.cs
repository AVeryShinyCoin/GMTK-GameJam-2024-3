using UnityEngine;

[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip clip;

    [Range(0f, 10f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;

    [HideInInspector]
    public float volumeChangeTimeTarget;
    [HideInInspector]
    public float volumeChangeTimeRemaining;
    [HideInInspector]
    public float volumeChangeNewTarget;
    [HideInInspector]
    public float volumeChangeOriginal;

}
