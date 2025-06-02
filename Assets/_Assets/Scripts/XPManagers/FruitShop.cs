using System;
using System.Collections;
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
  public int maxTime;
  private float elapsed;
  private bool started;
  public static class PointValues
  {
    public const int CORRECT_ORDER = 100;
    public const int DO_NOT_HAVE_FRUIT_CORRECT = 60;
    public const int DO_NOT_HAVE_FRUIT_WRONG = 30;
    public const int WRONG_ORDER = 50;
    public const int HELPING = 50;
  }
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
  void Update()
  {
    if (started)
    {
      elapsed += Time.deltaTime;
    }
  }
  public float GetRemainingTime()
  {
    return 1 - (elapsed / maxTime);
  }
  public void StartCounter()
  {
    started = true;
    OnEndSession(maxTime);
  }
}