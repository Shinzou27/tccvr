using System.Collections.Generic;
using UnityEngine;

public class RestaurantOrder : BaseManager<RestaurantOrder>
{
  public List<Table> tables;
  public Prompt prompt;
  public List<AudioClip> audios;
  public enum OrderState { GREETING, ON_ORDER, ON_PAYMENT, PAYMENT_DONE }
  public enum SpeakState { NONE, PLAYER_CAN_SPEAK, WAITING_WAITER, WAITER_SPEAKING, PLAYER_SPEAKING }
  private SpeakState currentSpeakState;
  private OrderState currentOrderState;
  public List<FoodOrder> totalOrder;
  public float orderPrepareTime;
  void Start()
  {
    prompt = new();
    currentOrderState = OrderState.GREETING;
    currentSpeakState = SpeakState.NONE;
    TableDisplayManager.Instance.SetLabel(GetNiceSpeakState(SpeakState.NONE));
  }

  public Table GetTableById(int id)
  {
    if (id > 0 && id <= tables.Count) return tables[id - 1];
    return null;
  }
  public Table GetTableByPlayerId(int id)
  {
    foreach (Table table in tables)
    {
      for (int i = 0; i < table.playersOnTable.Length; i++)
      {
        if (table.playersOnTable[i] == id)
        {
          return table;
        }
      }
    }
    return null;
  }
  public Table GetTableWithWaiter()
  {
    foreach (Table table in tables)
    {
      if (table.withWaiter) return table;
    }
    return null;
  }
  public Prompt UpdatePrompt(PromptMessage newMessage)
  {
    prompt = new(prompt.messages, newMessage);
    return prompt;
  }
  public void UpdateOrderState(int newState)
  {
    currentOrderState = (OrderState)newState;
    TableDisplayManager.Instance.SetOrderState(currentOrderState.ToString());
  }
  public void UpdateSpeakState(SpeakState newState)
  {
    currentSpeakState = newState;
    TableDisplayManager.Instance.SetLabel(GetNiceSpeakState(newState));
  }
  public OrderState GetOrderState()
  {
    return currentOrderState;
  }
  public SpeakState GetSpeakState()
  {
    return currentSpeakState;
  }
  private string GetNiceSpeakState(SpeakState newState)
  {
    switch (newState)
    {
      case SpeakState.NONE:
        return "Call the waiter by pressing the red button!";
      case SpeakState.PLAYER_CAN_SPEAK:
        return "Press A to talk.";
      case SpeakState.PLAYER_SPEAKING:
        return "Listening...";
      case SpeakState.WAITER_SPEAKING:
        return "Waiter speaking...";
      case SpeakState.WAITING_WAITER:
        return "Processing...";
      default:
        return "...";
    }
  }
  public void AddToOrder(FoodOrder food)
  {
    totalOrder.Add(food);
  }
  public float GetBill()
  {
    float bill = 0f;
    foreach (FoodOrder order in totalOrder)
    {
      bill += order.price;
    }
    return bill;
  }
  public string GetBillDetails()
  {
    string toReturn = "";
    if (totalOrder.Count == 0)
    {
      toReturn = "As you did not ordered anything, we will charge you only for 4 dollars.";
    }
    else
    {
      foreach (FoodOrder order in totalOrder)
      {
        toReturn += $"One {order.keyword}: {order.price} dollars. ";
      }
      toReturn += $"So, the total charge will be {GetBill()} dollars.";
    }
    return toReturn;
  }
  public void Clear()
  {
    currentOrderState = OrderState.GREETING;
    currentSpeakState = SpeakState.NONE;
    totalOrder.Clear();
    prompt = new();
  }
}