using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using System.Numerics;

public class AchievementManager : MonoBehaviour
{
    public GameObject achPop;
    public GameObject pufferfish;

    private List<Achievement> achievements;
    private int totalCoins;
    private int totalXP;

    private int[] currentAchs = new int[3];
    public Achievement currentAchievementA = null;
    public Achievement currentAchievementB = null;
    public Achievement currentAchievementC = null;

    private int index = 0;

    private ScoreManager scoreManager;
    private Pufferfish pufferfishScript;

    public bool scubahSnipahFlag = false;
    public bool ricochetFlag = false;
    public bool ptFlag = false;
    public bool pinballFlag = false;

    private MusicController musicController;


    private void Start()
    {
        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        scoreManager = GetComponent<ScoreManager>();
        pufferfishScript = pufferfish.GetComponent<Pufferfish>();


        achievements = new List<Achievement>();
        Achievement[] loadedAchievements = Resources.LoadAll<Achievement>("Achievements");
        achievements.AddRange(loadedAchievements);

        LoadPlayerProgress();

        SetCurrentAchievementInput(currentAchs[0]);
        SetCurrentAchievementInput(currentAchs[1]);
        SetCurrentAchievementInput(currentAchs[2]);

    }

    private void Update()
    {
        AchievementCheck();
    }

    private void AchievementCheck()
    {
        int[] currAchs = { achievements.IndexOf(currentAchievementA), achievements.IndexOf(currentAchievementB), achievements.IndexOf(currentAchievementC) };

        for (int j = 0; j < currAchs.Length; j++)
        {
            int i = currAchs[j];

            switch (i)
            {
                case 0: // AA
                    achievements[i].progress = (float)scoreManager.coinsGain / 10f;
                    if (scoreManager.coinsGain >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 1: // AB
                    achievements[i].progress = (float)scoreManager.kills / 10f;
                    if (scoreManager.kills >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 2: // AC
                    achievements[i].progress = (float)scoreManager.score / 2500f;
                    if (scoreManager.score >= 2500)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 3: // AD
                    achievements[i].progress = (float)scoreManager.crabs / 10f;
                    if (scoreManager.crabs >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 4: // AE
                    achievements[i].progress = (float)scoreManager.hook / 10f;
                    if (scoreManager.hook >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 5: // AF
                    achievements[i].progress = (float)scoreManager.minuteMan / 5f;
                    if (scoreManager.minuteMan >= 5)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 6: // AG
                    achievements[i].progress = (float)scoreManager.minuteCounter / 5f;
                    if (scoreManager.minuteCounter >= 5)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 7: // AH
                    achievements[i].progress = (float)scoreManager.tomahawk / 25f;
                    if (scoreManager.tomahawk >= 25)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 8: // AI
                    achievements[i].progress = (float)scoreManager.mantas / 10f;
                    if (scoreManager.mantas >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 9: // AJ
                    achievements[i].progress = (float)scoreManager.coinsGain / 25f;
                    if (scoreManager.coinsGain >= 25)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 10: // AK
                    achievements[i].progress = (float)scoreManager.minuteCounter / 5f;
                    if (scoreManager.minuteCounter == 5)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 11: // AL
                    achievements[i].progress = (float)scoreManager.turtles / 10f;
                    if (scoreManager.turtles >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 12: // AM
                    achievements[i].progress = (float)scoreManager.kills / 50f;
                    if (scoreManager.kills >= 50)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 13: // AN
                    achievements[i].progress = (float)scoreManager.turret / 25f;
                    if (scoreManager.turret >= 25)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 14: // AO
                    int ao = (pufferfishScript.untouchFlag) ? 1 : 0;
                    achievements[i].progress = ((float)scoreManager.minuteCounter / 1f) * (float)ao;  // Assuming minuteCounter is a float
                    if (scoreManager.minuteCounter >= 1 && pufferfishScript.untouchFlag)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 15: // AP
                    achievements[i].progress = (float)scoreManager.minuteMan / 10f;
                    if (scoreManager.minuteMan >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 16: // AQ
                    achievements[i].progress = (float)scoreManager.seahorses / 10f;
                    if (scoreManager.seahorses >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 17: // AR
                    achievements[i].progress = (float)scoreManager.score / 15000f;
                    if (scoreManager.score >= 15000)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 18: // AS
                    achievements[i].progress = (float)scoreManager.lobsters / 10f;
                    if (scoreManager.lobsters >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 19: // AT
                    achievements[i].progress = scoreManager.minuteCounter / 10f;
                    if (scoreManager.minuteCounter >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 20: // AU
                    achievements[i].progress = (float)scoreManager.kills / 100f;
                    if (scoreManager.kills >= 100)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 21: // AV
                    achievements[i].progress = (float)scoreManager.coinsGain / 50f;
                    if (scoreManager.coinsGain >= 50)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 22: // AW
                    achievements[i].progress = (float)scoreManager.anglerfish / 10f;
                    if (scoreManager.anglerfish >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 23: // AX
                    achievements[i].progress = (float)scoreManager.rocket / 50f;
                    if (scoreManager.rocket >= 50)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 24: // AY
                    int ay = (pufferfishScript.untouchFlag) ? 1 : 0;
                    achievements[i].progress = ((float)scoreManager.minuteCounter / 2f) * (float)ay; // Assuming minuteCounter is a float
                    if (scoreManager.minuteCounter >= 2 && pufferfishScript.untouchFlag)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 25: // AZ
                    achievements[i].progress = (ricochetFlag) ? 1 : 0;
                    if (ricochetFlag)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 26: // BA
                    achievements[i].progress = (float)scoreManager.octopi / 10f;
                    if (scoreManager.octopi >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 27: // BB
                    achievements[i].progress = (float)scoreManager.poison / 25f;
                    if (scoreManager.poison >= 25)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 28: // BC
                    achievements[i].progress = (float)scoreManager.minuteMan / 20f;
                    if (scoreManager.minuteMan >= 20)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 29: // BD
                    int[] numbers = { scoreManager.crabs, scoreManager.mantas, scoreManager.turtles, scoreManager.seahorses, scoreManager.lobsters,
            scoreManager.anglerfish, scoreManager.octopi, scoreManager.archerfish, scoreManager.jellyfish, scoreManager.sawfish,
            scoreManager.belugas, scoreManager.hammerheads, scoreManager.eels, scoreManager.makos, scoreManager.blues,
            scoreManager.greatwhites, scoreManager.orcas, scoreManager.seals };

                    achievements[i].progress = numbers.Count(number => number >= 10) / 3f;
                    if (achievements[i].progress >= 1)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 30: // BE
                    achievements[i].progress = scoreManager.minuteCounter / 15f;
                    if (scoreManager.minuteCounter >= 15)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 31: // BF
                    achievements[i].progress = (float)scoreManager.archerfish / 10f;
                    if (scoreManager.archerfish >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 32: // BG
                    achievements[i].progress = (float)scoreManager.heatseeker / 50f;
                    if (scoreManager.heatseeker >= 50)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 33: // BH
                    achievements[i].progress = (ptFlag) ? 1 : 0;
                    if (ptFlag)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 34: // BI
                    achievements[i].progress = (scoreManager.minuteCounter == 15 && scoreManager.secondCounter >= 0f && scoreManager.secondCounter < 30f) ? 1f : 0f;
                    if (achievements[i].progress == 1)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 35: // BJ
                    achievements[i].progress = (scoreManager.crabs + scoreManager.lobsters) / 50f;
                    if ((scoreManager.crabs + scoreManager.lobsters) >= 50)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 36: // BK
                    achievements[i].progress = (float)scoreManager.kills / 500f;
                    if (scoreManager.kills >= 500)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 37: // BL
                    achievements[i].progress = (float)scoreManager.jellyfish / 10f;
                    if (scoreManager.jellyfish >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 38: // BM
                    achievements[i].progress = scubahSnipahFlag ? 1f : 0f;
                    if (scubahSnipahFlag)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 39: // BN
                    achievements[i].progress = scoreManager.score / 50000f;
                    if (scoreManager.score >= 50000)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 40: // BO
                    achievements[i].progress = (float)scoreManager.sawfish / 10f;
                    if (scoreManager.sawfish >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 41: // BP
                    achievements[i].progress = ((float)scoreManager.lobsters >= 20 && (float)scoreManager.seahorses >= 20) ? 1f : 0f;
                    if (achievements[i].progress == 1)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 42: // BQ
                    int bq = (pufferfishScript.untouchFlag) ? 1 : 0;
                    achievements[i].progress = ((float)scoreManager.minuteCounter / 5f) * (float)bq;
                    if (scoreManager.minuteCounter >= 5 && pufferfishScript.untouchFlag)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 43: // BR
                    achievements[i].progress = (float)scoreManager.belugas / 10f;
                    if (scoreManager.belugas >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 44: // BS
                    achievements[i].progress = (float)scoreManager.flare / 10f;
                    if (scoreManager.flare >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 45: // BT
                    achievements[i].progress = (float)scoreManager.minuteMan / 30f;
                    if (scoreManager.minuteMan >= 30)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 46: // BU
                    achievements[i].progress = (float)scoreManager.hammerheads / 10f;
                    if (scoreManager.hammerheads >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 47: // BV
                    achievements[i].progress = scoreManager.score / 100000f;
                    if (scoreManager.score >= 100000)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 48: // BW
                    achievements[i].progress = (float)scoreManager.eels / 10f;
                    if (scoreManager.eels >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 49: // BX
                    achievements[i].progress = (float)scoreManager.emw / 10f;
                    if (scoreManager.emw >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 50: // BY
                    achievements[i].progress = (float)scoreManager.makos / 10f;
                    if (scoreManager.makos >= 10)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 51: // BZ
                    achievements[i].progress = (float)scoreManager.seamine / 50f;
                    if (scoreManager.seamine >= 50)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 52: // CA
                    achievements[i].progress = (float)scoreManager.blues / 5f;
                    if (scoreManager.blues >= 5)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 53: // CB
                    achievements[i].progress = (pinballFlag) ? 1 : 0;
                    if (pinballFlag)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 54: // CC
                    achievements[i].progress = (float)scoreManager.greatwhites / 5f;
                    if (scoreManager.greatwhites >= 5)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 55: // CD
                    achievements[i].progress = ((scoreManager.minuteCounter == 25) && (scoreManager.secondCounter >= 0f) && (scoreManager.secondCounter < 10f)) ? 1f : 0f;
                    if (achievements[i].progress == 1)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 56: // CE
                    achievements[i].progress = (float)scoreManager.orcas / 3f;
                    if (scoreManager.orcas >= 3)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 57: // CF
                    achievements[i].progress = (float)scoreManager.seals / 3f;
                    if (scoreManager.seals >= 3)
                        CompleteCurrentAchievement(achievements[i]);
                    break;

                case 58: // CG
                    achievements[i].progress = (float)scoreManager.coinsGain / 100f;
                    if (scoreManager.coinsGain >= 100)
                        CompleteCurrentAchievement(achievements[i]);
                    break;
            }
        }
    }

    private void SetCurrentAchievementInput(int i)
    {
        if (i < (achievements.Count - 1))
        {
            if (currentAchievementA == null)
            {
                currentAchievementA = achievements[i];
            }
            else if (currentAchievementB == null)
            {
                currentAchievementB = achievements[i];
            }
            else if (currentAchievementC == null)
            {
                currentAchievementC = achievements[i];
            }
            else
            {
                UnityEngine.Debug.LogError("ERROR: Achievement slots full.");
            }

        }
        else
        {
            if (currentAchievementA == null)
            {
                currentAchievementA = achievements.Last();
            }
            else if (currentAchievementB == null)
            {
                currentAchievementB = achievements.Last();
            }
            else if (currentAchievementC == null)
            {
                currentAchievementC = achievements.Last();
            }
            else
            {
                UnityEngine.Debug.LogError("ERROR: Achievement slots full.");
            }
            UnityEngine.Debug.LogError("CONGRATULATIONS: All achievements complete");

        }

        if (i >= index)
        {
            index = i + 1;
        }
    }

    private void SetCurrentAchievement()
    {
        if (index < (achievements.Count - 1))
        {
            if (currentAchievementA == null)
            {
                currentAchievementA = achievements[index];
                index += 1;
            }
            else if (currentAchievementB == null)
            {
                currentAchievementB = achievements[index];
                index += 1;
            }
            else if (currentAchievementC == null)
            {
                currentAchievementC = achievements[index];
                index += 1;
            }
            else
            {
                UnityEngine.Debug.LogError("ERROR: Achievement slots full.");
            }
            
        }
        else
        {
            if (currentAchievementA == null)
            {
                currentAchievementA = achievements.Last();
            }
            else if (currentAchievementB == null)
            {
                currentAchievementB = achievements.Last();
            }
            else if (currentAchievementC == null)
            {
                currentAchievementC = achievements.Last();
            }
            else
            {
                UnityEngine.Debug.LogError("ERROR: Achievement slots full.");
            }
            UnityEngine.Debug.LogError("CONGRATULATIONS: All achievements complete");
        }

        PlayerPrefs.SetString("CurrentAchs", achievements.IndexOf(currentAchievementA) + "," + achievements.IndexOf(currentAchievementB) + "," + achievements.IndexOf(currentAchievementC));
        PlayerPrefs.Save();
    }

    public void CompleteCurrentAchievement(Achievement ach)
    {
        if (ach == currentAchievementA)
        {
            totalCoins += currentAchievementA.coinsReward;
            totalXP += currentAchievementA.xpReward;
            StartCoroutine(PopAchievement(ach));
            currentAchievementA = null;
        }
        else if (ach == currentAchievementB)
        {
            totalCoins += currentAchievementB.coinsReward;
            totalXP += currentAchievementB.xpReward;
            StartCoroutine(PopAchievement(ach));
            currentAchievementB = null;
        }
        else if (ach == currentAchievementC)
        {
            totalCoins += currentAchievementC.coinsReward;
            totalXP += currentAchievementC.xpReward;
            StartCoroutine(PopAchievement(ach));
            currentAchievementC = null;
        }

        PlayerPrefs.SetInt("Coins", totalCoins);
        PlayerPrefs.SetInt("XP", totalXP);
        PlayerPrefs.Save();

        SetCurrentAchievement();
    }

    private void LoadPlayerProgress()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            totalCoins = PlayerPrefs.GetInt("Coins");
        }

        if (PlayerPrefs.HasKey("XP"))
        {
            totalXP = PlayerPrefs.GetInt("XP");
        }

        if (PlayerPrefs.HasKey("CurrentAchs"))
        {
            string caString = PlayerPrefs.GetString("CurrentAchs");
            string[] caStrings = caString.Split(',');
            currentAchs[0] = int.Parse(caStrings[0]);
            currentAchs[1] = int.Parse(caStrings[1]);
            currentAchs[2] = int.Parse(caStrings[2]);
            index = currentAchs.Max() + 1;
            UnityEngine.Debug.LogError(caString);
        }
    }

    private IEnumerator PopAchievement(Achievement achievement)
    {
        StartCoroutine(musicController.PlaySoundEffect("AchievementPop"));

        TextMeshProUGUI title = achPop.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        title.text = achievement.aName;

        TextMeshProUGUI desc = achPop.transform.Find("Desc").GetComponent<TextMeshProUGUI>();
        desc.text = achievement.description;

        TextMeshProUGUI coins = achPop.transform.Find("Coins").GetComponent<TextMeshProUGUI>();
        coins.text = achievement.coinsReward.ToString();

        TextMeshProUGUI xp = achPop.transform.Find("XP").GetComponent<TextMeshProUGUI>();
        xp.text = achievement.xpReward.ToString();

        for (int i = 0; i < 100; i++)
        {
            achPop.transform.position = achPop.transform.position + new UnityEngine.Vector3(0f, 0.5f, 0f);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(4f);

        for (int j = 0; j < 100; j++)
        {
            achPop.transform.position = achPop.transform.position - new UnityEngine.Vector3(0f, 0.5f, 0f);
            yield return new WaitForSeconds(0.01f);
        }

    }
}
