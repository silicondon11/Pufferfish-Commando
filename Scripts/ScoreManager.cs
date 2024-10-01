using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System.Numerics;
using System;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public Light directionalLight;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI clock;
    public int score;
    public int tier = 0;
    public int xp;
    public int coins;
    private int xpChange;
    private int coinsChange;
    public int coinsGain = 0;
    public int xpGain = 0;
    private float elapsedTime = 0f;
    public float secondCounter = 0f;
    public int minuteCounter = 0;
    private bool dayFlag = true;
    public bool frenzyFlag = false;

    //Stats
    public int kills = 0;
    public int shots = 0;
    public int hits = 0;
    public int minuteMan = 0;

    public int hook = 0;
    public int spring = 0;
    public int tomahawk = 0;
    public int turret = 0;
    public int turbine = 0;
    public int bubble = 0;
    public int net = 0;
    public int rocket = 0;
    public int harpoon = 0;
    public int umbrella = 0;
    public int poison = 0;
    public int heatseeker = 0;
    public int flare = 0;
    public int emw = 0;
    public int hypno = 0;
    public int seamine = 0;
    public int totalCols = 0;

    public int crabs = 0;
    public int mantas = 0;
    public int turtles = 0;
    public int seahorses = 0;
    public int lobsters = 0;
    public int anglerfish = 0;
    public int octopi = 0;
    public int archerfish = 0;
    public int jellyfish = 0;
    public int sawfish = 0;
    public int belugas = 0;
    public int hammerheads = 0;
    public int eels = 0;
    public int makos = 0;
    public int blues = 0;
    public int greatwhites = 0;
    public int orcas = 0;
    public int seals = 0;

    public int level;
    private List<int> levelsUnlocked;
    public int levelThresh;
    public static bool newLevelFlag = false;
    private float spinSpeed = 1880f; // degrees per second
    private int spinTimes = 8;

    public GameObject xpPrefab;
    public GameObject coinPrefab;
    public GameObject levelPop;
    public GameObject hitScorePrefab;
    private float minXpSpawn = 30f;
    private float maxXpSpawn = 90f;
    private float minCoinSpawn = 10f;
    private float maxCoinSpawn = 30f;
    private float minFrenzySpawn = 3f;
    private float maxFrenzySpawn = 6f;

    private UnityEngine.Vector2 xBounds = new UnityEngine.Vector2(200f, 1200f);
    private UnityEngine.Vector2 zBounds = new UnityEngine.Vector2(600f, 2000f);

    private MusicController musicController;


    private void Start()
    {
        musicController = GameObject.Find("Audio").GetComponent<MusicController>();
        StartCoroutine(musicController.PlaySoundEffect("PanelOpen"));

        StartCoroutine(GameSceneTransition());

        StartCoroutine(XpSpawnLoop());
        StartCoroutine(CoinSpawnLoop());

        coins = PlayerPrefs.GetInt("Coins");
        xp = PlayerPrefs.GetInt("XP");
        level = PlayerPrefs.GetInt("Level");

        coinsText.text = coins.ToString();
        levelThresh = GetLevelThresh(level);
        xpText.text = xp.ToString() + "/" + levelThresh.ToString();

        TextMeshProUGUI levelText = levelPop.transform.Find("Level").GetComponent<TextMeshProUGUI>();
        levelText.text = level.ToString();

        coinsChange = coins;
        xpChange = xp;

        coinsGain = 0;
        xpGain = 0;

        newLevelFlag = false;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        FormatTime(elapsedTime);
        secondCounter += Time.deltaTime;
        if (secondCounter >= 60f)
        {
            minuteCounter += 1;
            score = (int)((float)score * (1f + (0.1f * (float)minuteCounter)));
            scoreText.text = score.ToString();
            StartCoroutine(ShowTimeScore((1f + (0.1f * (float)minuteCounter)), new UnityEngine.Vector3(543f, 923f, 1222f)));
            secondCounter = 0f;

            minuteMan = 0;

            if ((minuteCounter % 1) == 0)
            {
                StartCoroutine(ChangeLighting(dayFlag));
                dayFlag = !dayFlag;
            }
        }

        int frameScore = GetFrameScore();
        if (frameScore != 0)
        {
            score += frameScore;
            scoreText.text = score.ToString();
        }

        if (coins != coinsChange)
        {
            coinsText.text = coins.ToString();
            PlayerPrefs.SetInt("Coins", coins);
            PlayerPrefs.Save();
            coinsGain += coins - coinsChange;
            coinsChange = coins;
        }

        if (xp != xpChange)
        {
            if (xp > levelThresh)
            {
                level += 1;
                levelsUnlocked.Add(level);
                PlayerPrefs.SetInt("Level", level);
                PlayerPrefs.Save();
                newLevelFlag = true;
                StartCoroutine(LevelPop());
            }
            levelThresh = GetLevelThresh(level);
            xpText.text = xp.ToString() + "/" + levelThresh.ToString();
            PlayerPrefs.SetInt("XP", xp);
            PlayerPrefs.Save();
            xpGain += xp - xpChange;
            xpChange = xp;
        }

        int tierThresh = Sum(tier + 1) * 100;
        if (score >= tierThresh)
        {
            tier += 1;
            StartCoroutine(TierUp(tier));
        }
    }

    private int GetFrameScore()
    {
        List<TargetType> deadTargetTypes = new List<TargetType>();

        GameObject[] targetsArray = GameObject.FindGameObjectsWithTag("Target");
        List<GameObject> targets = targetsArray.ToList();

        int frameScore = 0;

        foreach (GameObject target in targets)
        {
            TargetControl targetControlScript = target.GetComponent<TargetControl>();

            if (targetControlScript != null && targetControlScript.deathFlag)
            {
                kills += 1;
                minuteMan += 1;

                int killScore = GetTypeScore(targetControlScript.type);
                UnityEngine.Vector3 pos = target.transform.position;
                StartCoroutine(ShowHitScore(killScore, pos));
                frameScore += killScore;

                if (targetControlScript.type == TargetType.Jellyfish)
                {
                    Jellyfish script = target.GetComponent<Jellyfish>();
                    targetControlScript.deathFlag = false;
                    script.explodeFlag = true;
                    StartCoroutine(script.Explode());
                }
                else
                {
                    Destroy(target);
                }
            }
        }
        return frameScore;
    }

    private int GetTypeScore(TargetType type)
    {
        int typeScore = 0;
        if (type == TargetType.Crab)
        {
            typeScore = 10;
            crabs += 1;

            StartCoroutine(musicController.PlaySoundEffect("CrabDeath"));
        }
        else if (type == TargetType.Manta)
        {
            typeScore = 20;
            mantas += 1;

            StartCoroutine(musicController.PlaySoundEffect("MantaDeath"));
        }
        else if (type == TargetType.Turtle)
        {
            typeScore = 30;
            turtles += 1;

            StartCoroutine(musicController.PlaySoundEffect("TurtleDeath"));
        }
        else if (type == TargetType.Seahorse)
        {
            typeScore = 40;
            seahorses += 1;

            StartCoroutine(musicController.PlaySoundEffect("SeaHorseDeath"));
        }
        else if (type == TargetType.Lobster)
        {
            typeScore = 50;
            lobsters += 1;

            StartCoroutine(musicController.PlaySoundEffect("LobsterDeath"));
        }
        else if (type == TargetType.Anglerfish)
        {
            typeScore = 5;
            anglerfish += 1;

            StartCoroutine(musicController.PlaySoundEffect("AnglerfishDeath"));
        }
        else if (type == TargetType.Octopus)
        {
            typeScore = 60;
            octopi += 1;

            StartCoroutine(musicController.PlaySoundEffect("OctopusDeath"));
        }
        else if (type == TargetType.Archerfish)
        {
            typeScore = 70;
            archerfish += 1;

            StartCoroutine(musicController.PlaySoundEffect("ArcherfishDeath"));
        }
        else if (type == TargetType.Jellyfish)
        {
            //typeScore = 10;
            jellyfish += 1;

            StartCoroutine(musicController.PlaySoundEffect("JellyfishDeath"));
        }
        else if (type == TargetType.Sawfish)
        {
            typeScore = 80;
            sawfish += 1;

            StartCoroutine(musicController.PlaySoundEffect("SawfishDeath"));
        }
        else if (type == TargetType.Beluga)
        {
            typeScore = 80;
            belugas += 1;

            StartCoroutine(musicController.PlaySoundEffect("BelugaDeath"));
        }
        else if (type == TargetType.Hammerhead)
        {
            typeScore = 90;
            hammerheads += 1;

            StartCoroutine(musicController.PlaySoundEffect("HammerheadDeath"));
        }
        else if (type == TargetType.Eel)
        {
            typeScore = 90;
            eels += 1;

            StartCoroutine(musicController.PlaySoundEffect("EelDeath"));
        }
        else if (type == TargetType.Mako)
        {
            typeScore = 100;
            makos += 1;

            StartCoroutine(musicController.PlaySoundEffect("MakoDeath"));
        }
        else if (type == TargetType.Blue)
        {
            typeScore = 100;
            blues += 1;

            StartCoroutine(musicController.PlaySoundEffect("BlueWhaleDeath"));
        }
        else if (type == TargetType.GreatWhite)
        {
            typeScore = 110;
            greatwhites += 1;

            StartCoroutine(musicController.PlaySoundEffect("GreatWhiteDeath"));
        }
        else if (type == TargetType.Orca)
        {
            typeScore = 110;
            orcas += 1;

            StartCoroutine(musicController.PlaySoundEffect("OrcaDeath"));
        }
        else if (type == TargetType.Seal)
        {
            typeScore = 150;
            seals += 1;

            StartCoroutine(musicController.PlaySoundEffect("SealDeath"));
        }
        return typeScore;
    }

    private IEnumerator XpSpawnLoop()
    {
        while (true)
        {
            if (frenzyFlag)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(minFrenzySpawn, maxFrenzySpawn));
                SpawnPrefabAtRandomPosition(xpPrefab);
            }
            else
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(minXpSpawn, maxXpSpawn));
                SpawnPrefabAtRandomPosition(xpPrefab);
            }
        }
    }

    private IEnumerator CoinSpawnLoop()
    {
        while (true)
        {
            if (frenzyFlag)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(minFrenzySpawn, maxFrenzySpawn));
                SpawnPrefabAtRandomPosition(coinPrefab);
            }
            else
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(minCoinSpawn, maxCoinSpawn));
                SpawnPrefabAtRandomPosition(coinPrefab);
            }

        }
    }

    private IEnumerator LevelPop()
    {
        StartCoroutine(musicController.PlaySoundEffect("LevelUp"));

        levelPop.SetActive(true);
        Transform levelUp = levelPop.transform.Find("LevelUp");
        Renderer renderer = levelUp.GetComponent<Renderer>();
        Material material = renderer.material;
        Color initialColor = material.color;
        float alpha = initialColor.a;
        float totalRotation = 360 * spinTimes;
        float rotationPerSegment = totalRotation / spinTimes;

        TextMeshProUGUI levelText = levelPop.transform.Find("Level").GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < 250; i++)
        {
            //spinning here
            float rotationThisFrame = spinSpeed * Time.deltaTime;
            levelUp.Rotate(0, 0, rotationThisFrame);

            if (i > 125)
            {
                alpha = initialColor.a - (initialColor.a * ((float)(i - 125) / 125));
                Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
                material.color = newColor;

                levelText.text = level.ToString();
            }

            yield return new WaitForSeconds(0.005f);
        }

        levelPop.SetActive(false);
    }

    private IEnumerator ShowHitScore(int score, UnityEngine.Vector3 deathPos)
    {
        if (score > 0)
        {
            GameObject hitScore = Instantiate(hitScorePrefab, deathPos, UnityEngine.Quaternion.identity);
            TMP_Text hitScoreText = hitScore.GetComponent<TMP_Text>();
            hitScoreText.text = "+" + score.ToString();

            for (int i = 0; i < 250; i++)
            {
                hitScore.transform.position = hitScore.transform.position + new UnityEngine.Vector3(0f, 0.5f, 0f);

                yield return new WaitForSeconds(0.005f);
            }

            Destroy(hitScore);
        } 
    }

    private IEnumerator ShowTimeScore(float multiplier, UnityEngine.Vector3 deathPos)
    {
        StartCoroutine(musicController.PlaySoundEffect("TimeMultiplier"));

        GameObject hitScore = Instantiate(hitScorePrefab, deathPos, UnityEngine.Quaternion.identity);
        TMP_Text hitScoreText = hitScore.GetComponent<TMP_Text>();
        hitScoreText.text = "x" + multiplier.ToString();
        hitScoreText.color = Color.yellow;
        clock.color = Color.yellow;
        for (int i = 0; i < 250; i++)
        {
            hitScore.transform.position = hitScore.transform.position + new UnityEngine.Vector3(0f, 0.5f, 0f);

            yield return new WaitForSeconds(0.005f);
        }
        clock.color = Color.white;
        Destroy(hitScore);
    }

    private IEnumerator TierUp(int tier)
    {
        StartCoroutine(musicController.PlaySoundEffect("TierUp"));

        GameObject hitScore = Instantiate(hitScorePrefab, new UnityEngine.Vector3(793f, 863f, 1222f), UnityEngine.Quaternion.identity);
        TMP_Text hitScoreText = hitScore.GetComponent<TMP_Text>();
        hitScoreText.text = "+" + tier;
        hitScoreText.color = Color.red;
        scoreText.color = Color.red;
        for (int i = 0; i < 250; i++)
        {
            hitScore.transform.position = hitScore.transform.position + new UnityEngine.Vector3(0f, 0.5f, 0f);

            yield return new WaitForSeconds(0.005f);
        }
        scoreText.color = Color.white;
        Destroy(hitScore);
    }

    private IEnumerator ChangeLighting(bool day)
    {
        if (day)
        {
            for (int i = 0; i < 100; i++)
            {
                directionalLight.intensity = directionalLight.intensity - 0.015f;
                yield return new WaitForSeconds(0.1f);
            }
            directionalLight.intensity = 0.5f;
        }
        else
        {
            for (int i = 0; i < 100; i++)
            {
                directionalLight.intensity = directionalLight.intensity + 0.015f;
                yield return new WaitForSeconds(0.1f);
            }
            directionalLight.intensity = 2f;
        }

        yield return null;
    }

    private void SpawnPrefabAtRandomPosition(GameObject prefab)
    {
        UnityEngine.Vector3 randomPosition = new UnityEngine.Vector3(UnityEngine.Random.Range(xBounds.x, xBounds.y), 1500, UnityEngine.Random.Range(zBounds.x, zBounds.y));

        Instantiate(prefab, randomPosition, UnityEngine.Quaternion.Euler(0, -90, 0));
    }

    private int GetLevelThresh(int level)
    {
        int levelThresh = 100 + 500 * ((level * (level - 1)) / 2);
        return levelThresh;
    }

    private void FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

        clock.text = formattedTime;
    }

    private int Sum(int n)
    {
        if (n == 0 || n == 1)
        {
            return 1;
        }

        return n + Sum(n - 1);
    }

    private IEnumerator GameSceneTransition()
    {

        GameObject up = GameObject.Find("Canvas").transform.Find("UpPanel").gameObject;
        GameObject down = GameObject.Find("Canvas").transform.Find("DownPanel").gameObject;

        if (up == null || down == null)
        {
            UnityEngine.Debug.LogError("ERROR: Transition panels not found.");
        }
        else
        {
            RectTransform upRect = up.GetComponent<RectTransform>();
            RectTransform downRect = down.GetComponent<RectTransform>();

            UnityEngine.Vector2 initUpPos = new UnityEngine.Vector2(upRect.anchoredPosition.x, upRect.anchoredPosition.y);
            UnityEngine.Vector2 initDownPos = new UnityEngine.Vector2(downRect.anchoredPosition.x, downRect.anchoredPosition.y);

            for (int i = 0; i < 873;)
            {
                if (i < 849)
                {
                    upRect.anchoredPosition = new UnityEngine.Vector2(initUpPos.x, initUpPos.y + i);
                }
                downRect.anchoredPosition = new UnityEngine.Vector2(initDownPos.x, initDownPos.y - i);

                i += 8;

                yield return new WaitForSeconds(0.001f);
            }

        }

        yield return new WaitForSeconds(1f);
    }
}
