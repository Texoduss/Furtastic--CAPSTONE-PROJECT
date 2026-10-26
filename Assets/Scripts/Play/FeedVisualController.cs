using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FeedVisualController : MonoBehaviour, IDataPersistence
{
    [Header("Feed Item Visuals")]
    [SerializeField] private List<GameObject> feedVisuals = new();

    private Dictionary<string, GameObject> feedVisualMap = new Dictionary<string, GameObject>();

    public System.Action OnFeedVisualComplete;
    private void Awake()
    {
        foreach (var visual in feedVisuals)
        {
            if (visual == null)
            {
                Debug.LogWarning("[FeedVisualController] A feed visual is not assigned.");
                continue;
            }

            string key = visual.name;
            if (!feedVisualMap.ContainsKey(key))
            {
                feedVisualMap.Add(key, visual);
                visual.SetActive(false); // hide all at start
            }
            else
            {
                Debug.LogWarning($"[FeedVisualController] Duplicate visual name detected: {key}");
            }
        }
    }

    public void LoadData(GameData data)
    {
        RefresFeedVisual();
    }

    public void SaveData(GameData data)
    {
        // visuals don't persist separately
    }

    // Called by Feed Button in Play Scene
    public void ShowActiveFeedVisual()
    {
        RefresFeedVisual();
        StartCoroutine(HideAfterFeedAnimation());
    }

    private IEnumerator HideAfterFeedAnimation() // 2 seconds delay
    {
        yield return new WaitForSeconds(2.0f);
        OnFeedVisualComplete?.Invoke();
    }
    private void RefresFeedVisual() // updates which visual to show based on selected feed
    {
        // defensive checks
        if (ShopDataManager.Instance == null)
        {
            Debug.LogError("[FeedVisualController] ShopDataManager.Instance is missing!");
            return;
        }

        string selectedFeed = ShopDataManager.Instance.GetSelectedFeedItem();
        if (string.IsNullOrEmpty(selectedFeed))
        {
            Debug.LogWarning("[FeedVisualController] No feed item is currently selected.");
            HideAllVisuals();
            return;
        }

        bool found = false;

        foreach (var pair in feedVisualMap)
        {
            string key = pair.Key;
            GameObject visual = pair.Value;

            bool shouldShow = key == selectedFeed && ShopDataManager.Instance.IsItemPurchased(key);
            visual.SetActive(shouldShow);

            if (shouldShow)
            {
                found = true;
                Debug.Log($"[FeedVisualController] Showing selected feed visual: {key}");
            }
        }
        if (!found)
        {
            Debug.LogWarning($"[FeedVisualController] Selected feed '{selectedFeed}' is not purchased or has no associated visual.");
        }
    }
    public void HideAllVisuals() // hides all feed visuals
    {
        foreach (var visual in feedVisualMap)
        {
            visual.Value.SetActive(false);
        }
    }
}
