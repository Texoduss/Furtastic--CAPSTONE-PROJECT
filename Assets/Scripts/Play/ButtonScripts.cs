using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ButtonScripts : MonoBehaviour
{
    [Header("Button To Click")]
    [SerializeField]
    public Button Button;

    [Header("Buttons To Not Interact With")]
    public Button[] buttonsToNotInteract;
    public bool isInteractable = true;

    public Image buttonImage;
    private void Start()
    {
        if (Button == null)
        {
            Debug.LogError("Button component not found!");
            return;
        }
        Button.onClick.AddListener(ButtonClicked);
    }

    public void ButtonClicked()
    {
        isInteractable = !isInteractable;

        foreach (Button button in buttonsToNotInteract)
        {
            button.interactable = isInteractable;
            Debug.Log("Set button " + button.name + " interactable to " + isInteractable);
        }
    }
}
