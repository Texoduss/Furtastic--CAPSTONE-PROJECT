using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlace : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    ARRaycastManager raycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        if (raycastManager == null) Debug.LogError("[ARTapToPlace] No ARRaycastManager in scene!");
        if (spawnPrefab == null) Debug.LogError("[ARTapToPlace] spawnPrefab not assigned!");
    }

    void Update()
    {
        // mouse (editor) OR touch
        if (Input.touchCount == 0 && !Input.GetMouseButtonDown(0)) return;
        Vector2 screenPos = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;

        if (raycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            Instantiate(spawnPrefab, pose.position, pose.rotation);
            Debug.Log("[ARTapToPlace] Spawned prefab at: " + pose.position);
        }
        else
        {
            Debug.Log("[ARTapToPlace] Raycast did not hit any plane at " + screenPos);
        }
    }
}
