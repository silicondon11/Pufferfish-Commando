using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using System;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject settingsScreen;
    public GameObject exitPrompt;

    public AudioMixer mixer;
    private Slider musicSlider;
    private Slider sfxSlider;
    private float musicOut;
    private float sfxOut;

    private bool isPaused = false;
    private float progressScale = 155f;

    private MusicController musicController;

    private void Start()
    {
        musicSlider = settingsScreen.transform.Find("Music").GetComponent<Slider>();
        sfxSlider = settingsScreen.transform.Find("SFX").GetComponent<Slider>();

        mixer.GetFloat("musicVolume", out musicOut);
        musicSlider.value = (float)Math.Pow(10, (musicOut / 20f));

        mixer.GetFloat("sfxVolume", out sfxOut);
        sfxSlider.value = (float)Math.Pow(10, (sfxOut / 20f));

        musicController = GameObject.Find("Audio").GetComponent<MusicController>();
    }

    public void TogglePauseResume()
    {
        isPaused = !isPaused;

        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);

            Achievement achA = transform.gameObject.GetComponent<AchievementManager>().currentAchievementA;
            Achievement achB = transform.gameObject.GetComponent<AchievementManager>().currentAchievementB;
            Achievement achC = transform.gameObject.GetComponent<AchievementManager>().currentAchievementC;

            Transform slotA = pauseScreen.transform.Find("AchievementA");
            slotA.Find("Title").GetComponent<TextMeshProUGUI>().text = achA.aName;
            slotA.Find("Description").GetComponent<TextMeshProUGUI>().text = achA.description;
            slotA.Find("Coins").GetComponent<TextMeshProUGUI>().text = achA.coinsReward.ToString();
            slotA.Find("XP").GetComponent<TextMeshProUGUI>().text = achA.xpReward.ToString();
            RectTransform rectA = slotA.Find("Progress").GetComponent<RectTransform>();
            rectA.sizeDelta = new UnityEngine.Vector2(achA.progress * progressScale, rectA.sizeDelta.y);

            Transform slotB = pauseScreen.transform.Find("AchievementB");
            slotB.Find("Title").GetComponent<TextMeshProUGUI>().text = achB.aName;
            slotB.Find("Description").GetComponent<TextMeshProUGUI>().text = achB.description;
            slotB.Find("Coins").GetComponent<TextMeshProUGUI>().text = achB.coinsReward.ToString();
            slotB.Find("XP").GetComponent<TextMeshProUGUI>().text = achB.xpReward.ToString();
            RectTransform rectB = slotB.Find("Progress").GetComponent<RectTransform>();
            rectB.sizeDelta = new UnityEngine.Vector2(achB.progress * progressScale, rectB.sizeDelta.y);


            Transform slotC = pauseScreen.transform.Find("AchievementC");
            slotC.Find("Title").GetComponent<TextMeshProUGUI>().text = achC.aName;
            slotC.Find("Description").GetComponent<TextMeshProUGUI>().text = achC.description;
            slotC.Find("Coins").GetComponent<TextMeshProUGUI>().text = achC.coinsReward.ToString();
            slotC.Find("XP").GetComponent<TextMeshProUGUI>().text = achC.xpReward.ToString();
            RectTransform rectC = slotC.Find("Progress").GetComponent<RectTransform>();
            rectC.sizeDelta = new UnityEngine.Vector2(achC.progress * progressScale, rectC.sizeDelta.y);
        }
        else
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }
    }

    public void ResumeButton()
    {
        isPaused = true;
        TogglePauseResume();
    }

    public void SettingsButton()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20f);
    }

    public void BackButton()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        settingsScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }

    public void QuitButton()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        exitPrompt.SetActive(true);
    }

    public void CancelExitGame()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        exitPrompt.SetActive(false);
    }

    public void ExitGame()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RetryButton()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void ItemsButton()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        Time.timeScale = 1f;
        SceneManager.LoadScene("InventoryScene");
    }
}
