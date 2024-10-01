using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject crab;
    public GameObject manta;
    public GameObject turtle;
    public GameObject seaHorse;
    public GameObject lobster;
    public GameObject anglerfish;
    public GameObject octopus;
    public GameObject archerfish;
    public GameObject jellyfish;
    public GameObject sawfish;
    public GameObject beluga;
    public GameObject hammerhead;
    public GameObject eel;
    public GameObject mako;
    public GameObject blue;
    public GameObject greatWhite;
    public GameObject orca;
    public GameObject seal;
    private List<GameObject> enemies = new List<GameObject>();

    private UnityEngine.Vector3 spawnPointCBL = new UnityEngine.Vector3(-807f, 0f, 822f);
    private UnityEngine.Vector3 spawnPointFBL = new UnityEngine.Vector3(-807f, 0f, 1622f);
    private UnityEngine.Vector3 spawnPointCTL = new UnityEngine.Vector3(-807f, 1000f, 822f);
    private UnityEngine.Vector3 spawnPointFTL = new UnityEngine.Vector3(-807f, 1000f, 1622f);
    private UnityEngine.Vector3 spawnPointCBR = new UnityEngine.Vector3(2193f, 0f, 822f);
    private UnityEngine.Vector3 spawnPointFBR = new UnityEngine.Vector3(2193f, 0f, 1622f);
    private UnityEngine.Vector3 spawnPointCTR = new UnityEngine.Vector3(2193f, 1000f, 822f);
    private UnityEngine.Vector3 spawnPointFTR = new UnityEngine.Vector3(2193f, 1000f, 1622f);
    public List<UnityEngine.Vector3> spawnPoints = new List<UnityEngine.Vector3>();

    private int tier = 0;
    private int tierHolder = 0;
    private ScoreManager scoreManager;
    public bool spawnFlag = true;

    private int semiTier = 0;

    private bool tutFlag = false;

    void Start()
    {
        enemies.Add(crab);
        enemies.Add(manta);
        enemies.Add(turtle);
        enemies.Add(seaHorse);
        enemies.Add(lobster);
        enemies.Add(anglerfish);
        enemies.Add(octopus);
        enemies.Add(archerfish);
        enemies.Add(jellyfish);
        enemies.Add(sawfish);
        enemies.Add(beluga);
        enemies.Add(hammerhead);
        enemies.Add(eel);
        enemies.Add(mako);
        enemies.Add(blue);
        enemies.Add(greatWhite);
        enemies.Add(orca);
        enemies.Add(seal);

        spawnPoints.Add(spawnPointCBL);
        spawnPoints.Add(spawnPointFBL);
        spawnPoints.Add(spawnPointCTL);
        spawnPoints.Add(spawnPointFTL);
        spawnPoints.Add(spawnPointCBR);
        spawnPoints.Add(spawnPointFBR);
        spawnPoints.Add(spawnPointCTR);
        spawnPoints.Add(spawnPointFTR);

        scoreManager = GetComponent<ScoreManager>();

        if (PlayerPrefs.HasKey("TutFlag"))
        {
            if (PlayerPrefs.GetInt("TutFlag") == 0)
            {
                tutFlag = true;
            }
        }
    }


    void Update()
    {
        if (spawnFlag == true)
        {
            StartCoroutine(SpawnEnemy());
            spawnFlag = false;
        }

        tier = scoreManager.tier;
    }

    private IEnumerator SpawnEnemy()
    {
        if (tierHolder != tier)
        {
            semiTier = UnityEngine.Random.Range(0, 3);
            tierHolder = tier;

            int frenzyOdds = UnityEngine.Random.Range(0,4);
            if (frenzyOdds > 2)
            {
                scoreManager.frenzyFlag = true;
            }
        }


        float spawnDelay = 10f;
        int enemy = 12;


        switch (tier)
        {
            case 0:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(0, 3);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:
                            List<int> list1 = new List<int> { 0, 1, 3 };
                            int randomElement1 = list1[UnityEngine.Random.Range(0, list1.Count)];

                            spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                            enemy = randomElement1;
                            break;
                        case 1:
                            List<int> list2 = new List<int> { 1, 2, 4 };
                            int randomElement2 = list2[UnityEngine.Random.Range(0, list2.Count)];

                            spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                            enemy = randomElement2;
                            break;
                        case 2:
                            List<int> list3 = new List<int> { 0, 2, 3 };
                            int randomElement3 = list3[UnityEngine.Random.Range(0, list3.Count)];

                            spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                            enemy = randomElement3;
                            break;
                    }
                }

                // Crabs, mantas, seahorses

                break;
            case 1:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(0, 4);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:
                            List<int> list1 = new List<int> { 2, 3, 6 };
                            int randomElement1 = list1[UnityEngine.Random.Range(0, list1.Count)];

                            spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                            enemy = randomElement1;
                            break;
                        case 1:
                            List<int> list2 = new List<int> { 4, 5, 6, 7 };
                            int randomElement2 = list2[UnityEngine.Random.Range(0, list2.Count)];

                            spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                            enemy = randomElement2;
                            break;
                        case 2:
                            List<int> list3 = new List<int> { 0, 5, 3, 7 };
                            int randomElement3 = list3[UnityEngine.Random.Range(0, list3.Count)];

                            spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                            enemy = randomElement3;
                            break;
                    }
                }

                break;
            case 2:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(0, 5);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:
                            List<int> list1 = new List<int> { 4, 6, 8, 10 };
                            int randomElement1 = list1[UnityEngine.Random.Range(0, list1.Count)];

                            spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                            enemy = randomElement1;
                            break;
                        case 1:
                            List<int> list2 = new List<int> { 2, 7, 0, 10, 1 };
                            int randomElement2 = list2[UnityEngine.Random.Range(0, list2.Count)];

                            spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                            enemy = randomElement2;
                            break;
                        case 2:
                            List<int> list3 = new List<int> { 3, 9, 5 };
                            int randomElement3 = list3[UnityEngine.Random.Range(0, list3.Count)];

                            spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                            enemy = randomElement3;
                            break;
                    }
                }

                
                break;
            case 3:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(1, 6);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:

                            break;
                        case 1:

                            break;
                        case 2:

                            break;
                    }
                }

                
                break;
            case 4:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(2, 7);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:

                            break;
                        case 1:

                            break;
                        case 2:

                            break;
                    }
                }
                break;
            case 5:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(3, 9);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:

                            break;
                        case 1:

                            break;
                        case 2:

                            break;
                    }
                }
                break;
            case 6:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(4, 10);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:

                            break;
                        case 1:

                            break;
                        case 2:

                            break;
                    }
                }

                break;
            case 7:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(5, 11);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:

                            break;
                        case 1:

                            break;
                        case 2:

                            break;
                    }
                }
                break;
            case 8:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(6, 12);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:

                            break;
                        case 1:

                            break;
                        case 2:

                            break;
                    }
                }
                break;
            case 9:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(7, 12);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:

                            break;
                        case 1:

                            break;
                        case 2:

                            break;
                    }
                }
                break;
            case 10:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(8, 12);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:

                            break;
                        case 1:

                            break;
                        case 2:

                            break;
                    }
                }
                break;
            case 11:
                if (tutFlag)
                {
                    spawnDelay = (float)UnityEngine.Random.Range(5f, 10f);
                    enemy = (int)UnityEngine.Random.Range(9, 12);
                }
                else
                {
                    switch (semiTier)
                    {
                        case 0:

                            break;
                        case 1:

                            break;
                        case 2:

                            break;
                    }
                }
                break;
            default:
                // Handle cases outside of the specified range (if needed)
                break;
        }

        if (scoreManager.frenzyFlag)
        {
            spawnDelay = 15f;
            UnityEngine.Debug.LogError("FRENZY");
        }
        //...

        int spawnPoint = UnityEngine.Random.Range(0, spawnPoints.Count - 1);
        GameObject spawn = Instantiate(enemies[enemy], spawnPoints[spawnPoint], UnityEngine.Quaternion.identity);

        int squadOdds = UnityEngine.Random.Range(0, 10);
        if (squadOdds >= 8)
        {
            CreateSquad(spawn);
        }

        yield return new WaitForSeconds(spawnDelay);

        if (scoreManager.frenzyFlag)
        {
            scoreManager.frenzyFlag = false;
        }

        spawnFlag = true;
    }

    private void CreateSquad(GameObject enemy)
    {
        int squadSize = 1;
        List<int> squadSizes = new List<int>();

        if (enemy.name == "Crab(Clone)")
        {
            squadSizes.Add(3);
            squadSize = squadSizes[UnityEngine.Random.Range(0, squadSizes.Count)];

            Crab script = enemy.GetComponent<Crab>();

            UnityEngine.Vector3 spawnBar = new UnityEngine.Vector3(0f, 0f, 300f);
            for (int i = 0; i < squadSize - 1; i++)
            {
                GameObject subject = Instantiate(crab, enemy.transform.position + spawnBar, enemy.transform.rotation);
                script.subjects.Add(subject);
                spawnBar = spawnBar + new UnityEngine.Vector3(0f, 0f, 300f);
            }
        }
        else if (enemy.name == "Manta(Clone)")
        {
            squadSizes.Add(2);
            squadSizes.Add(5);
            squadSize = squadSizes[UnityEngine.Random.Range(0, squadSizes.Count)];

            Manta script = enemy.GetComponent<Manta>();

            UnityEngine.Vector3 spawnBar = new UnityEngine.Vector3(0f, 0f, 300f);
            for (int i = 0; i < squadSize - 1; i++)
            {
                GameObject subject = Instantiate(manta, enemy.transform.position + spawnBar, enemy.transform.rotation);
                script.subjects.Add(subject);
                spawnBar = spawnBar + new UnityEngine.Vector3(100f, 0f, 300f);
            }
        }
        else if (enemy.name == "Seahorse(Clone)")
        {
            squadSizes.Add(4);
            squadSize = squadSizes[UnityEngine.Random.Range(0, squadSizes.Count)];

            Seahorse script = enemy.GetComponent<Seahorse>();

            UnityEngine.Vector3 spawnBar = new UnityEngine.Vector3(0f, 0f, 300f);
            for (int i = 0; i < squadSize - 1; i++)
            {
                GameObject subject = Instantiate(seaHorse, enemy.transform.position + spawnBar, enemy.transform.rotation);
                script.subjects.Add(subject);
                spawnBar = spawnBar + new UnityEngine.Vector3(0f, 0f, 300f);
            }
        }
        else if (enemy.name == "Lobster(Clone)")
        {
            squadSizes.Add(5);
            squadSize = squadSizes[UnityEngine.Random.Range(0, squadSizes.Count)];

            Lobster script = enemy.GetComponent<Lobster>();

            UnityEngine.Vector3 spawnBar = new UnityEngine.Vector3(0f, 0f, 300f);
            for (int i = 0; i < squadSize - 1; i++)
            {
                GameObject subject = Instantiate(lobster, enemy.transform.position + spawnBar, enemy.transform.rotation);
                script.subjects.Add(subject);
                spawnBar = spawnBar + new UnityEngine.Vector3(0f, 0f, 300f);
            }
        }
        else if (enemy.name == "Beluga(Clone)")
        {
            squadSizes.Add(3);
            squadSize = squadSizes[UnityEngine.Random.Range(0, squadSizes.Count)];

            Beluga script = enemy.GetComponent<Beluga>();

            UnityEngine.Vector3 spawnBar = new UnityEngine.Vector3(0f, 0f, 300f);
            for (int i = 0; i < squadSize - 1; i++)
            {
                GameObject subject = Instantiate(beluga, enemy.transform.position + spawnBar, enemy.transform.rotation);
                script.subjects.Add(subject);
                spawnBar = spawnBar + new UnityEngine.Vector3(0f, 0f, 300f);
            }
        }
        //...
    }
}
