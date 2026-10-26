using UnityEngine;
using UnityEngine.UI;

public class PetToyFoodToggle : MonoBehaviour
{
    [Header("Item Info")]
    public string itemKey;
    [Tooltip("Category of the item, e.g., 'Toy' or 'Food'")]
    public string category; // "Toy" or "Food"

    [Header("Toggle Button")]
    [SerializeField] private Toggle toggleButton;

    private void Awake()
    {
        if (toggleButton == null)
            toggleButton = GetComponent<Toggle>();

        if (string.IsNullOrEmpty(itemKey))
            itemKey = System.Guid.NewGuid().ToString();
    }

    private void OnEnable() // Subscribe to toggle event
    {
        toggleButton.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnDisable() // Unsubscribe from toggle event
    {
        toggleButton.onValueChanged.RemoveListener(OnToggleChanged);
    }

    private void Start() 
    {
        // Restore toggle state when scene loads
        if (ShopDataManager.Instance != null)
        {
            bool savedState = ShopDataManager.Instance.IsItemPurchased(itemKey);
            toggleButton.isOn = savedState;
        }
    }

    private void OnToggleChanged(bool value) // Called when the toggle state changes
    {
        if (ShopDataManager.Instance == null)
        {
            Debug.LogWarning("[PetToyFoodToggle] ShopDataManager instance is null. Cannot save toggle state.");
            return;
        }

        ShopDataManager.Instance.SetItemPurchased(itemKey, value);
        Debug.Log($"[PetToyFoodToggle] {itemKey} toggled = {value}");

        if (value)
        {
            ShopDataManager.Instance.SetSelectedItem(itemKey, category);
            Debug.Log($"[PetToyFoodToggle] {itemKey} selected in category {category}");
        }
        
    }
}
