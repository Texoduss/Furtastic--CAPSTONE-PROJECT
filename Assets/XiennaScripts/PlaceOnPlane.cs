using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For Button
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class PlaceOnPlane : PressInputBase
{
    [SerializeField] GameObject placedPrefab; // Drag your cat prefab here
    [SerializeField] Button pointerButton; // Assign UI button in Inspector

    GameObject spawnedObject;
    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    bool isPressed;

    protected override void Awake()
    {
        base.Awake();
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (spawnedObject != null) return; // Only place once
        if (Pointer.current == null || isPressed == false) return;

        var touchPosition = Pointer.current.position.ReadValue();

        if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {

            var hitPose = hits[0].pose; // Get the pose of the hit

            // Spawn prefab
            spawnedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);

            // Rotate prefab to face camera
            Vector3 lookPos = Camera.main.transform.position - spawnedObject.transform.position;

            lookPos.y = 0;// Keep only the horizontal direction

            spawnedObject.transform.rotation = Quaternion.LookRotation(lookPos); // Rotate to face camera

            // Hook up UI button to CatMovement
            CatMovement cat = spawnedObject.GetComponent<CatMovement>();
            if (cat != null && pointerButton != null)
            {
                pointerButton.onClick.AddListener(cat.OnPointerButton);
                Debug.Log("[UI] Pointer button is now linked to CatMovement.");
            }
        }
    }

    protected override void OnPress(Vector3 position) => isPressed = true; // While pressing 
    protected override void OnPressCancel() => isPressed = false; // When press is released
}
