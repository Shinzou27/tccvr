using System.Collections.Generic;
using UnityEngine;

public class RestaurantOrder : BaseManager<RestaurantOrder> {
  public List<Table> tables;
  public Prompt prompt;
  public List<AudioClip> audios;
  public enum OrderState { GREETING, ON_ORDER, WAITING_PAYMENT}
  private OrderState currentState;
  void Start()
  {
    prompt = new();
    currentState = OrderState.GREETING;
  }

  public Table GetTableById(int id) {
    if (id > 0 && id <= tables.Count) return tables[id-1];
    return null;
  }
  public Table GetTableByPlayerId(int id) {
    foreach (Table table in tables) {
      for (int i = 0; i < table.playersOnTable.Length; i++) {
        if (table.playersOnTable[i] == id) {
          return table;
        }
      }
    }
    return null;
  }
  public Table GetTableWithWaiter() {
    foreach (Table table in tables) {
      if (table.withWaiter) return table;
    }
    return null;
  }
  public Prompt UpdatePrompt(PromptMessage newMessage) {
    prompt = new(prompt.messages, newMessage);
    return prompt;
  }
  public void UpdateOrderState(int newState) {
    currentState = (OrderState) newState;
  }
  public OrderState GetOrderState() {
    return currentState;
  }
}