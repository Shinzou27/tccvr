using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
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
    void Start()
    {
        orderUI.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        agent.destination = defaultDestination.position;
        EventManager.Instance.OnOrderDone += LeaveTend;
        tends = new(GameObject.FindGameObjectsWithTag("Tend").Select((go) => go.transform));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) ChangeDestination(1, tends[0]);
        if (agent.remainingDistance <= 1.5f && !agent.isStopped && !reachedTend)
        {
            if (agent.destination == defaultDestination.position) {
                OnDestroyBehavior();
                Destroy(gameObject);
            }
            animationStateHandler.WaitingOrder();
            agent.isStopped = true;
            reachedTend = true;
            orderUI.SetActive(true);
            EventManager.Instance.OnCustomerEnter?.Invoke(this, EventArgs.Empty);
        }
        if (!turned) {
            foreach (Transform t in tends) {
                // Debug.Log($"Distância do {gameObject.name} à {t.name}: {Vector3.Distance(transform.position, t.position)}");
                if (Vector3.Distance(transform.position, t.position) < 4) {
                    float interest = UnityEngine.Random.Range(0, 100);
                    Debug.Log($"Interesse do {gameObject.name} na {t.name}: " + interest);
                    if (interest <= FruitShop.Instance.interestRate * 100) {
                        Debug.Log("Indo para a tenda");
                        ChangeDestination(-1, t);
                        turned = true;
                    }
                }
            }
        }
    }
    public void SetDefaultDestination(Transform destination) {
        defaultDestination = destination;
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
