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
  public enum CurrentSpeakBehaviorType {ENTER, STAYING_STILL, LEAVE}
  private CurrentSpeakBehaviorType currentSpeakBehaviorType;
  // private WaiterBehavior waiterBehavior;
  void Start()
  {
    // waiterBehavior = GetComponent<WaiterBehavior>();
    speaker.Events.OnPlaybackComplete.AddListener(LeaveTable);
  }

  private void LeaveTable(TTSSpeaker arg0, TTSClipData arg1)
  {
    //TODO: logica de mandar o audio pra requisicao
    Debug.Log("Terminou de falar");
    if (currentSpeakBehaviorType == CurrentSpeakBehaviorType.LEAVE) {
      LeaveTableAction();
    }
  }

  public void SpeakEnter(string dialogue = "Hello! It's friday! Who did it, did it.")
  {
    Debug.Log("A");
    // speaker.
    Speak(dialogue);
    currentSpeakBehaviorType = CurrentSpeakBehaviorType.ENTER;
  }
  public void SpeakStayingStill(string dialogue = "Hello! It's friday! Who did it, did it.")
  {
    Debug.Log("B");
    // speaker.
    Speak(dialogue);
    currentSpeakBehaviorType = CurrentSpeakBehaviorType.STAYING_STILL;
  }
  public void SpeakLeave(string dialogue = "Hello! It's friday! Who did it, did it.")
  {
    Debug.Log("C");
    // speaker.
    Speak(dialogue);
    currentSpeakBehaviorType = CurrentSpeakBehaviorType.LEAVE;
  }
  public void Speak(string dialogue) {
    // PromptMessage newMessage = new(latestUserSpeech, PromptMessage.Role.ASSISTANT);
    // RestaurantOrder.Instance.UpdatePrompt(newMessage);
    speaker.Speak(dialogue);
  }
}
