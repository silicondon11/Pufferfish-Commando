using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Numerics;

public class MenuManager : MonoBehaviour
{
    public int coins;
    public int xp;
    public int level;

    public float lerpDistance = 5.0f;
    public float lerpSpeed = 100.0f;

    private bool isLerpingRight = true;
    private float startX = 0f;

    public TextMeshProUGUI pfcText;
    public GameObject levelUpMessage;

    private MusicController musicController;


    void Start()
    {
        //PlayerPrefs.DeleteAll();//delete after testing

        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        if (PlayerPrefs.HasKey("Coins"))
        {
            //if needed
            coins = PlayerPrefs.GetInt("Coins");
        }
        else
        {
            PlayerPrefs.SetInt("Coins", 10000);
        }

        if (PlayerPrefs.HasKey("XP"))
        {
            //if needed
            xp = PlayerPrefs.GetInt("XP");
        }
        else
        {
            PlayerPrefs.SetInt("XP", 0);
        }

        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            PlayerPrefs.SetInt("Level", 16);
        }

        if (!PlayerPrefs.HasKey("CurrentAchs"))
        {
            PlayerPrefs.SetString("CurrentAchs", "0,1,2");
        }

        if (!PlayerPrefs.HasKey("TotalEnemyString"))
        {
            PlayerPrefs.SetString("TotalEnemyString", "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
        }

        if (!PlayerPrefs.HasKey("Accuracy"))
        {
            PlayerPrefs.SetFloat("Accuracy", 100f);
        }

        if (!PlayerPrefs.HasKey("GP"))
        {
            PlayerPrefs.SetInt("GP", 0);
        }

        if (!PlayerPrefs.HasKey("Kills"))
        {
            PlayerPrefs.SetInt("Kills", 0);
        }

        if (!PlayerPrefs.HasKey("MP"))
        {
            PlayerPrefs.SetInt("MP", 0);
        }

        if (!PlayerPrefs.HasKey("Points"))
        {
            PlayerPrefs.SetInt("Points", 0);
        }

        if (!PlayerPrefs.HasKey("PersonalBests"))
        {
            PlayerPrefs.SetString("PersonalBests", "0,0,0,0,0,0,0,0");
        }

        startX = pfcText.rectTransform.anchoredPosition.x;

        if (ScoreManager.newLevelFlag)
        {
            StartCoroutine(ShowLevelUp());
        }
    }

    void Update()
    {
        float targetX = isLerpingRight ? (startX + lerpDistance) : (startX - lerpDistance);

        float newX = Mathf.Lerp(pfcText.rectTransform.anchoredPosition.x, targetX, lerpSpeed * Time.deltaTime);

        pfcText.rectTransform.anchoredPosition = new UnityEngine.Vector2(newX, pfcText.rectTransform.anchoredPosition.y);

        if (MathF.Abs(targetX - pfcText.rectTransform.anchoredPosition.x) <= 0.1f)
        {
            isLerpingRight = !isLerpingRight;
        }
    }

    public void LoadGameScene()
    {
        StartCoroutine(GameSceneTransition());

        StartCoroutine(musicController.PlaySoundEffect("PanelClose"));

    }

    public void LoadInventory()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        SceneManager.LoadScene("InventoryScene");
    }

    public void LoadAwards()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        SceneManager.LoadScene("AwardsScene");
    }

    public void LoadSettings()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        SceneManager.LoadScene("SettingsScene");
    }

    public void StartTutorial()
    {
        TutorialManager tut = FindObjectOfType<TutorialManager>();

        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        tut.StartTutorial();

    }

    private IEnumerator ShowLevelUp()
    {
        levelUpMessage.SetActive(true);

        UnityEngine.Vector3 ogSize = levelUpMessage.transform.localScale;
        levelUpMessage.transform.localScale = levelUpMessage.transform.localScale * 0.002f;

        UnityEngine.Vector3 sizeInterval = levelUpMessage.transform.localScale;

        for (int i = 0; i < 500; i++)
        {
            levelUpMessage.transform.localScale += sizeInterval;

            yield return new WaitForSeconds(0.0001f);
        }
        levelUpMessage.transform.localScale = ogSize;

        //put in sound effect

    }

    public void DismissButton()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        levelUpMessage.SetActive(false);
    }

    private IEnumerator GameSceneTransition()
    {
        GameObject up = GameObject.Find("Canvas").transform.Find("UpPanel").gameObject;
        GameObject down = GameObject.Find("Canvas").transform.Find("DownPanel").gameObject;

        if (up == null|| down == null)
        {
            UnityEngine.Debug.LogError("ERROR: Transition panels not found.");
        }
        else
        {
            RectTransform upRect = up.GetComponent<RectTransform>();
            RectTransform downRect = down.GetComponent<RectTransform>();

            UnityEngine.Vector2 initUpPos = new UnityEngine.Vector2(upRect.anchoredPosition.x, upRect.anchoredPosition.y);
            UnityEngine.Vector2 initDownPos = new UnityEngine.Vector2(downRect.anchoredPosition.x, downRect.anchoredPosition.y);

            for (int i = 0; i < 871;)
            {
                if (i < 851)
                {
                    upRect.anchoredPosition = new UnityEngine.Vector2(initUpPos.x, initUpPos.y - i);
                }
                downRect.anchoredPosition = new UnityEngine.Vector2(initDownPos.x, initDownPos.y + i);

                i += 10;

                yield return new WaitForSeconds(0.001f);
            }


            SceneManager.LoadScene("GameScene");
        }

        yield return new WaitForSeconds(1f);
    }


}
