using System;
using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.TTS.Data;
using Meta.WitAi.TTS.Utilities;
using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private TTSSpeaker speaker;
    private Order order;
    string fruitListDialogue;
    void Start()
    {
        EventManager.Instance.OnOrderCreated += Greet;
        speaker.Events.OnPlaybackStart.AddListener(HandleStartAudio);
        speaker.Events.OnPlaybackComplete.AddListener(HandleCompleteAudio);

    }

    private void HandleStartAudio(TTSSpeaker arg0, TTSClipData arg1)
    {
        FruitShop.Instance.IsCustomerTalking = true;
    }
    private void HandleCompleteAudio(TTSSpeaker arg0, TTSClipData arg1)
    {
        FruitShop.Instance.IsCustomerTalking = false;
    }


    void OnDestroy()
    {
        EventManager.Instance.OnOrderCreated -= Greet;
        speaker.Events.OnPlaybackStart.RemoveListener(HandleStartAudio);
        speaker.Events.OnPlaybackComplete.RemoveListener(HandleCompleteAudio);
    }
    void Greet(object sender, GameObject e)
    {
        if (e == gameObject)
        {
            Debug.Log("A");
            order = GetComponent<NPCFruitOrderController>().order;
            order.DebugOrder();
            fruitListDialogue = Utils.FormatFruitList(order.list);
            speaker.Speak($"Hello! Can you help me gather some fruits? I would like {fruitListDialogue}.");
        }
    }
    public void LeaveCorrect()
    {
        speaker.Speak($"Thank you very much!");
    }
    public void LeaveNoFruit()
    {
        speaker.Speak($"Oh, it's fine, no need to apology. See you next time!");
    }
    public void LeaveIncorrect()
    {
        speaker.Speak($"Humm, that wasn't what I wanted. I appreciate your offer, but I am going to another tent.");
    }
    public void Repeat()
    {
        speaker.Speak($"Oh, okay. I want {fruitListDialogue}");
    }
}
