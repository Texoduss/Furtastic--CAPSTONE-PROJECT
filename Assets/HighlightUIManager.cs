using UnityEngine;

public class HighlightUIManager : MonoBehaviour
{
    public static HighlightUIManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
