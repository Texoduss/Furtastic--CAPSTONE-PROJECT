using UnityEngine;
using System.Collections.Generic;

public class ShopDataManager : MonoBehaviour, IDataPersistence
{
    public static ShopDataManager Instance { get; private set; }

    private SerializableDictionary<string, bool> purchasedItems = new SerializableDictionary<string, bool>();

    private string selectedFeedKey = null; // 🔹 keeps track of which toggle is selected
    private string selectedToyKey = null; // 🔹 keeps track of which toggle is selected

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log($"[DEBUG] Is 'catLore' purchased? " + IsItemPurchased("catLore"));
    }

    public void LoadData(GameData data) // load purchased items and selected item
    {
        if (data == null)
        {
            Debug.LogError("[ShopDataManager] LoadData failed: data is null!");
            return;
        }

        purchasedItems = data.purchasedItems ?? new SerializableDictionary<string, bool>();
        selectedFeedKey = data.selectedFeedItem; // load selected feed item
        selectedToyKey = data.selectedToyItem; // load selected toy item

        Debug.Log($"[ShopDataManager] Loaded {purchasedItems.Count} items. Selected Feed: {selectedFeedKey}, Selected Toy: {selectedToyKey}");
    }

    public void SaveData(GameData data) // save purchased items and selected item
    {
        if (data == null)
        {
            Debug.LogError("[ShopDataManager] SaveData failed: data is null!");
            return;
        }

        data.purchasedItems = purchasedItems;
        data.selectedFeedItem = selectedFeedKey; // save currently selected item
        data.selectedToyItem = selectedToyKey; // save currently selected item

        Debug.Log($"[ShopDataManager] Saved {purchasedItems.Count} items. Selected Feed: {selectedFeedKey}, Selected Toy: {selectedToyKey}");
    }

    public bool IsItemPurchased(string key) // check if item is purchased
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning("[ShopDataManager] IsItemPurchased called with null or empty key.");
            return false;
        }

        return purchasedItems.TryGetValue(key, out bool purchased) && purchased;
    }

    public void SetItemPurchased(string key, bool value) // set item as purchased or not
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning("[ShopDataManager] SetItemPurchased called with null or empty key.");
            return;
        }

        purchasedItems[key] = value;
        Debug.Log($"[ShopDataManager] Set item {key} purchased = {value}");

        if (DataPersistenceManager.instance != null)
            DataPersistenceManager.instance.SaveGame();
    }

    // Call this when a toggle is pressed
    public void SetSelectedItem(string key, string category)
    {
        if (!IsItemPurchased(key))
        {
            Debug.LogWarning($"[ShopDataManager] Cannot select item {key} in category {category} because it is not purchased.");
            return;
        }

        if (category == "Food")
        {
            selectedFeedKey = key; // for feed items
        }
        else if (category == "Toy")
        {
            selectedToyKey = key; // for toy items
        }
        else
        {
            Debug.LogWarning($"[ShopDataManager] Unknown category {category} for item selection.");
            return;
        }

        if (DataPersistenceManager.instance != null)
            DataPersistenceManager.instance.SaveGame();
    }

    public string GetSelectedFeedItem() => selectedFeedKey; // get currently selected feed item
    public string GetSelectedToyItem() => selectedToyKey; // get currently selected toy item
}
