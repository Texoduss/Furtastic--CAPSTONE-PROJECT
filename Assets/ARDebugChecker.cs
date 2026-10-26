using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class ARDebugChecker : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        StartCoroutine(DelayedCheck());
    }

    private IEnumerator DelayedCheck()
    {
        yield return new WaitForSeconds(0.2f); // let AR initialize a bit
        var origin = FindObjectOfType<XROrigin>();
        var session = FindObjectOfType<ARSession>();
        var planeManager = FindObjectOfType<ARPlaneManager>();
        var raycastMgr = FindObjectOfType<ARRaycastManager>();
        var cam = Camera.main;

        Debug.Log($"[ARDebugChecker] XROrigin: {(origin ? origin.name : "NULL")}");
        Debug.Log($"[ARDebugChecker] ARSession: {(session ? "present" : "NULL")}");
        Debug.Log($"[ARDebugChecker] ARPlaneManager: {(planeManager ? planeManager.trackables.count.ToString() : "NULL")}");
        Debug.Log($"[ARDebugChecker] ARRaycastManager: {(raycastMgr ? "present" : "NULL")}");
        Debug.Log($"[ARDebugChecker] Camera.main: {(cam ? cam.name : "NULL")}");
    }
}
