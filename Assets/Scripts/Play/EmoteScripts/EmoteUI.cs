using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteUI : MonoBehaviour, IDataPersistence
{
    [Header("Cat Emotes")]
    [SerializeField] GameObject catSad;
    [SerializeField] GameObject catHungry;
    [SerializeField] GameObject catToilet;
    [SerializeField] GameObject catPetting;

    [Header("Dog Emotes")]
    [SerializeField] GameObject dogSad;
    [SerializeField] GameObject dogHungry;
    [SerializeField] GameObject dogPetting;
    [SerializeField] GameObject dogToilet;

    private string petType;

    public void LoadData(GameData data)
    {
        this.petType = data.petType;
    }
    public void SaveData(GameData data)
    {
        //throw new System.NotImplementedException();
    }

    private void Start()
    {
        HideEmotes();
    }

    // Sad
    public void ShowPetSadEmote()
    {
        if(petType == "Cat")
        {
            if (catSad != null)
                catSad.SetActive(true);
            Debug.Log("[EmoteUI] Showing Cat Sad Emote.");
        }
        else if (petType == "Dog")
        {

            if (dogSad != null)
                dogSad.SetActive(true);
            Debug.Log("[EmoteUI] Showing Dog Sad Emote.");

        }
        else
        {
            Debug.Log("[EmoteUI] Unknown pet type for Sad Emote.");
        }
        
    }
    public void HidePetSadEmote()
    {
        if (petType == "Cat")
        {
            if (catSad != null)
                catSad.SetActive(false);
            Debug.Log("[EmoteUI] Hiding Cat Sad Emote.");
        }
        else if (petType == "Dog")
        {
            if (dogSad != null)
                dogSad.SetActive(false);
            Debug.Log("[EmoteUI] Hiding Dog Sad Emote.");
        }
        else
        {
            Debug.Log("[EmoteUI] Unknown pet type for Sad Emote.");
        }
    }

    // Petting
    public void ShowPetPettingEmote()
    {
        if(petType == "Cat")
        {
            if (catPetting != null)
                catPetting.SetActive(true);
            Debug.Log("[EmoteUI] Showing Cat Petting Emote.");
        }
        else if (petType == "Dog")
        {
            if (dogPetting != null)
                dogPetting.SetActive(true);
            Debug.Log("[EmoteUI] Showing Dog Petting Emote.");
        }
        else
        {
            Debug.Log("[EmoteUI] Unknown pet type for Petting Emote.");
        }

    }

    public void HidePetPettingEmote()
    {
        if(petType == "Cat")
        {
            if (catPetting != null)
                catPetting.SetActive(false);
            Debug.Log("[EmoteUI] Hiding Cat Petting Emote.");
        }
        else if (petType == "Dog")
        {
            if (dogPetting != null)
                dogPetting.SetActive(false);
            Debug.Log("[EmoteUI] Hiding Dog Petting Emote.");
        }
        else
        {
            Debug.Log("[EmoteUI] Unknown pet type for Petting Emote.");
        }
    }

    // Hungry
    public void ShowPetHungryEmote()
    {
        if (petType == "Cat")
        {
            if (catHungry != null)
                catHungry.SetActive(true);
            Debug.Log("[EmoteUI] Showing Cat Hungry Emote.");
        }
        else if (petType == "Dog")
        {
            if (dogHungry != null)
                dogHungry.SetActive(true);
            Debug.Log("[EmoteUI] Showing Dog Hungry Emote.");
        }
        else
        {
            Debug.Log("[EmoteUI] Unknown pet type for Hungry Emote.");
        }
    }

    public void HidePetHungryEmote()
    {
        if (petType == "Cat")
        {
            if (catHungry != null)
                catHungry.SetActive(false);
            Debug.Log("[EmoteUI] Hiding Cat Hungry Emote.");
        }
        else if (petType == "Dog")
        {
            if (dogHungry != null)
                dogHungry.SetActive(false);
            Debug.Log("[EmoteUI] Hiding Dog Hungry Emote.");
        }
        else
        {
            Debug.Log("[EmoteUI] Unknown pet type for Hungry Emote.");
        }
    }

    // Toilet
    public void ShowPetToiletEmote() 
    {
        if(petType == "Cat")
        {
            if (catToilet != null)
                catToilet.SetActive(true);
            Debug.Log("[EmoteUI] Showing Cat Toilet Emote.");
        }
        else if (petType == "Dog")
        {
            if (dogToilet != null)
                dogToilet.SetActive(true);
            Debug.Log("[EmoteUI] Showing Dog Toilet Emote.");
        }
        else
        {
            Debug.Log("[EmoteUI] Unknown pet type for Toilet Emote.");
        }
    }

    public void HidePetToiletEmote()
    {
        if (petType == "Cat")
        {
            if (catToilet != null)
                catToilet.SetActive(false);
            Debug.Log("[EmoteUI] Hiding Cat Toilet Emote.");
        }
        else if (petType == "Dog")
        {
            if (dogToilet != null)
                dogToilet.SetActive(false);
            Debug.Log("[EmoteUI] Hiding Dog Toilet Emote.");
        }
        else
        {
            Debug.Log("[EmoteUI] Unknown pet type for Toilet Emote.");
        }
    }

    public void HideEmotes()
    {
        HidePetSadEmote();
        HidePetHungryEmote();
        HidePetPettingEmote();
        HidePetToiletEmote();
    }

}
