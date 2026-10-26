using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoredBehavior : StateMachineBehaviour
{
    [SerializeField]
    private float _timeUntilBored;

    [SerializeField]
    private int _nubmerOfBoredAnimations;

    private bool _isBored = false;
    private float _timer;
    private int _boredAnimations;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (_isBored == false)
        {

            _timer += Time.deltaTime;
            if (_timer >= _timeUntilBored && stateInfo.normalizedTime % 1 < 0.02f)
            {
                _isBored = true;
                _boredAnimations = Random.Range(1, _nubmerOfBoredAnimations + 1);
                _boredAnimations = _boredAnimations * 2 - 1; // make it odd so that it ends on idle

                animator.SetFloat("BoredAnimation", _boredAnimations - 1);

            }
        }

        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }
        animator.SetFloat("BoredAnimation", _boredAnimations, 0.2f, Time.deltaTime);
    }

    private void ResetIdle()
    {

        if (_isBored)
        {
            _boredAnimations--;
        }

        _isBored = false;
        _timer = 0f;
        
    }
}
