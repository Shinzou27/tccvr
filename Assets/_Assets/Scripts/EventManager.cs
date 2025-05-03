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
    Eventos MULTIPLAYER
    ----------------------------------------------------------
    */
    public EventHandler<Transform> OnPlayerEnter;
    /*
    ----------------------------------------------------------
    Eventos FRUIT SHOP
    ----------------------------------------------------------
    */
    public EventHandler<GameObject> OnCustomerEnter;
    public EventHandler<GameObject> OnOrderCreated;
    public class OnOrderDoneArgs : EventArgs {
        public bool isCorrect;
        public GameObject customer;
    }
    public EventHandler<OnOrderDoneArgs> OnOrderDone;
    /*
    ----------------------------------------------------------
    Eventos RESTAURANT ORDER
    ----------------------------------------------------------
    */
    public EventHandler<Table> OnWaiterCalled;
    public EventHandler<string> OnPlayerFinishedSpeaking;
    public EventHandler<string> OnOpenAIResponseStay;
    public EventHandler<string> OnOpenAIResponseLeave;
}