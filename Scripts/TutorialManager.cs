using System.Numerics;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class TutorialManager : MonoBehaviour
{
    private static TutorialManager instance;

    public GameObject tutorialPrefab;
    public GameObject fullPanel;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;
    public GameObject panel5;
    public GameObject panel6;
    private GameObject tutorialInstance;
    private GameObject fullPanelInstance;

    public bool newScene = true;
    private TutorialObject[] tutorials;

    private Coroutine coroutine;

    public int index = 0;
    public int pageIdx = 1;
    public float currFontSize = 32f;
    public float skipButtonSize = 0.5f;
    private UnityEngine.Vector2 skipButtonProp = new UnityEngine.Vector2(160f, 60f);
    public bool horzVertFlag = false;


    private InventoryManager inv;
    private string curr;

    private EnemyManager enm;
    public bool spawnFlag = false;

    public bool eotFlag = false;

    private void Awake()
    {
        if (instance == null)
        {
            // This is the first instance, so set it as the singleton instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // An instance already exists, so destroy this one
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        tutorials = Resources.LoadAll<TutorialObject>("Tutorials");

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "InventoryScene")
        {
            inv = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            pageIdx = 2;
            index = 5;
            currFontSize = 32f;
            skipButtonSize = 1f;
            horzVertFlag = false;
        }
        else if (scene.name == "GameScene")
        {
            enm = GameObject.Find("GameManager").GetComponent<EnemyManager>();
            pageIdx = 3;
            index = 13;
            currFontSize = 36f;
            skipButtonSize = 1.5f;
            horzVertFlag = true;
        }
        else if (scene.name == "AwardsScene")
        {
            pageIdx = 4;
            index = 21;
            currFontSize = 32f;
            skipButtonSize = 0.5f;
            horzVertFlag = false;
        }
        else if (scene.name == "SettingsScene")
        {
            pageIdx = 5;
            index = 23;
            currFontSize = 32f;
            skipButtonSize = 0.5f;
            horzVertFlag = false;
        }
        else if (scene.name == "MainMenu" && pageIdx == 5)
        {
            pageIdx = 6;
            index = 24;
            currFontSize = 32f;
            skipButtonSize = 0.5f;
            horzVertFlag = false;
        }

        newScene = true;
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("TutFlag"))
        {
            if (PlayerPrefs.GetInt("TutFlag") == 0)
            {
                if (tutorials[index].page < pageIdx && (newScene == true || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
                {
                    
                    Tutorial(index);
                }
                else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) && tutorialInstance != null)
                {
                    if (spawnFlag)
                    {
                        enm.spawnFlag = true;
                        spawnFlag = false;
                    }

                    Destroy(tutorialInstance);
                    Destroy(fullPanelInstance);

                    if (eotFlag)
                    {
                        PlayerPrefs.SetInt("TutFlag", 1);
                    }
                    //condition for last tutorial, set playerprefs to 1
                }
            }
        }
        else
        {
            PlayerPrefs.SetInt("TutFlag", 0);
            //At end of first tutorial set to 1

            Tutorial(0);
        }
        

    }

    private void Tutorial(int idx)
    {
        if (newScene)
        {
            if (fullPanelInstance != null)
            {
                Destroy(fullPanelInstance);
            }

            tutorialInstance = Instantiate(tutorialPrefab);
            tutorialInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);

            newScene = false;
        }

        bool indexFlag = true;

        if (inv != null)
        {
            curr = inv.currCarousel;
        }

        switch (idx)
        {
            case 0:
                if (fullPanelInstance != null)
                {
                    Destroy(fullPanelInstance);
                }

                fullPanelInstance = Instantiate(fullPanel);
                fullPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);

                tutorialInstance.transform.SetAsLastSibling();
                break;
            case 1:
                Destroy(fullPanelInstance);

                fullPanelInstance = Instantiate(panel1);
                fullPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);

                tutorialInstance.transform.SetAsLastSibling();
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-100f, 0f, 0f);
                break;
            case 2:
                Destroy(fullPanelInstance);

                fullPanelInstance = Instantiate(panel2);
                fullPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);

                tutorialInstance.transform.SetAsLastSibling();
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-100f, 0f, 0f);
                break;
            case 3:
                Destroy(fullPanelInstance);

                fullPanelInstance = Instantiate(panel3);
                fullPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);

                tutorialInstance.transform.SetAsLastSibling();
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-100f, 0f, 0f);
                break;
            case 4:
                Destroy(fullPanelInstance);

                fullPanelInstance = Instantiate(panel4);
                fullPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);

                tutorialInstance.transform.SetAsLastSibling();
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-100f, 0f, 0f);
                break;
            case 5:
                if (fullPanelInstance == null)
                {
                    fullPanelInstance = Instantiate(panel5);
                    fullPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
                }

                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-400f, 0f, 0f);
                indexFlag = false;
                if (curr == "pent")
                {
                    index = 6;
                    indexFlag = true;
                }
                break;
            case 6:
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-400f, 0f, 0f);

                
                break;
            case 7:
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-400f, 0f, 0f);


                break;
            case 8:
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-400f, 0f, 0f);


                break;
            case 9:
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-400f, 0f, 0f);
                indexFlag = false;
                if (curr == "costume")
                {
                    index = 10;
                    indexFlag = true;
                }
                break;
            case 10:
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-400f, 0f, 0f);
                break;
            case 11:
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-400f, 0f, 0f);
                break;
            case 12:
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-400f, 0f, 0f);

                break;
            case 13:
                if (fullPanelInstance == null)
                {
                    fullPanelInstance = Instantiate(panel6);
                    fullPanelInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
                }

                enm.spawnFlag = false;
                break;
            case 14:
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(-700f, 0f, 0f);
                break;
            case 15:

                break;
            case 16:
                
                break;
            case 17:

                break;
            case 18:
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(700f, 0f, 0f);
                break;
            case 19:
                
                break;
            case 20:
                spawnFlag = true;
                tutorialInstance.GetComponent<RectTransform>().anchoredPosition = new UnityEngine.Vector3(0f, 0f, 0f);
                break;
            case 21:

                break;
            case 22:

                break;
            case 23:

                break;
            case 24:

                break;
            case 25:

                break;
            case 26:
                eotFlag = true;
                break;

        }

        TextMeshProUGUI textBox = tutorialInstance.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        if (textBox != null)
        {
            if (horzVertFlag)
            {
                textBox.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(800, 400);
            }

            textBox.fontSize = currFontSize;
            textBox.text = tutorials[index].description;
            AdjustHeightBasedOnText(tutorialInstance, textBox);

            textBox.text = "";
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(TypeText(textBox, tutorials[index].description));
        }

        if (indexFlag && (index < tutorials.Length))
        {
            index += 1;
        }
        
    }

    private IEnumerator TypeText(TextMeshProUGUI textBox, string fullText)
    {
        //UnityEngine.Debug.LogError(index);
        foreach (char letter in fullText.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void AdjustHeightBasedOnText(GameObject tutorialInstance, TextMeshProUGUI textBox)
    {
        RectTransform buttonRect = tutorialInstance.transform.Find("Button").GetComponent<RectTransform>();
        //buttonRect.sizeDelta = skipButtonProp * skipButtonSize;
        //buttonRect.transform.Find("Text").GetComponent<TextMeshProUGUI>().fontSize = currFontSize;

        RectTransform prefabRect = tutorialInstance.GetComponent<RectTransform>();
        RectTransform textRect = textBox.GetComponent<RectTransform>();

        float textHeight = textBox.preferredHeight;
        float textWidth = textRect.sizeDelta.x;

        if (!horzVertFlag)
        {
            

            // Calculate the new height based on the text

            // Add an extra buffer to the height
            float buffer = 50f; // Adjust the buffer as needed
            textHeight += buffer;

            // Set the new size of the prefab
            prefabRect.sizeDelta = new UnityEngine.Vector2(prefabRect.sizeDelta.x, textHeight);

            textRect.anchoredPosition = new UnityEngine.Vector2(textRect.anchoredPosition.x, prefabRect.anchoredPosition.y + (prefabRect.sizeDelta.y / 2f) - (buffer / 2));
        }
        else
        {

            float buffer = 50f; // Adjust the buffer as needed
            textHeight += buffer;
            textWidth += buffer;

            prefabRect.sizeDelta = new UnityEngine.Vector2(textWidth, textHeight);

            textRect.anchoredPosition = new UnityEngine.Vector2(textRect.anchoredPosition.x, prefabRect.anchoredPosition.y + (prefabRect.sizeDelta.y / 2f) - (buffer / 2));
        }

        buttonRect.anchoredPosition = new UnityEngine.Vector2(textRect.anchoredPosition.x, prefabRect.anchoredPosition.y - (prefabRect.sizeDelta.y / 2f) - (25f / 2));
    }

    public void StartTutorial()
    {
        index = 0;
        pageIdx = 1;
        currFontSize = 32f;
        skipButtonSize = 0.5f;
        horzVertFlag = false;
        spawnFlag = false;
        eotFlag = false;
        newScene = true;
        Destroy(fullPanelInstance);

        PlayerPrefs.DeleteKey("TutFlag");
    }

    public void SkipTutorial()
    {

        index = tutorials.Length - 1;
        pageIdx = 6;

        if (enm != null)
        {
            enm.spawnFlag = true;
            spawnFlag = false;
        }

        Destroy(tutorialInstance);
        Destroy(fullPanelInstance);

        PlayerPrefs.SetInt("TutFlag", 1);

    }

}

