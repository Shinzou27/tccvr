using System;
using UnityEngine;

[Serializable]
public class Table {
  public int Id;
  public int maxPlayers;
  public int[] playersOnTable;
  public bool waitingOrder;
  public bool withWaiter;
  public Transform waitPosition;
  public Table() {
    playersOnTable = new int[maxPlayers];
  }
  public void InsertPlayerOnTable(int playerId) {
    for (int i = 0; i < playersOnTable.Length; i++) {
      if (playersOnTable[i] == 0) {
        playersOnTable[i] = playerId;
      }
    }
  }
}