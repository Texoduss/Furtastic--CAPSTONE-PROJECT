using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class ARSessionAutoReset : MonoBehaviour
{
    [SerializeField] private ARSession arSession;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (arSession != null)
        {
            Debug.Log("[ARSessionAutoReset] Resetting ARSession on scene load: " + scene.name);
            arSession.Reset();
        }
        else
        {
            Debug.LogWarning("[ARSessionAutoReset] ARSession reference is missing!");
        }
    }
}
