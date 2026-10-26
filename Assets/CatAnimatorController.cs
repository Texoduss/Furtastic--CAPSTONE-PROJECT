using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    private Animator animator;
    private CatMovement cat;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cat = GetComponent<CatMovement>();
    }

    /// <summary>
    /// Plays the SleepLoop animation (loops) and disables movement.
    /// </summary>
    public void Sleep()
    {
        if (animator == null) return;

        if (cat != null) cat.enabled = false; // stop moving
        animator.Play("SleepLoop"); // must match state name in Animator
        Debug.Log("[Cat] Sleeping...");
    }

    /// <summary>
    /// Plays the Petting animation (one-shot) and disables movement.
    /// </summary>
    public void Pet()
    {
        if (animator == null) return;

        if (cat != null) cat.enabled = false; // stop moving
        animator.Play("Petting"); // must match state name in Animator
        Debug.Log("[Cat] Being petted...");

        // If petting animation has a known duration, you could re-enable idle after
        // StartCoroutine(ResumeIdleAfter(2.5f)); // example duration
    }

    /// <summary>
    /// Return to Idle and re-enable movement.
    /// </summary>
    public void ResumeIdle()
    {
        if (animator == null) return;

        if (cat != null) cat.enabled = true;
        animator.Play("Idle"); // must match state name in Animator
        Debug.Log("[Cat] Back to idle. Movement re-enabled.");
    }
}

