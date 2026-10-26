using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public static class ToySpriteDatabase
{
    private static Dictionary<string, Sprite> toySprites = new();

    public static void Register(string key, Sprite sprite)
    {
        toySprites[key] = sprite;
    }

    public static Sprite GetSprite(string key)
    {
        return toySprites.TryGetValue(key, out Sprite sprite) ? sprite : null;
    }
}

public class ToyVisualPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Image toyImage;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private float showDuration = 2f;

    [SerializeField] private ToyVisualController toyVisualController;

    [Header("Animation (Optional)")]
    [SerializeField] private bool useFadeEffect = true;
    [SerializeField] private float fadeDuration = 0.3f;

    private Coroutine currentCoroutine;
    private CanvasGroup popupCanvasGroup;

    private void Awake()
    {
        if (popupPanel == null)
        {
            Debug.LogError("[ToyVisualPopup] PopupPanel reference missing!");
            return;
        }

        popupCanvasGroup = popupPanel.GetComponent<CanvasGroup>();
        if (popupCanvasGroup == null)
            popupCanvasGroup = popupPanel.AddComponent<CanvasGroup>();

        popupPanel.SetActive(false);
        popupCanvasGroup.alpha = 0f;
    }

    private void Start()
    {
        if (playButton == null)
        {
            Debug.LogError("[ToyVisualPopup] PlayButton reference missing!");
            return;
        }

        playButton.onClick.AddListener(OnPlayPressed);

        if (toyImage == null)
            Debug.LogWarning("[ToyVisualPopup] ToyImage missing — popup will still work but no sprite will show.");

        if (toyVisualController == null)
            Debug.LogWarning("[ToyVisualPopup] ToyVisualController reference missing — toy visuals won’t update.");
        toyVisualController.OnToyVisualComplete += () => HidePopupNow();

    }

    private void OnPlayPressed()
    {
        if (popupPanel == null)
        {
            Debug.LogWarning("[ToyVisualPopup] PopupPanel is null — cannot display popup.");
            return;
        }

        // --- Step 1: Trigger Toy Visual ---
        if (toyVisualController != null)
        {
            try
            {
                toyVisualController.HideAllVisuals();
                toyVisualController.ShowActiveToyVisual();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[ToyVisualPopup] Error triggering ToyVisualController: {ex.Message}");
            }
        }

        // --- Step 2: Optional — show toy sprite based on selected item ---
        string selectedToy = ShopDataManager.Instance?.GetSelectedToyItem();
        if (!string.IsNullOrEmpty(selectedToy))
        {
            Sprite toySprite = ToySpriteDatabase.GetSprite(selectedToy);
            if (toySprite != null && toyImage != null)
                toyImage.sprite = toySprite;
        }

        // --- Step 3: Show Popup ---
        ShowPopup();
        Debug.Log("[ToyVisualPopup] Play button pressed, showing popup.");
    }

    public void ShowToyVisual(Sprite sprite)
    {
        if (popupPanel == null)
        {
            Debug.LogWarning("[ToyVisualPopup] PopupPanel not assigned, cannot show visual.");
            return;
        }

        if (toyImage != null)
            toyImage.sprite = sprite;
        else
            Debug.LogWarning("[ToyVisualPopup] ToyImage not assigned, cannot display sprite.");

        ShowPopup();
    }

    private void ShowPopup()
    {
        if (popupPanel == null)
        {
            Debug.LogWarning("[ToyVisualPopup] Cannot show popup — panel missing.");
            return;
        }

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

    public void HidePopupNow()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        popupCanvasGroup.alpha = 0f;
        popupPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (playButton != null)
            playButton.onClick.RemoveListener(OnPlayPressed);
    }
}
