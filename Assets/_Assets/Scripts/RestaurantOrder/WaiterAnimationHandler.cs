using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private readonly string WAITING_ANIM = "Waiting";
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Walk() {
        animator.SetBool(WAITING_ANIM, false);
        Debug.Log("Andando");
    }
    public void Stop() {
        animator.SetBool(WAITING_ANIM, true);
        Debug.Log("Parando");
    }
}
