using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARRaycastTester : MonoBehaviour
{
    ARRaycastManager raycastManager;

    void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        Debug.Log("[ARRaycastTester] ARRaycastManager: " + (raycastManager != null));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // press Space to test
        {
            var screenPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            bool hit = raycastManager != null &&
                       raycastManager.Raycast(screenPoint, hits, TrackableType.PlaneWithinPolygon);
            Debug.Log($"[ARRaycastTester] Raycast center hit: {hit}, hits count: {hits.Count}");
            if (hit) Debug.Log($"Hit pose: {hits[0].pose.position}");
        }
    }
}
