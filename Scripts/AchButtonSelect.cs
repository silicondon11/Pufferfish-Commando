using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using System.Linq;
using UnityEngine.SceneManagement;

public class AchButtonSelect : MonoBehaviour
{
    public GameObject statsButton;
    public GameObject achsButton;

    private Image statsButtonImage;
    private Image achsButtonImage;

    public Sprite statsSelectedSprite;
    public Sprite achsSelectedSprite;

    private Sprite statsNormalSprite;
    private Sprite achsNormalSprite;

    private bool isSelected = true;

    private List<Achievement> achievements;
    private int[] currentAchs = new int[3];
    public Transform contentPanel;
    public GameObject achievementImagePrefab;
    public GameObject odometerPrefab;
    public GameObject enemyGraphPrefab;
    public GameObject pbPrefab;
    private float enemyImageHeight = 80f;

    private MusicController musicController;


    void Start()
    {
        statsButtonImage = statsButton.GetComponent<Image>();
        statsNormalSprite = statsButtonImage.sprite;

        achsButtonImage = achsButton.GetComponent<Image>();
        achsNormalSprite = achsButtonImage.sprite;

        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        achievements = new List<Achievement>();
        Achievement[] loadedAchievements = Resources.LoadAll<Achievement>("Achievements");
        achievements.AddRange(loadedAchievements);

        if (PlayerPrefs.HasKey("CurrentAchs"))
        {
            string caString = PlayerPrefs.GetString("CurrentAchs");
            string[] caStrings = caString.Split(',');
            currentAchs[0] = int.Parse(caStrings[0].ToString());
            currentAchs[1] = int.Parse(caStrings[1].ToString());
            currentAchs[2] = int.Parse(caStrings[2].ToString());
        }

        ToggleButton();
    }

    public void ToggleButton()
    {
        isSelected = !isSelected;

        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        if (isSelected)
        {
            achsButtonImage.sprite = achsSelectedSprite;
            statsButtonImage.sprite = statsNormalSprite;

            AchievementsScrollView();
        }
        else
        {
            statsButtonImage.sprite = statsSelectedSprite;
            achsButtonImage.sprite = achsNormalSprite;

            StatsScrollView();
        }
    }

    private void AchievementsScrollView()
    {
        ClearScrollView();

        for (int i = 0; i < achievements.Count; i++)
        {
            if (achievements[i].progress >= 1f || i <= currentAchs.Max())
            {
                GameObject achievementImageObject = Instantiate(achievementImagePrefab, contentPanel.transform);
                //achievementImageObject.transform.SetParent(contentPanel, true);

                TextMeshProUGUI title = achievementImageObject.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                title.text = achievements[i].aName;

                TextMeshProUGUI desc = achievementImageObject.transform.Find("Desc").GetComponent<TextMeshProUGUI>();
                desc.text = achievements[i].description;

                TextMeshProUGUI coins = achievementImageObject.transform.Find("Coins").GetComponent<TextMeshProUGUI>();
                coins.text = achievements[i].coinsReward.ToString();

                TextMeshProUGUI xp = achievementImageObject.transform.Find("XP").GetComponent<TextMeshProUGUI>();
                xp.text = achievements[i].xpReward.ToString();
            }
            else
            {
                GameObject achievementImageObject = Instantiate(achievementImagePrefab, contentPanel.transform);

                TextMeshProUGUI title = achievementImageObject.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                title.text = "???";

                TextMeshProUGUI desc = achievementImageObject.transform.Find("Desc").GetComponent<TextMeshProUGUI>();
                desc.text = "??????";

                TextMeshProUGUI coins = achievementImageObject.transform.Find("Coins").GetComponent<TextMeshProUGUI>();
                coins.text = "???";

                TextMeshProUGUI xp = achievementImageObject.transform.Find("XP").GetComponent<TextMeshProUGUI>();
                xp.text = "???";
            }
            
        }

        float totalHeight = contentPanel.childCount * 93f;
        RectTransform contentRectTransform = contentPanel.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new UnityEngine.Vector2(contentRectTransform.sizeDelta.x, totalHeight);
    }

    private void StatsScrollView()
    {
        ClearScrollView();

        GameObject odometerImage = Instantiate(odometerPrefab, contentPanel.transform);

        TextMeshProUGUI accText = odometerImage.transform.Find("Accuracy").GetComponent<TextMeshProUGUI>();
        float tempAcc = PlayerPrefs.GetFloat("Accuracy");
        if (tempAcc >= 100f)
        {
            accText.text = "00";
        }
        else
        {
            accText.text = PlayerPrefs.GetFloat("Accuracy").ToString();
        }
        

        TextMeshProUGUI gpText = odometerImage.transform.Find("GamesPlayed").GetComponent<TextMeshProUGUI>();
        gpText.text = PlayerPrefs.GetInt("GP").ToString();

        TextMeshProUGUI killsText = odometerImage.transform.Find("Kills").GetComponent<TextMeshProUGUI>();
        killsText.text = PlayerPrefs.GetInt("Kills").ToString();

        TextMeshProUGUI mpText = odometerImage.transform.Find("MinsPlayed").GetComponent<TextMeshProUGUI>();
        mpText.text = PlayerPrefs.GetInt("MP").ToString();

        TextMeshProUGUI ptsText = odometerImage.transform.Find("Points").GetComponent<TextMeshProUGUI>();
        ptsText.text = PlayerPrefs.GetInt("Points").ToString();

        string pString = ((float)PlayerPrefs.GetInt("Points") / (float)PlayerPrefs.GetInt("GP")).ToString();
        string[] pStringParts = pString.Split('.');
        TextMeshProUGUI ppgText = odometerImage.transform.Find("PPG1").GetComponent<TextMeshProUGUI>();
        ppgText.text = pStringParts[0];
        TextMeshProUGUI ppgdText = odometerImage.transform.Find("PPG2").GetComponent<TextMeshProUGUI>();
        if (pStringParts.Length > 1)
        {
            ppgdText.text = pStringParts[1][0].ToString();
        }
        else
        {
            ppgdText.text = "0";
        }

        string kString = ((float)PlayerPrefs.GetInt("Kills") / (float)PlayerPrefs.GetInt("MP")).ToString();
        string[] kStringParts = kString.Split('.');
        TextMeshProUGUI kpmText = odometerImage.transform.Find("KPM1").GetComponent<TextMeshProUGUI>();
        kpmText.text = kStringParts[0];
        TextMeshProUGUI kpmdText = odometerImage.transform.Find("KPM2").GetComponent<TextMeshProUGUI>();
        if (kStringParts.Length > 1)
        {
            kpmdText.text = kStringParts[1][0].ToString();
        }
        else
        {
            kpmdText.text = "0";
        }
        


        List<Tuple<string, int, Color>> enemyUsage = new List<Tuple<string, int, Color>>
        {
            Tuple.Create("Crabs", 0, Color.red),
            Tuple.Create("Manta Rays", 0, Color.blue),
            Tuple.Create("Turtles", 0, Color.green),
            Tuple.Create("Sea Horses", 0, Color.cyan),
            Tuple.Create("Lobsters", 0, Color.red),
            Tuple.Create("Anglerfish", 0, Color.yellow),
            Tuple.Create("Octopi", 0, Color.magenta),
            Tuple.Create("Archerfish", 0, Color.gray),
            Tuple.Create("Jellyfish", 0, Color.magenta),
            Tuple.Create("Sawfish", 0, Color.yellow),
            Tuple.Create("Belugas", 0, Color.white),
            Tuple.Create("Hammerheads", 0, Color.gray),
            Tuple.Create("Electric Eels", 0, Color.green),
            Tuple.Create("Makos", 0, Color.gray),
            Tuple.Create("Blue Whales", 0, Color.blue),
            Tuple.Create("Great Whites", 0, Color.white),
            Tuple.Create("Orcas", 0, Color.black),
            Tuple.Create("Seals", 0, Color.gray),
        };

        if (PlayerPrefs.HasKey("TotalEnemyString"))
        {
            string totalEnemies = PlayerPrefs.GetString("TotalEnemyString");
            string[] teStrings = totalEnemies.Split(',');

            for (int i = 0; i < teStrings.Length; i++)
            {
                int intValue = int.Parse(teStrings[i]);

                Tuple<string, int, Color> updatedTuple = Tuple.Create(enemyUsage[i].Item1, intValue, enemyUsage[i].Item3);

                enemyUsage[i] = updatedTuple;
            }
        }

        int totalSum = enemyUsage.Sum(tuple => tuple.Item2);

        GameObject enemyGraphImage = Instantiate(enemyGraphPrefab, contentPanel.transform);

        enemyUsage.Sort((tuple1, tuple2) => tuple2.Item2.CompareTo(tuple1.Item2));

        TextMeshProUGUI text1 = enemyGraphImage.transform.Find("Text1").GetComponent<TextMeshProUGUI>();
        text1.text = enemyUsage[0].Item1;
        text1.color = enemyUsage[0].Item3;
        TextMeshProUGUI num1 = enemyGraphImage.transform.Find("Num1").GetComponent<TextMeshProUGUI>();
        num1.text = enemyUsage[0].Item2.ToString();
        Image image1 = enemyGraphImage.transform.Find("Image1").GetComponent<Image>();
        image1.rectTransform.sizeDelta = new UnityEngine.Vector2(image1.rectTransform.sizeDelta.x, ((float)enemyUsage[0].Item2 / (float)totalSum) * enemyImageHeight);
        image1.color = enemyUsage[0].Item3;


        TextMeshProUGUI text2 = enemyGraphImage.transform.Find("Text2").GetComponent<TextMeshProUGUI>();
        text2.text = enemyUsage[1].Item1;
        text2.color = enemyUsage[1].Item3;
        TextMeshProUGUI num2 = enemyGraphImage.transform.Find("Num2").GetComponent<TextMeshProUGUI>();
        num2.text = enemyUsage[1].Item2.ToString();
        Image image2 = enemyGraphImage.transform.Find("Image2").GetComponent<Image>();
        image2.rectTransform.sizeDelta = new UnityEngine.Vector2(image2.rectTransform.sizeDelta.x, ((float)enemyUsage[1].Item2 / (float)totalSum) * enemyImageHeight);
        image2.color = enemyUsage[1].Item3;

        TextMeshProUGUI text3 = enemyGraphImage.transform.Find("Text3").GetComponent<TextMeshProUGUI>();
        text3.text = enemyUsage[2].Item1;
        text3.color = enemyUsage[2].Item3;
        TextMeshProUGUI num3 = enemyGraphImage.transform.Find("Num3").GetComponent<TextMeshProUGUI>();
        num3.text = enemyUsage[2].Item2.ToString();
        Image image3 = enemyGraphImage.transform.Find("Image3").GetComponent<Image>();
        image3.rectTransform.sizeDelta = new UnityEngine.Vector2(image3.rectTransform.sizeDelta.x, ((float)enemyUsage[2].Item2 / (float)totalSum) * enemyImageHeight);
        image3.color = enemyUsage[2].Item3;

        TextMeshProUGUI text4 = enemyGraphImage.transform.Find("Text4").GetComponent<TextMeshProUGUI>();
        text4.text = enemyUsage[3].Item1;
        text4.color = enemyUsage[3].Item3;
        TextMeshProUGUI num4 = enemyGraphImage.transform.Find("Num4").GetComponent<TextMeshProUGUI>();
        num4.text = enemyUsage[3].Item2.ToString();
        Image image4 = enemyGraphImage.transform.Find("Image4").GetComponent<Image>();
        image4.rectTransform.sizeDelta = new UnityEngine.Vector2(image4.rectTransform.sizeDelta.x, ((float)enemyUsage[3].Item2 / (float)totalSum) * enemyImageHeight);
        image4.color = enemyUsage[3].Item3;

        TextMeshProUGUI text5 = enemyGraphImage.transform.Find("Text5").GetComponent<TextMeshProUGUI>();
        text5.text = enemyUsage[4].Item1;
        text5.color = enemyUsage[4].Item3;
        TextMeshProUGUI num5 = enemyGraphImage.transform.Find("Num5").GetComponent<TextMeshProUGUI>();
        num5.text = enemyUsage[4].Item2.ToString();
        Image image5 = enemyGraphImage.transform.Find("Image5").GetComponent<Image>();
        image5.rectTransform.sizeDelta = new UnityEngine.Vector2(image5.rectTransform.sizeDelta.x, ((float)enemyUsage[4].Item2 / (float)totalSum) * enemyImageHeight);
        image5.color = enemyUsage[4].Item3;



        GameObject personalBestsImage = Instantiate(pbPrefab, contentPanel.transform);
        string pbString = PlayerPrefs.GetString("PersonalBests");
        string[] pbStrings = pbString.Split(',');

        TextMeshProUGUI kills = personalBestsImage.transform.Find("Kills").GetComponent<TextMeshProUGUI>();
        kills.text = pbStrings[0];

        TextMeshProUGUI score = personalBestsImage.transform.Find("Score").GetComponent<TextMeshProUGUI>();
        score.text = pbStrings[1];

        TextMeshProUGUI accuracy = personalBestsImage.transform.Find("Accuracy").GetComponent<TextMeshProUGUI>();
        accuracy.text = pbStrings[2];

        TextMeshProUGUI kps = personalBestsImage.transform.Find("Kpm").GetComponent<TextMeshProUGUI>();
        kps.text = pbStrings[3];

        TextMeshProUGUI eKills = personalBestsImage.transform.Find("EKills").GetComponent<TextMeshProUGUI>();
        eKills.text = pbStrings[4];

        TextMeshProUGUI ppg = personalBestsImage.transform.Find("Ppg").GetComponent<TextMeshProUGUI>();
        ppg.text = pbStrings[5];

        TextMeshProUGUI highAccuracy = personalBestsImage.transform.Find("HighAccuracy").GetComponent<TextMeshProUGUI>();
        highAccuracy.text = pbStrings[6];

        TextMeshProUGUI kpm = personalBestsImage.transform.Find("HighKpm").GetComponent<TextMeshProUGUI>();
        kpm.text = pbStrings[7];


        float totalHeight = 640f;
        RectTransform contentRectTransform = contentPanel.GetComponent<RectTransform>();
        contentRectTransform.sizeDelta = new UnityEngine.Vector2(contentRectTransform.sizeDelta.x, totalHeight);
    }

    private void ClearScrollView()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    public void Back()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        SceneManager.LoadScene("MainMenu");
    }

}
