using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaiterBehavior : MonoBehaviour
{

    private Vector3 kitchenDefaultPosition;
    private NavMeshAgent agent;
    private List<Table> tablesToVisit;
    private WaiterAnimationHandler animationHandler;
    private WaiterSpeakingHandler speakingHandler;

    public enum WaiterState { WALKING_TO_TABLE, WALKING_TO_KITCHEN, NOT_MOVING }
    private WaiterState currentState;
    private Vector3 currentDestination;
    [SerializeField] private Transform tray;


    void Start()
    {
        kitchenDefaultPosition = transform.position;
        tablesToVisit = new();
        currentState = WaiterState.NOT_MOVING;
        agent = GetComponent<NavMeshAgent>();
        animationHandler = GetComponent<WaiterAnimationHandler>();
        speakingHandler = GetComponent<WaiterSpeakingHandler>();
        speakingHandler.LeaveTableAction = UpdateDestination;
        EventManager.Instance.OnWaiterCalled += InsertTable;
        EventManager.Instance.OnOpenAIResponseStay += SpeakButStay;
        EventManager.Instance.OnOpenAIResponseLeave += PrepareToLeave;
    }
  void OnDestroy()
  {
        EventManager.Instance.OnWaiterCalled -= InsertTable;
        EventManager.Instance.OnOpenAIResponseStay -= SpeakButStay;
        EventManager.Instance.OnOpenAIResponseLeave -= PrepareToLeave;
  }

  private void SpeakButStay(object sender, string e)
    {
        speakingHandler.SpeakStay(e);
    }
    private void PrepareToLeave(object sender, string e)
    {
        speakingHandler.SpeakLeave(e);
    }

    private void RemoveTable(Table table)
    {
        tablesToVisit.Remove(table);
    }
    private void InsertTable(object sender = null, Table table = null)
    {
        if (table != null)
        {
            if (tablesToVisit.Contains(table)) return;
            tablesToVisit.Add(table);
            if (tablesToVisit.Count == 1)
            {
                currentState = WaiterState.WALKING_TO_TABLE;
                SetDestination(table.waitPosition.position); // se so tiver essa mesa na lista, vai direto pra ela. se n tiver é pq ele já deve estar a caminho de outra ou em outra
                if (RestaurantOrder.Instance.GetOrderState() == RestaurantOrder.OrderState.ON_ORDER)
                {
                    ChangeTrayVisibility(true);
                }
                else
                {
                    ChangeTrayVisibility(false);
                }
            }
        }
    }
    void Update()
    {
        if (currentState != WaiterState.NOT_MOVING)
        {
            float xzDistance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(currentDestination.x, 0, currentDestination.z));
            // Debug.Log($"distancia XZ: {xzDistance}");
            if (xzDistance < 0.25f)
            {
                if (RestaurantOrder.Instance.GetOrderState() == RestaurantOrder.OrderState.ON_ORDER)
                {
                    Debug.Log("Garçom ta com pedido");
                    animationHandler.StopWithOrder();
                }
                else
                {
                    Debug.Log("Garçom n ta com pedido");
                    animationHandler.Stop();
                }
                if (currentState == WaiterState.WALKING_TO_TABLE)
                {
                    speakingHandler.SpeakEnter();
                }
                currentState = WaiterState.NOT_MOVING;
            }
        }
    }
    private void SetDestination(Vector3 destination)
    {
        agent.destination = destination;
        currentDestination = destination;
        if (RestaurantOrder.Instance.GetOrderState() == RestaurantOrder.OrderState.ON_ORDER && currentState == WaiterState.WALKING_TO_TABLE)
        {
            animationHandler.WalkWithOrder();
        }
        else
        {
            animationHandler.Walk();
        }
    }
    public void UpdateDestination()
    {
        RemoveTable(tablesToVisit[0]);
        if (tablesToVisit.Count > 0)
        {
            currentState = WaiterState.WALKING_TO_TABLE;
            SetDestination(tablesToVisit[0].waitPosition.position);
        }
        else
        {
            currentState = WaiterState.WALKING_TO_KITCHEN;
            SetDestination(kitchenDefaultPosition);
            if (RestaurantOrder.Instance.GetOrderState() == RestaurantOrder.OrderState.PAYMENT_DONE)
            {
                RestaurantOrder.Instance.OnEndSession(5, RestaurantOrder.Instance.Clear);
            }
        }
    }
    public void ChangeTrayVisibility(bool active)
    {
        tray.gameObject.SetActive(active);
    }
}