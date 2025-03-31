using System;

[Serializable]
public class OrderSet {
  public FruitSO fruit;
  public int amount;
  public OrderSet(FruitSO fruit, int amount) {
    this.fruit = fruit;
    this.amount = amount;
  }
  public void IncreaseAmount() {
    amount++;
  }
}