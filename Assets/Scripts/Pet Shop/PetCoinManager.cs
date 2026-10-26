using UnityEngine;
using System;

public class PetCoinManager : MonoBehaviour, IDataPersistence
{
    public static PetCoinManager Instance { get; private set; }

    // Global coin value
    private int petCoins;

    // 🔔 Event fired when coins change
    public event Action<int> OnCoinsChanged;

    // Singleton pattern
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }

    public void LoadData(GameData data)
    {
        petCoins = data.petCoins;
        OnCoinsChanged?.Invoke(petCoins);   // notify UI if present
    }

    public void SaveData(GameData data)
    {
        data.petCoins = petCoins;
    }

    // Adds coins, ensuring no negative values
    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        petCoins = Mathf.Max(0, petCoins + amount);
        Debug.Log($"[PetCoinManager] Coins changed to {petCoins}, invoking event");
        OnCoinsChanged?.Invoke(petCoins);
    }


    // Returns true if spend was successful
    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return false;
        if (petCoins < amount) return false;

        petCoins -= amount;
        OnCoinsChanged?.Invoke(petCoins);
        Debug.Log($"[PetCoinManager] Spent {amount}, remaining {petCoins}");
        return true;
    }

    // Resets coins to zero (for testing or game reset)
    public void ResetCoin()
    {
        petCoins = 0;
        OnCoinsChanged?.Invoke(petCoins);
        Debug.Log("[PetCoinManager] Coins reset to 0");
    }



    public int GetCoins() => petCoins; // return current coin amount

}
