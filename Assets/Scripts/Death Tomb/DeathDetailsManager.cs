using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathDetailManager : MonoBehaviour, IDataPersistence
{
    public static DeathDetailManager Instance { get; private set; }

    [Header("Health Settings")]
    [SerializeField] private string deathSceneName = "DeathScene";
   
    private bool isPetDead;
    public int consecutiveMissedDays => GameData.Instance.consecutiveMissedDays;
    public int maxDaysBeforeDeath = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(WaitForDataAndInit());
    }

    private IEnumerator WaitForDataAndInit()
    {
        yield return new WaitUntil(() => DataPersistenceManager.instance != null && GameData.Instance != null);
        yield return null;

        CheckForPetDeath();
    }
    // ---------------- IDataPersistence ----------------
    public void LoadData(GameData data)
    {
        
        this.isPetDead = data.petIsDead;

        Debug.Log($"[DeathDetailManager] Loaded data: dead={isPetDead}");
    }

    public void SaveData(GameData data)
    {
        
    }

    public void CheckForPetDeath()
    {
        if (GameData.Instance == null)
        {
            Debug.LogWarning("[DeathDetailManager] GameData not initialized. Cannot check pet death yet.");
            return;
        }

        if (isPetDead)
        {
            Debug.Log("[DeathDetailManager] Pet is already dead. No further checks needed.");
            return;
        }

        if (GameData.Instance.consecutiveMissedDays >= maxDaysBeforeDeath)
        {
            Debug.Log("[DeathDetailManager] Pet has exceeded max missed days. Handling death.");
            HandlePetDeath();
        }
        else
        {
            Debug.Log($"[DeathDetailManager] Pet is alive. Missed days: {GameData.Instance.consecutiveMissedDays}/{maxDaysBeforeDeath}");
        }
    }


    private void HandlePetDeath()
    {
        if (isPetDead)
        {
            Debug.Log("[DeathDetailManager] Pet is already marked as dead. No action taken.");
            return;
        }
        isPetDead = true;
        var data = DataPersistenceManager.instance?.GetGameData();
        if (data != null)
        {
            data.petIsDead = true;
            DataPersistenceManager.instance.SaveGame();
            Debug.Log("[DeathDetailManager] Pet death handled and saved.");
        }
        else
        {
            Debug.LogError("[DeathDetailManager] GameData NULL when handling pet death.");
        }
        StartCoroutine(DeletePetAndLoadDeathScene());
    }

    private IEnumerator DeletePetAndLoadDeathScene()
    {
        yield return new WaitForSeconds(0.5f);

        var data = DataPersistenceManager.instance?.GetGameData();
        if (data == null)
        {
            Debug.LogError("[DeathDetailManager] GameData NULL during deletion.");
            yield break;
        }

        string activeProfileId = DataPersistenceManager.instance.GetActiveProfileId();
        if (string.IsNullOrEmpty(activeProfileId))
            activeProfileId = "0";

        AddPetLog(data.getPetName(), data.petType, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm"));
        DataPersistenceManager.instance.DeleteProfileData(activeProfileId, suppressReload: true);

        yield return new WaitForSeconds(0.3f);
        SafeLoadDeathScene();
    }

    private void AddPetLog(string petName, string petType, string dateCleared)
    {
        string logPath = Path.Combine(Application.persistentDataPath, "PetLogs.json");
        PetLogsData logsData = new PetLogsData();

        if (File.Exists(logPath))
        {
            try
            {
                string json = File.ReadAllText(logPath);
                logsData = JsonUtility.FromJson<PetLogsData>(json) ?? new PetLogsData();
            }
            catch (Exception e)
            {
                Debug.LogError($"[DeathDetailManager] Error loading logs: {e}");
            }
        }
        logsData.logs.Insert(0, new PetLogEntry { petName = petName, petType = petType, dateCleared = dateCleared });
        if (logsData.logs.Count > 3)
            logsData.logs.RemoveRange(3, logsData.logs.Count - 3);
        try
        {
            string json = JsonUtility.ToJson(logsData, true);
            File.WriteAllText(logPath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[DeathDetailManager] Failed to write PetLogs.json: {e}");
        }
    }

    private void SafeLoadDeathScene()
    {
        if (Application.CanStreamedLevelBeLoaded(deathSceneName))
        {
            SceneManager.LoadScene(deathSceneName);
            Debug.Log("[DeathDetailManager] DeathScene loaded.");
        }
        else
        {
            Debug.LogError($"[DeathDetailManager] Scene '{deathSceneName}' not found in build settings!");
        }
    }
}
