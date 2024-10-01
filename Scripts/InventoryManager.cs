using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;


public class InventoryManager : MonoBehaviour
{
    public GameObject pufferfish;

    public Camera myCamera;
    private float camMoveDuration = 1.5f;
    private float moveDuration = 10f;
    private UnityEngine.Vector3 targetPosition;
    private UnityEngine.Quaternion targetRotation;
    private bool shouldMove = false;
    private float moveStartTime;

    public GameObject vertCarousel;
    public GameObject pentCarousel;
    public GameObject shellCarousel;
    public GameObject costumeCarousel;
    private UnityEngine.Quaternion vertTargetRotation;
    private UnityEngine.Quaternion pentTargetRotation;
    private UnityEngine.Quaternion shellTargetRotation;
    private UnityEngine.Quaternion costumeTargetRotation;
    private UnityEngine.Vector3 pufferfishTargetPosition;

    private UnityEngine.Vector3 pentSeperation = new UnityEngine.Vector3(0f, 18f, 0f);
    private UnityEngine.Vector3 shellSeperation = new UnityEngine.Vector3(0f, 36f, 0f);
    private UnityEngine.Vector3 costumeSeperation = new UnityEngine.Vector3(0f, 18f, 0f);
    private UnityEngine.Vector3 vertSeperation = new UnityEngine.Vector3(0f, 27f, 0f);
    private UnityEngine.Vector3 pufferBuffer = new UnityEngine.Vector3(0f, 150f, 0f);

    private bool shouldRotateVert = false;
    private bool shouldRotatePent = false;
    private bool shouldRotateShell = false;
    private bool shouldRotateCostume = false;
    private bool shouldMovePufferfish = false;

    private GameObject es1 = null;
    private GameObject es2 = null;
    private GameObject es3 = null;
    private GameObject es4 = null;
    private GameObject es5 = null;
    private GameObject es6 = null;
    private GameObject es7 = null;
    private GameObject es8 = null;

    public GameObject pentInfo;
    public GameObject equipSlot;
    public GameObject lockIcon;
    public GameObject buyButton;
    public TextMeshProUGUI buyText;
    public GameObject levelUpMessage;
    public GameObject unavailableImage;
    public Canvas canvas;
    private GameObject newPentInfo;
    private float barHeight = 80f;
    private float maxRange = 100f;
    private float maxDam = 75f;
    private float maxDamRad = 20f;
    private float maxProjSpeed = 400f;
    private float maxReload = 10f;
    private int pentIdxHolder;
    private Coroutine pentCoroutine;
    private bool pentIdxFlag = false;
    private Color pentColor;
    private bool newShellFlag = true;

    private float spinSpeed = 20f;
    UnityEngine.Quaternion spinnerOffset = UnityEngine.Quaternion.Euler(0, 0, -34);
    UnityEngine.Quaternion spinnerOffsetNeg = UnityEngine.Quaternion.Euler(0, 0, 34);
    private int shellIdxHolder;
    private bool shellLock = false;

    public string currCarousel;
    private int pentIdx = 0;
    private int shellIdx = 0;
    private int costumeIdx;

    public GameObject costumeMessage;
    public Sprite noneImage;

    public TextMeshProUGUI xpText;
    public TextMeshProUGUI coinsText;
    public GameObject insufFundsPrompt;

    public List<int> equipped = new List<int>(new int[10]);
    private List<int> purchased = new List<int>();

    private Transform parent;

    private int level = 1;//remove after testing
    private int coins;
    private int xp;

    private bool infoFlag;
    private GameObject infoShell;

    private MusicController musicController;


    private void Start()
    {
        musicController = GameObject.Find("Audio").GetComponent<MusicController>();

        currCarousel = "shell";
        Deparent(currCarousel);

        parent = vertCarousel.transform;

        vertTargetRotation = vertCarousel.transform.rotation;
        pentTargetRotation = shellCarousel.transform.rotation;
        shellTargetRotation = shellCarousel.transform.rotation;
        costumeTargetRotation = shellCarousel.transform.rotation;

        ResetEquipped();

        level = PlayerPrefs.GetInt("Level");
        coins = PlayerPrefs.GetInt("Coins");
        xp = PlayerPrefs.GetInt("XP");

        coinsText.text = coins.ToString();

        int levelThresh = GetLevelThresh(level);
        xpText.text = xp.ToString() + "/" + levelThresh.ToString();

        GetPurchased();

        if (ScoreManager.newLevelFlag)
        {
            StartCoroutine(ShowLevelUp());
        }

        //DataMove.Instance.CurrentShellType = ShellType.Null;

        DataMove.Instance.CurrentPentTypes = new List<string>();

        infoShell = pufferfish.transform.Find("PFC_info_shell").gameObject;
    }

    private void Update()
    {
        if (shouldMove)
        {
            float t = (Time.time - moveStartTime) / camMoveDuration;

            myCamera.transform.position = UnityEngine.Vector3.Lerp(myCamera.transform.position, targetPosition, t);
            myCamera.transform.rotation = UnityEngine.Quaternion.Slerp(myCamera.transform.rotation, targetRotation, t);

            if (t >= 1)
                shouldMove = false;
        }

        if (shouldRotateVert)
        {
            vertCarousel.transform.rotation = UnityEngine.Quaternion.Slerp(vertCarousel.transform.rotation, vertTargetRotation, moveDuration * Time.deltaTime);
            if (UnityEngine.Quaternion.Angle(vertCarousel.transform.rotation, vertTargetRotation) < 0.1f)
            {
                vertCarousel.transform.rotation = vertTargetRotation;
                Deparent(currCarousel);
                shouldRotateVert = false;

            }
        }

        if (shouldRotatePent)
        {
            //CheckPentLocked();
            pentCarousel.transform.rotation = UnityEngine.Quaternion.Slerp(pentCarousel.transform.rotation, pentTargetRotation, moveDuration * Time.deltaTime);
            if (UnityEngine.Quaternion.Angle(pentCarousel.transform.rotation, pentTargetRotation) < 0.1f)
            {
                pentCarousel.transform.rotation = pentTargetRotation;  // Snap to the target rotation
                shouldRotatePent = false;
                
            }
        }

        if (shouldRotateShell)
        {
            shellCarousel.transform.rotation = UnityEngine.Quaternion.Slerp(shellCarousel.transform.rotation, shellTargetRotation, moveDuration * Time.deltaTime);
            if (UnityEngine.Quaternion.Angle(shellCarousel.transform.rotation, shellTargetRotation) < 0.1f)
            {
                shellCarousel.transform.rotation = shellTargetRotation;  // Snap to the target rotation
                shouldRotateShell = false;
            }
        }


        if (shouldRotateCostume)
        {
            costumeCarousel.transform.rotation = UnityEngine.Quaternion.Slerp(costumeCarousel.transform.rotation, costumeTargetRotation, moveDuration * Time.deltaTime);
            if (UnityEngine.Quaternion.Angle(costumeCarousel.transform.rotation, costumeTargetRotation) < 0.1f)
            {
                costumeCarousel.transform.rotation = costumeTargetRotation;  // Snap to the target rotation
                shouldRotateCostume = false;
            }
        }

        if (shouldMovePufferfish)
        {
            pufferfish.transform.position = UnityEngine.Vector3.Lerp(pufferfish.transform.position, pufferfishTargetPosition, moveDuration * Time.deltaTime);
            if (UnityEngine.Vector3.Distance(pufferfish.transform.position, pufferfishTargetPosition) < 1f)
            {
                pufferfish.transform.position = pufferfishTargetPosition;  // Snap to the target rotation
                shouldMovePufferfish = false;
            }
        }

        if (currCarousel == "pent")
        {
            if ((pentIdxHolder != pentIdx) || !pentIdxFlag)
            {
                if (!pentIdxFlag)
                {
                    pentIdxFlag = true;
                    //Instantiate equip slots based on shellIdx
                    if (newShellFlag)
                    {
                        if (shellIdx == 0)
                        {
                            es1 = Instantiate(equipSlot, canvas.transform);
                            es1.transform.position = new UnityEngine.Vector3(1266f-225f, 100f, 0f);
                            Button es1Button = es1.GetComponent<Button>();
                            es1Button.onClick.AddListener(() => EquipButton(es1));
                            TextMeshProUGUI t1 = es1.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t1.text = "1";

                            es2 = Instantiate(equipSlot, canvas.transform);
                            es2.transform.position = new UnityEngine.Vector3(1266f - 75f, 100f, 0f);
                            Button es2Button = es2.GetComponent<Button>();
                            es2Button.onClick.AddListener(() => EquipButton(es2));
                            TextMeshProUGUI t2 = es2.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t2.text = "3";

                            es3 = Instantiate(equipSlot, canvas.transform);
                            es3.transform.position = new UnityEngine.Vector3(1266f+75f, 100f, 0f);
                            Button es3Button = es3.GetComponent<Button>();
                            es3Button.onClick.AddListener(() => EquipButton(es3));
                            TextMeshProUGUI t3 = es3.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t3.text = "8";

                            es4 = Instantiate(equipSlot, canvas.transform);
                            es4.transform.position = new UnityEngine.Vector3(1266f+225f, 100f, 0f);
                            Button es4Button = es4.GetComponent<Button>();
                            es4Button.onClick.AddListener(() => EquipButton(es4));
                            TextMeshProUGUI t4 = es4.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t4.text = "9";
                        }
                        else if (shellIdx == 1)
                        {
                            es1 = Instantiate(equipSlot, canvas.transform);
                            es1.transform.position = new UnityEngine.Vector3(1266f - 300f, 100f, 0f);
                            Button es1Button = es1.GetComponent<Button>();
                            es1Button.onClick.AddListener(() => EquipButton(es1));
                            TextMeshProUGUI t1 = es1.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t1.text = "1";

                            es2 = Instantiate(equipSlot, canvas.transform);
                            es2.transform.position = new UnityEngine.Vector3(1266f - 150f, 100f, 0f);
                            Button es2Button = es2.GetComponent<Button>();
                            es2Button.onClick.AddListener(() => EquipButton(es2));
                            TextMeshProUGUI t2 = es2.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t2.text = "3";

                            es3 = Instantiate(equipSlot, canvas.transform);
                            es3.transform.position = new UnityEngine.Vector3(1266f, 100f, 0f);
                            Button es3Button = es3.GetComponent<Button>();
                            es3Button.onClick.AddListener(() => EquipButton(es3));
                            TextMeshProUGUI t3 = es3.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t3.text = "5";

                            es4 = Instantiate(equipSlot, canvas.transform);
                            es4.transform.position = new UnityEngine.Vector3(1266f + 150f, 100f, 0f);
                            Button es4Button = es4.GetComponent<Button>();
                            es4Button.onClick.AddListener(() => EquipButton(es4));
                            TextMeshProUGUI t4 = es4.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t4.text = "6";

                            es5 = Instantiate(equipSlot, canvas.transform);
                            es5.transform.position = new UnityEngine.Vector3(1266f + 300f, 100f, 0f);
                            Button es5Button = es5.GetComponent<Button>();
                            es5Button.onClick.AddListener(() => EquipButton(es5));
                            TextMeshProUGUI t5 = es5.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t5.text = "7";
                        }
                        else if (shellIdx == 2)
                        {
                            es1 = Instantiate(equipSlot, canvas.transform);
                            es1.transform.position = new UnityEngine.Vector3(1266f - 375f, 100f, 0f);
                            Button es1Button = es1.GetComponent<Button>();
                            es1Button.onClick.AddListener(() => EquipButton(es1));
                            TextMeshProUGUI t1 = es1.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t1.text = "1";

                            es2 = Instantiate(equipSlot, canvas.transform);
                            es2.transform.position = new UnityEngine.Vector3(1266f - 225f, 100f, 0f);
                            Button es2Button = es2.GetComponent<Button>();
                            es2Button.onClick.AddListener(() => EquipButton(es2));
                            TextMeshProUGUI t2 = es2.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t2.text = "3";

                            es3 = Instantiate(equipSlot, canvas.transform);
                            es3.transform.position = new UnityEngine.Vector3(1266f - 75f, 100f, 0f);
                            Button es3Button = es3.GetComponent<Button>();
                            es3Button.onClick.AddListener(() => EquipButton(es3));
                            TextMeshProUGUI t3 = es3.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t3.text = "7";

                            es4 = Instantiate(equipSlot, canvas.transform);
                            es4.transform.position = new UnityEngine.Vector3(1266f+75f, 100f, 0f);
                            Button es4Button = es4.GetComponent<Button>();
                            es4Button.onClick.AddListener(() => EquipButton(es4));
                            TextMeshProUGUI t4 = es4.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t4.text = "8";

                            es5 = Instantiate(equipSlot, canvas.transform);
                            es5.transform.position = new UnityEngine.Vector3(1266f+225f, 100f, 0f);
                            Button es5Button = es5.GetComponent<Button>();
                            es5Button.onClick.AddListener(() => EquipButton(es5));
                            TextMeshProUGUI t5 = es5.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t5.text = "9";

                            es6 = Instantiate(equipSlot, canvas.transform);
                            es6.transform.position = new UnityEngine.Vector3(1266f+375f, 100f, 0f);
                            Button es6Button = es6.GetComponent<Button>();
                            es6Button.onClick.AddListener(() => EquipButton(es6));
                            TextMeshProUGUI t6 = es6.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t6.text = "10";
                        }
                        else if (shellIdx == 3)
                        {
                            es1 = Instantiate(equipSlot, canvas.transform);
                            es1.transform.position = new UnityEngine.Vector3(1266f-375f, 100f, 0f);
                            Button es1Button = es1.GetComponent<Button>();
                            es1Button.onClick.AddListener(() => EquipButton(es1));
                            TextMeshProUGUI t1 = es1.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t1.text = "1";

                            es2 = Instantiate(equipSlot, canvas.transform);
                            es2.transform.position = new UnityEngine.Vector3(1266f-225f, 100f, 0f);
                            Button es2Button = es2.GetComponent<Button>();
                            es2Button.onClick.AddListener(() => EquipButton(es2));
                            TextMeshProUGUI t2 = es2.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t2.text = "3";

                            es3 = Instantiate(equipSlot, canvas.transform);
                            es3.transform.position = new UnityEngine.Vector3(1266f-75f, 100f, 0f);
                            Button es3Button = es3.GetComponent<Button>();
                            es3Button.onClick.AddListener(() => EquipButton(es3));
                            TextMeshProUGUI t3 = es3.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t3.text = "5";

                            es4 = Instantiate(equipSlot, canvas.transform);
                            es4.transform.position = new UnityEngine.Vector3(1266f+75f, 100f, 0f);
                            Button es4Button = es4.GetComponent<Button>();
                            es4Button.onClick.AddListener(() => EquipButton(es4));
                            TextMeshProUGUI t4 = es4.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t4.text = "6";

                            es5 = Instantiate(equipSlot, canvas.transform);
                            es5.transform.position = new UnityEngine.Vector3(1266f+225f, 100f, 0f);
                            Button es5Button = es5.GetComponent<Button>();
                            es5Button.onClick.AddListener(() => EquipButton(es5));
                            TextMeshProUGUI t5 = es5.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t5.text = "7";

                            es6 = Instantiate(equipSlot, canvas.transform);
                            es6.transform.position = new UnityEngine.Vector3(1266f+375f, 100f, 0f);
                            Button es6Button = es6.GetComponent<Button>();
                            es6Button.onClick.AddListener(() => EquipButton(es6));
                            TextMeshProUGUI t6 = es6.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t6.text = "10";
                        }
                        else if (shellIdx == 4)
                        {
                            es1 = Instantiate(equipSlot, canvas.transform);
                            es1.transform.position = new UnityEngine.Vector3(1266f-525f, 100f, 0f);
                            Button es1Button = es1.GetComponent<Button>();
                            es1Button.onClick.AddListener(() => EquipButton(es1));
                            TextMeshProUGUI t1 = es1.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t1.text = "1";

                            es2 = Instantiate(equipSlot, canvas.transform);
                            es2.transform.position = new UnityEngine.Vector3(1266f-375f, 100f, 0f);
                            Button es2Button = es2.GetComponent<Button>();
                            es2Button.onClick.AddListener(() => EquipButton(es2));
                            TextMeshProUGUI t2 = es2.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t2.text = "2";

                            es3 = Instantiate(equipSlot, canvas.transform);
                            es3.transform.position = new UnityEngine.Vector3(1266f-225f, 100f, 0f);
                            Button es3Button = es3.GetComponent<Button>();
                            es3Button.onClick.AddListener(() => EquipButton(es3));
                            TextMeshProUGUI t3 = es3.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t3.text = "3";

                            es4 = Instantiate(equipSlot, canvas.transform);
                            es4.transform.position = new UnityEngine.Vector3(1266f - 75f, 100f, 0f);
                            Button es4Button = es4.GetComponent<Button>();
                            es4Button.onClick.AddListener(() => EquipButton(es4));
                            TextMeshProUGUI t4 = es4.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t4.text = "4";

                            es5 = Instantiate(equipSlot, canvas.transform);
                            es5.transform.position = new UnityEngine.Vector3(1266f+75f, 100f, 0f);
                            Button es5Button = es5.GetComponent<Button>();
                            es5Button.onClick.AddListener(() => EquipButton(es5));
                            TextMeshProUGUI t5 = es5.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t5.text = "5";

                            es6 = Instantiate(equipSlot, canvas.transform);
                            es6.transform.position = new UnityEngine.Vector3(1266f+225f, 100f, 0f);
                            Button es6Button = es6.GetComponent<Button>();
                            es6Button.onClick.AddListener(() => EquipButton(es6));
                            TextMeshProUGUI t6 = es6.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t6.text = "6";


                            es7 = Instantiate(equipSlot, canvas.transform);
                            es7.transform.position = new UnityEngine.Vector3(1266f+375f, 100f, 0f);
                            Button es7Button = es7.GetComponent<Button>();
                            es7Button.onClick.AddListener(() => EquipButton(es7));
                            TextMeshProUGUI t7 = es7.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t7.text = "7";

                            es8 = Instantiate(equipSlot, canvas.transform);
                            es8.transform.position = new UnityEngine.Vector3(1266f+525f, 100f, 0f);
                            Button es8Button = es8.GetComponent<Button>();
                            es8Button.onClick.AddListener(() => EquipButton(es8));
                            TextMeshProUGUI t8 = es8.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
                            t8.text = "10";
                        }
                    }
                    else
                    {
                        ActivateES(true);
                    }

                    
                }

                if (newPentInfo != null)
                {
                    Destroy(newPentInfo);
                }
                pentCoroutine = StartCoroutine(InstantiatePentInfo(pentIdx));
            }
            CheckPentLocked();
            //Spinner
            Transform spinner = pentCarousel.transform.GetChild(pentIdx);
            spinner.Rotate(0, spinSpeed * Time.deltaTime, 0);
        }
        else if (currCarousel == "shell")
        {
            if (newPentInfo != null)
            {
                Destroy(newPentInfo);
                pentIdxFlag = false;

                ActivateES(false);
                newShellFlag = false;
            }

            CheckShellLocked();
            //Spinner
            Transform spinner = pufferfish.transform;
            spinner.Rotate(0, 0, spinSpeed * Time.deltaTime);

            Transform shellTransform = shellCarousel.transform.GetChild(shellIdx);
            if (shellIdxHolder < shellIdx)
            {
                shellTransform.rotation = spinner.rotation * spinnerOffset;

                Transform hat = pufferfish.transform.Find("Details").Find("Cube.015");
                if ((shellIdx == 2) || (shellIdx == 3) || (shellIdx == 4))
                {
                    hat.gameObject.SetActive(false);
                }
                else
                {
                    hat.gameObject.SetActive(true);
                }
            }
            else if (shellIdxHolder > shellIdx)
            {
                shellTransform.rotation = spinner.rotation * spinnerOffsetNeg;

                Transform hat = pufferfish.transform.Find("Details").Find("Cube.015");
                if ((shellIdx == 2) || (shellIdx == 3) || (shellIdx == 4))
                {
                    hat.gameObject.SetActive(false);
                }
                else
                {
                    hat.gameObject.SetActive(true);
                }
            }

            shellTransform.Rotate(0, 0, spinSpeed * Time.deltaTime);

            shellIdxHolder = shellIdx;
        }

        if (currCarousel == "costume")
        {
            costumeMessage.SetActive(true);
        }
        else
        {
            costumeMessage.SetActive(false);
        }
    }

    public void LeftButton()
    {
        if (infoFlag)
        {
            Transform hat = pufferfish.transform.Find("Details").Find("Cube.015");
            hat.gameObject.SetActive(true);

            infoShell.SetActive(false);
            shellCarousel.SetActive(true);
            vertCarousel.SetActive(true);
            infoFlag = false;
        }
        else
        {
            if (!shouldRotateVert)
            {
                if (currCarousel == "pent")
                {
                    if (pentIdx > 0)
                    {
                        pentTargetRotation *= UnityEngine.Quaternion.Euler(-pentSeperation);
                        pentIdx -= 1;
                        shouldRotatePent = true;
                    }
                }
                else if (currCarousel == "shell")
                {
                    if (shellIdx > 0)
                    {
                        shellTargetRotation *= UnityEngine.Quaternion.Euler(-shellSeperation);
                        shellIdx -= 1;
                        shouldRotateShell = true;
                        newShellFlag = true;
                        DestroyES();
                    }
                }
                else if (currCarousel == "costume")
                {
                    if (costumeIdx > 0)
                    {
                        costumeTargetRotation *= UnityEngine.Quaternion.Euler(-costumeSeperation);
                        costumeIdx -= 1;
                        shouldRotateCostume = true;
                    }
                }
            }
            //make end of carousel line

            StartCoroutine(musicController.PlaySoundEffect("CarouselChange"));
        }

    }

    public void RightButton()
    {
        if (infoFlag)
        {
            Transform hat = pufferfish.transform.Find("Details").Find("Cube.015");
            hat.gameObject.SetActive(true);

            infoShell.SetActive(false);
            shellCarousel.SetActive(true);
            vertCarousel.SetActive(true);
            infoFlag = false;
        }
        else
        {
            if (!shouldRotateVert)
            {
                if (currCarousel == "pent")
                {
                    if (pentIdx < pentCarousel.transform.childCount - 1)
                    {
                        pentTargetRotation *= UnityEngine.Quaternion.Euler(pentSeperation);
                        pentIdx += 1;
                        shouldRotatePent = true;
                    }
                }
                else if (currCarousel == "shell")
                {
                    if (shellIdx < shellCarousel.transform.childCount - 1)
                    {
                        shellTargetRotation *= UnityEngine.Quaternion.Euler(shellSeperation);
                        shellIdx += 1;
                        shouldRotateShell = true;
                        newShellFlag = true;
                        DestroyES();
                    }
                }
                else if (currCarousel == "costume")
                {
                    if (costumeIdx < costumeCarousel.transform.childCount - 1)
                    {
                        costumeTargetRotation *= UnityEngine.Quaternion.Euler(costumeSeperation);
                        costumeIdx += 1;
                        shouldRotateCostume = true;
                    }
                }
            }
            //make end of line

            StartCoroutine(musicController.PlaySoundEffect("CarouselChange"));
        }

    }

    public void UpButton()
    {
        if (infoFlag)
        {
            Transform hat = pufferfish.transform.Find("Details").Find("Cube.015");
            hat.gameObject.SetActive(true);

            infoShell.SetActive(false);
            shellCarousel.SetActive(true);
            vertCarousel.SetActive(true);
            infoFlag = false;
        }
        else
        {
            if (currCarousel == "shell")
            {
                if (!shellLock)
                {
                    MoveAndRotateCamera(0);
                    Reparent(currCarousel);
                    vertTargetRotation *= UnityEngine.Quaternion.Euler(vertSeperation);
                    shouldRotateVert = true;
                    pufferfishTargetPosition = pufferfish.transform.position - pufferBuffer;
                    shouldMovePufferfish = true;
                    currCarousel = "pent";
                }

            }
            else if (currCarousel == "costume")
            {
                Reparent(currCarousel);
                vertTargetRotation *= UnityEngine.Quaternion.Euler(vertSeperation);
                shouldRotateVert = true;
                currCarousel = "shell";

            }

            StartCoroutine(musicController.PlaySoundEffect("VertCarouselChange"));
        }
        
    }

    public void DownButton()
    {
        if (infoFlag)
        {
            Transform hat = pufferfish.transform.Find("Details").Find("Cube.015");
            hat.gameObject.SetActive(true);

            infoShell.SetActive(false);
            shellCarousel.SetActive(true);
            vertCarousel.SetActive(true);
            infoFlag = false;
        }
        else
        {
            if (currCarousel == "shell")
            {
                if (!shellLock)
                {
                    Reparent(currCarousel);
                    vertTargetRotation *= UnityEngine.Quaternion.Euler(-vertSeperation);
                    shouldRotateVert = true;
                    currCarousel = "costume";
                }
            }
            else if (currCarousel == "pent")
            {
                MoveAndRotateCamera(1);
                Reparent(currCarousel);
                vertTargetRotation *= UnityEngine.Quaternion.Euler(-vertSeperation);
                shouldRotateVert = true;
                pufferfishTargetPosition = pufferfish.transform.position + pufferBuffer;
                shouldMovePufferfish = true;
                currCarousel = "shell";
            }

            StartCoroutine(musicController.PlaySoundEffect("VertCarouselChange"));
        }
        
    }

    public void PlayButton()
    {
        if (infoFlag)
        {
            Transform hat = pufferfish.transform.Find("Details").Find("Cube.015");
            hat.gameObject.SetActive(true);

            infoShell.SetActive(false);
            shellCarousel.SetActive(true);
            vertCarousel.SetActive(true);
            infoFlag = false;
        }
        else
        {
            if (currCarousel == "shell")
            {
                StartCoroutine(musicController.PlaySoundEffect("StartGame"));

                SaveInventory();

                StartCoroutine(GameSceneTransition());
            }
        }
        
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
            yield return new WaitForSeconds(2f);

            StartCoroutine(musicController.PlaySoundEffect("PanelClose"));

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

    public void BuyButton()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        SavePurchased();
        CheckPentLocked();
    }

    public void BackButton()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        SceneManager.LoadScene("MainMenu");
    }

    public void InfoButton()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        if (infoFlag)
        {
            Transform hat = pufferfish.transform.Find("Details").Find("Cube.015");
            hat.gameObject.SetActive(true);

            infoShell.SetActive(false);
            shellCarousel.SetActive(true);
            vertCarousel.SetActive(true);
            infoFlag = false;
        }
        else
        {
            if (currCarousel == "pent")
            {
                DownButton();
            }
            else if (currCarousel == "costume")
            {
                UpButton();
            }

            Transform hat = pufferfish.transform.Find("Details").Find("Cube.015");
            hat.gameObject.SetActive(false);

            infoShell.SetActive(true);
            shellCarousel.SetActive(false);
            vertCarousel.SetActive(false);
            infoFlag = true;
        }
        
    }

    void EquipButton(GameObject slot)
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        TextMeshProUGUI slotNumText = slot.transform.Find("SlotNum").GetComponent<TextMeshProUGUI>();
        int slotIdx = int.Parse(slotNumText.text);


        bool equipFlag = false;
        foreach (int e in equipped)
        {
            if (e == pentIdx)
            {
                equipped[slotIdx - 1] = -1;
                Image image = slot.GetComponent<Image>();
                image.color = Color.white;
                equipFlag = true;
            }
        }
        if (equipFlag == false)
        {
            equipped[slotIdx - 1] = pentIdx;
            Image image = slot.GetComponent<Image>();
            image.color = pentColor;
        }
    }

    public void LoadSettings()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        SceneManager.LoadScene("SettingsScene");
    }

    private void Deparent(string curr)
    {
        if (curr == "pent")
        {
            pentCarousel.transform.parent = null;
        }
        else if (curr == "shell")
        {
            shellCarousel.transform.parent = null;
        }
        else if (curr == "costume")
        {
            costumeCarousel.transform.parent = null;
        }
    }

    private void Reparent(string curr)
    {
        if (curr == "pent")
        {
            pentCarousel.transform.parent = parent;
        }
        else if (curr == "shell")
        {
            shellCarousel.transform.parent = parent;
        }
        else if (curr == "costume")
        {
            costumeCarousel.transform.parent = parent;
        }
    }

    private void SaveInventory()
    {
        if (shellIdx == 0)
        {
            DataMove.Instance.CurrentShellType = ShellType.Four;
        }
        else if (shellIdx == 1)
        {
            DataMove.Instance.CurrentShellType = ShellType.Five;
        }
        else if (shellIdx == 2)
        {
            DataMove.Instance.CurrentShellType = ShellType.SixA;
        }
        else if (shellIdx == 3)
        {
            DataMove.Instance.CurrentShellType = ShellType.SixB;
        }
        else if (shellIdx == 4)
        {
            DataMove.Instance.CurrentShellType = ShellType.Eight;
        }


        foreach (int e in equipped)
        {
            if (e == -1)
            {
                DataMove.Instance.CurrentPentTypes.Add("None");
            }
            else if (e == 0)
            {
                DataMove.Instance.CurrentPentTypes.Add("Lamp");
            }
            else if (e == 1)
            {
                DataMove.Instance.CurrentPentTypes.Add("Hook");
            }
            else if (e == 2)
            {
                DataMove.Instance.CurrentPentTypes.Add("Spring");
            }
            else if (e == 3)
            {
                DataMove.Instance.CurrentPentTypes.Add("TomahawkLauncher");
            }
            else if (e == 4)
            {
                DataMove.Instance.CurrentPentTypes.Add("Turret");
            }
            else if (e == 5)
            {
                DataMove.Instance.CurrentPentTypes.Add("Turbine");
            }
            else if (e == 6)
            {
                DataMove.Instance.CurrentPentTypes.Add("BubbleGun");
            }
            else if (e == 7)
            {
                DataMove.Instance.CurrentPentTypes.Add("NetGun");
            }
            else if (e == 8)
            {
                DataMove.Instance.CurrentPentTypes.Add("RocketLauncher");
            }
            else if (e == 9)
            {
                DataMove.Instance.CurrentPentTypes.Add("HarpoonLauncher");
            }
            else if (e == 10)
            {
                DataMove.Instance.CurrentPentTypes.Add("Umbrella");
            }
            else if (e == 11)
            {
                DataMove.Instance.CurrentPentTypes.Add("PoisonGun");
            }
            else if (e == 12)
            {
                DataMove.Instance.CurrentPentTypes.Add("HeatseekerLauncher");
            }
            else if (e == 13)
            {
                DataMove.Instance.CurrentPentTypes.Add("FlareGun");
            }
            else if (e == 14)
            {
                DataMove.Instance.CurrentPentTypes.Add("EMWG");
            }
            else if (e == 15)
            {
                DataMove.Instance.CurrentPentTypes.Add("Hypnotizer");
            }
            else if (e == 16)
            {
                DataMove.Instance.CurrentPentTypes.Add("SeamineLauncher");
            }
        }
    }

    public void MoveAndRotateCamera(int cmd)
    {
        if (cmd == 0)
        {
            UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(7f, 0f, 0f);
            targetPosition = myCamera.transform.position + new UnityEngine.Vector3(0f, 100f, 250f);
            targetRotation = myCamera.transform.rotation * rot;

            moveStartTime = Time.time;
            shouldMove = true;
        }
        else if (cmd == 1)
        {
            UnityEngine.Quaternion rot = UnityEngine.Quaternion.Euler(-7f, 0f, 0f);
            targetPosition = myCamera.transform.position - new UnityEngine.Vector3(0f, 100f, 250f);
            targetRotation = myCamera.transform.rotation * rot;

            moveStartTime = Time.time;
            shouldMove = true;
        }
        
    }

    private IEnumerator InstantiatePentInfo(int pentIdx)
    {
        pentIdxHolder = pentIdx;
        PentInfo[] pentInfos = Resources.LoadAll<PentInfo>("PentInfos");
        List<PentInfo> pentInfosList = new List<PentInfo>(pentInfos); ;
        pentInfosList = pentInfosList.OrderBy(x => x.name).ToList();
        PentInfo pentInfoData = pentInfosList[pentIdx];
        
        newPentInfo = Instantiate(pentInfo, canvas.transform);
        newPentInfo.transform.position = new UnityEngine.Vector3(1266f, 900f, 0f);
        Image backImage = newPentInfo.GetComponent<Image>();
        pentColor = new Color(pentInfoData.color.r, pentInfoData.color.g, pentInfoData.color.b, 1f);
        backImage.color = pentColor;

        TextMeshProUGUI pentName = newPentInfo.transform.Find("PentName").GetComponent<TextMeshProUGUI>();
        pentName.text = pentInfoData.title;

        TextMeshProUGUI rScoreA = newPentInfo.transform.Find("RScoreA").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI rScoreB = newPentInfo.transform.Find("RScoreB").GetComponent<TextMeshProUGUI>();
        rScoreA.text = pentInfoData.range[0].ToString();
        rScoreB.text = pentInfoData.range[1].ToString();

        TextMeshProUGUI dScore = newPentInfo.transform.Find("DScore").GetComponent<TextMeshProUGUI>();
        dScore.text = pentInfoData.damage.ToString();

        TextMeshProUGUI drScore = newPentInfo.transform.Find("DrScore").GetComponent<TextMeshProUGUI>();
        drScore.text = pentInfoData.damageRadius.ToString();

        TextMeshProUGUI psScore = newPentInfo.transform.Find("PsScore").GetComponent<TextMeshProUGUI>();
        psScore.text = pentInfoData.projectileSpeed.ToString();

        TextMeshProUGUI rlScore = newPentInfo.transform.Find("RlScore").GetComponent<TextMeshProUGUI>();
        rlScore.text = pentInfoData.reloadTime.ToString();

        TextMeshProUGUI projText = newPentInfo.transform.Find("ProjText").GetComponent<TextMeshProUGUI>();
        projText.text = pentInfoData.projectile;

        Image projImage = newPentInfo.transform.Find("ProjImage").GetComponent<Image>();
        if (projText.text == "None")
        {
            projImage.sprite = noneImage;
        }
        else
        {
            Sprite[] images = Resources.LoadAll<Sprite>("Images");
            Sprite image = images[pentIdx];
            projImage.sprite = image;
        }

        
        Image rangeBar = newPentInfo.transform.Find("RangeBar").GetComponent<Image>();
        RectTransform rectTransformA = rangeBar.GetComponent<RectTransform>();

        barHeight = rectTransformA.sizeDelta.y;
        float range = (pentInfoData.range[1] - pentInfoData.range[0]);

        rectTransformA.sizeDelta = new UnityEngine.Vector2(rectTransformA.sizeDelta.x, (range / maxRange) * barHeight);

        Image damBar = newPentInfo.transform.Find("DamBar").GetComponent<Image>();
        RectTransform rectTransformB = damBar.GetComponent<RectTransform>();
        rectTransformB.sizeDelta = new UnityEngine.Vector2(rectTransformB.sizeDelta.x, (pentInfoData.damage / maxDam) * barHeight);

        Image damRadBar = newPentInfo.transform.Find("DamRadBar").GetComponent<Image>();
        RectTransform rectTransformC = damRadBar.GetComponent<RectTransform>();
        rectTransformC.sizeDelta = new UnityEngine.Vector2(rectTransformC.sizeDelta.x, (pentInfoData.damageRadius / maxDamRad) * barHeight);

        Image projSpeedBar = newPentInfo.transform.Find("ProjSpeedBar").GetComponent<Image>();
        RectTransform rectTransformD = projSpeedBar.GetComponent<RectTransform>();
        rectTransformD.sizeDelta = new UnityEngine.Vector2(rectTransformD.sizeDelta.x, (pentInfoData.projectileSpeed / maxProjSpeed) * barHeight);

        Image reloadBar = newPentInfo.transform.Find("ReloadBar").GetComponent<Image>();
        RectTransform rectTransformE = reloadBar.GetComponent<RectTransform>();
        rectTransformE.sizeDelta = new UnityEngine.Vector2(rectTransformE.sizeDelta.x, ((maxReload - pentInfoData.reloadTime) / maxReload) * barHeight);

        yield return null;
         
    }

    private void CheckPentLocked()
    {
        lockIcon.SetActive(false);
        buyButton.SetActive(false);

        ActivateES(true);

        if (pentIdx == 9)
        {
            unavailableImage.SetActive(true);
            lockIcon.SetActive(false);
            ActivateES(false);
            newPentInfo.SetActive(false);
        }
        else
        {
            unavailableImage.SetActive(false);

            if ((level == 1 && pentIdx > 3) || (level <= 3 && pentIdx > 5) || (level <= 4 && pentIdx > 6) || (level <= 5 && pentIdx > 7)
            || (level <= 7 && pentIdx > 8) || (level <= 9 && pentIdx > 10) || (level <= 10 && pentIdx > 11) || (level <= 12 && pentIdx > 13)
            || (level <= 15 && pentIdx > 14))
            {
                lockIcon.SetActive(true);
                ActivateES(false);
                newPentInfo.SetActive(false);
            }
            else
            {
                bool purchasedFlag = false;
                for (int i = 0; i < purchased.Count; i++)
                {
                    if (pentIdx == purchased[i])
                    {
                        purchasedFlag = true;
                    }
                }

                if (!purchasedFlag)
                {
                    buyButton.SetActive(true);
                    ActivateES(false);

                    int pentCost = GetPentCost(pentIdx);
                    if (pentCost > 0)
                    {
                        buyText.text = GetPentCost(pentIdx).ToString();
                    }
                    else
                    {
                        buyText.text = "FREE";
                    }
                }

            }
        }

    }

    private void CheckShellLocked()
    {
        lockIcon.SetActive(false);
        shellLock = false;
        buyButton.SetActive(false);

        if ((level <= 3 && shellIdx > 0) || (level <= 8 && shellIdx > 1) || (level <= 13 && shellIdx > 3))
        {
            shellLock = true;
            lockIcon.SetActive(true);
        }

    }

    private void GetPurchased()
    {
        string purchasedString = PlayerPrefs.GetString("Purchased");

        for (int i = 0; i < purchasedString.Length; i++)
        {
            purchased.Add((int)purchasedString[i]);
            
        }
    }

    private void SavePurchased()
    {
        int coinHolder = coins - GetPentCost(pentIdx);

        if (coinHolder >= 0)
        {
            purchased.Add(pentIdx);
            char[] charArray = new char[purchased.Count];

            for (int i = 0; i < purchased.Count; i++)
            {
                charArray[i] = (char)(purchased[i]);
            }

            // Convert the char array to a string
            string purchasedString = new string(charArray);
            PlayerPrefs.SetString("Purchased", purchasedString);

            

            coins -= GetPentCost(pentIdx);
            coinsText.text = coins.ToString();
            PlayerPrefs.SetInt("Coins", coins);

            PlayerPrefs.Save();
        }
        else
        {
            StartCoroutine(InsufficientFunds());
        }
        
    }

    private IEnumerator InsufficientFunds()
    {
        insufFundsPrompt.SetActive(true);

        yield return new WaitForSeconds(2f);

        insufFundsPrompt.SetActive(false);

        yield return null;
    }

    private void DestroyES()
    {
        if (es1)
        {
            Destroy(es1);
        }

        if (es2)
        {
            Destroy(es2);
        }

        if (es3)
        {
            Destroy(es3);
        }

        if (es4)
        {
            Destroy(es4);
        }

        if (es5)
        {
            Destroy(es5);
        }

        if (es6)
        {
            Destroy(es6);
        }

        if (es7)
        {
            Destroy(es7);
        }

        if (es8)
        {
            Destroy(es8);
        }

        ResetEquipped();
    }

    private void ActivateES(bool flag)
    {
        if (es1)
        {
            es1.SetActive(flag);
        }

        if (es2)
        {
            es2.SetActive(flag);
        }

        if (es3)
        {
            es3.SetActive(flag);
        }

        if (es4)
        {
            es4.SetActive(flag);
        }

        if (es5)
        {
            es5.SetActive(flag);
        }

        if (es6)
        {
            es6.SetActive(flag);
        }

        if (es7)
        {
            es7.SetActive(flag);
        }

        if (es8)
        {
            es8.SetActive(flag);
        }
    }

    private void ResetEquipped()
    {
        for (int i = 0; i < equipped.Count; i++)
        {
            equipped[i] = -1;
        }
    }

    private int GetPentCost(int idx)
    {
        int initialCost = 100;
        int targetCost = 2000;
        float growthRate = 0.15f;

        if (idx < 4)
        {
            return 0;
        }
        else
        {
            int cost = (int)(initialCost + (targetCost - initialCost) * (1 - Mathf.Exp(-growthRate * (idx - 4))));
            cost = Mathf.RoundToInt(cost / 5.0f) * 5;
            return cost;
        }
    }

    private IEnumerator ShowLevelUp()
    {
        levelUpMessage.SetActive(true);

        StartCoroutine(musicController.PlaySoundEffect("LevelUp"));

        UnityEngine.Vector3 ogSize = levelUpMessage.transform.localScale;
        levelUpMessage.transform.localScale = levelUpMessage.transform.localScale * 0.002f;

        UnityEngine.Vector3 sizeInterval = levelUpMessage.transform.localScale;

        for (int i = 0; i < 500; i++)
        {
            levelUpMessage.transform.localScale += sizeInterval;

            yield return new WaitForSeconds(0.0001f);
        }
        levelUpMessage.transform.localScale = ogSize;

    }

    public void DismissButton()
    {
        levelUpMessage.SetActive(false);
    }

    private int GetLevelThresh(int level)
    {
        int levelThresh = 100 + 500 * ((level * (level - 1)) / 2);
        return levelThresh;
    }
}
