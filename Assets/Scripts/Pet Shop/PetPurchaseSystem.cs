using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PetPurchaseSystem : MonoBehaviour, IDataPersistence
{
    [Header("Item to Price")]
    [SerializeField] private Button button;
    [SerializeField] private string id;
    [SerializeField] private int price;

    [Header("Toggle Button")]
    [SerializeField] private GameObject toggleButton;

    private bool isPurchased = false;
    private string petType;

    [ContextMenu("Generate GUID for ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    // Called when loading saved data
    public void LoadData(GameData data)
    {
        data.purchasedItems.TryGetValue(id, out isPurchased);
        Debug.Log($"[PetPurchaseSystem] Loaded purchase state for item {id}: {isPurchased}");
        // Update button state based on purchase status

        if (isPurchased)
        {
            if (button != null)
            {
                button.interactable = false;
                TextMeshProUGUI purchasedText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (purchasedText != null)
                {
                    purchasedText.text = "Purchased";
                }
            }
        }

        this.petType = data.petType;
    }

    // Called when saving data
    public void SaveData(GameData data)
    {
        if (data.purchasedItems.ContainsKey(id))
        {
            data.purchasedItems[id] = isPurchased;
        }
        else
        {
            data.purchasedItems.Add(id, isPurchased);
        }
    }

    private void Start()
    {
        // Initialize button state
        if (toggleButton != null)
        {
            if (isPurchased)
            {
                toggleButton.SetActive(true);
            }
            else
            {
                toggleButton.SetActive(false);
            }
        }

        button.onClick.AddListener(() => BuyButton(price));
    }

    private void Update()
    {
        // Continuously update toggle button state
        if (toggleButton != null)
        {
            if (isPurchased)
            {
                toggleButton.SetActive(true);
            }
            else
            {
                toggleButton.SetActive(false);
            }
        }
    }

    // Call this when the buy button is clicked
    public void BuyButton(int price)
    {
        if (button == null)
        {
            Debug.LogError("[PetPurchaseSystem] No button assigned!");
            return;
        }

        if (PetCoinManager.Instance == null)
        {
            Debug.LogError("[PetPurchaseSystem] No PetCoinManager found in scene!");
            return;
        }

        if (PetCoinManager.Instance.GetCoins() >= price)
        {
            PetCoinManager.Instance.SpendCoins(price);
            Debug.Log($"[PetPurchaseSystem] Purchase successful! You bought an item for {price} coins.");
            

            isPurchased = true; // Mark as purchased

            button.interactable = false;
            TextMeshProUGUI purchasedText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (purchasedText != null)
            {
                purchasedText.text = "Purchased";
            }

            // Trigger data save
            DataPersistenceManager.instance.SaveGame();
            Debug.Log("[PetPurchaseSystem] Game data saved after purchase.");
        }
        else
        {
            Debug.Log("[PetPurchaseSystem] Not enough coins to buy this item.");
        }
    }

    public void BuyMedicine(int price) // Call this when the buy medicine button is clicked
    {
        if (PetCoinManager.Instance == null)
        {
            Debug.LogError("[PetPurchaseSystem] No PetCoinManager found in scene!");
            return;
        }
        if (MedicineDataManager.Instance == null)
        {
            Debug.LogError("[PetPurchaseSystem] No MedicineDataManager found in scene!");
            return;
        }
        if (PetCoinManager.Instance.GetCoins() >= price)
        {
            if (id.ToLower().Contains("medicine"))
            {
                if (MedicineDataManager.Instance != null)
                {
                    if (MedicineDataManager.Instance.CanAddMedicine())
                    {
                        MedicineDataManager.Instance.AddMedicine(1);
                        DataPersistenceManager.instance.SaveGame(); // Save immediately after adding medicine
                        Debug.Log("[PetPurchaseSystem] Medicine added via purchase!");
                        return;
                    }
                    else
                    {
                        Debug.Log("[PetPurchaseSystem] Cannot add more medicine, already at max (3).");
                    }
                }
                else
                {
                    Debug.LogWarning("[PetPurchaseSystem] MedicineDataManager not found in scene!");
                }
            }
            else
            {
                Debug.LogWarning("[PetPurchaseSystem] Item ID does not indicate a medicine item.");
            }
        }
        else
        {
            Debug.Log("[PetPurchaseSystem] Not enough coins to buy this medicine.");
        }
    }

    public bool CheckIfPurchasingIsMedicine()
    {
        return id.Contains("Medicine");
    }
}
