using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO; 

public class FileDataHandlerManager
{
    private string dataDirPath = ""; //THIS IS THE LOCATION AND ADRESS OF THE FIEL
    private string dataFileName = ""; //MAKES THE FILE NAME
    private bool useEncryption = false;
    private readonly string encryptionKey = "sige"; // Replace with your actual encryption key

    public FileDataHandlerManager(string dataDirPath, string dataFileName, bool useEncryption) //ALLOWS THE SAVE AND LOAD OF DATA
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }
    public GameData Load(string profileId) // LOADS THE GAME DATA FROM A FILE
    {
        if (profileId == null)
        {
            return null;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName); //COMBINES THE PATH AND THE FILE NAME TO MAKE A FULL PATH
        GameData loadedData = null;

        if (!File.Exists(fullPath)) //IF THE FILE DOESN'T EXIST IT WILL GIVE A WARNING
        {
            Debug.LogWarning($"[FileDataHandlerManager] No save file found at {fullPath}");
        }
        else
        {
            Debug.Log($"[FileDataHandlerManager] Found save file at {fullPath}, attempting to load...");
        }


        if (File.Exists(fullPath)) //IF THE FILE EXISTS IT WILL TRY TO LOAD IT
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))// THIS LOADS DATA
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        // Read the entire file content
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)//THIS ENCRYPTS DATA
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }

            catch (Exception e)
            {
                Debug.LogError("Failed to load data from " + fullPath + ": " + e.Message);

            }
        }
        return loadedData;

    }
    // Saves the game data to a file
    public void Save(GameData data, string profileId)
    {
        if(profileId == null)
        {
            Debug.LogError("Profile ID is null. Cannot save data.");
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        Debug.Log($"[FileDataHandlerManager] Attempting to save to: {fullPath}");
        try
        {
            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }
            // Write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }

        catch (Exception e)
        {
            Debug.LogError("Failed to save data to " + fullPath + ": " + e.Message);
        }
            
    }

    public void Delete(string profileId) // Deletes the profile data for the given profile ID
    {
        if (profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName); //COMBINES THE PATH AND THE FILE NAME TO MAKE A FULL PATH
        try
        {
            if (File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }

            else
            {
                Debug.LogWarning("Tried to delete profile data at " + fullPath + " but file does not exist");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete profile data at " + fullPath + " : " + e.Message);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles() // Loads all profiles and returns a dictionary of profile IDs and their corresponding game data
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();
        
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;
            string fullPath = Path.Combine(dirInfo.FullName, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory " + dirInfo.Name + " because it does not contain data file");
                continue;
            }

            GameData profileData = Load(profileId);

            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {

                Debug.LogError("Tried to load profile at " + fullPath + " but something went wrong");
            }
        }
        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileId() // Gets the profile ID of the most recently updated profile
    {
        string mostRecentProfileId = null;

        Dictionary<string, GameData> profilesData = LoadAllProfiles();
        foreach (KeyValuePair<string, GameData > pair in profilesData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            if (gameData == null)
            {
                continue;
            }

            if (mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesData[mostRecentProfileId].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileId = profileId;
                }
            }

        }

        return mostRecentProfileId;
    }

    // Encrypts or decrypts the data using a simple XOR operation with the encryption key
    private string EncryptDecrypt(string data) // XOR ENCRYPTION
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
        }
        return modifiedData;
    }

    
}
