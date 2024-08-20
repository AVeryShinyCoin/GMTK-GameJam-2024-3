using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    // "Sounds" is a custom class that contain information about a soundclip
    //Information in Sounds[] array is populated in Awake().
    public Sounds[] sounds;
    public static SoundManager Instance;

    [SerializeField] AudioMixerGroup musicMixer;
    [SerializeField] AudioMixerGroup sfxMixer;

    private List<string> queuedPitchReset = new List<string>();

    private List<Sounds> soundChangeOverTime = new List<Sounds>();
    private bool soundChangeEnabled;

    private bool soundOverride;
    public List<string> overrides = new List<string>();

    void Awake()
    {
        // This ensures the AudioManager is only loaded once and isn't destroyed between scenes.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // This populates the class:Sounds array at the start of the game
        // Variables can be customized in the inspector
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            if (s.name.StartsWith("bgm"))
            {
                s.source.outputAudioMixerGroup = musicMixer;
            }
            else
            {
                s.source.outputAudioMixerGroup = sfxMixer;
            }
        }
    }

    //All methods that play sound uses FindSound() to convert the requested string into the Sound to play
    private Sounds FindSound(string name)
    {
        Sounds s = Array.Find(sounds, sounds => sounds.name == name);
        if (s.source == null)
        {
            Debug.LogWarning("Could not find Sound with name " + name);
            return null;
        }
        return s;
    }

    // Plays the sound as defined in the Sounds[] array
    // method needs the name of the sound saved in the inspector
    public void PlaySound (string name)
    {
        Sounds s = FindSound(name);
        if (s.source == null)
        {
            return;
        }

        if (s.source.isPlaying && s.source.loop)
        {
            return;
        }

        s.source.Stop();
        s.source.pitch = s.pitch;
        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sounds s = FindSound(name);
        if (s.source == null)
        {
            return;
        }
        s.source.Stop();
    }

    public void StopMusic()
    {
        foreach (Sounds s in sounds)
        {
            if (s.name.StartsWith("bgm"))
            {
                s.source.Stop();
            }
        }
    }

    // Plays sound with custom pitch
    public void PlaySoundPitch(string name, float newPitch)
    {
        Sounds s = FindSound(name);
        if (s.source == null)
        {
            return;
        }
        
        s.source.pitch = newPitch;
        s.source.Play();
    }

    // Plays sound with random pitch
    public void PlaySoundRandomPitch(string name, float low, float high)
    {
        Sounds s = FindSound(name);
        if (s.source == null)
        {
            return;
        }

        float rndPitch = UnityEngine.Random.Range(low, high);
        s.source.pitch = rndPitch;
        s.source.Play();
    }

    // changes volume of sound. NOTE: doesn't play sound
    public void ChangeSoundVolume(string name, float volume)
    {
        Sounds s = FindSound(name);
        if (s.source == null)
        {
            return;
        }
        if (overrides.Contains(name))
        {
            return;
        }
        s.source.volume = volume;
    }

    // Plays sound using the Music Volume instead of SFX volume
    public void PlayMusic(string name)
    {
        Sounds s = FindSound(name);
        if (s.source == null)
        {
            return;
        }
        s.source.Stop();
        s.source.pitch = s.pitch;
        s.source.Play();
    }

    // changes volume of sound over a number of seconds NOTE: doesn't play sound
    public void ChangeSoundVolumeOverTime(string name, float volume, float seconds)
    {
        Sounds s = FindSound(name);
        if (s.source == null)
        {
            return;
        }

        if (overrides.Contains(name))
        {
            return;
        }

        if (!soundChangeOverTime.Contains(s))
        {
            soundChangeOverTime.Add(s);
        }
        s.volumeChangeTimeTarget = seconds;
        s.volumeChangeTimeRemaining = seconds;
        s.volumeChangeNewTarget = s.volume * volume;
        s.volumeChangeOriginal = s.source.volume;
        soundChangeEnabled = true;

        if (soundOverride == true)
        {
            soundOverride = false;
            overrides.Add(name);
        }
        // incremental sound change happens in FixedUpdate() further below
    }

    // enabling Override makes it so that the sound doesn't change by other sources unless first cleared in the list
    public void ChangeSoundVolumeOverTime(string name, float volume, float seconds, bool enableOverride)
    {
        soundOverride = true;
        ChangeSoundVolumeOverTime(name, volume, seconds);
    }

    private void FixedUpdate()
    {
        if (soundChangeEnabled == true)
        {
            foreach (Sounds s in soundChangeOverTime)
            {
                s.volumeChangeTimeRemaining = s.volumeChangeTimeRemaining - Time.deltaTime;
                float volumeDifference = s.volumeChangeNewTarget - s.volumeChangeOriginal;
                float timeRatio = s.volumeChangeTimeRemaining / s.volumeChangeTimeTarget;
                s.source.volume = s.volumeChangeOriginal + ((volumeDifference) * (1 - (timeRatio)));

                if (s.volumeChangeTimeRemaining <= 0)
                {
                    s.source.volume = s.volumeChangeNewTarget;
                    soundChangeOverTime.Remove(s);
                    if (soundChangeOverTime.Count == 0)
                    {
                        soundChangeEnabled = false;
                        return;
                    }
                }
            }
        }
    }

    // **DISABLED**
    IEnumerator QueueResetPitch(Sounds s)
    {
        queuedPitchReset.Add(s.name);
        // waitTime is calculated to adjust time waited with the length distortion caused by changing pitch.
        float waitTime = (s.clip.length / Math.Abs(s.source.pitch));
        yield return new WaitForSeconds(waitTime);

        s.source.pitch = s.pitch;
        for (int i = 0; i < queuedPitchReset.Count; i++)
        {
            if (queuedPitchReset.Contains(name) == true)
            {
                queuedPitchReset.Remove(name);
            }
            else
            {
                break;
            }
        }
    }
}


