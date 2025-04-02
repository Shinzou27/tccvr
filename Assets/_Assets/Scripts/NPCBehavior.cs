using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    [SerializeField] private Transform tend;
    [SerializeField] private Transform tendInterestPivot;
    [SerializeField] private Transform defaultDestination;
    [SerializeField] private NPCAnimationStateHandler animationStateHandler;
    [SerializeField] private GameObject orderUI;
    private NavMeshAgent agent;
    private bool turned = false;
    private bool reachedTend = false;
    // Start is called before the first frame update
    void Start()
    {
        orderUI.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        agent.destination = defaultDestination.position;
        EventManager.Instance.OnOrderDone += LeaveTend;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) ChangeDestination(1);
        if (agent.remainingDistance <= 1.5f && !agent.isStopped && !reachedTend)
        {
            animationStateHandler.WaitingOrder();
            agent.isStopped = true;
            reachedTend = true;
            orderUI.SetActive(true);
            EventManager.Instance.OnCustomerEnter?.Invoke(this, EventArgs.Empty);
        }
        Debug.Log(Vector3.Distance(transform.position, tendInterestPivot.position));
        if (!turned && Vector3.Distance(transform.position, tendInterestPivot.position) < 1) {
            Debug.Log("Indo para a tenda");
            ChangeDestination(-1);
            turned = true;
        }
    }
    public void ChangeDestination(float dir)
    {
        agent.isStopped = true;
        agent.destination = tend.position;
        animationStateHandler.Turn(dir, () => agent.isStopped = false);
    }
    public void LeaveTend(object sender, bool correctOrder)
    {
        agent.destination = defaultDestination.position;
        if (correctOrder) animationStateHandler.OrderCorrect(() =>
        {
            agent.isStopped = false;

        });
        else animationStateHandler.OrderIncorrect(() =>
        {
            agent.isStopped = false;

        });
    }
}
