using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.WitAi.TTS.Utilities;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class WaiterBehavior : MonoBehaviour
{
    private Animator animator;
    private readonly string WAITING = "Waiting";
    private NavMeshAgent agent;
    [SerializeField] private Transform kitchenDefaultPosition;
    [SerializeField] private Transform waitPosition;
    private bool goingToTable = false;
    [SerializeField] private TTSSpeaker speaker;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        EventManager.Instance.OnWaiterShouldMove += SetDestination;
        agent.destination = kitchenDefaultPosition.position;
    }

    private void SetDestination(object sender, EventManager.OnWaiterShouldMoveArgs e)
    {
        if (e.waiting) {
            DebugTTS();
        }
        animator.SetBool(WAITING, e.waiting);
        // agent.destination = e.destination.position; // Trocar isso pela posição da mesa depois que tiver ajeitadin
        agent.destination = waitPosition.position;

    }
    void Update()
    {
        if (goingToTable && agent.remainingDistance <= 1f)
        {
            animator.SetBool(WAITING, true);
            DebugTTS();
        }
    }
    private void DebugTTS()
    {
        // speaker.Speak("Hello! What can I do for you today?");
        Debug.Log("AAA");
        speaker.Speak("Hello! It's friday! Who did it, did it.");
    }
}