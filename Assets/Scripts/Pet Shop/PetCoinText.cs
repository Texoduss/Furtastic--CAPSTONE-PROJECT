using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PetCoinText : MonoBehaviour
{
    private TextMeshProUGUI textUI;

    private void Awake()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }


    // Subscribe to coin changes when enabled
    private void OnEnable()
    {
        if (PetCoinManager.Instance != null)
        {
            Debug.Log("[PetCoinText] Subscribing to OnCoinsChanged");
            PetCoinManager.Instance.OnCoinsChanged += UpdateUI;
            UpdateUI(PetCoinManager.Instance.GetCoins()); // <-- initial update
        }
        else
        {
            Debug.LogWarning("[PetCoinText] PetCoinManager instance not found at OnEnable!");
        }
    }


    private void OnDisable()
    {
        if (PetCoinManager.Instance != null)
            PetCoinManager.Instance.OnCoinsChanged -= UpdateUI;
    }

    private void UpdateUI(int value)
    {
        textUI.text = value.ToString();
    }
}
