using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSlider : MonoBehaviour
{
    public Slider slider; // Reference to the Slider component
    public Image backgroundImage; // Reference to the background image of the slider
    public Color onColor = Color.green; // Color when toggle is on
    public Color offColor = Color.red; // Color when toggle is off

    private bool isOn = false; // Current state of the toggle

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>(); // Get the Slider component if not assigned
        }
        if (backgroundImage == null)
        {
            // Assuming the background image is a child of the slider
            backgroundImage = slider.GetComponentInChildren<Image>();
        }

        slider.onValueChanged.AddListener(OnSliderValueChanged); // Subscribe to the Slider's value change event
        UpdateToggleVisual(isOn); // Initialize visual state
    }

    void OnSliderValueChanged(float value)
    {
        // Determine if the toggle is "on" based on the slider's value
        isOn = (value > 0.5f);
        UpdateToggleVisual(isOn); // Update the visual representation
                                  // Optionally, add other actions here based on the toggle state
        Debug.Log("Toggle is " + (isOn ? "ON" : "OFF"));
    }

    void UpdateToggleVisual(bool state)
    {
        // Set the slider's value to 0 or 1 based on the state
        slider.value = state ? 1f : 0f;

        // Change the background color based on the state
        if (backgroundImage != null)
        {
            backgroundImage.color = state ? onColor : offColor;
        }
    }

    // Public method to manually set the toggle state (e.g., from a button)
    public void SetToggleState(bool newState)
    {
        isOn = newState;
        UpdateToggleVisual(isOn);
    }
}
