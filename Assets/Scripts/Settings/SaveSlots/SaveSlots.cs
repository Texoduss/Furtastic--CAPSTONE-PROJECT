using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlots : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = " ";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI petNameText;
    [SerializeField] private TextMeshProUGUI petTypeText;

    [Header("Clear Data Button")]
    [SerializeField] private Button clearButton;

    private Button saveSlotButton;

    public void Awake()
    {
        saveSlotButton = GetComponent<Button>();
        
    }

    public void SetData(GameData data)
    {
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearButton.interactable = false;
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearButton.interactable = true;

            petNameText.text = "Pet Name: " + data.petName;
            petTypeText.text = "Pet Type: " + data.petType.ToString();
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearButton.interactable = interactable;
        Debug.Log($"[SaveSlots] SetInteractable: {interactable} for profileId: {profileId}");
    }
}
