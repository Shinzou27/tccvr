using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private readonly string WAITING_ANIM = "Waiting";
    private readonly string WITH_ORDER = "WithOrder";
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void WalkWithOrder() {
        animator.SetBool(WITH_ORDER, true);
        animator.SetBool(WAITING_ANIM, false);
        Debug.Log("Andando com a bandeja");
    }
    public void Walk() {
        animator.SetBool(WAITING_ANIM, false);
        animator.SetBool(WITH_ORDER, false);
        Debug.Log("Andando");
    }
    public void Stop() {
        animator.SetBool(WAITING_ANIM, true);
        animator.SetBool(WITH_ORDER, false);
        Debug.Log("Parando");
    }
    public void StopWithOrder() {
        animator.SetBool(WAITING_ANIM, true);
        animator.SetBool(WITH_ORDER, true);
        Debug.Log("Parando com bandeja");
    }
}
