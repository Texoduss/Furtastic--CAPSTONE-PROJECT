using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetToiletUI : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject cleanPoopPanel;
    public GameObject wipePeePanel;
    public Button cleanButton;
    public Button flushButton;

    private PetToiletLogic currentPet;
    private EmoteUI emoteUI;

    private void Awake()
    {
        emoteUI = FindObjectOfType<EmoteUI>();
        HideButtons();
    }

    public void ShowCleanPoopButton(PetToiletLogic pet) // called by ToiletSystemLogic when pet finishes pooping
    {
        Debug.Log("[PetToiletUI]Showing Clean Poop Button...");
        HideButtons();
        

        currentPet = pet;

        if (cleanPoopPanel != null)
            cleanPoopPanel.SetActive(true);
        if (cleanButton != null)
        {
            cleanButton.onClick.RemoveAllListeners();
            cleanButton.onClick.AddListener(() => currentPet.OnCleaned());
        }
    }    

    public void ShowFlushPeeButton(PetToiletLogic pet) // called by ToiletSystemLogic when pet finishes peeing
    {
        Debug.Log("[PetToiletUI]Showing Flush Pee Button...");
        HideButtons();
        

        currentPet = pet;

        if (wipePeePanel != null)
            wipePeePanel.SetActive(true);

        if (flushButton != null)
        {
            flushButton.onClick.RemoveAllListeners();
            flushButton.onClick.AddListener(() => currentPet.OnFlushed());
        }
    }

    public void HideButtons() // called after cleaning or flushing
    {
        if (cleanPoopPanel != null)
            cleanPoopPanel.SetActive(false);
        if (wipePeePanel != null)
            wipePeePanel.SetActive(false);
    }
}
