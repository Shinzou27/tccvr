using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NPCFruitOrderController : NetworkBehaviour
{
    public Order order;
    void Start()
    {
        SetOrder();
    }

    private void SetOrder()
    {
        order = Utils.GenerateOrder();
        order._dynamic = false;
        order.DebugOrder();
    }

}
