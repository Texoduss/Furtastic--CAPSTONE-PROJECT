using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// Manages the movement and interactions of the AR dog
public class DogMovement : MonoBehaviour
{
    [Header("Movement Settigns")]
    public float moveSpeed = 0.3f;
    public float minMoveDistance = 0.3f;
    public float maxMoveDistance = 0.8f;
    public float idleTime = 3f;

    [Header("Angle of Prefab")]
    public float rotationOffset = 90f; 

    // Private State
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isSleeping = false;
    private bool isTouched = false;
    private bool waitingForTouch = false;
    private bool isPointerMode = false;
    private bool isDoingToilet = false;
    private bool isPlayingNow = false;
    private Coroutine currentIdleRoutine;


    [Header("Touch Logic")]
    public float holdDurationThreshold = 0.5f;
    private float touchStartTime;
    private bool isHolding = false;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Ar Manager")]
    private ARPlane movementPlane;
    private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;

    AudioManager audioManager;

    private void Awake() // Called even if the script component is disabled
    {
        audioManager = GameObject.FindGameObjectsWithTag("Audio")[0].GetComponent<AudioManager>();

        CheckFriendshipLevelToSizeUp();
    }

    private void Start() // Called only if the script component is enabled
    {
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.applyRootMotion = false;

        planeManager = FindObjectOfType<ARPlaneManager>();
        raycastManager = FindObjectOfType<ARRaycastManager>();

        // Defensive checks
        if (animator == null)
            Debug.LogError("[AR Dog] No Animator found on DogMovement or its children!");
        if (planeManager == null)
            Debug.LogError("[AR Dog] No ARPlaneManager found in scene!");
        if (raycastManager == null)
            Debug.LogError("[AR Dog] No ARRaycastManager found in scene");

        
    }
    private void Update() // Called every frame
    {
        if (animator != null) // Sleep check
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            bool inSleep = stateInfo.IsName("Sleep") || stateInfo.IsTag("Sleep");

            if (inSleep || animator.IsInTransition(0))
                return;

            if (animator.GetBool("isSleeping")) return;
        }

        if (isSleeping || isTouched || isDoingToilet) { return; } // Don't move if sleeping or being petted or doing toilet

        if (movementPlane == null) // Auto-assign plane if none assigned
        {
            if (planeManager != null && planeManager.trackables.count > 0 && !isPointerMode) // && !isPointerMode)
            {
                movementPlane = GetFirstDetectedPlane();
                if (movementPlane != null && movementPlane.alignment == PlaneAlignment.HorizontalUp) // Only use horizontal planes 
                {
                    Debug.Log("[AR Dog] Plane assigned automatically: " + movementPlane.trackableId);

                    Vector3 newPos = transform.position;
                    newPos.y = movementPlane.transform.position.y;
                    transform.position = newPos;

                    PickNewDirection();
                    StartMoving();
                }
            }
            else
                return;
            
        }

        if (isMoving) // Handle movement
            MoveTowardsTarget();

        if (isPointerMode && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // Pointer mode touch to move
        {
            Vector2 touchPos = Input.GetTouch(0).position;
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon)) // Raycast against planes
            {
                targetPosition = hits[0].pose.position;
                targetPosition.y = transform.position.y;
                StartMoving();
                Debug.Log("[AR Dog] Moving to tapped position: " +  targetPosition);
            }
        }

        if (waitingForTouch) // Handle touch/hold to pet
        {
            if (Camera.main == null) // Defensive check
            {
                Debug.LogError("[Dog] No MainCamera found. Cannot detect taps/clicks.");
                return;
            }

            if (Input.GetMouseButtonDown(0)) // Mouse click (for editor/testing)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray from camera through mouse position
                if (Physics.Raycast(ray, out RaycastHit hit)) // Raycast hit something
                {
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        touchStartTime = Time.time;
                        isHolding = true;
                    }
                }
            }
            if (isHolding && Input.GetMouseButton(0)) // Holding mouse button
            {
                if (Time.time - touchStartTime >= holdDurationThreshold) // Held long enough
                {
                    StartTouchAnimation(Input.mousePosition);
                    isHolding = false; // Prevent multiple triggers
                    Debug.Log("[Dog] Hold detected, starting touch animation.");
                }
            }

            if (Input.GetMouseButtonUp(0)) // Mouse button released
            {
                if (isHolding)
                {
                    StopTouchAnimation();
                }
                isHolding = false;
                touchStartTime = 0f;
            }

            if (Input.touchCount > 0) // Touch input (for mobile)
            {
                Touch touch = Input.GetTouch(0);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                switch (touch.phase)
                {
                    case TouchPhase.Began: // Touch began
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                            if (hit.collider != null && hit.collider.gameObject == gameObject)
                            {
                                touchStartTime = Time.time;
                                isHolding = true;
                            }
                        }
                        break;

                    case TouchPhase.Stationary: // Touch held in place
                        if (isHolding && Time.time - touchStartTime >= holdDurationThreshold)
                        {
                            isHolding = false;
                            StartTouchAnimation(touch.position);
                            Debug.Log("[Dog] Hold detected, starting touch animation.");
                        }
                        break;

                    case TouchPhase.Ended: 
                    case TouchPhase.Canceled: // Touch ended or canceled
                        if (isHolding)
                        {
                            StopTouchAnimation();
                        }
                        isHolding = false;
                        touchStartTime = 0f;
                        break;
                }
            }
        }
    }

    ARPlane GetFirstDetectedPlane() // Get the first detected horizontal plane
    {
        foreach (var plane in planeManager.trackables)
        {
            return plane;
        }
        return null;
    }

    void MoveTowardsTarget() // Move and rotate towards target position
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance < 0.05f)
        {
            StopMoving();
            if (!isPointerMode)
            {
                currentIdleRoutine = StartCoroutine(IdleAndChangeDirection());
            }
        }
    }
    IEnumerator IdleAndChangeDirection() // Idle for a bit then pick new direction
    {
        StopMoving();
        yield return new WaitForSeconds(idleTime);
        if (!isDoingToilet || !isSleeping || !isTouched)
        {
            PickNewDirection();
            StartMoving();
        }
    }

    public void StartMoving() // Begin moving towards target
    {
        if (isDoingToilet) return;

        if (animator != null)
        {
            animator.SetBool("isWalking", true);
        }

        isMoving = true;
        Debug.Log("[AR Dog] Moving to: " + targetPosition);
    }

    public void StopMoving() // Stop movement
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }

        isMoving = false;
        Debug.Log("[AR Dog] Idle...");

        if (currentIdleRoutine != null)
        {
            StopCoroutine(currentIdleRoutine);
            currentIdleRoutine = null;
        }

    }

    public void PickNewDirection() // Pick a new random target position within the movement plane
    {
        if (movementPlane == null) return;

        using (var boundary2D = movementPlane.boundary)
        {
            if (!boundary2D.IsCreated || boundary2D.Length < 3 ) return;

            List<Vector3> boundary = new List<Vector3>(boundary2D.Length);
            for (int i = 0; i < boundary2D.Length; i++)
            {
                Vector3 localPoint = new Vector3(boundary2D[i].x, 0f, boundary2D[i].y);
                boundary.Add(movementPlane.transform.TransformPoint(localPoint));
            }

            int safety = 0;

            do
            {
                targetPosition = GetRandomPointInPolygon(boundary);
                targetPosition.y = transform.position.y;
                safety++;
            }

            while ((Vector3.Distance(transform.position, targetPosition) < minMoveDistance || 
                    Vector3.Distance(transform.position, targetPosition) > maxMoveDistance) && safety < 50); 
        }
    }

    Vector3 GetRandomPointInPolygon(List<Vector3> polygon) // Get a random point inside a polygon
    {
        Bounds bounds = new Bounds(polygon[0], Vector3.zero);
        foreach (var p in polygon) bounds.Encapsulate(p);

        Vector3 randomPoint;
        int safety = 0;
        do
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            randomPoint = new Vector3(x, transform.position.y, z);
            safety++;
        }
        while (!IsPointInPolygon(randomPoint, polygon) && safety < 50);

        return randomPoint;
    }

    bool IsPointInPolygon(Vector3 point, List<Vector3> polygon) // Ray-casting algorithm to check if point is inside polygon
    {
        bool inside = false;
        int j = polygon.Count - 1;
        for (int i = 0; i < polygon.Count; j = i++)
        {
            if (((polygon[i].z > point.z) != (polygon[j].z > point.z)) &&
                (point.x < (polygon[j].x - polygon[i].x) * (point.z - polygon[i].z) /
                (polygon[j].z - polygon[i].z) + polygon[i].x))
            {
                inside = !inside;
            }
        }
        return inside;
    }

    // --- Sleep Button ---
    public void OnSleepButton() // Toggle sleep state
    {
        if (!isSleeping)
        {
            StopMoving();
            isSleeping = true;
            if (animator != null) animator.SetBool("isSleeping", true);
        }
        else 
        { 
            isSleeping = false;
            if (animator != null) animator.SetBool("isSleeping", false);
            StartCoroutine(DelaySleepToWalk());
                   
        }
    }

    public void OnPlayButton()
    {
        if (isPlayingNow)
        {
            Debug.Log("[DogMovement] Already playing, ignoring extra Play button press.");
            return;
        }

        if (isMoving)
        {
            StopMoving();
        }
        if (!isSleeping && !isDoingToilet && !isPointerMode)
        {
            if (animator != null) animator.SetBool("isPlaying", true);
            StartCoroutine(PlayAnimationAndResume());
        }
    }

    private IEnumerator PlayAnimationAndResume() 
    {
        isPlayingNow = true;
        yield return null;

        float playDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(playDuration);

        if (animator != null)
        {
            animator.SetBool("isPlaying", false);
        }

        yield return new WaitForSeconds(0.5f);
        PickNewDirection();
        StartMoving();

        isPlayingNow = false;
    }
    IEnumerator DelaySleepToWalk() // Delay walking after waking up
    {
        Debug.Log("[Dog] Waking up... waiting 5 seconds before walking.");
        yield return new WaitForSeconds(3f);
        if (!isSleeping)
        {
            PickNewDirection();
            StartMoving();
        }

    }
    // --- Touch Button ---
    public void OnTouchButton() // Toggle touch mode
    {
        waitingForTouch = !waitingForTouch;
        Debug.Log("[Dog] Touch Mode: " + (waitingForTouch ? "Enabled" : "Disabled"));

        if (waitingForTouch)
        {
            StopMoving();
        }
        else
        {
            if (!isSleeping && !isDoingToilet)
            {
                PickNewDirection();
                StartMoving();
            }
            StopTouchAnimation();
        }
    }

    private void StartTouchAnimation(Vector3 screenPos) // Start touch animation if dog is tapped/held
    {
        if (!TryGetComponent<Collider>(out var collider))
        {
            Debug.LogError("[Dog] No collider found on DogMovement. Cannot detect touches.");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform && animator != null)
            {
                animator.SetBool("isTouched", true);
                isTouched = true;
                Debug.Log("[Dog] Touched! Playing touch animation.");
            }
        }
    }

    private void StopTouchAnimation() // Stop touch animation
    {
        if(animator != null)
            animator.SetBool("isTouched", false);
        isTouched = false;
        Debug.Log("[Dog] Touch ended.");

        if (!isSleeping && !isDoingToilet)
        {
            PickNewDirection();
            StartMoving();
        }
    }
    // --- Utility ---
    bool HasParameter(Animator animator, string paramName, AnimatorControllerParameterType type) // Check if animator has a parameter
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == type && param.name == paramName)
                return true;
        }
        return false;
    }

    public void PetNoiseButton() // Play pet sound
    {
        if (audioManager == null)
        {
            Debug.LogError("[AR Dog] No AudioManager found in scene!");
            return;
        }
        if (audioManager.dogSound == null)
        {
            Debug.LogError("[AR Dog] No cat sound assigned in AudioManager!");
            return;
        }
        audioManager.PlaySFX(audioManager.dogSound);
    }

    public void OnPointerButton() // Toggle pointer mode
    {
        isPointerMode = !isPointerMode;
        Debug.Log("[AR Dog] Pointer Mode: " + (isPointerMode ? "Enabled" : "Disabled"));

        if (isPointerMode) 
            StopMoving();
        else
        {
            PickNewDirection();
            StartMoving();
        }
    }

    public void StartPooping() // Start pooping animation
    {
        if (animator == null) return;
        if (!HasParameter(animator, "isPooping", AnimatorControllerParameterType.Bool))
        {
            Debug.LogWarning("[Dog] Animator missing 'isPooping' Bool parameter.");
            return;
        }
        isDoingToilet = true;
        StopMoving();
        if (!isMoving)
        {
            if (animator != null && HasParameter(animator, "isPooping", AnimatorControllerParameterType.Bool))
            {
                animator.SetBool("isPooping", true);

                Debug.Log("[Dog] Pooping animation triggered.");
            }
        }
        else
        {
            Debug.Log("[Dog] Cannot poop while moving.");
        }
    }

    public void StopPooping() // Stop pooping animation
    {
        if (animator == null) return;
        if (!HasParameter(animator, "isPooping", AnimatorControllerParameterType.Bool))
        {
            Debug.LogWarning("[Dog] Animator missing 'isPooping' Bool parameter.");
            return;
        }
        
        if (animator != null && HasParameter(animator, "isPooping", AnimatorControllerParameterType.Bool))
        {
            animator.SetBool("isPooping", false);
            isDoingToilet = false;
            Debug.Log("[Dog] Pooping animation ended.");
        }
    }

    public void StartPeeing() // Start peeing animation
    {
        if (animator == null) return;
        if (!HasParameter(animator, "isPeeing", AnimatorControllerParameterType.Bool))
        {
            Debug.LogWarning("[Dog] Animator missing 'isPeeing' Bool parameter.");
            return;
        }
        isDoingToilet = true;
        StopMoving();
        if (!isMoving)
        {
            if (animator != null && HasParameter(animator, "isPeeing", AnimatorControllerParameterType.Bool))
            {
                animator.SetBool("isPeeing", true);

                Debug.Log("[Dog] Peeing animation triggered.");
            }
            
        }
        else
        {
            Debug.Log("[Dog] Cannot pee while moving.");
        }
    }

    public void StopPeeing() // Stop peeing animation
    {
        if (animator == null) return;
        if (!HasParameter(animator, "isPeeing", AnimatorControllerParameterType.Bool))
        {
            Debug.LogWarning("[Dog] Animator missing 'isPeeing' Bool parameter.");
            return;
        }

        if (animator != null && HasParameter(animator, "isPeeing", AnimatorControllerParameterType.Bool))
        {
            animator.SetBool("isPeeing", false);
            isDoingToilet = false;
            Debug.Log("[Dog] Peeing animation ended.");
        }
    }

    public void DogCubSize() // Set dog to cub size
    {
        transform.localScale = Vector3.one * 0.4f;
    }

    public void DogAdultSize() // Set dog to adult size
    {
        transform.localScale = Vector3.one * 0.8f;
    }

    public void CheckFriendshipLevelToSizeUp() // Check friendship level and size up if needed
    {
        if (FriendShipLevelManager.Instance == null)
        {
            Debug.LogError("[Dog] No FriendshipLevelManager found in scene.");
            return;
        }
        int level = FriendShipLevelManager.Instance.GetCurrentFriendshipLevel();

        if (level >= 10)
        {
            DogAdultSize();
            Debug.Log("[Dog] Friendship level 5 reached. Dog is now adult size.");
        }
        else
        {
            DogCubSize();
            Debug.Log("[Dog] Friendship level below 5. Dog is cub size.");
        }
    }

    private void OnDrawGizmos() // Draw target position in editor for debugging
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPosition, 0.05f);
    }
}
