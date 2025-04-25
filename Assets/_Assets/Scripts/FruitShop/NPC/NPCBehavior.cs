using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : NetworkBehaviour
{
    private List<Transform> tends;
    private Transform defaultDestination;
    [SerializeField] private NPCAnimationStateHandler animationStateHandler;
    [SerializeField] private GameObject orderUI;
    private NavMeshAgent agent;
    private bool turned = false;
    private bool reachedTend = false;
    public Action OnDestroyBehavior;
    // Start is called before the first frame update
    private void SetAgent() {
        if (agent == null) {
            agent = GetComponent<NavMeshAgent>();
        }
    }
    void Start()
    {
        orderUI.SetActive(false);
        SetAgent();
        EventManager.Instance.OnOrderDone += LeaveTend;
        tends = new(GameObject.FindGameObjectsWithTag("Tend").Select((go) => go.transform));

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) ChangeDestination(1, tends[0]);
        if (agent.remainingDistance <= 1.5f && !agent.isStopped && !reachedTend)
        {
            if (turned) {
                Debug.Log("Cheguei na tenda");
                animationStateHandler.WaitingOrder();
                agent.isStopped = true;
                reachedTend = true;
                orderUI.SetActive(true);
                EventManager.Instance.OnCustomerEnter?.Invoke(this, EventArgs.Empty);
            } else {
                Debug.Log("Não tive interesse em tenda alguma e serei destruído.");
                OnDestroyBehavior();
                Destroy(gameObject);
            }
        }
        Debug.Log($"Distância do {gameObject.name} ao destino: {Vector3.Distance(transform.position, agent.destination)}");
        if (!turned) {
            for (int i = 0; i < tends.Count; i++) {
                Transform t = tends[i];
                Debug.Log($"Distância do {gameObject.name} à {t.name}: {Vector3.Distance(transform.position, t.position)}");
                if (Vector3.Distance(transform.position, t.position) < 8) {
                    float interest = UnityEngine.Random.Range(0, 100);
                    bool shouldGo = interest <= FruitShop.Instance.interestRate * 100;
                    Debug.Log($"Interesse do {gameObject.name} na {t.name}: " + interest);
                    if (shouldGo) {
                        Debug.Log("Indo para a tenda");
                        TentInfo tentInfo = t.GetComponent<TentInfo>();
                        ChangeDestination(tentInfo.direction, tentInfo.npcStopPoint);
                        turned = true;
                    } else {
                        tends.Remove(t);
                    }
                }
            }
        }
    }
    public void SetDefaultDestination(Transform destination) {
        defaultDestination = destination;
        SetAgent();
        agent.destination = defaultDestination.position;
    }
    public void ChangeDestination(float dir, Transform tendToGo)
    {
        agent.isStopped = true;
        agent.destination = tendToGo.position;
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