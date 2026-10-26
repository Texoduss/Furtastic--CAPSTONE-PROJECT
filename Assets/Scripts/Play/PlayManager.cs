
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayManager : MonoBehaviour
{ 
    public GameObject playModeButton;
    [Header("Mute and Unmute Buttons")]
    public GameObject muteButton;
    public GameObject unmuteButton;

    [Header("AR Guide Animations")]
    public Animator scanSurfaceAnimator;
    public Animator tapSurfaceAnimator;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.instance;
        audioManager = GameObject.FindGameObjectsWithTag("Audio")[0].GetComponent<AudioManager>();
        
    }
    public void PlayMenuSceneLoad()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        // Load the Play Menu scene
        SceneManager.LoadScene("PlayMenuScene");
    }
    public void BackButton()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenuScene");
    }
    public void PlayModeButton()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
    }
    public void MuteButton()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        muteButton.SetActive(false);
        unmuteButton.SetActive(true);
        AudioManager.instance.MuteButton();
    }

    public void UnmuteButton()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        muteButton.SetActive(true);
        unmuteButton.SetActive(false);
        AudioManager.instance.UnmuteButton();
    }
}
