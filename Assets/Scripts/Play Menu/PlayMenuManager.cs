using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayMenuManager : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.instance;

    }

    public void LoadPetShop()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        SceneManager.LoadScene("PetShopScene");
    }

    public void LoadPetProfile()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        SceneManager.LoadScene("PetProfileScene");
    }

    public void LoadSettings()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        SceneManager.LoadScene("SettingsScene");
    }

    public void BackButton()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        LoaderUtility.Deinitialize();
        LoaderUtility.Initialize();
        SceneManager.LoadScene("PlayScene");
    }

    public void LoadPawStamp()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        SceneManager.LoadScene("PawStampScene");
    }
}
