using System;
using System.Collections.Generic;
using UnityEngine;

public class FruitShop : BaseManager<FruitShop> {
  public List<FruitSO> fruits;
  public int maxUniqueFruits;
  public int maxFruitAmount;
  public int rounds;
  [Range(0, 1)]  public float interestRate;
  public Order currentOrder = new();
  public Order fruitsPlacedByPlayer = new();
  private void Start() {
    currentOrder._dynamic = false;
  }
  public void UpdateOrder(Order order) {
    currentOrder = order;
    currentOrder.DebugOrder();
    EventManager.Instance.OnOrderCreated?.Invoke(this, EventArgs.Empty);
  }
  public bool EvaluateOrder() {
    foreach (OrderSet set in currentOrder.list) {
      OrderSet setPlacedByPlayer = fruitsPlacedByPlayer.GetOrderSet(set.fruit.type);
      if (setPlacedByPlayer.amount != set.amount) {
        return false;
      }
    }
    return true;
  }
}