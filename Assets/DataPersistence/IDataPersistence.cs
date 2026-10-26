using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence // Interface for data persistence
{
    void LoadData(GameData data); // Method to load data into the object

    void SaveData(GameData data); // Method to save data from the object
}
