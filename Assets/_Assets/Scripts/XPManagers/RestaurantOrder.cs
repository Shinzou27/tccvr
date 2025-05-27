using System.Collections.Generic;
using UnityEngine;

public class RestaurantOrder : BaseManager<RestaurantOrder>
{
  public List<Table> tables;
  public Prompt prompt;
  public List<AudioClip> audios;
  public enum OrderState { GREETING, ON_ORDER, WAITING_PAYMENT }
  public enum SpeakState { NONE, PLAYER_CAN_SPEAK, WAITING_WAITER, WAITER_SPEAKING, PLAYER_SPEAKING }
  private SpeakState currentSpeakState;
  private OrderState currentOrderState;
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
        return "Aperte o botão vermelho para chamar o garçom à sua mesa!";
      case SpeakState.PLAYER_CAN_SPEAK:
        return "Aperte A para falar.";
      case SpeakState.PLAYER_SPEAKING:
        return "Ouvindo...";
      case SpeakState.WAITER_SPEAKING:
        return "Garçom está falando...";
      case SpeakState.WAITING_WAITER:
        return "Processando...";
      default:
        return "...";
    }
  }
}