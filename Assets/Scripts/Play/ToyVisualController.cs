using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyVisualController : MonoBehaviour, IDataPersistence
{
    [Header("Toy Visuals")]
    [SerializeField] private List<GameObject> toyVisuals = new();

    private Dictionary<string, GameObject> toyVisualMap = new();

    public System.Action OnToyVisualComplete;

    private void Awake()
    {
        foreach (var visual in toyVisuals)
        {
            if (visual == null)
            {
                Debug.LogWarning("[ToyVisualController] A toy visual is not assigned.");
                continue;
            }
            string key = visual.name;
            if (!toyVisualMap.ContainsKey(key))
            {
                toyVisualMap.Add(key, visual);
                visual.SetActive(false); // hide all at start
            }
            else
            {
                Debug.LogWarning($"[ToyVisualController] Duplicate visual name detected: {key}");
            }
        }
    }
    public void LoadData(GameData data)
    {
        RefreshToyVisual();
    }
    public void SaveData(GameData data)
    {
        // No data to save from this controller
    }
    private IEnumerator HideAfterToyAnimation()
    {
        yield return new WaitForSeconds(2.0f); // wait for 2 seconds
        OnToyVisualComplete?.Invoke();
    }
    public void ShowActiveToyVisual()
    {
        RefreshToyVisual();
        StartCoroutine(HideAfterToyAnimation());
    }

    private void RefreshToyVisual()
    {
        if (ShopDataManager.Instance == null)
        {
            Debug.LogWarning("[ToyVisualController] ShopDataManager instance is null. Cannot refresh toy visuals.");
            return;
        }
        string selectedToyKey = ShopDataManager.Instance.GetSelectedToyItem();

        if (string.IsNullOrEmpty(selectedToyKey))
        {
            Debug.Log("[ToyVisualController] No toy selected. Hiding all toy visuals.");
            HideAllVisuals();
            return;
        }
        bool found = false;
        foreach (var pair in toyVisualMap)
        {
            
            string key = pair.Key;
            GameObject visual = pair.Value;

            bool shouldShow = key == selectedToyKey && ShopDataManager.Instance.IsItemPurchased(key);
            visual.SetActive(shouldShow);

            if (shouldShow)
            {
                found = true;
                Debug.Log($"[ToyVisualController] Showing toy visual for selected item: {key}");
            }
        }
        if (!found)
        {
            Debug.LogWarning($"[ToyVisualController] Selected toy '{selectedToyKey}' not found or not purchased. Hiding all toy visuals.");
        }
    }
    // Hides all toy visuals
    public void HideAllVisuals()
    {
        foreach(var visual in toyVisualMap.Values)
        {
            visual.SetActive(false);
        }
    }
}
