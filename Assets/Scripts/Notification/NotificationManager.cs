using System;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private const string CHANNEL_ID = "reminder_channel";

    void Start()
    {
        // Register notification channel
        var channel = new AndroidNotificationChannel()
        {
            Id = CHANNEL_ID,
            Name = "Reminders",
            Importance = Importance.Default,
            Description = "Task reminder notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    // Check if there is any incomplete task
    private bool HasIncompleteTasks()
    {
        if (DailyTaskManager.Instance == null) return false;

        foreach (var task in DailyTaskManager.Instance.GetTasks())
        {
            if (!task.isCompleted) return true;
        }
        return false;
    }

    // Called when app goes to background
    void OnApplicationPause(bool paused)
    {
        if (paused && HasIncompleteTasks())
        {
            ScheduleNotification("Daily Tasks Pending",
                "You haven’t completed your daily tasks yet.", 1);
        }
    }

    // Schedule a notification if tasks are incomplete (safe to call manually)
    public void ScheduleIfIncomplete()
    {
        if (HasIncompleteTasks())
        {
            ScheduleNotification("Daily Tasks Pending",
                "You haven’t completed your daily tasks yet.", 1);
        }
    }

    // Schedule a notification after X minutes
    public void ScheduleNotification(string title, string message, int minutesFromNow)
    {
        var notification = new AndroidNotification
        {
            Title = title,
            Text = message,
            SmallIcon = "icon_0",
            FireTime = DateTime.Now.AddMinutes(minutesFromNow)
        };

        AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
        Debug.Log($"[Notification] Scheduled: {title} in {minutesFromNow} minutes.");
    }

    // Cancel all notifications
    public void CancelAllNotifications()
    {
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
        AndroidNotificationCenter.CancelAllScheduledNotifications();
    }

    // Call this when a task is completed
    public void MarkTaskDone()
    {
        CancelAllNotifications();
        Debug.Log("[NotificationManager] All tasks completed, notifications cancelled.");
    }
}
