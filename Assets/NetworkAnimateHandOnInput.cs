using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkAnimateHandOnInput : NetworkBehaviour
{
    [SerializeField] private InputActionProperty trigger;
    [SerializeField] private InputActionProperty grip;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Trigger", trigger.action.ReadValue<float>());
        animator.SetFloat("Grip", grip.action.ReadValue<float>());
    }
}
