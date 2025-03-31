using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationStateHandler : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Turn(float dir, Action action) {
        /*
        esq = -1 | dir = 1
        */
        animator.SetFloat("TurnDirection", dir);
        animator.SetTrigger("Interest");
        // animator.ResetTrigger("Interest");
        DebugAnimationLength();
        string animName = dir == -1 ? "TurnLeft" : "TurnRight";
        StartCoroutine(ExecuteAfterAnimationEnd(animName, action));
    }
    public void OrderCorrect(Action action) {
        animator.SetTrigger("Thanking");
        animator.SetBool("OrderDone", true);
        animator.SetBool("OrderSucceeded", true);
        // animator.ResetTrigger("Thanking");
        DebugAnimationLength();
        StartCoroutine(ExecuteAfterAnimationEnd("WalkingTurn", action));
    }
    public void OrderIncorrect(Action action) {
        animator.SetBool("OrderDone", true);
        animator.SetBool("OrderSucceeded", false);
        // animator.ResetTrigger("OrderDone");
        DebugAnimationLength();
        StartCoroutine(ExecuteAfterAnimationEnd("WalkingTurn", action));
    }
    public void WaitingOrder() {
        animator.SetBool("WaitingOrder", true);
        DebugAnimationLength();
    }
    private void DebugAnimationLength() {
    }
    private void Update() {
    }
    public IEnumerator ExecuteAfterAnimationEnd(string animationName, Action action) {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
            yield return null;
        }
        float prev = 0;
        while (prev <= animator.GetCurrentAnimatorStateInfo(0).normalizedTime) {
            prev = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            yield return null;
        }
        action?.Invoke();
    }
}
