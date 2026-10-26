using UnityEngine;

public class DailyTaskSetup : MonoBehaviour
{
    private void Start()
    {
        if (DailyTaskManager.Instance == null)
        {
            Debug.LogError("[DailyTaskSetup] DailyTaskManager not found!");
            return;
        }

        // Add a single daily task if it does not yet exist
        if (!DailyTaskManager.Instance.GetTasks().Exists(t => t.taskName == "Make a Noise!"))
        {
            DailyTaskManager.Instance.AddTask(new DailyTask("Make a Noise!", 5));
            Debug.Log("[DailyTaskSetup] Task 'Make a Noise!' added with limit 5.");
        }
    }
}
