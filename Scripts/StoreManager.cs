using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public GameObject store;

    private GameObject coinsButton;
    private GameObject xpButton;
    private GameObject miscButton;
    private GameObject sCoinsButton;
    private GameObject mCoinsButton;
    private GameObject lCoinsButton;
    private GameObject sXpButton;
    private GameObject mXpButton;
    private GameObject lXpButton;
    private GameObject lifeButton;
    private GameObject achButton;
    private GameObject backButton;

    private MusicController musicController;

    private InAppPurchaseManager iapScript;


    void Start()
    {
        coinsButton = store.transform.Find("CoinsButton").gameObject;
        xpButton = store.transform.Find("XpButton").gameObject;
        miscButton = store.transform.Find("MiscButton").gameObject;
        sCoinsButton = store.transform.Find("sCoinsButton").gameObject;
        mCoinsButton = store.transform.Find("mCoinsButton").gameObject;
        lCoinsButton = store.transform.Find("lCoinsButton").gameObject;
        sXpButton = store.transform.Find("sXpButton").gameObject;
        mXpButton = store.transform.Find("mXpButton").gameObject;
        lXpButton = store.transform.Find("lXpButton").gameObject;
        lifeButton = store.transform.Find("lifeButton").gameObject;
        achButton = store.transform.Find("achButton").gameObject;
        backButton = store.transform.Find("BackButton").gameObject;

        coinsButton.SetActive(true);
        xpButton.SetActive(true);
        miscButton.SetActive(true);
        sCoinsButton.SetActive(false);
        mCoinsButton.SetActive(false);
        lCoinsButton.SetActive(false);
        sXpButton.SetActive(false);
        mXpButton.SetActive(false);
        lXpButton.SetActive(false);
        lifeButton.SetActive(false);
        achButton.SetActive(false);
        backButton.SetActive(false);

        musicController = GameObject.Find("Audio").GetComponent<MusicController>();
        iapScript = GetComponent<InAppPurchaseManager>();
    }

    public void ShowStore()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        store.SetActive(true);
        coinsButton.SetActive(true);
        xpButton.SetActive(true);
        miscButton.SetActive(true);
        sCoinsButton.SetActive(false);
        mCoinsButton.SetActive(false);
        lCoinsButton.SetActive(false);
        sXpButton.SetActive(false);
        mXpButton.SetActive(false);
        lXpButton.SetActive(false);
        lifeButton.SetActive(false);
        achButton.SetActive(false);
        backButton.SetActive(false);
    }

    public void HideStore()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        store.SetActive(false);
    }

    public void Back()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        coinsButton.SetActive(true);
        xpButton.SetActive(true);
        miscButton.SetActive(true);
        sCoinsButton.SetActive(false);
        mCoinsButton.SetActive(false);
        lCoinsButton.SetActive(false);
        sXpButton.SetActive(false);
        mXpButton.SetActive(false);
        lXpButton.SetActive(false);
        lifeButton.SetActive(false);
        achButton.SetActive(false);
        backButton.SetActive(false);
    }

    public void ShowCoins()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        coinsButton.SetActive(false);
        xpButton.SetActive(false);
        miscButton.SetActive(false);
        sCoinsButton.SetActive(true);
        mCoinsButton.SetActive(true);
        lCoinsButton.SetActive(true);
        sXpButton.SetActive(false);
        mXpButton.SetActive(false);
        lXpButton.SetActive(false);
        lifeButton.SetActive(false);
        achButton.SetActive(false);
        backButton.SetActive(true);
    }

    public void ShowXP()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        coinsButton.SetActive(false);
        xpButton.SetActive(false);
        miscButton.SetActive(false);
        sCoinsButton.SetActive(false);
        mCoinsButton.SetActive(false);
        lCoinsButton.SetActive(false);
        sXpButton.SetActive(true);
        mXpButton.SetActive(true);
        lXpButton.SetActive(true);
        lifeButton.SetActive(false);
        achButton.SetActive(false);
        backButton.SetActive(true);
    }

    public void ShowExtras()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        coinsButton.SetActive(false);
        xpButton.SetActive(false);
        miscButton.SetActive(false);
        sCoinsButton.SetActive(false);
        mCoinsButton.SetActive(false);
        lCoinsButton.SetActive(false);
        sXpButton.SetActive(false);
        mXpButton.SetActive(false);
        lXpButton.SetActive(false);
        lifeButton.SetActive(true);
        achButton.SetActive(true);
        backButton.SetActive(true);
    }

    public void SmallCoins()
    {
        iapScript.PurchaseItem("s_coins");
        StartCoroutine(musicController.PlaySoundEffect("StorefrontPurchase"));
    }

    public void MedCoins()
    {
        iapScript.PurchaseItem("m_coins");
        StartCoroutine(musicController.PlaySoundEffect("StorefrontPurchase"));
    }

    public void LargeCoins()
    {
        iapScript.PurchaseItem("l_coins");
        StartCoroutine(musicController.PlaySoundEffect("StorefrontPurchase"));
    }

    public void SmallXp()
    {
        iapScript.PurchaseItem("s_xp");
        StartCoroutine(musicController.PlaySoundEffect("StorefrontPurchase"));
    }

    public void MedXp()
    {
        iapScript.PurchaseItem("m_xp");
        StartCoroutine(musicController.PlaySoundEffect("StorefrontPurchase"));
    }

    public void LargeXp()
    {
        iapScript.PurchaseItem("l_xp");
        StartCoroutine(musicController.PlaySoundEffect("StorefrontPurchase"));
    }

    public void ExtraLife()
    {
        iapScript.PurchaseItem("x_life");
        StartCoroutine(musicController.PlaySoundEffect("StorefrontPurchase"));
    }

    public void AchievementPass()
    {
        iapScript.PurchaseItem("a_pass");
        StartCoroutine(musicController.PlaySoundEffect("StorefrontPurchase"));
    }
}
