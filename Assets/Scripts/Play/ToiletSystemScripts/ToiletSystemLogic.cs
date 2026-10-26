using System.Collections;
using UnityEngine;

public class PetToiletLogic : MonoBehaviour
{
    [Header("Toilet Timing (seconds)")]
    public float minInterval = 30f; // 30 sec
    public float maxInterval = 60f; // 1 mins

    [Header("UI Reference")]
    public PetToiletUI toiletUI;
    public EmoteUI emoteUI;

    private DogMovement dogMovement;
    private CatMovement catMovement;
    private bool toiletTriggered = false;
    private Coroutine toiletCoroutine;

    private void Awake()
    {
        // Get movement component on the same prefab
        dogMovement = GetComponent<DogMovement>();
        catMovement = GetComponent<CatMovement>();

        // Optional but helpful
        emoteUI = FindObjectOfType<EmoteUI>();

        // Validate that at least one movement script exists
        if (dogMovement == null && catMovement == null)
        {
            Debug.LogError($"[{name}] PetToiletLogic: No DogMovement or CatMovement found on this prefab!");
            enabled = false;
            return;
        }

        // Assign UI if missing
        if (toiletUI == null)
        {
            toiletUI = FindObjectOfType<PetToiletUI>();
            if (toiletUI == null)
            {
                Debug.LogError($"[{name}] PetToiletLogic: PetToiletUI not found in scene!");
                enabled = false;
                return;
            }
        }
    }


    private void Start()
    {
        toiletCoroutine = StartCoroutine(ToiletCycle());
    }

    private IEnumerator ToiletCycle() // infinite loop
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            if (toiletTriggered) continue;

            TriggerRandomToilet();
        }
    }

    private void TriggerRandomToilet() // 50% chance to pee or poop
    {
        if (dogMovement == null || catMovement == null) return;

        toiletTriggered = true;
        bool doPoop = Random.value > 0.5f; // 50% chance to poop or pee

        if (emoteUI == null)
            Debug.LogWarning($"[{name}] EmoteUI reference is missing.");

        if (emoteUI != null)
            emoteUI.ShowPetToiletEmote();

        if (doPoop)
            StartCoroutine(HandlePoop());
        else
            StartCoroutine(HandlePee());
    }

    private IEnumerator HandlePoop() // IEnumerator for timing
    {
        
        // Let pet finish wandering before stopping
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));

        if (catMovement != null)
        {
            catMovement.StartPooping();
            catMovement.StopMoving();
        }
        else if (dogMovement != null)
        {
            dogMovement.StartPooping();
            dogMovement.StopMoving();
        }

        
        Debug.Log($"[{name}] Pet is pooping...");

        // Wait for animation duration
        yield return new WaitForSeconds(3f);

        toiletUI.ShowCleanPoopButton(this);
    }

    private IEnumerator HandlePee() // IEnumerator for timing
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));

        if (catMovement != null)
        {
            catMovement.StartPeeing();
            catMovement.StopMoving();
        }
        else if (dogMovement != null)
        {
            dogMovement.StartPeeing();
            dogMovement.StopMoving();
        }

        Debug.Log($"[{name}] Pet is peeing...");

        yield return new WaitForSeconds(3f);

        toiletUI.ShowFlushPeeButton(this);
    }

    public void OnCleaned() // called by ToiletSystemUI when player cleans poop
    {
        if (dogMovement != null)
        {
            dogMovement.StopPooping();
            dogMovement.PickNewDirection();
            dogMovement.StartMoving();
        }
        else if (catMovement != null)
        {
            catMovement.StopPooping();
            catMovement.PickNewDirection();
            catMovement.StartMoving();
        }
        Debug.Log($"[{name}] Poop cleaned. Pet resumes wandering.");
        if (emoteUI != null)
            emoteUI.HidePetToiletEmote();

        toiletUI.HideButtons();
        toiletTriggered = false;
    }

    public void OnFlushed() // called by ToiletSystemUI when player flushes pee
    {
        if (dogMovement != null)
        {
            dogMovement.StopPeeing();
            dogMovement.PickNewDirection();
            dogMovement.StartMoving();
        }
        else if (catMovement != null)
        {
            catMovement.StopPeeing();
            catMovement.PickNewDirection();
            catMovement.StartMoving();
        }
        Debug.Log($"[{name}] Pee flushed. Pet resumes wandering.");
        if (emoteUI != null)
            emoteUI.HidePetToiletEmote();

        toiletUI.HideButtons();
        toiletTriggered = false;
        
    }
    private void OnDisable() // stop coroutine when disabled
    {
        if (toiletCoroutine != null)
            StopCoroutine(toiletCoroutine);
    }
}
