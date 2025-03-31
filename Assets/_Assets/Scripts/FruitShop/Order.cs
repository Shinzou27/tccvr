using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Order
{
    public List<OrderSet> list = new();
    public int amountOnOrder;
    public bool _dynamic = true;
    public void DebugOrder() {
        string toPrint = "";
        foreach (OrderSet order in list) {
            toPrint += $"Fruta: {order.fruit.fruitName} | Quantidade: {order.amount}";
        }
        Debug.Log(toPrint);
    }
    public void PlaceFruit(FruitSO fruit) {
        if (!_dynamic) return;
        if (HaveOrderSetOfFruit(fruit.type)) {
            Debug.Log("Já tenho essa fruta no order set");
            foreach (OrderSet order in list) {
                if (order.fruit == fruit) {
                    order.IncreaseAmount();
                }
            }
        } else {
            Debug.Log("Não tenho essa fruta no order set");
            list.Add(new OrderSet(fruit, 1));
        }
    }
    public OrderSet GetOrderSet(FruitType fruit) {
        foreach (OrderSet order in list) {
            if (order.fruit.type == fruit) {
                return order;
            }
        }
        return null;
    }
    private bool HaveOrderSetOfFruit(FruitType type) {
        foreach (OrderSet order in list) {
            if (order.fruit.type == type) {
                return true;
            }
        }
        return false;
    }
}
