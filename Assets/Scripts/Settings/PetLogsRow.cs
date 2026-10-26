using TMPro;
using UnityEngine;

public class PetLogRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI dateText;

    public void SetData(string petName, string petType, string date)
    {
        if (nameText == null || typeText == null || dateText == null)
        {
            Debug.LogError("[PetLogRow] Missing TMP references!");
            return;
        }

        if (nameText != null)
        {
            nameText.gameObject.SetActive(true);
            nameText.enabled = true;
        }
        if (typeText != null)
        {
            typeText.gameObject.SetActive(true);
            typeText.enabled = true;
        }
        if (dateText != null)
        {
            dateText.gameObject.SetActive(true);
            dateText.enabled = true;
        }

        Debug.Log($"[PetLogRow] Setting data → {petName}, {petType}, {date}");
        nameText.text = petName;
        typeText.text = petType;
        dateText.text = date;
    }
}
