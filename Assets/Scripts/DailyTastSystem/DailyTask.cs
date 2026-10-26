using System;
using UnityEngine;

/// <summary>
/// Represents a single daily task that can be serialized into GameData.
/// Tracks progress, completion state, and whether rewards were granted.
/// </summary>
[System.Serializable]
public class DailyTask
{
    [Tooltip("Display name of the task")]
    public string taskName;

    [Tooltip("Required progress to complete this task")]
    public int requiredProgress;

    [Tooltip("Current progress of the task")]
    public int currentProgress;

    [Tooltip("True if the friendship EXP reward was already granted")]
    public bool expGiven;

    /// <summary>
    /// Returns true if the task is completed.
    /// </summary>
    public bool isCompleted => currentProgress >= requiredProgress;

    /// <summary>
    /// Creates a new DailyTask.
    /// </summary>
    public DailyTask(string name, int required)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("[DailyTask] Creating a task with an empty name!");
            name = "Unnamed Task";
        }

        taskName = name;
        requiredProgress = Mathf.Max(1, required); // always at least 1
        currentProgress = 0;
        expGiven = false;
    }

    /// <summary>
    /// Adds progress to the task. Automatically clamps to requiredProgress.
    /// Grants friendship EXP when first completed.
    /// </summary>
    public void AddProgress(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"[DailyTask] AddProgress called with non-positive amount: {amount}");
            return;
        }

        if (isCompleted)
        {
            Debug.Log($"[DailyTask] '{taskName}' is already completed. No further progress added.");
            var data = DataPersistenceManager.instance.GetGameData();
            if (data != null) DataPersistenceManager.instance.SaveGame();

            return;
        }

        int before = currentProgress;
        currentProgress = Mathf.Clamp(currentProgress + amount, 0, requiredProgress);

        Debug.Log($"[DailyTask] '{taskName}' progress {before} -> {currentProgress}/{requiredProgress}");

        // ✅ Grant friendship EXP only once when completing
        if (isCompleted && !expGiven)
        {
            expGiven = true;

            if (FriendShipLevelManager.Instance != null)
            {
                var data = DataPersistenceManager.instance.GetGameData();
                if (data != null)
                {
                    data.lastCareTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    DataPersistenceManager.instance.SaveGame();
                }
                else
                {
                    Debug.LogWarning("[DailyTask] GameData.Instance is NULL! Care timestamp not saved.");
                }

                FriendShipLevelManager.Instance.AddProgress();
                Debug.Log($"[DailyTask] '{taskName}' completed -> Friendship EXP granted!");
                DailyTaskManager.Instance.MarkTasksCompletedForToday();
                
            }
            else
            {
                Debug.LogWarning("[DailyTask] FriendshipMeter.Instance is NULL! EXP not granted.");
            }
        }
    }

    /// <summary>
    /// Resets the task to allow it to be completed again (e.g., daily reset).
    /// </summary>
    public void ResetProgress()
    {
        Debug.Log($"[DailyTask] ResetProgress called on '{taskName}' (was {currentProgress})");

        currentProgress = 0;
        expGiven = false;
        Debug.Log($"[DailyTask] '{taskName}' progress reset.");
    }
}
