using System;
using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.Speech;
using Meta.WitAi.TTS.Data;
using Meta.WitAi.TTS.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class WaiterSpeakingHandler : MonoBehaviour
{
  [SerializeField] private TTSSpeaker speaker;
  public Action LeaveTableAction;
  public enum CurrentSpeakBehaviorType { ENTER, STAY, LEAVE }
  private CurrentSpeakBehaviorType currentSpeakBehaviorType;
  [SerializeField] private float waiterReturnTime;
  // private WaiterBehavior waiterBehavior;
  void Start()
  {
    // waiterBehavior = GetComponent<WaiterBehavior>();
    // speaker.Events.OnPlaybackStart.AddListener(HandleStartAudio);
    // speaker.Events.OnPlaybackComplete.AddListener(HandleCompleteAudio);
  }

  public void HandleStartAudio(TTSSpeaker arg0, TTSClipData arg1)
  {
    Debug.Log("Garçom começou a hablar");
  }

  public void HandleCompleteAudio(TTSSpeaker arg0, TTSClipData arg1)
  {
    //TODO: logica de mandar o audio pra requisicao
    Debug.Log("Terminou de falar");
    RestaurantOrder.Instance.audios.Add(arg1.clip);
    switch (currentSpeakBehaviorType)
    {
      case CurrentSpeakBehaviorType.ENTER:
        if (RestaurantOrder.Instance.GetOrderState() == RestaurantOrder.OrderState.ON_ORDER)
        {
          Debug.Log("Botando a comida no prato");
          EventManager.Instance.OnDropFoodOnTable?.Invoke(this, EventArgs.Empty);    
        }
        RestaurantOrder.Instance.UpdateSpeakState(RestaurantOrder.SpeakState.PLAYER_CAN_SPEAK);
        VoiceServiceHandler.Instance.ResetTimeSinceChanged();
        break;
      case CurrentSpeakBehaviorType.STAY:
        RestaurantOrder.Instance.UpdateSpeakState(RestaurantOrder.SpeakState.PLAYER_CAN_SPEAK);
        VoiceServiceHandler.Instance.ResetTimeSinceChanged();
        break;
      case CurrentSpeakBehaviorType.LEAVE:
        StartCoroutine(WaitToUpdateDisplay());
        LeaveTableAction();
        break;
      default:
        break;
    }
  }

  public void SpeakEnter()
  {
    string dialogue = "";
    Debug.Log("A");
    switch (RestaurantOrder.Instance.GetOrderState())
    {
      case RestaurantOrder.OrderState.GREETING:
        dialogue = "Hello! Did you already check the menu tonight?";
        break;
      case RestaurantOrder.OrderState.ON_ORDER:
        dialogue = "Apologies for the wait. Here is you order. Do you want anything more?";
        break;
      case RestaurantOrder.OrderState.WAITING_PAYMENT:
        string bill = RestaurantOrder.Instance.GetBillDetails();
        dialogue = "Ok, so, here is your bill. " + bill;
        break;
    }
    // speaker.
    Speak(dialogue);
    RestaurantOrder.Instance.UpdatePrompt(new(dialogue, PromptMessage.Role.assistant));
    currentSpeakBehaviorType = CurrentSpeakBehaviorType.ENTER;
  }
  public void SpeakStay(string dialogue = "Hello! Did you already check the menu tonight?")
  {
    Debug.Log("B");
    // speaker.
    Speak(dialogue);
    currentSpeakBehaviorType = CurrentSpeakBehaviorType.STAY;
  }
  public void SpeakLeave(string dialogue = "Hello! Did you already check the menu tonight?")
  {
    Debug.Log("C");
    // speaker.
    Speak(dialogue);
    currentSpeakBehaviorType = CurrentSpeakBehaviorType.LEAVE;
  }
  public void Speak(string dialogue)
  {
    speaker.Speak(dialogue);
    RestaurantOrder.Instance.UpdateSpeakState(RestaurantOrder.SpeakState.WAITER_SPEAKING);
  }
  public IEnumerator WaitToUpdateDisplay()
  {
    float elapsed = 0;
    TableDisplayManager.Instance.SetLabel("Aguarde o garçom voltar à cozinha.");
    TableDisplayManager.Instance.SetWaiterToRed();
    while (elapsed < waiterReturnTime)
    {
      elapsed += Time.deltaTime;
      yield return null;
    }
    TableDisplayManager.Instance.SetWaiterToGreen();
    RestaurantOrder.Instance.UpdateSpeakState(RestaurantOrder.SpeakState.NONE);

  }
  public void GetAudioLength(AudioClip clip)
  {
    Debug.Log(clip.length);
  }
}
