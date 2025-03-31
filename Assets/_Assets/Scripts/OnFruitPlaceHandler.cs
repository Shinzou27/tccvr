using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFruitPlaceHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Fruit")) {
            Debug.Log("Deixei uma fruta aqui: " + other.gameObject.name);
            if (other.gameObject.TryGetComponent(out FruitInfo info)) {
                FruitShop.Instance.fruitsPlacedByPlayer.PlaceFruit(info.data);
                if (FruitShop.Instance.currentOrder.amountOnOrder == FruitShop.Instance.fruitsPlacedByPlayer.amountOnOrder) {
                    bool isCorrect = FruitShop.Instance.EvaluateOrder();
                    EventManager.Instance.OnOrderDone?.Invoke(this, isCorrect);
                }
            }
        }
    }
}
