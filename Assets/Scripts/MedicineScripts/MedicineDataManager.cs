using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineDataManager : MonoBehaviour, IDataPersistence
{
    public static MedicineDataManager Instance { get; private set; }

    private const int MaxMedicineCount = 3;
    private int currentMedicineCount = 0;
    private bool isPetSick = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public bool CanAddMedicine() => currentMedicineCount < MaxMedicineCount;
    public int GetMedicineCount() => currentMedicineCount;
    public bool IsPetSick() => isPetSick;

    public void AddMedicine(int amount = 1)
    {
        currentMedicineCount = Mathf.Min(currentMedicineCount + amount, MaxMedicineCount);
        Debug.Log($"Medicine added. Current medicine count: {currentMedicineCount}");
    }

    public bool UseMedicine()
    {
        if (currentMedicineCount <= 0)
        {
            Debug.LogWarning("[MedicineDataManager] No medicine available to use!");
            return false;
        }

        currentMedicineCount--;
        Debug.Log($"[MedicineDataManager] Medicine used. Remaining: {currentMedicineCount}");

        isPetSick = false;
        DataPersistenceManager.instance.SaveGame();
        return true;

    }

    public void SetPetSick(bool isSick)
    {
        isPetSick = isSick;
        Debug.Log($"Pet sickness status updated: {(isPetSick ? "Sick" : "Healthy")}");

        if(DeathDetailManager.Instance != null && isSick)
        {
            Debug.Log("[MedicineDataManager] Syncing sickness with DeathDetailManager.");

        }
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        this.currentMedicineCount = data.currentMedicineCount;
        this.isPetSick = data.isPetSick;
        Debug.Log($"Medicine data loaded. Current medicine count: {currentMedicineCount}, Pet is sick: {isPetSick}");
    }

    public void SaveData(GameData data)
    {
        data.currentMedicineCount = this.currentMedicineCount;
        data.isPetSick = this.isPetSick;
        Debug.Log($"Medicine data saved. Current medicine count: {currentMedicineCount}, Pet is sick: {isPetSick}");
    }

}
