using System;
using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.TTS.Utilities;
using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private TTSSpeaker speaker;
    private Order order;
    void Start()
    {
        EventManager.Instance.OnOrderCreated += Greet;
    }
    void OnDestroy()
    {
        EventManager.Instance.OnOrderCreated -= Greet;
    }
    void Greet(object sender, GameObject e) {
        if (e == gameObject) {
            Debug.Log("A");
            order = GetComponent<NPCFruitOrderController>().order;
            order.DebugOrder();
            speaker.Speak($"Hello! Can you help me gather some fruits? I would like {Utils.FormatFruitList(order.list)}.");
        }
    }
    public void LeaveCorrect() {
        speaker.Speak($"Thank you very much!");
    }
    public void LeaveIncorrect() {
        speaker.Speak($"Humm, that wasn't what I wanted. I appreciate your offer, but I am going to another tent.");
    }
}
