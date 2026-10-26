using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandlerManager dataHandlerManager;

    private string selectedProfileId = "0";

    public static DataPersistenceManager instance { get; private set; }

    /// <summary>
    // Initializes the DataPersistenceManager instance.
    /// 

    private void Awake() // Called when the script instance is being loaded
    {
        if(instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is disabled. All data will be lost when exiting play mode.");
        }

        this.dataHandlerManager = new FileDataHandlerManager(Application.persistentDataPath, fileName, useEncryption);
        Debug.Log($"[DataPersistenceManager] Persistent Path: {Application.persistentDataPath}");
        InitializeSelectedProfileId();
    }
    private void OnEnable()// Called when the object becomes enabled and active
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void OnDisable() // Called when the behaviour becomes disabled or inactive
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SaveGame();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) // Called when a new scene is loaded
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void DeleteProfileData(string profileId, bool suppressReload = false) // Deletes the profile data for the given profile ID
    {
        if (string.IsNullOrEmpty(profileId))
        {
            Debug.LogWarning("[DataPersistenceManager] DeleteProfileData called with null/empty profileId. Aborting.");
            return;
        }

        GameData profileData = null;

        try
        {
            profileData = dataHandlerManager.Load(profileId);
        }
        catch (Exception e)
        {
            Debug.LogError($"[DataPersistenceManager] Failed to load profile {profileId}: {e.Message}");
        }

        if (profileData == null)
        {
            Debug.LogWarning($"[DataPersistenceManager] No data found for profile {profileId}. Nothing to delete.");
            return;
        }

        // Mark as deleted and log death
        try
        {
            profileData.petIsDeleted = true;
            profileData.deathDate = DateTime.Now.ToBinary().ToString();
            dataHandlerManager.Save(profileData, profileId);

            PetLogsManager logsManager = FindObjectOfType<PetLogsManager>();
            if (logsManager != null)
            {
                logsManager.AddLog(profileData.getPetName(), profileData.getPetType(), DateTime.Now);
                Debug.Log($"[DataPersistenceManager] Logged deletion for {profileData.getPetName()} ({profileData.getPetType()})");
            }
            else
            {
                Debug.LogWarning("[DataPersistenceManager] PetLogsManager not found in scene. Skipping log.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[DataPersistenceManager] Failed to mark profile as deleted or log: {e.Message}");
        }

        // Delete the save file
        try
        {
            dataHandlerManager.Delete(profileId);
            Debug.Log($"[DataPersistenceManager] Profile {profileId} deleted from disk.");
        }
        catch (Exception e)
        {
            Debug.LogError($"[DataPersistenceManager] Failed to delete profile file {profileId}: {e.Message}");
        }

        // Clear in-memory data for safety
        if (this.selectedProfileId == profileId)
        {
            this.gameData = null;
            this.selectedProfileId = null;
        }

        if (!suppressReload)
        {
            try
            {
                // Find a new valid profile (if any remain)
                InitializeSelectedProfileId();

                // Reload cleanly
                if (!string.IsNullOrEmpty(this.selectedProfileId))
                    LoadGame();
                else
                    Debug.Log("[DataPersistenceManager] No remaining profiles found after deletion.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[DataPersistenceManager] Error reloading after delete: {e.Message}");
            }
        }
    }
    // Sets the selected profile ID, using the most recently updated profile or a test ID if specified.
    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandlerManager.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overriding selected profile id with test id: " + testSelectedProfileId);

        }

        if (string.IsNullOrEmpty(this.selectedProfileId))
        {
            this.selectedProfileId = "0";
            Debug.Log("[DataPersistenceManager] No profiles found, using default id: test");
        }
    }
    // Changes the selected profile ID and loads the corresponding game data.
    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }
    // Starts a new game by initializing a new GameData instance and saving it.
    public void NewGame()
    {
        this.gameData = new GameData();
        SaveGame();
    }
    // Loads game data from a file and populates the relevant objects.
    public void LoadGame()
    {
        if (string.IsNullOrEmpty(selectedProfileId))
        {
            Debug.LogWarning("[DataPersistenceManager] LoadGame called with null/empty profileId. Aborting.");
            return;
        }
        string fullPath = Path.Combine(Application.persistentDataPath, selectedProfileId, fileName);
        Debug.Log($"[LoadGame] Trying to load file from: {fullPath}");
        Debug.Log($"[LoadGame] Selected Profile ID: {selectedProfileId}");

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is disabled. Not loading data.");
            return;
        }

        this.gameData = dataHandlerManager.Load(selectedProfileId);

        if (this.gameData == null && initializeDataIfNull)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        if (this.gameData == null)
        {
            Debug.Log("No game data found, A New Game needs to be started before data can be loaded.");
            return;
        }

         Debug.Log("Loaded data for profile id: " + selectedProfileId + ". Data last updated: " + System.DateTime.FromBinary(gameData.lastUpdated).ToString());

        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }
    
    public void SaveGame() // Saves the game data to a file
    {
        if (disableDataPersistence) // Data persistence is disabled
        {
            Debug.LogWarning("Data Persistence is disabled. Not loading data.");
            return;
        }

        if (this.gameData == null) // no game data to save
        {
            Debug.LogWarning("No game data to save for " + selectedProfileId + ". A New Game must be started before data can be saved.");
            return;
        }

        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects) // save data for all relevant objects
        {
            dataPersistenceObject.SaveData(gameData);
        }
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        if (string.IsNullOrEmpty(selectedProfileId))
        {
            Debug.LogWarning("[DataPersistenceManager] Cannot save — profile ID is null or empty.");
            return;
        }
        dataHandlerManager.Save(gameData, selectedProfileId);
    }

    public void OnApplicationQuit() // Called when the application is quitting
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() // Finds all objects that implement IDataPersistence
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData() // Checks if game data is available
    {
        return this.gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData() // Returns a dictionary of all profiles and their game data
    {
        return dataHandlerManager.LoadAllProfiles();
    }

    public GameData GetGameData() // Returns the current game data
    {
        return this.gameData;
    }

    public string GetActiveProfileId()
    {
        return this.selectedProfileId;
    }

    private string GetProfileFilePath(string profileId)
    {
        return Path.Combine(Application.persistentDataPath, profileId + ".json");

    }

    public void ClearCurrentData()
    {
        Debug.Log("[DataPersistenceManager] Clearing current GameData from memory.");
        gameData = null;
    }
}
