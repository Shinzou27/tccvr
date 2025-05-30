using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : NetworkBehaviour
{
  private List<Transform> tents;
  private Transform defaultDestination;
  [SerializeField] private NPCAnimationStateHandler animationStateHandler;
  [SerializeField] private float angleOffsetToCheck;
  private NavMeshAgent agent;
  private bool turned = false;
  private bool reachedTent = false;
  public Action OnDestroyBehavior;
  private NPCDialogue npcDialogue;
  private TentInfo currentTent;
  // Start is called before the first frame update
  private void SetAgent()
  {
    if (agent == null)
    {
      agent = GetComponent<NavMeshAgent>();
    }
  }
  void Start()
  {
    npcDialogue = GetComponent<NPCDialogue>();
    SetAgent();
    EventManager.Instance.OnOrderDone += LeaveTent;
    tents = new(GameObject.FindGameObjectsWithTag("Tent").Select((go) => go.transform));
  }
  public override void OnDestroy()
  {
    base.OnDestroy();
    EventManager.Instance.OnOrderDone -= LeaveTent;
  }
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.D)) ChangeDestination(1, tents[0]);
    if (agent.remainingDistance <= 1.5f && !agent.isStopped && !reachedTent)
    {
      if (turned)
      {
        // Debug.Log("Cheguei na tenda");
        animationStateHandler.WaitingOrder();
        agent.isStopped = true;
        reachedTent = true;
        EventManager.Instance.OnOrderCreated?.Invoke(this, gameObject);
        AllowOnOrderCreatedClientRpc();
      }
      else
      {
        if (IsServer) {
          // Debug.Log("Não tive interesse em tenda alguma e serei destruído.");
          OnDestroyBehavior?.Invoke();
          Destroy(gameObject);
        }
      }
    }
    // Debug.Log($"Distância do {gameObject.name} ao destino: {Vector3.Distance(transform.position, agent.destination)}");
    if (!turned)
    {
      for (int i = 0; i < tents.Count; i++)
      {
        Transform t = tents[i];

        // Debug.Log($"Angulo entre {gameObject.name} e a tenda {t.name}: {Vector3.Angle(transform.position - t.position, t.right)}");
        // Debug.Log($"Distância do {gameObject.name} à {t.name}: {Vector3.Distance(transform.position, t.position)}");
        if (Mathf.Abs(Vector3.Angle(transform.position - t.position, t.right) - 90) <= angleOffsetToCheck)
        {
          TentInfo tentInfo = t.GetComponent<TentInfo>();
          float interest = UnityEngine.Random.Range(0, 100);
          bool shouldGo = tentInfo.IsFree && interest <= FruitShop.Instance.interestRate * 100;
          Debug.Log($"Interesse do {gameObject.name} na {t.name}: " + interest);
          if (shouldGo)
          {
            Debug.Log("Indo para a tenda");
            ChangeDestination(tentInfo.direction, tentInfo.npcStopPoint);
            tentInfo.IsFree = false;
            tentInfo.customer = gameObject;
            turned = true;
            currentTent = tentInfo;
          }
          else
          {
            tents.Remove(t);
          }
        }
      }
    }
  }

  [ClientRpc]
  public void AllowOnOrderCreatedClientRpc() {
    if (IsHost) return;
    EventManager.Instance.OnOrderCreated?.Invoke(this, gameObject);
  }
  public void SetDefaultDestination(Transform destination)
  {
    defaultDestination = destination;
    SetAgent();
    agent.destination = defaultDestination.position;
  }
  public void ChangeDestination(float dir, Transform tentToGo)
  {
    agent.isStopped = true;
    agent.destination = tentToGo.position;
    animationStateHandler.Turn(dir, () => agent.isStopped = false);
  }
  public void LeaveTent(object sender, EventManager.OnOrderDoneArgs onOrderDoneArgs)
  {
    if (onOrderDoneArgs.customer == gameObject)
    {
      agent.destination = defaultDestination.position;
      if (onOrderDoneArgs.isCorrect)
      {
        npcDialogue.LeaveCorrect();
        animationStateHandler.OrderCorrect(() =>
        {
          agent.isStopped = false;
        });
      }
      else
      {
        npcDialogue.LeaveIncorrect();
        animationStateHandler.OrderIncorrect(() =>
        {
          agent.isStopped = false;

        });
      }
      currentTent.IsFree = true;
      currentTent.customer = null;
    }
  }
}