using UnityEngine;
using Unity.Notifications.Android;
using System.Collections;
using System.Collections.Generic;

public class DailyTaskManager : MonoBehaviour, IDataPersistence
{
    public string lastTasksCompleteDate;
    public bool wasTaskYesterdayComplete;
    public static DailyTaskManager Instance { get; private set; }

    [SerializeField] private List<DailyTask> tasks = new List<DailyTask>();

    private const string NotificationChannelId = "daily_tasks_channel";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Setup Android notification channel
        CreateNotificationChannel();
    }

    private void Start()
    {
        CheckForDailyReset();
        StartCoroutine(InitializeDailyTasksAfterLoad());
    }

    private IEnumerator InitializeDailyTasksAfterLoad()
    {
        yield return new WaitUntil(() => DataPersistenceManager.instance != null && DataPersistenceManager.instance.HasGameData());
        yield return null; // wait one more frame

        DataPersistenceManager.instance.SaveGame();
        Debug.Log("[DailyTaskManager] Registered with DataPersistenceManager.");
    }

    // ---- Notification Setup ----
    private void CreateNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = NotificationChannelId,
            Name = "Daily Tasks",
            Importance = Importance.Default,
            Description = "Reminders for incomplete daily tasks",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void ScheduleIncompleteTasksNotification()
    {
        var notification = new AndroidNotification
        {
            Title = "Daily Tasks Reminder",
            Text = "You haven't completed your daily tasks today! Your pet might get sick!",
            FireTime = System.DateTime.Now.AddMinutes(1) // for testing, 1 min later
        };

        AndroidNotificationCenter.SendNotification(notification, NotificationChannelId);
        Debug.Log("[DailyTaskManager] Incomplete tasks notification scheduled.");
    }

    private void ScheduleNextDayResetNotification()
    {
        var notification = new AndroidNotification
        {
            Title = "New Day Started",
            Text = "Your daily tasks have been reset. Time to take care of your pet!",
            FireTime = System.DateTime.Now.AddMinutes(1) // for testing
        };

        AndroidNotificationCenter.SendNotification(notification, NotificationChannelId);
        Debug.Log("[DailyTaskManager] Next day reset notification scheduled.");
    }

    // ---- Daily Task Logic ----
    public List<DailyTask> GetTasks() => tasks;

    public void AddTask(DailyTask task)
    {
        if (tasks.Exists(t => t.taskName == task.taskName))
        {
            Debug.LogWarning($"[DailyTaskManager] Task '{task.taskName}' already exists.");
            return;
        }
        tasks.Add(task);
        Debug.Log($"[DailyTaskManager] Task '{task.taskName}' added.");
        DataPersistenceManager.instance.SaveGame();
    }

    public void AddTaskProgress(string name, int amount)
    {
        var t = tasks.Find(x => x.taskName == name);
        if (t == null)
        {
            Debug.LogError($"[DailyTaskManager] Task '{name}' not found!");
            return;
        }

        int before = t.currentProgress;
        t.AddProgress(amount);

        if (t.currentProgress != before)
            DataPersistenceManager.instance.SaveGame();
    }

    public bool AreAllTasksComplete()
    {
        return tasks.TrueForAll(t => t.isCompleted);
    }

    public bool WereAllTasksCompletedOn()
    {
        return wasTaskYesterdayComplete;
    }

    public void MarkTasksCompletedForToday()
    {
        lastTasksCompleteDate = System.DateTime.Now.ToString("yyyy-MM-dd");

        if (!AreAllTasksComplete())
        {
            ScheduleIncompleteTasksNotification();
        }
    }

    
    public void ResetAllTasks()
    {
        Debug.Log("[DailyTaskManager] ResetAllTasks called");
        foreach (var t in tasks)
        {
            t.ResetProgress();
        }
        DataPersistenceManager.instance.SaveGame();

        // Schedule notification for new day
        ScheduleNextDayResetNotification();
    }
    public void CheckForDailyReset()
    {
        if (GameData.Instance == null)
        {
            Debug.LogWarning("[DailyTaskManager] GameData not yet ready. Skipping daily reset check.");
            return;
        }

        System.DateTime today = System.DateTime.Now.Date;
        System.DateTime lastDate;

        if (string.IsNullOrEmpty(lastTasksCompleteDate) ||
            !System.DateTime.TryParse(lastTasksCompleteDate, out lastDate))
        {
            lastDate = System.DateTime.MinValue;
        }

        if (today > lastDate.Date)
        {
            Debug.Log("[DailyTaskManager] New day detected. Resetting tasks.");

            // If tasks weren’t completed yesterday → increment missed days
            if (!WereAllTasksCompletedOn())
            {
                IncrementMissedDays();
            }
            else
            {
                ResetMissedDays();
            }

            // Reset tasks for the new day
            ResetAllTasks();

            // Re-check pet death condition (safe)
            DeathDetailManager.Instance?.CheckForPetDeath();
        }
        else
        {
            Debug.Log("[DailyTaskManager] Same day detected. No reset needed.");
        }
    }


    public void DebugSimulateNextDay()
    {
        string yesterday = System.DateTime.Now.ToString("yyyy-MM-dd");
        ResetAllTasks();
        Debug.Log($"[DailyTaskManager] Simulating next day. Yesterday: {yesterday}");

        lastTasksCompleteDate = System.DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
        Debug.Log("[DailyTaskManager] Next day simulated. Tasks reset, last completed date set to two days ago.");
    }

    private void EnsureDefaultTask()
    {
        if (tasks.Count == 0)
        {
            AddTask(new DailyTask("Make a Noise!", 5));
            Debug.Log("[DailyTaskManager] Default task added.");
        }
    }

    private void IncrementMissedDays()
    {
        if (GameData.Instance == null)
        {
            Debug.LogWarning("[DailyTaskManager] GameData not initialized. Cannot increment missed days.");
            return;
        }

        GameData.Instance.consecutiveMissedDays++;
        Debug.Log($"[DailyTaskManager] Missed day added. Total consecutive missed days: {GameData.Instance.consecutiveMissedDays}");
        DataPersistenceManager.instance?.SaveGame();
    }

    private void ResetMissedDays()
    {
        if (GameData.Instance == null)
        {
            Debug.LogWarning("[DailyTaskManager] GameData not initialized. Cannot reset missed days.");
            return;
        }

        if (GameData.Instance.consecutiveMissedDays > 0)
        {
            GameData.Instance.consecutiveMissedDays = 0;
            Debug.Log("[DailyTaskManager] Consecutive missed days reset to 0.");
            DataPersistenceManager.instance?.SaveGame();
        }
    }


    // IDataPersistence
    public void LoadData(GameData data)
    {
        if (data.dailyTasks != null && data.dailyTasks.Count > 0)
            tasks = new List<DailyTask>(data.dailyTasks);
        else
            EnsureDefaultTask();
    }

    public void SaveData(GameData data)
    {
        data.dailyTasks = new List<DailyTask>(tasks);
    }
}
