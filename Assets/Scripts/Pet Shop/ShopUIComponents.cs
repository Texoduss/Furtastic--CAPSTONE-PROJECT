using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Controls the shop UI to display items based on the selected pet type (Cat or Dog)
public class ShopUIController : MonoBehaviour, IDataPersistence 
{
    [Header("Cat Items")]
    [SerializeField] private List<GameObject> catItems; // List of cat item GameObjects to toggle

    [Header("Dog Items")]
    [SerializeField] private List<GameObject> dogItems; // List of dog item GameObjects to toggle

    private string petType;

    public void LoadData(GameData data) // Load the pet type from saved data
    {
        this.petType = data.petType;
        UpdateShopItems();
    }

    public void SaveData(GameData data)
    {
        // No need to save anything here for shop UI
    }

    private void UpdateShopItems() // Call this method to update the shop UI based on the pet type
    {
        if (petType == "Cat")
        {
            SetActiveItems(catItems, true);
            SetActiveItems(dogItems, false);
            Debug.Log("[ShopUIController] Displaying Cat Items.");
        }
        else if (petType == "Dog")
        {
            SetActiveItems(catItems, false);
            SetActiveItems(dogItems, true);
            Debug.Log("[ShopUIController] Displaying Dog Items.");
        }
        else
        {
            Debug.LogWarning("[ShopUIController] Unknown pet type. No items to display.");
            SetActiveItems(catItems, false);
            SetActiveItems(dogItems, false);
        }
    }

    private void SetActiveItems(List<GameObject> items, bool isActive) // Helper method to set active state for a list of items
    {
        foreach (var item in items)
        {
            if (item != null)
            {
                item.SetActive(isActive);
            }
        }
    }
}
