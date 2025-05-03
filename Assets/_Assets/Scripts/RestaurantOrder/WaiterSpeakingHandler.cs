using System;
using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.TTS.Data;
using Meta.WitAi.TTS.Utilities;
using UnityEngine;

public class WaiterSpeakingHandler : MonoBehaviour
{
  [SerializeField] private TTSSpeaker speaker;
  public Action LeaveTableAction;
  public enum CurrentSpeakBehaviorType {ENTER, STAY, LEAVE}
  private CurrentSpeakBehaviorType currentSpeakBehaviorType;
  // private WaiterBehavior waiterBehavior;
  void Start()
  {
    // waiterBehavior = GetComponent<WaiterBehavior>();
    speaker.Events.OnPlaybackComplete.AddListener(HandleCompleteAudio);
  }

  private void HandleCompleteAudio(TTSSpeaker arg0, TTSClipData arg1)
  {
    //TODO: logica de mandar o audio pra requisicao
    Debug.Log("Terminou de falar");
    RestaurantOrder.Instance.audios.Add(arg1.clip);
    if (currentSpeakBehaviorType == CurrentSpeakBehaviorType.ENTER || currentSpeakBehaviorType == CurrentSpeakBehaviorType.STAY) {
      VoiceServiceHandler.Instance.StartService();
    } else if (currentSpeakBehaviorType == CurrentSpeakBehaviorType.LEAVE) {
      LeaveTableAction();
    }
  }

  public void SpeakEnter()
  {
    string dialogue = "";
    Debug.Log("A");
    switch (RestaurantOrder.Instance.GetOrderState()) {
      case RestaurantOrder.OrderState.GREETING:
        dialogue = "Hello! It's friday! Who did it, did it.";
        break;
      case RestaurantOrder.OrderState.ON_ORDER:
        dialogue = "Apologies for the wait. Here is you order.";
        break;
      case RestaurantOrder.OrderState.WAITING_PAYMENT:
        dialogue = "Ok, so... Here is your bill!";
        break;
    }
    // speaker.
    Speak(dialogue);
    RestaurantOrder.Instance.UpdatePrompt(new(dialogue, PromptMessage.Role.assistant));
    currentSpeakBehaviorType = CurrentSpeakBehaviorType.ENTER;
  }
  public void SpeakStay(string dialogue = "Hello! It's friday! Who did it, did it.")
  {
    Debug.Log("B");
    // speaker.
    Speak(dialogue);
    currentSpeakBehaviorType = CurrentSpeakBehaviorType.STAY;
  }
  public void SpeakLeave(string dialogue = "Hello! It's friday! Who did it, did it.")
  {
    Debug.Log("C");
    // speaker.
    Speak(dialogue);
    currentSpeakBehaviorType = CurrentSpeakBehaviorType.LEAVE;
  }
  public void Speak(string dialogue) {
    speaker.Speak(dialogue);
  }
}
