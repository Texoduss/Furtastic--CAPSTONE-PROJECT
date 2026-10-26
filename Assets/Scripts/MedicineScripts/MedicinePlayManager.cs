using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
// Manages the pet's sickness state and related UI elements
public class MedicineUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject medicineButton;
    [SerializeField] private GameObject background;
    [SerializeField] private Button debugNextDayButton;

    private EmoteUI emoteUI;

    private void Start()
    {
        medicineButton = GameObject.Find("MedicineButton");
        background = GameObject.Find("Background");

        if (background != null) background.SetActive(false);
        emoteUI = FindObjectOfType<EmoteUI>();
        if (medicineButton != null) medicineButton.SetActive(false);

        RefreshUI();

        if (debugNextDayButton != null)
            debugNextDayButton.onClick.AddListener(DebugSimulateNextDay);
    }

    public void DebugSimulateNextDay()
    {
        DailyTaskManager.Instance.DebugSimulateNextDay();
        CheckDailyTasks();
        
    }

    private void CheckDailyTasks()
    {
        if (DailyTaskManager.Instance == null)
        {
            Debug.LogWarning("[MedicinePlayManager] DailyTaskManager not found.");
            return;
        }

        List<DailyTask> tasks = DailyTaskManager.Instance.GetTasks();

        // If no tasks, assume first day
        if (tasks == null || tasks.Count == 0)
        {
            Debug.Log("[MedicinePlayManager] First day — pet stays healthy.");
            MedicineDataManager.Instance.SetPetSick(false);
            return;
        }

        // Skip sickness check if already checked today
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");
        if (DataPersistenceManager.instance.GetGameData().lastDailyCheckDate == today)
        {
            Debug.Log("[MedicinePlayManager] Already checked daily tasks today.");
            return;
        }

        if (!DailyTaskManager.Instance.WereAllTasksCompletedOn())
        {
            Debug.Log("[MedicinePlayManager] Not all daily tasks completed — pet becomes sick.");
            MedicineDataManager.Instance.SetPetSick(true);
        }
        else
        {
            Debug.Log("[MedicinePlayManager] All daily tasks completed — pet stays healthy.");
            MedicineDataManager.Instance.SetPetSick(false);
        }

        // Update last check date
        DataPersistenceManager.instance.GetGameData().lastDailyCheckDate = today;

        RefreshUI();    
    }
    private void RefreshUI() // Updates the UI based on the pet's sickness state
    {
        Debug.Log($"[MedicinePlayManager] RefreshUI check: sick = {MedicineDataManager.Instance.IsPetSick()}");

        bool sick = MedicineDataManager.Instance.IsPetSick();

        if (sick)
        {
            emoteUI.ShowPetSadEmote();
            if (medicineButton != null) medicineButton.SetActive(true);
            if (background != null) background.SetActive(true);
        }
        else
        {
            emoteUI.HidePetSadEmote();
            if (medicineButton != null) medicineButton.SetActive(false);
            if (background != null) background.SetActive(false);
        }
    }

    public void OnUseMedicine() // Called when the player uses medicine
    {
        if (MedicineDataManager.Instance == null)
        {
            Debug.LogWarning("[MedicinePlayManager] MedicineDataManager instance not found.");
            return;
        }
        if (!MedicineDataManager.Instance.IsPetSick())
        {
            Debug.Log("[MedicinePlayManager] Pet is not sick, cannot use medicine.");
            return;
        }

        Debug.Log("[MedicinePlayManager] Medicine used successfully.");
        emoteUI.HidePetSadEmote();
        RefreshUI();
    }

    
    
}
