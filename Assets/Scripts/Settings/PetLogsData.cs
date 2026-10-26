using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PetLogEntry
{
    public string petName;
    public string petType;
    public string dateCleared;
}

[Serializable]
public class PetLogsData
{
    public List<PetLogEntry> logs = new List<PetLogEntry>();
}

