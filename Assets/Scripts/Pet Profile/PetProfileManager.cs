using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PetProfileManager : MonoBehaviour, IDataPersistence
{
    private TextMeshProUGUI pet;
    private TextMeshProUGUI petName;
    private TextMeshProUGUI petBirth;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.instance;

        pet = GameObject.Find("Pet").GetComponent<TextMeshProUGUI>();
        petName = GameObject.Find("PetName").GetComponent<TextMeshProUGUI>();
        petBirth = GameObject.Find("PetBirthday").GetComponent<TextMeshProUGUI>();
        DataPersistenceManager.instance.LoadGame();
    }
    public void LoadData(GameData data)
    {
        this.petName.text = data.petName;
        this.pet.text = data.petType;
        this.petBirth.text = data.petBirthday;
    }

    public void SaveData(GameData data)
    {
        
    }

    public void BackButton()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        // Logic to go back to the previous scene or menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayMenuScene");
    }
}
