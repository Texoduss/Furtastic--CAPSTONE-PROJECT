using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using static PetSelectionManager;


public class PetNameManager : MonoBehaviour, IDataPersistence
{
    public TMP_InputField petNameInput;
    private string petName;
    public TextMeshProUGUI invalidText;
    private string birthday;
    private string selectedPet;

    [Header("Pet Selected")]
    public GameObject dogSelected;
    public GameObject catSelected;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.instance;

    }

    public void LoadData(GameData data)
    {
        this.selectedPet = data.petType;
    }

    public void SaveData(GameData data)
    {
        
        data.petName = this.petName;
        data.petBirthday = this.birthday;
        data.petType = this.selectedPet;
    }

    public void Start()
    {
        
        invalidText.text = "";

        // If selectedPet wasn’t loaded from save, use what was picked in selection scene
        if (string.IsNullOrEmpty(selectedPet))
        {
            selectedPet = PetSelectionData.SelectedPetType;
        }

        // Display selected pet
        if (selectedPet == "Dog")
        {
            dogSelected.SetActive(true);
            catSelected.SetActive(false);
        }
        else if (selectedPet == "Cat")
        {
            dogSelected.SetActive(false);
            catSelected.SetActive(true);
        }
    }


    public void OnNextButtonClicked()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        petName = petNameInput.text.Trim();

        
        

        // Validation
        if (string.IsNullOrEmpty(petName))
        {
            invalidText.text = "Please enter a valid pet name.";
            return;
        }
        if (petName.Length < 3 || petName.Length > 15)
        {
            invalidText.text = "Pet name must be between 3 and 15 characters.";
            return;
        }
        foreach (char c in petName)
        {
            if (!char.IsLetter(c) && c != ' ')
            {
                invalidText.text = "Pet name can only contain letters and spaces.";
                return;
            }
        }

        // Passed all checks → Save data
        birthday = System.DateTime.Now.ToString("yyyy - MM - dd");
        DataPersistenceManager.instance.SaveGame();
        LoaderUtility.Deinitialize();
        LoaderUtility.Initialize();
        SceneManager.LoadScene("PlayScene");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("PetSelectionScene");
    }
}

