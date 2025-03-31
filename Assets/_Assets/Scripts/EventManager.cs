using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EventManager {
    private static readonly EventManager _instance = new();
    private EventManager() { }
    public static EventManager Instance => _instance;

    /*
    ----------------------------------------------------------
    Eventos FRUIT SHOP
    ----------------------------------------------------------
    */
    public EventHandler OnCustomerEnter;
    public EventHandler OnOrderCreated;
    public EventHandler<bool> OnOrderDone;
}