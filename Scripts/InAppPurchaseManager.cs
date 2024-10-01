using System.Diagnostics;
using UnityEngine;
using UnityEngine.Purchasing;

public class InAppPurchaseManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController = null;
    private static IExtensionProvider m_StoreExtensionProvider;

    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    private void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("s_coins", ProductType.Consumable);
        builder.AddProduct("m_coins", ProductType.Consumable);
        builder.AddProduct("l_coins", ProductType.Consumable);
        builder.AddProduct("s_xp", ProductType.Consumable);
        builder.AddProduct("m_xp", ProductType.Consumable);
        builder.AddProduct("l_xp", ProductType.Consumable);
        builder.AddProduct("x_life", ProductType.Consumable);
        builder.AddProduct("a_pass", ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Handle initialization failure
        UnityEngine.Debug.LogError("Unity IAP initialization failed: " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        UnityEngine.Debug.LogError("Unity IAP initialization failed: " + error);
    }

    public void PurchaseItem(string productId)
    {
        if (m_StoreController != null)
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                UnityEngine.Debug.LogError("Product not found or not available for purchase: " + productId);
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Store controller is not initialized.");
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // Handle purchase failure
        UnityEngine.Debug.LogError("Purchase of product " + product.definition.id + " failed due to " + failureReason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // Handle purchase success
        UnityEngine.Debug.Log("Purchase of product " + args.purchasedProduct.definition.id + " succeeded");
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseComplete(Product product)
    {
        UnityEngine.Debug.Log("Purchase of product " + product.definition.id + " completed");

        if (product.definition.id == "s_coins")
        {
            int coins = PlayerPrefs.GetInt("Coins");
            coins += 500;
            PlayerPrefs.GetInt("Coins", coins);
        }
        else if (product.definition.id == "m_coins")
        {
            int coins = PlayerPrefs.GetInt("Coins");
            coins += 2500;
            PlayerPrefs.GetInt("Coins", coins);
        }
        else if (product.definition.id == "l_coins")
        {
            int coins = PlayerPrefs.GetInt("Coins");
            coins += 10000;
            PlayerPrefs.GetInt("Coins", coins);
        }
        else if (product.definition.id == "s_xp")
        {
            int xp = PlayerPrefs.GetInt("XP");
            xp += 1000;
            PlayerPrefs.GetInt("XP", xp);
        }
        else if (product.definition.id == "m_xp")
        {
            int xp = PlayerPrefs.GetInt("XP");
            xp += 5000;
            PlayerPrefs.GetInt("XP", xp);
        }
        else if (product.definition.id == "l_xp")
        {
            int xp = PlayerPrefs.GetInt("XP");
            xp += 20000;
            PlayerPrefs.GetInt("XP", xp);
        }
        else if (product.definition.id == "a_pass")
        {
            //function to open select achievement prompt
        }
        else if (product.definition.id == "x_life")
        {
            //add extra life to player prefs
        }
    }
}
