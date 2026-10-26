using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public struct Tutorial
{
    public GoalTutorial.OnboardingGoals CurrentGoal;
    public bool Completed;

    public Tutorial(GoalTutorial.OnboardingGoals goal)
    {
        CurrentGoal = goal;
        Completed = false;
    }
}

public class GoalTutorial : MonoBehaviour
{
    public enum OnboardingGoals
    {
        None,
        FindSurfaces,
        TapSurface
    }

    [Serializable]
    public class Step
    {
        public GameObject stepObject;
        public string buttonText;
        public bool includeSkipButton;
    }

    [SerializeField] List<Step> steps = new List<Step>();
    [SerializeField] XRPlacePrefabsManager objectSpawner;

    const int surfacesToTapToComplete = 1;

    Queue<Tutorial> onboardingGoals;
    Coroutine currentCoroutine;
    Tutorial currentGoal;
    bool allGoalsFinished = false;
    int surfacesTapped;
    int currentGoalIndex = 0;

    private void Awake()
    {
        StartCoaching();
    }

    private void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame && !allGoalsFinished && currentGoal.CurrentGoal == OnboardingGoals.FindSurfaces)
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            CompleteGoal();

        }

    }

    void CompleteGoal()
    {
        if (currentGoal.CurrentGoal == OnboardingGoals.TapSurface)
            objectSpawner.objectSpawned -= OnObjectSpawned;

        currentGoal.Completed = true;
        currentGoalIndex++;

        if (onboardingGoals.Count > 0)
        {
            currentGoal = onboardingGoals.Dequeue();

            if (currentGoalIndex - 1 >= 0 && currentGoalIndex - 1 < steps.Count)
                steps[currentGoalIndex - 1].stepObject.SetActive(false);

            if (currentGoalIndex >= 0 && currentGoalIndex < steps.Count)
                steps[currentGoalIndex].stepObject.SetActive(true);

            PreprocessGoal();
        }
        else
        {
            if (currentGoalIndex - 1 >= 0 && currentGoalIndex - 1 < steps.Count)
                steps[currentGoalIndex - 1].stepObject.SetActive(false);

            allGoalsFinished = true;
        }
    }


    void PreprocessGoal()
    {
        switch (currentGoal.CurrentGoal)
        {
            case OnboardingGoals.FindSurfaces:
                currentCoroutine = StartCoroutine(WaitThenComplete(5f));
                break;
            case OnboardingGoals.TapSurface:
                surfacesTapped = 0;
                objectSpawner.objectSpawned += OnObjectSpawned;
                break;
        }
    }

    IEnumerator WaitThenComplete(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!Pointer.current.press.wasPressedThisFrame)
        {
            currentCoroutine = null;
            CompleteGoal();
        }
    }

    void OnObjectSpawned(GameObject spawnedObject)
    {
        surfacesTapped++;
        if(currentGoal.CurrentGoal == OnboardingGoals.TapSurface && surfacesTapped >= surfacesToTapToComplete)
        {
            CompleteGoal();
        }
    }

    public void ForcedCompleteGoal() => CompleteGoal();

    public void StartCoaching()
    {
        onboardingGoals?.Clear();
        onboardingGoals = new Queue<Tutorial>();

        if (!allGoalsFinished)
            onboardingGoals.Enqueue(new Tutorial(OnboardingGoals.FindSurfaces));

        onboardingGoals.Enqueue(new Tutorial(OnboardingGoals.TapSurface));

        currentGoal = onboardingGoals.Dequeue();
        allGoalsFinished = true;
        currentGoalIndex = 0;

        for(int i = 0;i < steps.Count; i++)
        {
            steps[i].stepObject.SetActive(i == 0);
        }

        PreprocessGoal();
    }
}
