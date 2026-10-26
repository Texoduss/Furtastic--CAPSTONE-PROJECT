using UnityEngine;

public class TogglePanelAnimation : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    private bool isOpen = false;

    public void ToggleAnimation()
    {
        if (isOpen)
        {
            animator.SetTrigger("Close");
        }
        else
        {
            animator.SetTrigger("Open");
        }

        isOpen = !isOpen;
    }
}
