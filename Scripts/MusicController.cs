using System;
using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioMixer audioMixer;

    private static MusicController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.loop = false;
        musicSource.Play();
        musicSource.SetScheduledEndTime(AudioSettings.dspTime + musicSource.clip.length - 2f);
    }

    void Update()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.PlayScheduled(AudioSettings.dspTime + 0.1);
        }
    }

    public IEnumerator PlaySoundEffect(string soundFileName)
    {
        AudioClip soundClip = Resources.Load<AudioClip>("Sound FX/" + soundFileName);

        if (soundClip != null)
        {
            GameObject soundObject = new GameObject("SoundObject");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();

            audioSource.clip = soundClip;
            audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];

            audioMixer.GetFloat("sfxVolume", out float sfxVolume);
            audioSource.volume = Mathf.Pow(10f, sfxVolume / 20f);

            audioSource.Play();

            Destroy(soundObject, soundClip.length);
        }
        else
        {
            UnityEngine.Debug.LogError("Sound effect not found: " + soundFileName);
        }

        yield return null;
    }
}

