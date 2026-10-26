using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MedicineCountText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public int maxCount = 3;

    private void Update()
    {
        if (MedicineDataManager.Instance != null && textMesh != null)
        {
            int count = MedicineDataManager.Instance.GetMedicineCount();
            textMesh.text = $"{count}/{maxCount}";

        }
    }
}
