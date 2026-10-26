using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FriendshipMeter : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Slider slider;

    [Header("Debug")]
    [SerializeField] private Button testButton;

    private void Start()
    {
        DataPersistenceManager.instance.LoadGame();
        if (FriendShipLevelManager.Instance != null)
        {
            FriendShipLevelManager.Instance.LevelChanged += UpdateUI;
            UpdateUI(FriendShipLevelManager.Instance.CurrentLevel, FriendShipLevelManager.Instance.CurrentProgress, FriendShipLevelManager.Instance.PointsPerLevel);
        }
        else
        {
            Debug.LogError("[FriendshipMeter] FriendShipLevelManager instance is null!");
        }

        if (testButton != null)
        {
            testButton.onClick.AddListener(() =>
            {
                if (FriendShipLevelManager.Instance != null)
                {
                    FriendShipLevelManager.Instance.AddProgress(1);
                }
                else
                {
                    Debug.LogError("[FriendshipMeter] Cannot add progress. FriendShipLevelManager instance is null!");
                }
            });
        }
        else
        {
            Debug.LogWarning("[FriendshipMeter] Test Button is not assigned.");
        }
    }

    private void UpdateUI(int level, int progress, int maxProgress)
    {
        if (slider != null)
        {
            slider.maxValue = maxProgress;
            slider.value = progress;
        }

        if (valueText != null)
        {
            valueText.text = level.ToString();
        }
        else
        {
            Debug.LogError("[FriendshipMeter] Slider reference is null!");
        }
    }

    

}
