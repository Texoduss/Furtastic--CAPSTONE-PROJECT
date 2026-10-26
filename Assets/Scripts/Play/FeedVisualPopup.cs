using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FeedSpriteDatabase
{
    private static Dictionary<string, Sprite> feedSpriteMap = new();

    public static void Register(string key, Sprite sprite)
    {
        feedSpriteMap[key] = sprite;
    }

    public static Sprite GetSprite(string key)
    {
        return feedSpriteMap.TryGetValue(key, out var sprite) ? sprite : null;
    }
}

public class FeedVisualPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button feedButton;
    [SerializeField] private Image foodImage;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private float showDuration = 2f;

    [SerializeField] private FeedVisualController feedVisualController;

    [Header("Animation (Optional)")]
    [SerializeField] private bool useFadeEffect = true;
    [SerializeField] private float fadeDuration = 0.3f;

    private Coroutine currentCoroutine;
    private CanvasGroup popupCanvasGroup;

    private void Awake()
    {
        if (popupPanel == null)
        {
            Debug.LogError("[FeedVisualPopup] PopupPanel reference missing!");
            return;
        }

        // Ensure the panel has a CanvasGroup
        popupCanvasGroup = popupPanel.GetComponent<CanvasGroup>();
        if (popupCanvasGroup == null)
            popupCanvasGroup = popupPanel.AddComponent<CanvasGroup>();

        popupPanel.SetActive(false);
        popupCanvasGroup.alpha = 0f;
    }

    private void Start()
    {
        if (feedButton == null)
        {
            Debug.LogError("[FeedVisualPopup] FeedButton reference missing!");
            return;
        }

        feedButton.onClick.AddListener(OnFeedPressed);

        if (foodImage == null)
        {
            Debug.LogWarning("[FeedVisualPopup] FoodImage missing — popup will still work but no sprite will show.");
        }

        if (feedVisualController == null)
        {
            Debug.LogWarning("[FeedVisualPopup] FeedVisualController reference missing — feed visuals won’t update.");
        }
        feedVisualController.OnFeedVisualComplete += () => HidePopupNow();
    }

    public void HidePopupNow()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        popupCanvasGroup.alpha = 0f;
        popupPanel.SetActive(false);
    }

    private void OnFeedPressed()
    {
        string selectedFeedKey = ShopDataManager.Instance != null ? ShopDataManager.Instance.GetSelectedFeedItem() : null;

        if (popupPanel == null)
        {
            Debug.LogWarning("[FeedVisualPopup] PopupPanel is null — cannot display popup.");
            return;
        }

        // Trigger feed visual controller safely

        if (feedVisualController != null)
        {
            feedVisualController.HideAllVisuals();
            feedVisualController.ShowActiveFeedVisual();
        }
        else
        {
            Debug.LogWarning("[FeedVisualPopup] FeedVisualController is null — cannot update feed visuals.");
        }
        if (!string.IsNullOrEmpty(selectedFeedKey))
        {
            Sprite selectedSprite = FeedSpriteDatabase.GetSprite(selectedFeedKey);
        }
        
        ShowPopup();
        Debug.Log("[FeedVisualPopup] Feed button pressed, showing popup.");
    }

    public void ShowFoodVisual(Sprite sprite)
    {
        if (popupPanel == null)
        {
            Debug.LogWarning("[FeedVisualPopup] PopupPanel not assigned, cannot show visual.");
            return;
        }

        if (foodImage != null)
            foodImage.sprite = sprite;

        else
            Debug.LogWarning("[FeedVisualPopup] FoodImage not assigned, cannot display sprite.");

        ShowPopup();
    }

    private void ShowPopup()
    {
        if (popupPanel == null)
        {
            Debug.LogWarning("[FeedVisualPopup] Cannot show popup — panel missing.");
            return;
        }

        // Stop any active coroutine before showing again
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        popupPanel.SetActive(true);

        if (useFadeEffect && popupCanvasGroup != null)
            StartCoroutine(FadeIn());
        else
            popupCanvasGroup.alpha = 1f;

        currentCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showDuration);

        if (useFadeEffect && popupCanvasGroup != null)
            yield return FadeOut();
        else
            popupPanel.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        popupCanvasGroup.alpha = 0f;

        while (elapsed < fadeDuration)
        {
            popupCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        popupCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            popupCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        popupCanvasGroup.alpha = 0f;
        popupPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (feedButton != null)
            feedButton.onClick.RemoveListener(OnFeedPressed);
    }
}
