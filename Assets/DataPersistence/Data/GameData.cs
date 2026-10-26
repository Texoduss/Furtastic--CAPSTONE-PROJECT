using System;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // Pet Info
    public string petType;
    public string petName;
    public string petBirthday;
    public string deathDate;
    public string selectedFeedItem;
    public string selectedToyItem;

    // Currency and Progression
    public int petCoins;
    public int currentLevel;
    public int pawStampCount;
    public int currentProgress;
    public int currentMedicineCount;

    // Time Tracking
    public long lastCareTimestamp;
    public long lastUpdated;

    // Health Status
    public bool petIsDead = false;
    public bool petIsDeleted;
    public bool isPetSick = false;
    public int sickDaysCount = 0;           
    public int consecutiveMissedDays; 

    // Friendship
    public float petFriendshipLevel;

    // Daily Tracking
    public int currentDay = 0;
    public int lastCareDay = 0;
    public string lastDailyCheckDate; // store as "yyyy-MM-dd"
    

    // Inventory and Collections
    public SerializableDictionary<string, bool> purchasedItems;
    public List<bool> petStampsCollected;
    public SerializableDictionary<string, int> petFriends;
    public List<DailyTask> dailyTasks;

    public static GameData Instance { get; private set; }

    public GameData()
    {
        // Default setup
        petType = "";
        petName = "";
        petBirthday = "";
        deathDate = "";
        selectedFeedItem = "";
        selectedToyItem = "";
        petCoins = 0;
        currentLevel = 0;
        pawStampCount = 0;
        currentProgress = 0;
        currentMedicineCount = 0;

        lastCareTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        lastUpdated = lastCareTimestamp;

        petIsDead = false;
        petIsDeleted = false;
        isPetSick = false;
        sickDaysCount = 0;
        consecutiveMissedDays = 0;

        petFriendshipLevel = 0f;

        currentDay = 0;
        lastCareDay = 0;

        purchasedItems = new SerializableDictionary<string, bool>();
        petStampsCollected = new List<bool>();
        petFriends = new SerializableDictionary<string, int>();
        dailyTasks = new List<DailyTask>();
    }

    // Helpers
    public string getPetType() => petType;
    public string getPetName() => petName;
    public string getPetBirthday() => petBirthday;
    public int getCurrentDay() => currentDay;
    public int getLastCareDay() => lastCareDay;
}
