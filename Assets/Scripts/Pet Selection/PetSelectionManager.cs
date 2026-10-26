using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PetSelectionManager : MonoBehaviour, IDataPersistence
{
    public string petType; // Variable to store the selected pet type
    private AudioManager audioManager;
    public static class PetSelectionData
    {
        public static string SelectedPetType;
    }
    private void Awake()
    {
        audioManager = AudioManager.instance;

    }
    public void LoadData(GameData data)
    {


    }

    public void SaveData(GameData data)
    {
        
        data.petType = this.petType; // Save the selected pet type to GameData
    }

    public void MainMenuSceneLoad(string scene)  // THIS IS THE FUNCTION TO SEND YOU BACK TO THE MAIN MENU
    {
        audioManager.PlaySFX(audioManager.buttonPress); // Play button press sound effect
        // Load the specified scene
        SceneManager.LoadScene(scene);
    }


    public void OnDogSelected()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        PetSelectionData.SelectedPetType = "Dog";
        petType = "Dog"; // Set pet type to Dog
        DataPersistenceManager.instance.SaveGame(); // Save the selected pet type

        

        SceneManager.LoadScene("PetNameScene"); // Load the PetNameScene

    }
    public void OnCatSelected()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        PetSelectionData.SelectedPetType = "Cat";
        petType = "Cat"; // Set pet type to Cat
        DataPersistenceManager.instance.SaveGame(); // Save the selected pet type

        

        SceneManager.LoadScene("PetNameScene"); // Load the PetNameScene
    }

}
