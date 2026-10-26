using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : MonoBehaviour
{
    [Header("Settings Navigation")]
    [SerializeField] private SettingsManager settings;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    [SerializeField] private GameObject blur;

    private SaveSlots[] saveSlots;
    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlots>();
    }

    public void OnSaveSlotClicked(SaveSlots saveSlots)
    {
        DisableMenuButtons();
      DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlots.GetProfileId());

        if (!isLoadingGame)
        {
            DataPersistenceManager.instance.NewGame();
        }

        SceneManager.LoadSceneAsync("PetSelectionScene");
    }

    public void OnCloseMenuClicked()
    {
        gameObject.SetActive(false);
    }

    public void OnClearClicked(SaveSlots saveSlot)
    {
        if (saveSlot == null)
        {
            Debug.LogError("[SaveSlotsMenu] OnClearClicked called with null SaveSlot.");
            return;
        }

        string profileId = saveSlot.GetProfileId();
        if (string.IsNullOrEmpty(profileId))
        {
            Debug.LogWarning("[SaveSlotsMenu] SaveSlot has no profile ID. Aborting clear.");
            return;
        }

        // Delete profile without reloading to avoid duplicate UI entries
        DataPersistenceManager.instance.DeleteProfileData(profileId, suppressReload: true);

        // Clear the slot locally
        saveSlot.SetData(null);
        saveSlot.SetInteractable(false);
        Debug.Log($"[SaveSlotsMenu] Cleared SaveSlot for profile {profileId}");

        // Load Death Scene
        if (Application.CanStreamedLevelBeLoaded("DeathScene"))
        {
            SceneManager.LoadScene("DeathScene");
            Debug.Log("[SaveSlotsMenu] DeathScene loaded after clear.");
        }
        else
        {
            Debug.LogError("[SaveSlotsMenu] DeathScene cannot be loaded. Check scene name and build settings.");
        }
    }

    public void OnBackCLicked()
    {

        settings.ActivateMenu();
        this.DeactivateMenu();
        blur.SetActive(false);

    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);

        this.isLoadingGame = isLoadingGame;

        Dictionary<string, GameData> profilesData = DataPersistenceManager.instance.GetAllProfilesGameData();


        
        foreach (SaveSlots saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
            }
        }
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
        Debug.Log("[SaveSlotsMenu] Menu deactivated.");
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlots saveSlots in saveSlots)
        {
            saveSlots.SetInteractable(false);
        }
        backButton.interactable = false;
        Debug.Log("[SaveSlotsMenu] Menu buttons disabled.");
    }
}
