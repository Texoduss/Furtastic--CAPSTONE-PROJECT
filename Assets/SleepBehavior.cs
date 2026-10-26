using UnityEngine;

public class SleepBehavior : StateMachineBehaviour
{
    [SerializeField]
    private float timeUntilSleep;

    [SerializeField]
    private int numberOfSleepAnimations;

    private bool isSleeping = false;
    private float timer;
    private int sleepAnimations;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isSleeping)
        {
            timer += Time.deltaTime;
            if (timer >= timeUntilSleep && stateInfo.normalizedTime % 1 < 0.02f)
            {
                isSleeping = true;
                sleepAnimations = Random.Range(1, numberOfSleepAnimations + 1);
                sleepAnimations = sleepAnimations * 2 - 1; // make it odd so that it ends on idle

                animator.SetFloat("SleepAnimation", sleepAnimations - 1);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }
        animator.SetFloat("SleepAnimation", sleepAnimations, 0.2f, Time.deltaTime);

    }

    private void ResetIdle()
    {
        if (isSleeping)
        {
            sleepAnimations--;
        }
        isSleeping = false;
        timer = 0f;
    }
}
