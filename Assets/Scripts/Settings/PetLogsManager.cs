using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PetLogsManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform logsContainer;   // Parent for all rows
    [SerializeField] private GameObject logRowPrefab;   // Prefab with PetLogRow script
    [SerializeField] private int maxLogs = 3;           // Limit logs to a maximum

    private string logsFilePath;
    private PetLogsData logsData;

    public static PetLogsManager Instance { get; private set; }

    private void Start()
    {
        Debug.Log($"[PetLogsManager] Runtime check → " +
                  $"Container: {(logsContainer ? logsContainer.name : "NULL")}, " +
                  $"Prefab: {(logRowPrefab ? logRowPrefab.name : "NULL")}");

        
    }

    private void Awake()
    {
        logsFilePath = Path.Combine(Application.persistentDataPath, "PetLogs.json");
        Debug.Log($"[PetLogsManager] Logs file path set: {logsFilePath}");
        LoadLogs();

        // Always enforce limit BEFORE first UI build
        EnforceLogLimit();
        RefreshUI();
    }

    public void AddLog(string petName, string petType, DateTime date)
    {
        if (string.IsNullOrEmpty(petName) || string.IsNullOrEmpty(petType))
        {
            Debug.LogWarning("[PetLogsManager] AddLog called with empty name or type!");
            return;
        }

        PetLogEntry entry = new PetLogEntry
        {
            petName = petName,
            petType = petType,
            dateCleared = date.ToString("yyyy-MM-dd HH:mm")
        };

        Debug.Log($"[PetLogsManager] Adding log → {entry.petName}");

        // Insert new entry at the top of the list
        logsData.logs.Insert(0, entry);

        SaveLogs();

        // Force a reload from disk to ensure fresh data
        LoadLogs();

        Debug.Log($"[PetLogsManager] Before Refresh count: {logsData.logs.Count}");
        RefreshUI();
    }


    private void EnforceLogLimit()
    {
        if (logsData.logs.Count > maxLogs)
        {
            int removeCount = logsData.logs.Count - maxLogs;
            Debug.Log($"[PetLogsManager] Trimming {removeCount} oldest logs...");
            logsData.logs.RemoveRange(logsData.logs.Count - removeCount, removeCount);
        }
        Debug.Log($"[PetLogsManager] List count after trim: {logsData.logs.Count}");
    }

    private void LoadLogs()
    {
        if (File.Exists(logsFilePath))
        {
            try
            {
                string json = File.ReadAllText(logsFilePath);
                logsData = JsonUtility.FromJson<PetLogsData>(json);
                if (logsData == null || logsData.logs == null)
                {
                    Debug.LogWarning("[PetLogsManager] Loaded data was null, creating new list.");
                    logsData = new PetLogsData();
                }
                else
                {
                    SortLogsByDate();   // ensure newest first
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[PetLogsManager] Failed to read file: {e}");
                logsData = new PetLogsData();
            }
        }
        else
        {
            Debug.LogWarning("[PetLogsManager] No logs file found, starting empty.");
            logsData = new PetLogsData();
        }
    }

    private void SaveLogs()
    {
        try
        {
            SortLogsByDate(); // optional safeguard
            string json = JsonUtility.ToJson(logsData, true);
            File.WriteAllText(logsFilePath, json);
            Debug.Log($"[PetLogsManager] Saved {logsData.logs.Count} log entries.");
        }
        catch (Exception e)
        {
            Debug.LogError($"[PetLogsManager] Save failed: {e}");
        }
    }

    /// <summary>
    /// Sort logs newest → oldest using DateTime parse
    /// </summary>
    private void SortLogsByDate()
    {
        logsData.logs.Sort((a, b) =>
        {
            DateTime da = DateTime.MinValue;
            DateTime db = DateTime.MinValue;
            DateTime.TryParse(a.dateCleared, out da);
            DateTime.TryParse(b.dateCleared, out db);
            return db.CompareTo(da); // descending
        });
    }


    private void RefreshUI()
    {
        if (logsContainer == null)
        {
            Debug.LogError("[PetLogsManager] LogsContainer is not assigned!");
            return;
        }

        Debug.Log($"[PetLogsManager] Refreshing UI → Destroying old children: {logsContainer.childCount}");
        foreach (Transform child in logsContainer)
            Destroy(child.gameObject);

        // Create rows for each entry (newest first)
        for (int i = 0; i < logsData.logs.Count; i++)
        {
            PetLogEntry entry = logsData.logs[i];
            CreateRow(entry, i);
        }

        Debug.Log($"[PetLogsManager] UI refreshed. Rows created: {logsContainer.childCount}");
    }

    private void CreateRow(PetLogEntry entry, int index)
    {
        if (logRowPrefab == null)
        {
            Debug.LogError("[PetLogsManager] Missing logRowPrefab reference!");
            return;
        }

        GameObject row = Instantiate(logRowPrefab, logsContainer);
        row.transform.SetSiblingIndex(0); // Ensure newest is always at top

        Debug.Log($"[PetLogsManager] Created row #{index} → {entry.petName}");

        PetLogRow rowScript = row.GetComponent<PetLogRow>();
        if (rowScript != null)
            rowScript.SetData(entry.petName, entry.petType, entry.dateCleared);
        else
            Debug.LogError("[PetLogsManager] Prefab missing PetLogRow script!");
    }
}
