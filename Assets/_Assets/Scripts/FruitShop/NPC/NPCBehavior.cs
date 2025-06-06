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
  private bool goingToDefault = true;
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
    VoiceServiceHandler.Instance.OnApology += LeaveNoFruit;
    VoiceServiceHandler.Instance.OnRepeatRequest += Repeat;
    tents = new(GameObject.FindGameObjectsWithTag("Tent").Select((go) => go.transform));
  }

  private void Repeat(int playerNumber)
  {
    if (currentTent != null)
    {
      Debug.Log(playerNumber + " | " + currentTent.playerNumber);
      if (playerNumber == currentTent.playerNumber)
      {
        Debug.Log(gameObject.name + " falando.");
        npcDialogue.Repeat();
      }
    }
  }

  private void LeaveNoFruit(int playerNumber)
  {
    if (currentTent != null)
    {
      if (playerNumber == currentTent.playerNumber)
      {
        npcDialogue.LeaveNoFruit();
        currentTent.Add(FruitShop.PointValues.DO_NOT_HAVE_FRUIT_CORRECT);
        agent.destination = defaultDestination.position;
        goingToDefault = true;
        animationStateHandler.OrderIncorrect(() =>
        {
          agent.isStopped = false;
        });
        currentTent.IsFree = true;
        FruitShop.Instance.PlayerCanSpeak = false;
        currentTent.customer = null;
        currentTent = null;
      }
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    EventManager.Instance.OnOrderDone -= LeaveTent;
    if (VoiceServiceHandler.Instance != null)
    {
      VoiceServiceHandler.Instance.OnApology -= LeaveNoFruit;
      VoiceServiceHandler.Instance.OnRepeatRequest -= Repeat;
    }
  }
  private void Update()
  {
    if (FruitShop.Instance.GetRemainingTime() < 0.1f) return;
    if (Input.GetKeyDown(KeyCode.D)) ChangeDestination(1, tents[0]);
    if (agent.remainingDistance <= 1.5f && !agent.isStopped)
    {
      if (goingToDefault && IsServer)
      {
          // Debug.Log("Serei destruído.");
          OnDestroyBehavior?.Invoke();
          Destroy(gameObject);
      } else if (turned)
      {
        // Debug.Log("Cheguei na tenda");
        animationStateHandler.WaitingOrder();
        agent.isStopped = true;
        reachedTent = true;
        EventManager.Instance.OnOrderCreated?.Invoke(this, gameObject);
        AllowOnOrderCreatedClientRpc();
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
          if (!tentInfo.hasPlayer) shouldGo = false;
          // Debug.Log($"Interesse do {gameObject.name} na {t.name}: " + interest);
          if (shouldGo)
          {
            // Debug.Log("Indo para a tenda");
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
    goingToDefault = true;
  }
  public void ChangeDestination(float dir, Transform tentToGo)
  {
    agent.isStopped = true;
    agent.destination = tentToGo.position;
    animationStateHandler.Turn(dir, () => agent.isStopped = false);
    goingToDefault = false;
  }
  public void LeaveTent(object sender, EventManager.OnOrderDoneArgs onOrderDoneArgs)
  {
    if (onOrderDoneArgs.customer == gameObject)
    {
      agent.destination = defaultDestination.position;
      goingToDefault = true;
      if (onOrderDoneArgs.isCorrect)
      {
        npcDialogue.LeaveCorrect();
        animationStateHandler.OrderCorrect(() =>
        {
          agent.isStopped = false;
        });
        currentTent.Add(FruitShop.PointValues.CORRECT_ORDER);
      }
      else
      {
        npcDialogue.LeaveIncorrect();
        animationStateHandler.OrderIncorrect(() =>
        {
          agent.isStopped = false;
        });
        currentTent.Subtract(FruitShop.PointValues.WRONG_ORDER);
      }
      currentTent.IsFree = true;
      FruitShop.Instance.PlayerCanSpeak = false;
      currentTent.customer = null;
      currentTent = null;
    }
  }
}