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
    speaker.Events.OnPlaybackComplete.AddListener(HandleCompleteAudio);
  }

  private void HandleCompleteAudio(TTSSpeaker arg0, TTSClipData arg1)
  {
    //TODO: logica de mandar o audio pra requisicao
    Debug.Log("Terminou de falar");
    RestaurantOrder.Instance.audios.Add(arg1.clip);
    if (currentSpeakBehaviorType == CurrentSpeakBehaviorType.ENTER) {
      VoiceServiceHandler.Instance.StartService();
    } else if (currentSpeakBehaviorType == CurrentSpeakBehaviorType.LEAVE) {
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
    speaker.Speak(dialogue);
  }
}
