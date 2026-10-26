using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PawStampsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> pawStampObjects;
    // Assign 10 paw stamp GameObjects in order (0 ? 9) in the Inspector

    [SerializeField] private Button addStampButton;
    // Assign the "Add Stamp" button in the Inspector

    [SerializeField] private Button backButton;
    // Assign the "Back" button in the Inspector

    private void Start()
    {
        // Defensive checks
        if (pawStampObjects == null || pawStampObjects.Count == 0)
        {
            Debug.LogError("[PawStampsUI] No paw stamp GameObjects assigned!");
        }

        if (addStampButton == null)
        {
            Debug.LogError("[PawStampsUI] Add Stamp Button is not assigned!");
        }
        else
        {
            addStampButton.onClick.AddListener(OnAddStamp);
        }

        if (PawStampManager.Instance == null)
        {
            Debug.LogError("[PawStampsUI] PawStampManager not found in scene!");
        }

        if (backButton == null)
        {
            Debug.LogError("[PawStampsUI] Back Button is not assigned!");
        }
        else
        {
            backButton.onClick.AddListener(BackButton);
        }

        UpdateUI();
    }

    private void OnAddStamp()
    {
        if (PawStampManager.Instance == null)
        {
            Debug.LogError("[PawStampsUI] Cannot add stamp. Manager is missing.");
            return;
        }

        if (PawStampManager.Instance != null)
        {
            PawStampManager.Instance.AddStamp();
            UpdateUI();

        }
        else
        {
            Debug.LogWarning("[PawStampsUI] PawStampManager instance is null.");
        }

    }

    /// <summary>
    /// Updates the visibility of each paw stamp GameObject based on current count.
    /// </summary>
    private void UpdateUI()
    {
        if (PawStampManager.Instance == null || pawStampObjects == null) return;

        int currentCount = PawStampManager.Instance.CurrentCount;

        for (int i = 0; i < pawStampObjects.Count; i++)
        {
            if (pawStampObjects[i] == null)
            {
                Debug.LogWarning($"[PawStampsUI] pawStampObjects[{i}] is missing!");
                continue;
            }

            // Enable stamps up to the current count
            pawStampObjects[i].SetActive(i < currentCount);
        }

        Debug.Log($"[PawStampsUI] Updated paw stamps. Visible count: {currentCount}");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("PlayMenuScene");
        Debug.Log("[PawStampsUI] Back button pressed. Implement navigation logic.");
    }
}
