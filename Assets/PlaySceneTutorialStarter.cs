using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneTutorialStarter : MonoBehaviour
{
    [Header("Animation References")]
    public Animator scanSurfaceAnimator;
    public Animator tapSurfaceAnimator;

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
        
    }
}
