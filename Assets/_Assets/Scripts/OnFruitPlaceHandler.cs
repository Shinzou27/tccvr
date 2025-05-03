using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFruitPlaceHandler : MonoBehaviour
{
    [SerializeField] private TentInfo tent;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Fruit")) {
            Debug.Log("Deixei uma fruta aqui: " + other.gameObject.name);
            if (other.gameObject.TryGetComponent(out FruitInfo info)) {
                FruitShop.Instance.fruitsPlacedByPlayer.PlaceFruit(info.data);
                Order customerOrder = Utils.GetOrderFromTentCustomer(tent);
                if (customerOrder.amountOnOrder == FruitShop.Instance.fruitsPlacedByPlayer.amountOnOrder) {
                    bool isCorrect = FruitShop.Instance.EvaluateOrder(customerOrder);
                    EventManager.Instance.OnOrderDone?.Invoke(this, new() {
                        isCorrect = isCorrect,
                        customer = tent.customer
                    });
                }
            }
        }
    }
}
