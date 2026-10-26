using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DailyTaskUI : MonoBehaviour
{
    [SerializeField] private Transform taskParent;
    [SerializeField] private GameObject taskTextPrefab;

    private readonly List<TextMeshProUGUI> taskTexts = new List<TextMeshProUGUI>();
    public static DailyTaskUI Instance { get; private set; }

    private void Awake() => Instance = this;

    private void Start()
    {
        if (taskParent == null || taskTextPrefab == null)
        {
            Debug.LogError("[DailyTaskUI] Assign TaskParent and TaskTextPrefab!");
            enabled = false;
            return;
        }
    }
    private void Update() // Consider optimizing with events for better performance
    {
        if (DailyTaskManager.Instance == null) return;

        var tasks = DailyTaskManager.Instance.GetTasks();
        if (tasks.Count != taskTexts.Count)
        {
            RefreshUI();
            return;
        }

        for (int i = 0; i < tasks.Count; i++)
        {
            var t = tasks[i];
            string progress = $"{t.currentProgress}/{t.requiredProgress}";
            string state = t.isCompleted ? "Completed" : "Not Completed";
            taskTexts[i].text = $"{t.taskName} ({progress}) {state}";
        }
    }
    public void RefreshUI() // call after loading or resetting tasks
    {
        foreach (Transform child in taskParent)
        {
            Debug.Log("[DailyTaskUI] Destroying child: " + child.name);
            Destroy(child.gameObject);
        }
        taskTexts.Clear();
        Debug.Log("[DailyTaskUI] Cleared taskTexts list. Total cleared: " + taskTexts.Count);


        if (DailyTaskManager.Instance == null) return;

        foreach (var t in DailyTaskManager.Instance.GetTasks())
        {
            GameObject go = Instantiate(taskTextPrefab, taskParent);
            TextMeshProUGUI txt = go.GetComponent<TextMeshProUGUI>();

            string progress = $"{t.currentProgress}/{t.requiredProgress}";
            string state = t.isCompleted ? "Completed" : "Not Completed";
            txt.text = $"{t.taskName} ({progress}) {state}";
            taskTexts.Add(txt);
        }
    }
}
