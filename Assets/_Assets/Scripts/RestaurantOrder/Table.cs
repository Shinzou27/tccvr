using System;
using UnityEngine;

[Serializable]
public class Table {
  public bool active;
  public int Id;
  public int maxPlayers;
  public int[] playersOnTable;
  public Transform[] tableSeats;
  public bool waitingOrder;
  public bool withWaiter;
  public Transform waitPosition;
  public void InsertPlayerOnTable(int playerId) {
    if (playersOnTable.Length == 0) {
      playersOnTable = new int[maxPlayers];
    }
    for (int i = 0; i < playersOnTable.Length; i++) {
      if (playersOnTable[i] == 0) {
        playersOnTable[i] = playerId;
        return;
      }
    }
  }
}