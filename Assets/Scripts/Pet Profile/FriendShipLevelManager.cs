using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendShipLevelManager : MonoBehaviour, IDataPersistence
{
    [Header("Settings")]
    [SerializeField] private int pointsPerLevel = 10;

    private int currentLevel = 0;
    private int currentProgress = 0;

    public static FriendShipLevelManager Instance { get; private set; }

    public int CurrentLevel => currentLevel;
    public int CurrentProgress => currentProgress;
    public int PointsPerLevel => pointsPerLevel;

    public delegate void OnLevelChanged(int level, int progress, int maxProgresss);
    public event OnLevelChanged LevelChanged;

    private void Start()
    {
        Debug.Log($"[FriendShipLevelManager] Loaded Level: {currentLevel}, Progress: {currentProgress}");
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }

    public void AddProgress(int points = 1) // Call this method to add friendship points
    {
        currentProgress += points;
        if (currentProgress >= pointsPerLevel)
        {
            currentProgress = 0;
            currentLevel++;

            if (PawStampManager.Instance != null)
            {
                PawStampManager.Instance.AddStamp();
            }
            else
            {
                Debug.LogError("[FriendShipLevelManager] PawStampManager instance is null!");
            }
        }

        LevelChanged?.Invoke(currentLevel, currentProgress, pointsPerLevel);
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.SaveGame();
        }
        else
        {
            Debug.LogError("[FriendShipLevelManager] DataPersistenceManager instance is null!");
        }
    }

    public int GetCurrentFriendshipLevel() // Returns the current friendship level
    {
        return currentLevel;
    }

    public void LoadData(GameData data)
    {
        currentLevel = data.currentLevel;
        currentProgress = data.currentProgress;
        LevelChanged?.Invoke(currentLevel, currentProgress, pointsPerLevel);
    }

    public void SaveData(GameData data)
    {
        data.currentLevel = currentLevel;
        data.currentProgress = currentProgress;
        Debug.Log($"[FriendShipLevelManager] SaveData -> Level:{currentLevel}, Progress:{currentProgress}");
    }

}
