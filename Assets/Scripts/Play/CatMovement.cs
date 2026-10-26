using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CatMovement : MonoBehaviour, IDataPersistence
{
    [Header("Movement Settings")]
    public float moveSpeed = 0.3f;
    public float minMoveDistance = 0.3f;
    public float maxMoveDistance = 0.8f;
    public float idleTime = 3f;

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
    private float holdDurationThreshold = 0.5f; // seconds to hold for "long press"
    private float touchStartTime;
    private bool isHolding = false;

    [SerializeField] private Animator animator;

    private ARPlane movementPlane; // plane to move on
    private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;

    AudioManager audioManager;

    void Awake() // called when the script instance is being loaded
    {
        audioManager = GameObject.FindGameObjectsWithTag("Audio")[0].GetComponent<AudioManager>();

        CheckFriendshipLevelToSizeUp();
    }

    void Start() // called before the first frame update
    {
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.applyRootMotion = false;

        planeManager = FindObjectOfType<ARPlaneManager>();
        raycastManager = FindObjectOfType<ARRaycastManager>();

        // Defensive checks
        if (animator == null)
            Debug.LogError("[AR Cat] No Animator found on CatMovement or its children!");
        if (planeManager == null)
            Debug.LogError("[AR Cat] No ARPlaneManager found in scene!");
        if (raycastManager == null)
            Debug.LogError("[AR Cat] No ARRaycastManager found in scene!");
    }

    void Update()
    {
        // Prevent movement while sleeping OR transitioning to/from Sleep
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            bool inSleep = stateInfo.IsName("Sleep") || stateInfo.IsTag("Sleep");

            if (inSleep || animator.IsInTransition(0))
                return;

            if (animator.GetBool("isSleeping")) return;
        }

        if (isSleeping || isTouched || isDoingToilet) return;

        if (movementPlane == null) // try to assign plane automatically
        {
            if (planeManager != null && planeManager.trackables.count > 0)
            {
                movementPlane = GetFirstDetectedPlane();
                if (movementPlane != null)
                {
                    Debug.Log("[AR Cat] Plane assigned automatically: " + movementPlane.trackableId);

                    Vector3 newPos = transform.position;
                    newPos.y = movementPlane.transform.position.y;
                    transform.position = newPos;

                    PickNewDirection();
                    StartMoving();
                }
            }
            else
            {
                return;
            }
        }
        // --- Movement Logic ---
        if (isMoving) 
            MoveTowardsTarget();

        // --- Pointer mode input check ---
        if (isPointerMode && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPos = Input.GetTouch(0).position;
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
            {
                targetPosition = hits[0].pose.position;
                targetPosition.y = transform.position.y;
                StartMoving();
                Debug.Log("[AR Cat] Moving to tapped position: " + targetPosition);
            }
        }

        // --- Touch / Click for "isTouched" ---

        if (waitingForTouch)
        {
            if (Camera.main == null)
            {
                Debug.LogError("[Cat] No MainCamera found. Cannot detect touches.");
                return;
            }

            // --- PC / Editor Input ---
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == gameObject)
                    {
                        touchStartTime = Time.time;
                        isHolding = true;
                        Debug.Log("[Cat] Touch started on cat (mouse).");
                    }
                }
            }

            if (isHolding && Input.GetMouseButton(0))
            {
                if (Time.time - touchStartTime >= holdDurationThreshold)
                {
                    StartTouchAnimation(Input.mousePosition);
                    isHolding = false; // trigger once
                    Debug.Log("[Cat] Long press detected on cat (mouse).");
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isTouched)
                    StopTouchAnimation();

                isHolding = false;
                touchStartTime = 0f;
            }

            // --- Mobile Input ---
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (Physics.Raycast(ray, out RaycastHit hitTouch))
                        {
                            if (hitTouch.collider != null && hitTouch.collider.gameObject == gameObject)
                            {
                                touchStartTime = Time.time;
                                isHolding = true;
                                Debug.Log("[Cat] Touch started on cat (mobile).");
                            }
                        }
                        break;

                    case TouchPhase.Stationary:
                        if (isHolding && Time.time - touchStartTime >= holdDurationThreshold)
                        {
                            isHolding = false;
                            StartTouchAnimation(touch.position);
                            Debug.Log("[Cat] Long press detected on cat (mobile).");
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (!isHolding)
                            StopTouchAnimation();

                        isHolding = false;
                        touchStartTime = 0f;
                        break;
                }
            }
        }
    }

    ARPlane GetFirstDetectedPlane()
    {
        foreach (var plane in planeManager.trackables)
            return plane;
        return null;
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
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

    IEnumerator IdleAndChangeDirection()
    {
        StopMoving();
        yield return new WaitForSeconds(idleTime);
        if (!isSleeping || !isTouched || !isDoingToilet)
        {
            PickNewDirection();
            StartMoving();
        }
    }

    public void StartMoving()
    {
        if (isDoingToilet) return;

        if (animator != null)
            animator.SetBool("isWalking", true);

        isMoving = true;
        Debug.Log("[AR Cat] Moving to: " + targetPosition);
    }

    public void StopMoving()
    {
        if (animator != null)
            animator.SetBool("isWalking", false);

        isMoving = false;
        Debug.Log("[AR Cat] Idle...");

        if (currentIdleRoutine != null)
        {
            StopCoroutine(currentIdleRoutine);
            currentIdleRoutine = null;
        }
    }

    public void PickNewDirection()
    {
        if (movementPlane == null) return;

        using (var boundary2D = movementPlane.boundary)
        {
            if (!boundary2D.IsCreated || boundary2D.Length < 3) return;

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

    Vector3 GetRandomPointInPolygon(List<Vector3> polygon)
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

    bool IsPointInPolygon(Vector3 point, List<Vector3> polygon)
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
    public void OnSleepButton()
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

    IEnumerator DelaySleepToWalk()
    {
        Debug.Log("[AR Cat] Waking up from sleep...");
        yield return new WaitForSeconds(3f);
        if (!isSleeping && !isTouched && !isDoingToilet)
        {
            PickNewDirection();
            StartMoving();
        }
    }

    // --- Touch Button ---
    public void OnTouchButton()
    {
        waitingForTouch = !waitingForTouch;
        Debug.Log("[AR Cat] Touch mode: " + (waitingForTouch ? "ENABLED" : "DISABLED"));

        if (waitingForTouch)
            StopMoving();
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
    private void StartTouchAnimation(Vector3 screenPos)
    {
        if (!TryGetComponent<Collider>(out var collider))
        {
            Debug.LogError("[Cat] No collider found on DogMovement. Cannot detect touches.");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("[Cat] Raycast hit: " + hit.transform.name);
            if (hit.transform == transform && animator != null)
            {
                animator.SetBool("isTouched", true);
                isTouched = true;
                Debug.Log("[Cat] Touched! Playing touch animation.");
            }
        }
    }

    private void StopTouchAnimation()
    {
        if (animator != null)
            animator.SetBool("isTouched", false);

        isTouched = false;
        Debug.Log("[Cat] Touch ended. Returning to normal state.");

        if (!isSleeping && !isDoingToilet)
        {
            PickNewDirection();
            StartMoving();
        }
    }
    // --- Helper to check animator parameters ---
    bool HasParameter(Animator animator, string paramName, AnimatorControllerParameterType type)
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == type && param.name == paramName)
                return true;
        }
        return false;
    }

    // --- Pet Noise Button ---
    public void PetNoiseButton()
    {
        if (audioManager == null)
        {
            Debug.LogError("[AR Cat] No AudioManager found in scene!");
            return;
        }
        if (audioManager.catSound == null)
        {
            Debug.LogError("[AR Cat] No cat sound assigned in AudioManager!");
            return;
        }

        audioManager.PlaySFX(audioManager.catSound);
    }

    // --- Pointer Button (Toggle) ---
    public void OnPointerButton()
    {
        isPointerMode = !isPointerMode;
        Debug.Log("[AR Cat] Pointer mode: " + (isPointerMode ? "ENABLED" : "DISABLED"));

        if (isPointerMode)
            StopMoving();
        else
        {
            PickNewDirection();
            StartMoving();
        }
    }

    // --- Toilet Button ---
    public void StartPooping()
    {
        if (animator == null) return;
        if (!HasParameter(animator, "isPooping", AnimatorControllerParameterType.Bool))
        {
            Debug.LogWarning("[Cat] Animator missing 'isPooping' Bool parameter.");
            return;
        }
        isDoingToilet = true;
        StopMoving();
        if (!isMoving)
        {
            if (animator != null && HasParameter(animator, "isPooping", AnimatorControllerParameterType.Bool))
            {
                animator.SetBool("isPooping", true);

                Debug.Log("[Cat] Pooping animation triggered.");
            }
        }
        else
        {
            Debug.Log("[Cat] Cannot poop while moving.");
        }
    }

    public void StopPooping()
    {
        if (animator == null) return;
        if (!HasParameter(animator, "isPooping", AnimatorControllerParameterType.Bool))
        {
            Debug.LogWarning("[Cat] Animator missing 'isPooping' Bool parameter.");
            return;
        }

        if (animator != null && HasParameter(animator, "isPooping", AnimatorControllerParameterType.Bool))
        {
            animator.SetBool("isPooping", false);
            isDoingToilet = false;
            Debug.Log("[Cat] Pooping animation ended.");
        }
    }

    public void StartPeeing()
    {
        if (animator == null) return;
        if (!HasParameter(animator, "isPeeing", AnimatorControllerParameterType.Bool))
        {
            Debug.LogWarning("[Cat] Animator missing 'isPeeing' Bool parameter.");
            return;
        }
        isDoingToilet = true;
        StopMoving();
        if (!isMoving)
        {
            if (animator != null && HasParameter(animator, "isPeeing", AnimatorControllerParameterType.Bool))
            {
                animator.SetBool("isPeeing", true);

                Debug.Log("[Cat] Peeing animation triggered.");
            }

        }
        else
        {
            Debug.Log("[Cat] Cannot pee while moving.");
        }
    }

    public void StopPeeing()
    {
        if (animator == null) return;
        if (!HasParameter(animator, "isPeeing", AnimatorControllerParameterType.Bool))
        {
            Debug.LogWarning("[Cat] Animator missing 'isPeeing' Bool parameter.");
            return;
        }

        if (animator != null && HasParameter(animator, "isPeeing", AnimatorControllerParameterType.Bool))
        {
            animator.SetBool("isPeeing", false);
            isDoingToilet = false;
            Debug.Log("[Cat] Peeing animation ended.");
        }
    }

    public void CatKittenSize()
    {
        transform.localScale = Vector3.one * 0.5f;
        Debug.Log("[Cat] Scaled to Kitten size.");
    }

    public void CatAdultSize()
    {
        transform.localScale = Vector3.one * 1f;
        Debug.Log("[Cat] Scaled to Adult size.");
    }

    public void CheckFriendshipLevelToSizeUp()
    {
        if (FriendShipLevelManager.Instance == null)
        {
            Debug.LogError("[Cat] No FriendShipLevelManager instance found.");
            return;
        }
        int level = FriendShipLevelManager.Instance.GetCurrentFriendshipLevel();

        if (level >= 5)
        {
            CatAdultSize();
        }
        else
        {
            CatKittenSize();
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 0.05f);
    }

    void IDataPersistence.LoadData(GameData data)
    {
        
    }

    void IDataPersistence.SaveData(GameData data)
    {
        
    }
}
