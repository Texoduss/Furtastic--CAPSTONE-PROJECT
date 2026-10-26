using UnityEngine;

[System.Serializable]
public class PawStamp
{
    public int currentCount; // Expected range: 0–9

    public PawStamp()
    {
        currentCount = 0;
    }

    public void Clamp(int max)
    {
        if (currentCount < 0 || currentCount >= max)
        {
            Debug.LogWarning($"[PawStamp] Count {currentCount} was out of range. Clamping.");
            currentCount = Mathf.Clamp(currentCount, 0, max - 1);
        }
    }
}

public class PawStampManager : MonoBehaviour, IDataPersistence
{
    public static PawStampManager Instance { get; private set; }

    [SerializeField] private int maxStamps = 11;   // Should not be < 1
    private PawStamp pawStamp = new PawStamp();

    public int CurrentCount => pawStamp.currentCount;

    private void Start()
    {
        DataPersistenceManager.instance.LoadGame();
        Debug.Log($"[PawStampManager] Loaded count: {pawStamp.currentCount}");
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("[PawStampManager] Multiple instances detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (maxStamps < 1)
        {
            Debug.LogWarning("[PawStampManager] maxStamps was less than 1. Resetting to 10.");
            maxStamps = 10;
        }
    }

    public void AddStamp()
    {
        if (pawStamp == null)
        {
            Debug.LogError("[PawStampManager] PawStamp data is null. Cannot add stamp.");
            return;
        }

        pawStamp.currentCount++;

        if (pawStamp.currentCount >= maxStamps)
        {
            Debug.Log($"[PawStampManager] Max stamps reached ({maxStamps}). Resetting to 0.");
            pawStamp.currentCount = 0;
        }

        if (PetCoinManager.Instance != null)
        {
            PetCoinManager.Instance.AddCoins(100); // Reward 100 coins for adding a stamp
            Debug.Log("[PawStampManager] Added 100 coins for adding a stamp.");
        }
        else
        {
            Debug.LogWarning("[PawStampManager] PetCoinManager instance is null. Cannot add coins.");
        }

        // Attempt to save if DataPersistenceManager exists
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGame();
        }
        else
        {
            Debug.LogWarning("[PawStampManager] DataPersistenceManager not found. Progress will not be saved.");
        }
    }

    // IDataPersistence implementation
    public void LoadData(GameData data)
    {
        if (data == null)
        {
            Debug.LogWarning("[PawStampManager] GameData was null during LoadData.");
            return;
        }

        pawStamp.currentCount = data.pawStampCount;
        pawStamp.Clamp(maxStamps);
        Debug.Log($"[PawStampManager] Loaded count: {pawStamp.currentCount}");
    }

    public void SaveData(GameData data)
    {
        if (data == null)
        {
            Debug.LogError("[PawStampManager] GameData was null during SaveData.");
            return;
        }

        data.pawStampCount = pawStamp.currentCount;
        Debug.Log($"[PawStampManager] Saved count: {pawStamp.currentCount}");
    }
}