using System.Collections.Generic;
using UnityEngine;

public class RestaurantOrder : BaseManager<RestaurantOrder> {
  public List<Table> tables;
  public Prompt prompt;

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
    Prompt prompt = new(this.prompt.messages, newMessage);
    return prompt;
  }
}