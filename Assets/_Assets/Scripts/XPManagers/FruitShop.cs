using System;
using System.Collections.Generic;
using UnityEngine;

public class FruitShop : BaseManager<FruitShop>
{
  public List<FruitSO> fruits;
  public int maxUniqueFruits;
  public int maxFruitAmount;
  public int rounds;
  [Range(0, 1)] public float interestRate;
  public bool IsCustomerTalking;
  public Order fruitsPlacedByPlayer = new();
  public bool EvaluateOrder(Order customerOrder)
  {
    foreach (OrderSet set in customerOrder.list)
    {
      OrderSet setPlacedByPlayer = fruitsPlacedByPlayer.GetOrderSet(set.fruit.type);
      if (setPlacedByPlayer == null) return false;
      if (setPlacedByPlayer.amount != set.amount) return false;
    }
    return true;
  }
  public void ClearOrder()
  {
    fruitsPlacedByPlayer = new();
  }
}