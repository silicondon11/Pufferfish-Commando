using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;
    private float musicOut;
    public Slider sfxSlider;
    private float sfxOut;

    private MusicController musicController;


    void Start()
    {
        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        mixer.GetFloat("musicVolume", out musicOut);
        musicSlider.value = (float)Math.Pow(10, (musicOut / 20f));

        mixer.GetFloat("sfxVolume", out sfxOut);
        sfxSlider.value = (float)Math.Pow(10, (sfxOut / 20f));

    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);

    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20f);
    }

    public void Back()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        SceneManager.LoadScene("MainMenu");
    }
}
