using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Requests;
using Meta.WitAi.Configuration;
using System;
using Oculus.VoiceSDK.UX;
using UnityEngine.InputSystem;
using Meta.WitAi.Json;
using System.Linq;
using UnityEngine.SceneManagement;

public class VoiceServiceHandler : MonoBehaviour
{
  [SerializeField] private VoiceService service;
  [SerializeField] private InputActionReference speakButton;
  private VoiceServiceRequest _request;
  public static VoiceServiceHandler Instance;

  public bool IsRecording = false;
  public string transcription;
  private float timeSinceChanged = 0;
  public Action OnRepeatRequest;
  public Action OnApology;
  // private AudioClip recordedClip;
  // private const int SampleRate = 16000;
  // private const int MaxRecordingSeconds = 10;
  // private float cooldown = 3;
  // private string prev = "";
  void Awake()
  {
    if (Instance == null) Instance = this;
  }
  void Start()
  {
    service.VoiceEvents.OnPartialTranscription.AddListener(OnChange); // teoricamente n precisa desse
    service.VoiceEvents.OnFullTranscription.AddListener(OnChange);
    service.VoiceEvents.OnResponse.AddListener(ParseResponse);
  }
  void OnDestroy()
  {
    Instance = null;
    service.VoiceEvents.OnPartialTranscription.RemoveListener(OnChange);
    service.VoiceEvents.OnFullTranscription.RemoveListener(OnChange);
    service.VoiceEvents.OnResponse.RemoveListener(ParseResponse);
  }

  private void ParseResponse(WitResponseNode arg0)
  {
    if (FoodOrderIntentHandler.Instance != null)
    { //TODO: melhorar essa verificacao depois
      FoodOrderIntentHandler.Instance.HandleSpawn(arg0.GetAllEntityValues("food:food").Distinct().ToArray());
    }
    else
    {
      string[] apologiesEntities = arg0.GetAllEntityValues("apologies:apologies").Distinct().ToArray();
      string[] repeatEntities = arg0.GetAllEntityValues("repeat:repeat").Distinct().ToArray();
      if (repeatEntities.Length > 0)
      {
        OnRepeatRequest.Invoke();
      }
      else if (apologiesEntities.Length > 0)
      {
        OnApology.Invoke();
      }
    }
    
  }

  void OnChange(string str)
  {
    Debug.Log(str);
    transcription = str;
  }
  public void StartService()
  {
    Debug.Log("iniciando gravacao");
    _request = service.Activate(new WitRequestOptions(), new VoiceServiceRequestEvents());
    // recordedClip = Microphone.Start(null, false, MaxRecordingSeconds, SampleRate);
    IsRecording = true;
    RestaurantOrder.Instance.UpdateSpeakState(RestaurantOrder.SpeakState.PLAYER_SPEAKING);
  }

  public void StopService()
  {
    RestaurantOrder.Instance.UpdateSpeakState(RestaurantOrder.SpeakState.WAITING_WAITER);
    EventManager.Instance.OnPlayerFinishedSpeaking?.Invoke(this, transcription);
    Debug.Log("terminando gravacao");
    IsRecording = false;
    if (_request != null)
    {
      _request.DeactivateAudio();
    }
    else
    {
      service.Deactivate();
    }

    // if (Microphone.IsRecording(null))
    // {
    //     Microphone.End(null);
    //     RestaurantOrder.Instance.audios.Add(recordedClip);
    // }
    transcription = "";
  }
  void Update()
  {
    if (SceneManager.GetActiveScene().name == "RestaurantOrder")
    {
      CheckMicOnRestaurant();
    }
    else if (SceneManager.GetActiveScene().name == "FruitShop")
    {
      CheckMicOnFruitShop();
    }
  }
  public void ResetTimeSinceChanged()
  {
    timeSinceChanged = 0f;
  }
  public void CheckMicOnRestaurant()
  {
    if (RestaurantOrder.Instance.GetSpeakState() == RestaurantOrder.SpeakState.PLAYER_CAN_SPEAK || RestaurantOrder.Instance.GetSpeakState() == RestaurantOrder.SpeakState.PLAYER_SPEAKING)
    {
      timeSinceChanged += Time.deltaTime;
      if (timeSinceChanged >= 5f)
      {
        TableDisplayManager.Instance.SetLabel("Dica: tente pedir um refrigerante!");
      }
      if (!IsRecording && speakButton.action.ReadValue<float>() >= 0.99f)
      {
        StartService();
      }
      else if (IsRecording && speakButton.action.ReadValue<float>() <= 0.001f)
      {
        StopService();
      }
    }
  }
  public void CheckMicOnFruitShop()
  {
    if (!FruitShop.Instance.IsCustomerTalking)
    {
      if (!IsRecording && speakButton.action.ReadValue<float>() >= 0.99f)
      {
        StartService();
      }
      else if (IsRecording && speakButton.action.ReadValue<float>() <= 0.001f)
      {
        StopService();
      }
    }
  }
}
