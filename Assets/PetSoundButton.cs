using UnityEngine;
using UnityEngine.UI;

public class PetSoundButton : MonoBehaviour
{
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayPetSound();
            }
            else
            {
                Debug.LogWarning("AudioManager instance not found!");
            }
        });
    }
}
