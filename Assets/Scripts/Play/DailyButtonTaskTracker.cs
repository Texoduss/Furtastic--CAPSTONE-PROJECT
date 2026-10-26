using UnityEngine;

public class DailyButtonTaskTracker : MonoBehaviour
{
    [SerializeField] private string taskName;
    [SerializeField] private int progressPerClick = 1;

    public void OnButtonPressed() // Call this method when the button is pressed
    {
        DailyTaskManager.Instance.AddTaskProgress(taskName, progressPerClick);
       
    }
}
