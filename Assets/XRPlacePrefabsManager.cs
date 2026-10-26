using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For Button
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class XRPlacePrefabsManager : PressInputBase, IDataPersistence
{
    [Header("Prefabs & UI")]
    [SerializeField] GameObject[] placeablePrefabs; // Array of prefabs to place

    [Header("Play Mode UI Buttons")]
    [SerializeField] Button pointerButton;          // Pointer mode toggle
    [SerializeField] Button sleepButton;            // Sleep button
    [SerializeField] Button touchButton;            // Touch/Interact button
    [SerializeField] Button resetButton;            // Reset placed prefab
    [SerializeField] Button playButton;
    [SerializeField] GameObject pickUpButton;       // Optional pick up button

    private CatMovement spawnedCat;
    private DogMovement spawnedDog;
    private GameObject spawnedObject;
    private ARRaycastManager aRRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool isPressed;
    public event System.Action<GameObject> objectSpawned;

    public GameData gameData;

    protected override void Awake()
    {
        base.Awake();
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        
        if (resetButton != null)
            resetButton.onClick.AddListener(ResetSpawnedObject);

        // Optional: Hide pick up button at start
        if (pickUpButton != null)
            pickUpButton.SetActive(false);

        if (spawnedObject != null && Camera.main != null)
        {
            Vector3 lookPos = Camera.main.transform.position - spawnedObject.transform.position;
            lookPos.y = 0;
            if (lookPos != Vector3.zero)
                spawnedObject.transform.rotation = Quaternion.LookRotation(lookPos);
        }

        if (aRRaycastManager == null)
        {
            Debug.LogError("ARRaycastManager component missing from GameObject.");
            return;
        }

    }

    void Update()
    {
        if (gameData == null)
        {
            Debug.LogWarning("[AR] GameData not yet loaded, skipping spawn check.");
            return;
        }

        // Show pick up button if prefab is spawned
        if (spawnedObject != null && pickUpButton != null)
        {
            pickUpButton.SetActive(true);
            return;
        }

        // Only place once
        if (Pointer.current == null || !isPressed) return;

        Vector2 clickPosition = Pointer.current.position.ReadValue();

        if (gameData == null)
        {
            Debug.LogWarning("[AR] GameData not loaded yet.");
            return;
        }



        if (aRRaycastManager.Raycast(clickPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Debug.Log("[AR] Pet type from GameData: '" + (gameData == null ? "NULL" : gameData.getPetType()) + "'");

            Pose hitPose = hits[0].pose;
            if (gameData.getPetType() == "Cat")
            {
                spawnedObject = Instantiate(
                    placeablePrefabs[0],
                    hitPose.position,
                    hitPose.rotation);
                Debug.Log("[AR] Spawned Cat prefab.");
                objectSpawned?.Invoke(spawnedObject);
            }

            else if (gameData.getPetType() == "Dog")
            {
                spawnedObject = Instantiate(
                    placeablePrefabs[1],
                    hitPose.position,
                    hitPose.rotation);
                Debug.Log("[AR] Spawned Dog prefab.");
                objectSpawned?.Invoke(spawnedObject);
            }
            else
            {
                Debug.LogError("[AR] Unknown pet type in GameData. Cannot spawn prefab.");
                return;
            }
        }
        

        // Rotate prefab to face camera
        if (Camera.main != null)
        {
            if (spawnedObject == null) return;
            Vector3 lookPos = Camera.main.transform.position - spawnedObject.transform.position;
            lookPos.y = 0;
            if (lookPos != Vector3.zero)
                spawnedObject.transform.rotation = Quaternion.LookRotation(lookPos);
        }

        // Hook buttons
        spawnedCat = spawnedObject.GetComponent<CatMovement>();
        spawnedDog = spawnedObject.GetComponent<DogMovement>();

        if (spawnedCat == null && spawnedDog == null)
        {
            Debug.LogError("[AR] Spawned object has no CatMovement or DogMovement script attached.");
            return;
        }

        if (spawnedCat != null)
        {
            if (pointerButton != null)
                pointerButton.onClick.AddListener(spawnedCat.OnPointerButton);

            if (sleepButton != null)
                sleepButton.onClick.AddListener(spawnedCat.OnSleepButton);

            if (touchButton != null)
                touchButton.onClick.AddListener(spawnedCat.OnTouchButton);
            if (playButton != null)
                playButton.onClick.AddListener(spawnedCat.OnPlayButton);
        }

        if (spawnedDog != null)
        {
            if (pointerButton != null)
                pointerButton.onClick.AddListener(spawnedDog.OnPointerButton);

            if (sleepButton != null)
                sleepButton.onClick.AddListener(spawnedDog.OnSleepButton);

            if (touchButton != null)
                touchButton.onClick.AddListener(spawnedDog.OnTouchButton);
            if(playButton != null)
                playButton.onClick.AddListener(spawnedDog.OnPlayButton);

        }
        if (gameData.getPetType() == "Cat")
            Debug.Log("[UI] Buttons linked to CatMovement.");
        if (gameData.getPetType() == "Dog")
            Debug.Log("[UI] Buttons linked to CatMovement.");
    }

    public void LoadData(GameData data)
    {
        StartCoroutine(WaitToLoad());
        gameData = data;
    }
    public void SaveData(GameData data)
    {
        // Nothing to save for now
    }

    private IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(1f);
    }

    protected override void OnPress(Vector3 position) => isPressed = true;
    protected override void OnPressCancel() => isPressed = false;

    void ResetSpawnedObject()
    {
        if (spawnedObject == null) return;

        if (spawnedCat != null)
        {
            if (pointerButton != null)
                pointerButton.onClick.RemoveListener(spawnedCat.OnPointerButton);
            if (sleepButton != null)
                sleepButton.onClick.RemoveListener(spawnedCat.OnSleepButton);
            if (touchButton != null)
                touchButton.onClick.RemoveListener(spawnedCat.OnTouchButton);
            if (playButton != null)
                playButton.onClick.RemoveListener(spawnedCat.OnPlayButton);
        }

        if (spawnedDog != null)
        {
            if (pointerButton != null)
                pointerButton.onClick.RemoveListener(spawnedDog.OnPointerButton);
            if (sleepButton != null)
                sleepButton.onClick.RemoveListener(spawnedDog.OnSleepButton);
            if (touchButton != null)
                touchButton.onClick.RemoveListener(spawnedDog.OnTouchButton);
            if(playButton != null)
                playButton.onClick.RemoveListener(spawnedDog.OnPlayButton);
        }

        if (pickUpButton != null)
            pickUpButton.SetActive(false);

        Destroy(spawnedObject);
        spawnedCat = null;
        spawnedDog = null;
        spawnedObject = null;

        Debug.Log("[AR] Reset: Prefab removed. You can place again.");
    }
}
