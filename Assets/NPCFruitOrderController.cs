using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFruitOrderController : MonoBehaviour
{
    public Order order;
    void Start()
    {
        EventManager.Instance.OnCustomerEnter += SetOrder;
    }
    void OnDestroy()
    {
        EventManager.Instance.OnCustomerEnter -= SetOrder;
    }

    private void SetOrder(object sender, GameObject e)
    {
        if (e == gameObject)
        {
            order = Utils.GenerateOrder();
            order._dynamic = false;
            order.DebugOrder();
            Debug.Log("Coisando o evento:");
            EventManager.Instance.OnOrderCreated?.Invoke(this, gameObject);
        }
    }
}
