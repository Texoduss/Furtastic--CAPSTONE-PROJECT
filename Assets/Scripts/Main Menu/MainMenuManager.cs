using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour 
{
    AudioManager audioManager;

    [SerializeField] private Button playButton;
    [SerializeField] private Button tutorialButton;

    private void Awake()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("[MainMenuManager] AudioManager.instance is NULL!");
        }
    }



    public void PlayButton()
    {
        Debug.Log("[MainMenuManager] PlayButton clicked!");

        

        // AudioManager safety
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.buttonPress);
            Debug.Log("[MainMenuManager] Play SFX triggered");
        }
        else
        {
            Debug.LogWarning("[MainMenuManager] AudioManager missing, skipping sound");
        }

        // DataPersistenceManager safety
        if (DataPersistenceManager.instance == null)
        {
            Debug.LogError("[MainMenuManager] DataPersistenceManager.instance is NULL!");
            return;
        }

        if (!DataPersistenceManager.instance.HasGameData())
        {
            Debug.Log("[MainMenuManager] No game data found, creating new game");
            DataPersistenceManager.instance.NewGame();
            SceneManager.LoadSceneAsync("PetSelectionScene");
        }
        else
        {
            Debug.Log("[MainMenuManager] Save data found, loading game");
            DataPersistenceManager.instance.LoadGame();
            LoaderUtility.Deinitialize();
            LoaderUtility.Initialize();
            SceneManager.LoadSceneAsync("PlayScene");
        }
    }


  
    public void LoadSettingsScene()
    {
        Debug.Log("[MainMenuManager] Settings button clicked");

        if (audioManager != null)
            audioManager.PlaySFX(audioManager.buttonPress);

        SceneManager.LoadSceneAsync("SettingsScene");
    }

    public void OnApplicationQuit()
    {
        Debug.Log("[MainMenuManager] Quit button clicked");

        if (audioManager != null)
            audioManager.PlaySFX(audioManager.buttonPress);

        if (DataPersistenceManager.instance != null)
        {
            Debug.Log("[MainMenuManager] Saving game before quit");
            DataPersistenceManager.instance.SaveGame();
        }

        // Schedule notification if tasks are incomplete
        FindObjectOfType<NotificationManager>()?.ScheduleIfIncomplete();


        Application.Quit();
    }



}
