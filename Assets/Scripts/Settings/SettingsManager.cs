using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private GameObject saveLoadMenu;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    public AudioManager audioManager;

    [SerializeField] private GameObject blurEffect;

    private void Awake()
    {
        audioManager = AudioManager.instance;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {

            SetMusicVolume();
            SetSFXVolume();
        }

        blurEffect.SetActive(false);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        if (volume <= 0.0001f) volume = 0.0001f; // prevent log10(0)
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        if (volume <= 0.0001f) volume = 0.0001f; // prevent log10(0)
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value=PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetMusicVolume();
        SetSFXVolume();
    }
    public void BackButton()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        // Load the main menu scene when the back button is pressed
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ActivateMenu()
    {
        saveLoadMenu.SetActive(true);
    }

    public void DeactivateMenu()
    {
        saveLoadMenu.SetActive(false);
    }

    public void OnSaveLoadClicked()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
        blurEffect.SetActive(true);

    }
}
