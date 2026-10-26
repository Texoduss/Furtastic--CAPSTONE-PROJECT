using UnityEngine;
using TMPro;
using System.IO;
using System;

public class DeathTombUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI petNameText;
    [SerializeField] private TextMeshProUGUI petTypeText;
    [SerializeField] private TextMeshProUGUI dateText;

    private string logsFilePath;

    private void Awake()
    {
        logsFilePath = Path.Combine(Application.persistentDataPath, "PetLogs.json");
    }

    private void Start()
    {
        DisplayLatestDeath();
    }

    private void DisplayLatestDeath()
    {
        if (!File.Exists(logsFilePath))
        {
            Debug.LogError("[DeathTombUI] No PetLogs.json found. Nothing to display.");
            return;
        }

        string json = File.ReadAllText(logsFilePath);
        PetLogsData logsData = JsonUtility.FromJson<PetLogsData>(json);

        if (logsData == null || logsData.logs == null || logsData.logs.Count == 0)
        {
            Debug.LogError("[DeathTombUI] Logs are empty. Nothing to display.");
            return;
        }

        PetLogEntry latest = logsData.logs[0]; // newest first
        Debug.Log($"[DeathTombUI] Displaying → {latest.petName}, {latest.petType}, {latest.dateCleared}");

        petNameText.text = latest.petName;
        petTypeText.text = latest.petType;
        dateText.text = latest.dateCleared;
    }
}
