using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IDataPersistence
{
    [Header("Audio Source")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip cameraShutter;
    public AudioClip buttonPress;

    [Header("Pet Noise")]
    public AudioClip dogSound;
    public AudioClip catSound;

    private string petType;
    private bool pendingPetSound = false;

    public static AudioManager instance { get; private set; }

    public void LoadData(GameData data)
    {
        petType = data.petType;

        if (pendingPetSound)
        {
            PlayPetSound();
        }
    }
    public void SaveData(GameData data)
    {
        // Implement if you want to save audio settings to GameData
    }

    public void MuteButton()
    {
        MusicSource.volume = 0;
        SFXSource.volume = 0;
        Debug.Log("Music and SFX volume muted.");

    }

    public void UnmuteButton()
    {
        MusicSource.volume = 1;
        SFXSource.volume = 1;
        Debug.Log("Music and SFX volume unmuted.");
    }

    private void Awake()
    {

        if (instance != null)
        {
            Debug.Log("Found more than one Audio Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        StartCoroutine(InitializeWithDelay());
        GameData data = GameData.Instance;
        if (data != null)
        {
            LoadData(data);
        }
    }

    private IEnumerator InitializeWithDelay()
    {
        // Delay so Unity audio engine fully initializes on mobile
        yield return new WaitForSeconds(0.2f);

        MusicSource.clip = background;
        MusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public IEnumerable DisableButtonTemporarily(float delayTime)
    {
        SFXSource.PlayOneShot(buttonPress);
        yield return new WaitForSeconds(delayTime);
    }



    public void PlayPetSound()
    {
        if (string.IsNullOrEmpty(petType))
        {
            pendingPetSound = true;
            Debug.Log("[AudioManager] Pet type not loaded yet. Will play pet sound once loaded.");
            return;
        }

        pendingPetSound = false;
        if (petType == "Dog")
        {
            SFXSource.PlayOneShot(dogSound);
            Debug.Log("[AudioManager] Playing pet sound for pet type: " + petType);
        }

        else if (petType == "Cat")
        {
            SFXSource.PlayOneShot(catSound);
            Debug.Log("[AudioManager] Playing pet sound for pet type: " + petType);
        }
    }

}
