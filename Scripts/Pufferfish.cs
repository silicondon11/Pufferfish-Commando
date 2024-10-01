using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Numerics;

public class Pufferfish : MonoBehaviour
{
    public int health = 1000;
    private int healthChange;
    private int healthBack;
    private float regenDelay;

    public GameObject healthBar;
    private Transform fill;
    private Transform backFill;
    private bool healthBarShown = false;

    float accHolder = 0f;

    public GameObject gameOverScreen;

    public GameObject xpGlow;
    public GameObject highScoreGlow;

    private Coroutine healthCoroutine;
    private Color initColor;

    public bool untouchFlag = true;
    private bool gameOverFlag = false;
    private bool pbFlag = false;
    private bool hsFlag = false;

    private int topEnemyKills = 0;

    public GameObject killsPB;
    public GameObject scorePB;
    public GameObject accuracyPB;
    public GameObject kpmPB;

    private ScoreManager scoreManager;
    private Renderer mRenderer;

    private MusicController musicController;
    private bool soundFlag = true;


    void Start()
    {
        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        fill = healthBar.transform.Find("Fill");
        backFill = healthBar.transform.Find("BackFill");
        healthChange = health;
        healthBar.SetActive(false);

        untouchFlag = true;

        mRenderer = GetComponent<Renderer>();

        initColor = mRenderer.material.color;
    }

    void Update()
    {
        if ((Time.timeScale >= 1f) && !gameOverFlag)
        {
            if (health < healthChange)
            {
                healthChange = health;
                healthBack = Mathf.Min(health + 100, 1000);

                untouchFlag = false;

                if (healthBarShown)
                {
                    // If yes, stop the previous instance
                    StopCoroutine(healthCoroutine);
                }

                healthCoroutine = StartCoroutine(ShowHealthBar());
            }

            if (health <= 0)
            {
                StartCoroutine(musicController.PlaySoundEffect("GameOver"));
                EndGame();
            }

            if (health < healthBack && regenDelay >= 2f)
            {
                health += 1;
                regenDelay = 0f;
            }
            regenDelay += 1f;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hitBy = collision.gameObject;

        if (hitBy.name == "Bullet(Clone)")
        {
            health -= 25;
            Destroy(hitBy);
        }
        else if (hitBy.name == "Poison(Clone)")
        {
            StartCoroutine(PoisonHit());
            Destroy(hitBy);
        }
        else if (hitBy.name == "Explosion(Clone)")
        {
            health -= 100;
            
        }
        else if (hitBy.name == "Rocket(Clone)")
        {
            health -= 200;

        }
        else if (hitBy.CompareTag("Target"))
        {
            UnityEngine.Vector3 vector = (hitBy.transform.position - transform.position).normalized;
            hitBy.GetComponent<Rigidbody>().AddForce(vector * 10f);
        }
        //...
    }

    private IEnumerator PoisonHit()
    {
        for (int i = 0; i < 4; i++)
        {
            health -= 18;

            mRenderer.material.color = Color.magenta;

            yield return new WaitForSeconds(0.25f);
            
            mRenderer.material.color = initColor;

            yield return new WaitForSeconds(0.75f);
        }
    }

    private IEnumerator ShowHealthBar()
    {
        healthBar.SetActive(true);
        healthBarShown = true;

        RectTransform fillRect = fill.GetComponent<RectTransform>();
        RectTransform backRect = backFill.GetComponent<RectTransform>();

        if (soundFlag)
        {
            soundFlag = false;
            StartCoroutine(musicController.PlaySoundEffect("PufferfishUgh"));
        }

        for (int i = 0; i < 200; i++)
        {
            backRect.sizeDelta = new UnityEngine.Vector2(((float)healthBack / 1000f) * 500f, fillRect.sizeDelta.y);
            fillRect.sizeDelta = new UnityEngine.Vector2(((float)health / 1000f) * 500f, fillRect.sizeDelta.y);

            yield return new WaitForSecondsRealtime(0.05f);
        }

        soundFlag = true;

        healthBar.SetActive(false);
        healthBarShown = false;
    }

    private void EndGame()
    {
        Time.timeScale = 0f;
        gameOverFlag = true;

        gameOverScreen.SetActive(true);
        TextMeshProUGUI killsText = gameOverScreen.transform.Find("Kills").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI accText = gameOverScreen.transform.Find("Accuracy").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI kpsText = gameOverScreen.transform.Find("KPS").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI coinsText = gameOverScreen.transform.Find("Coins").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI xpText = gameOverScreen.transform.Find("XP").GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI gpText = gameOverScreen.transform.Find("GoldPent").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI gppText = gameOverScreen.transform.Find("GoldPentPct").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI spText = gameOverScreen.transform.Find("SilverPent").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI sppText = gameOverScreen.transform.Find("SilverPentPct").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bpText = gameOverScreen.transform.Find("BronzePent").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bppText = gameOverScreen.transform.Find("BronzePentPct").GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI geText = gameOverScreen.transform.Find("GoldEnemy").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI gepText = gameOverScreen.transform.Find("GoldEnemyPct").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI seText = gameOverScreen.transform.Find("SilverEnemy").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI sepText = gameOverScreen.transform.Find("SilverEnemyPct").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI beText = gameOverScreen.transform.Find("BronzeEnemy").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI bepText = gameOverScreen.transform.Find("BronzeEnemyPct").GetComponent<TextMeshProUGUI>();

        scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();


        killsText.text = scoreManager.kills.ToString();
        accHolder = MathF.Min((float)System.Math.Round(((float)scoreManager.hits / (float)scoreManager.shots) * 100f, 1), 100f);
        accText.text = accHolder.ToString() + "%";
        kpsText.text = ((float)System.Math.Round((float)scoreManager.kills / (float)Mathf.Max(scoreManager.minuteCounter, 1), 1)).ToString();
        coinsText.text = scoreManager.coinsGain.ToString();
        xpText.text = scoreManager.xpGain.ToString();

        List<Tuple<string, float>> pentUsage = new List<Tuple<string, float>>
        {
            Tuple.Create("Fishing Hook", (float)scoreManager.hook / (float)scoreManager.totalCols),
            Tuple.Create("Spring", (float)scoreManager.spring / (float)scoreManager.totalCols),
            Tuple.Create("Tomahawks", (float)scoreManager.tomahawk / (float)scoreManager.totalCols),
            Tuple.Create("Turret", (float)scoreManager.turret / (float)scoreManager.totalCols),
            Tuple.Create("Turbine", (float)scoreManager.turbine / (float)scoreManager.totalCols),
            Tuple.Create("Bubbles", (float)scoreManager.bubble / (float)scoreManager.totalCols),
            Tuple.Create("Nets", (float)scoreManager.net / (float)scoreManager.totalCols),
            Tuple.Create("Rockets", (float)scoreManager.rocket / (float)scoreManager.totalCols),
            Tuple.Create("Harpoon", (float)scoreManager.harpoon / (float)scoreManager.totalCols),
            Tuple.Create("Umbrella", (float)scoreManager.umbrella / (float)scoreManager.totalCols),
            Tuple.Create("Poison", (float)scoreManager.poison / (float)scoreManager.totalCols),
            Tuple.Create("Heatseekers", (float)scoreManager.heatseeker / (float)scoreManager.totalCols),
            Tuple.Create("Flares", (float)scoreManager.flare / (float)scoreManager.totalCols),
            Tuple.Create("EMWG", (float)scoreManager.emw / (float)scoreManager.totalCols),
            Tuple.Create("Hypnotizer", (float)scoreManager.hypno / (float)scoreManager.totalCols),
            Tuple.Create("Seamines", (float)scoreManager.seamine / (float)scoreManager.totalCols),
        };

        pentUsage.Sort((tuple1, tuple2) => tuple2.Item2.CompareTo(tuple1.Item2));

        gpText.text = pentUsage[0].Item1;
        gppText.text = ((float)System.Math.Round(pentUsage[0].Item2 * 100f, 1)).ToString() + "%";
        spText.text = pentUsage[1].Item1;
        sppText.text = ((float)System.Math.Round(pentUsage[1].Item2 * 100f, 1)).ToString() + "%";
        bpText.text = pentUsage[2].Item1;
        bppText.text = ((float)System.Math.Round(pentUsage[2].Item2 * 100f, 1)).ToString() + "%";


        List<Tuple<string, float>> enemyUsage = new List<Tuple<string, float>>
        {
            Tuple.Create("Crabs", (float)scoreManager.crabs / (float)scoreManager.kills),
            Tuple.Create("Manta Rays", (float)scoreManager.mantas / (float)scoreManager.kills),
            Tuple.Create("Turtles", (float)scoreManager.turtles / (float)scoreManager.kills),
            Tuple.Create("Sea Horses", (float)scoreManager.seahorses / (float)scoreManager.kills),
            Tuple.Create("Lobsters", (float)scoreManager.lobsters / (float)scoreManager.kills),
            Tuple.Create("Anglerfish", (float)scoreManager.anglerfish / (float)scoreManager.kills),
            Tuple.Create("Octopi", (float)scoreManager.octopi / (float)scoreManager.kills),
            Tuple.Create("Archerfish", (float)scoreManager.archerfish / (float)scoreManager.kills),
            Tuple.Create("Jellyfish", (float)scoreManager.jellyfish / (float)scoreManager.kills),
            Tuple.Create("Sawfish", (float)scoreManager.sawfish / (float)scoreManager.kills),
            Tuple.Create("Beluga Whales", (float)scoreManager.belugas / (float)scoreManager.kills),
            Tuple.Create("Hammerhead Sharks", (float)scoreManager.hammerheads / (float)scoreManager.kills),
            Tuple.Create("Electric Eels", (float)scoreManager.eels / (float)scoreManager.kills),
            Tuple.Create("Mako Sharks", (float)scoreManager.makos / (float)scoreManager.kills),
            Tuple.Create("Blue Whales", (float)scoreManager.blues / (float)scoreManager.kills),
            Tuple.Create("Great White Sharks", (float)scoreManager.greatwhites / (float)scoreManager.kills),
            Tuple.Create("Orcas", (float)scoreManager.orcas / (float)scoreManager.kills),
            Tuple.Create("Seals", (float)scoreManager.seals / (float)scoreManager.kills),
        };

        enemyUsage.Sort((tuple1, tuple2) => tuple2.Item2.CompareTo(tuple1.Item2));

        topEnemyKills = (int)(enemyUsage[0].Item2 * (float)scoreManager.kills);

        geText.text = enemyUsage[0].Item1;
        gepText.text = ((float)System.Math.Round(enemyUsage[0].Item2 * 100f, 1)).ToString() + "%";
        seText.text = enemyUsage[1].Item1;
        sepText.text = ((float)System.Math.Round(enemyUsage[1].Item2 * 100f, 1)).ToString() + "%";
        beText.text = enemyUsage[2].Item1;
        bepText.text = ((float)System.Math.Round(enemyUsage[2].Item2 * 100f, 1)).ToString() + "%";


        GetEndStats();

        StartCoroutine(XpGainCoroutine());

        Destroy(healthBar);
    }

    private void GetEndStats()
    {

        string enemyString = scoreManager.crabs.ToString() + "," + scoreManager.mantas.ToString() + "," + scoreManager.turtles.ToString() + "," +
            scoreManager.seahorses.ToString() + "," + scoreManager.lobsters.ToString() + "," + scoreManager.anglerfish.ToString() + "," +
            scoreManager.octopi.ToString() + "," + scoreManager.archerfish.ToString() + "," + scoreManager.jellyfish.ToString() + "," +
            scoreManager.sawfish.ToString() + "," + scoreManager.belugas.ToString() + "," + scoreManager.hammerheads.ToString() + "," +
            scoreManager.eels.ToString() + "," + scoreManager.makos.ToString() + "," + scoreManager.blues.ToString() + "," +
            scoreManager.greatwhites.ToString() + "," + scoreManager.orcas.ToString() + "," + scoreManager.seals.ToString();

        string[] eStrings = enemyString.Split(',');
        string totalEnemies = PlayerPrefs.GetString("TotalEnemyString");
        string[] teStrings = totalEnemies.Split(',');

        for (int i = 0; i < eStrings.Length; i++)
        {
            int eIntValue = int.Parse(eStrings[i]);
            int teIntValue = int.Parse(teStrings[i]);

            teIntValue += eIntValue;
            teStrings[i] = teIntValue.ToString();
        }
        string resultString = string.Join(",", teStrings);
        PlayerPrefs.SetString("TotalEnemyString", resultString);

        float acc = ((PlayerPrefs.GetFloat("Accuracy") * (float)PlayerPrefs.GetInt("GP")) + accHolder) / (float)(PlayerPrefs.GetInt("GP") + 1);
        PlayerPrefs.SetFloat("Accuracy", acc);

        int gp = PlayerPrefs.GetInt("GP") + 1;
        PlayerPrefs.SetInt("GP", gp);

        int kills = PlayerPrefs.GetInt("Kills") + scoreManager.kills;
        PlayerPrefs.SetInt("Kills", kills);

        int mp = PlayerPrefs.GetInt("MP") + scoreManager.minuteCounter;
        PlayerPrefs.SetInt("MP", mp);

        int pts = PlayerPrefs.GetInt("Points") + scoreManager.score;
        PlayerPrefs.SetInt("Points", pts);

        string pbString = PlayerPrefs.GetString("PersonalBests");
        string[] pbStrings = pbString.Split(',');

        if (scoreManager.kills > int.Parse(pbStrings[0]))
        {
            pbStrings[0] = scoreManager.kills.ToString();
            killsPB.SetActive(true);
            pbFlag = true;
        }

        if (scoreManager.score > int.Parse(pbStrings[1]))
        {
            pbStrings[1] = scoreManager.score.ToString();
            pbFlag = true;
            hsFlag = true;
        }

        if (accHolder > float.Parse(pbStrings[2]))
        {
            pbStrings[2] = ((float)System.Math.Round(((float)scoreManager.hits / (float)scoreManager.shots) * 100f, 1)).ToString();
            accuracyPB.SetActive(true);
            pbFlag = true;
        }

        if (((float)System.Math.Round((float)scoreManager.kills / (float)Mathf.Max(scoreManager.minuteCounter, 1), 1)) > float.Parse(pbStrings[3]))
        {
            pbStrings[3] = ((float)System.Math.Round((float)scoreManager.kills / (float)scoreManager.minuteCounter, 1)).ToString();
            kpmPB.SetActive(true);
            pbFlag = true;
        }

        if (topEnemyKills > int.Parse(pbStrings[4]))
        {
            pbStrings[4] = topEnemyKills.ToString();
        }

        if (((float)pts / (float)gp) > float.Parse(pbStrings[5]))
        {
            pbStrings[5] = ((float)pts / (float)gp).ToString();
        }

        if (acc > float.Parse(pbStrings[6]))
        {
            pbStrings[6] = acc.ToString();
        }

        if (((float)kills / (float)Mathf.Max(mp, 1)) > float.Parse(pbStrings[7]))
        {
            pbStrings[7] = ((float)kills / (float)Mathf.Max(mp, 1)).ToString();
        }
        //put pb text coroutine in each one

        string pbResultString = string.Join(",", pbStrings);
        PlayerPrefs.SetString("PersonalBests", pbResultString);

        PlayerPrefs.Save();
    }

    private IEnumerator XpGainCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);

        StartCoroutine(musicController.PlaySoundEffect("GlowSound"));

        TextMeshProUGUI xpGainText = xpGlow.transform.Find("XpGained").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI xpGainNumberText = xpGlow.transform.Find("XpGainedNumber").GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI xpNumberText = xpGlow.transform.Find("XpNumber").GetComponent<TextMeshProUGUI>();
        GameObject xpMeter = xpGlow.transform.Find("XpMeter").gameObject;
        GameObject xpMeterBack = xpGlow.transform.Find("XpMeterBack").gameObject;

        xpGlow.SetActive(true);

        xpGainNumberText.text = "+0";
        xpGainText.text = "Game XP";

        Color currentColor = xpGlow.GetComponent<Image>().color;

        for (int c = 0; c < 100; c++)
        {
            currentColor.a = ((float)c * 2f) / 255f;

            xpGlow.GetComponent<Image>().color = currentColor;

            yield return new WaitForSecondsRealtime(1f / 256f);
        }

        xpGainText.gameObject.SetActive(true);
        xpGainNumberText.gameObject.SetActive(true);

        int gameXp = GetGameXp();
        int bonusXp = scoreManager.xpGain;
        int totalXp = gameXp + bonusXp;
        int userXp = scoreManager.xp + gameXp;
        UnityEngine.Debug.LogError(userXp);
        PlayerPrefs.SetInt("XP", userXp);
        PlayerPrefs.Save();

        float gameGainTime = 0f;
        float bonusGainTime = 0f;
        if (gameXp > 0)
        {
            gameGainTime = 2f / (float)gameXp;
        }

        if (bonusXp > 0)
        {
            bonusGainTime = 2f / (float)bonusXp;
        }

        int stepSizeA = 1;
        if (gameXp >= 100 && gameXp < 1000)
        {
            stepSizeA = 10;
        }
        else if (gameXp >= 1000)
        {
            stepSizeA = 100;
        }

        for (int i = 0; i <= gameXp;)
        {
            xpGainNumberText.text = "+" + i.ToString();

            if ((gameXp - i <= stepSizeA) && (stepSizeA > 1))
            {
                stepSizeA = stepSizeA / 10;
            }

            i += stepSizeA;

            //l = Mathf.Min(l, userXp);

            yield return new WaitForSecondsRealtime(gameGainTime);
        }

        yield return new WaitForSecondsRealtime(1f);
        xpGainText.text = "Bonus XP";


        int stepSizeB = 1;
        if (bonusXp >= 100 && bonusXp < 1000)
        {
            stepSizeB = 10;
        }
        else if (bonusXp >= 1000)
        {
            stepSizeB = 100;
        }

        for (int j = gameXp; j <= totalXp;)
        {
            xpGainNumberText.text = "+" + j.ToString();

            if ((totalXp - j <= stepSizeB) && (stepSizeB > 1))
            {
                stepSizeB = stepSizeB / 10;
            }

            j += stepSizeB;

            //l = Mathf.Min(l, userXp);

            yield return new WaitForSecondsRealtime(bonusGainTime);
        }

        xpGainText.text = "XP Gained";
        //yield return new WaitForSecondsRealtime(1f);
        
        for (int k = 0; k < 100; k++)
        {
            xpGainNumberText.rectTransform.anchoredPosition = new UnityEngine.Vector2(xpGainNumberText.rectTransform.anchoredPosition.x, xpGainNumberText.rectTransform.anchoredPosition.y + 1.01f);
            xpGainText.rectTransform.anchoredPosition = new UnityEngine.Vector2(xpGainText.rectTransform.anchoredPosition.x, xpGainText.rectTransform.anchoredPosition.y + 1.2f);

            xpGainNumberText.fontSize = xpGainNumberText.fontSize * 0.995f;
            xpGainText.fontSize = xpGainText.fontSize * 0.995f;

            yield return new WaitForSecondsRealtime(0.001f);
        }

        xpNumberText.gameObject.SetActive(true);
        xpMeter.SetActive(true);
        xpMeterBack.SetActive(true);

        xpNumberText.text = "0/" + scoreManager.levelThresh;

        float meterGainTime = 0f;
        float ogMeterWidth = xpMeterBack.GetComponent<RectTransform>().sizeDelta.x - 10f;
        xpMeter.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(0f, xpMeter.GetComponent<RectTransform>().sizeDelta.y);
        if (scoreManager.xp > 0)
        {
            meterGainTime = 2f / (float)userXp;
        }

        int stepSize = 1;
        if (userXp >= 100 && userXp < 1000)
        {
            stepSize = 10;
        }
        else if (userXp >= 1000)
        {
            stepSize = 100;
        }

        for (int l = 0; l <= userXp;)
        {
            xpNumberText.text = l + "/" + scoreManager.levelThresh;

            xpMeter.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(((float)l / (float)scoreManager.levelThresh) * ogMeterWidth, xpMeter.GetComponent<RectTransform>().sizeDelta.y);

            if ((userXp - l <= stepSize) && (stepSize > 1))
            {
                stepSize = stepSize / 10;
            }

            l += stepSize;

            //l = Mathf.Min(l, userXp);

            yield return new WaitForSecondsRealtime(meterGainTime);
        }

        yield return new WaitForSecondsRealtime(2f);

        xpNumberText.gameObject.SetActive(false);
        xpMeter.gameObject.SetActive(false);
        xpMeterBack.gameObject.SetActive(false);
        xpGainNumberText.gameObject.SetActive(false);
        xpGainText.gameObject.SetActive(false);

        for (int d = 99; d > 0; d--)
        {
            currentColor.a = ((float)d * 2f) / 255f;

            xpGlow.GetComponent<Image>().color = currentColor;

            yield return new WaitForSecondsRealtime(1f / 256f);
        }

        xpGlow.gameObject.SetActive(false);

        //high score routine and condition
        if (hsFlag)
        {
            //yield return new WaitForSecondsRealtime(1f);

            TextMeshProUGUI hsText = highScoreGlow.transform.Find("HighScore").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI hsNumberText = highScoreGlow.transform.Find("HighScoreNumber").GetComponent<TextMeshProUGUI>();


            highScoreGlow.gameObject.SetActive(true);

            Color hsCurrentColor = highScoreGlow.GetComponent<Image>().color;

            for (int a = 0; a < 100; a++)
            {
                hsCurrentColor.a = ((float)a * 2f) / 255f;

                highScoreGlow.GetComponent<Image>().color = hsCurrentColor;

                yield return new WaitForSecondsRealtime(1f / 256f);
            }

            hsText.gameObject.SetActive(true);
            hsNumberText.gameObject.SetActive(true);

            hsNumberText.text = "0";

            float hsTime = 0f;

            if (scoreManager.score > 0)
            {
                hsTime = 2f / (float)scoreManager.score;
            }

            int hstepSize = 1;
            if (scoreManager.score >= 1000 && scoreManager.score < 10000)
            {
                hstepSize = 10;
            }
            else if (scoreManager.score >= 10000)
            {
                hstepSize = 100;
            }

            for (int h = 0; h <= scoreManager.score;)
            {
                hsNumberText.text = h.ToString();

                if ((scoreManager.score - h <= hstepSize) && (stepSize > 1))
                {
                    hstepSize = hstepSize / 10;
                }

                h += hstepSize;

                //h = Mathf.Min(h, scoreManager.score);

                yield return new WaitForSecondsRealtime(hsTime);
            }

            yield return new WaitForSecondsRealtime(1f);

            hsText.gameObject.SetActive(false);
            hsNumberText.gameObject.SetActive(false);

            for (int b = 99; b > 0; b--)
            {
                hsCurrentColor.a = ((float)b * 2f) / 255f;

                highScoreGlow.GetComponent<Image>().color = hsCurrentColor;

                yield return new WaitForSecondsRealtime(1f / 256f);
            }

            highScoreGlow.gameObject.SetActive(false);
        }

    }

    private int GetGameXp()
    {
        float pbMultiplier = pbFlag ? 1.5f : 1f;
        int gameXp = (int)(Mathf.Pow((scoreManager.kills * 5) + ((float)scoreManager.score * 0.1f) + (scoreManager.minuteCounter * 10), (float)scoreManager.level / 16f) * pbMultiplier);

        return gameXp;
    }

}
