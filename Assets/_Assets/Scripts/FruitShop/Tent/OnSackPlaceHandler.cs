using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSackPlaceHandler : MonoBehaviour
{
    [SerializeField] private TentInfo tent;


    public void Check(Order fruitsOnSack, Action callback)
    {
        Debug.Log("checando frutas com pedido");
        Order customerOrder = Utils.GetOrderFromTentCustomer(tent);
        if (customerOrder != null)
        {
            bool isCorrect = FruitShop.Instance.EvaluateOrder(customerOrder, fruitsOnSack);
            EventManager.Instance.OnOrderDone?.Invoke(this, new()
            {
                isCorrect = isCorrect,
                customer = tent.customer
            });
        }
        callback();
    }

}
