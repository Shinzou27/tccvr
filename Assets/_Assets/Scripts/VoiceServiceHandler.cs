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
using Meta.WitAi.Composer.Integrations;

public class VoiceServiceHandler : MonoBehaviour
{
  [SerializeField] private VoiceService service;
  [SerializeField] private InputActionReference speakButton;
  private VoiceServiceRequest _request;
  public static VoiceServiceHandler Instance;

  public bool IsRecording = false;
  public string transcription;
  private float timeSinceChanged = 0;
  public Action<int> OnRepeatRequest;
  public Action<int> OnApology;
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
    if (RestaurantOrder.Instance != null)
    { //TODO: melhorar essa verificacao depois
      Debug.Log(arg0.GetResponseText());
      FoodOrderIntentHandler.Instance.HandleSpawn(arg0.GetAllEntityValues("food:food").Distinct().ToArray());
    } else {
      string[] apologiesEntities = arg0.GetAllEntityValues("apologies:apologies").Distinct().ToArray();
      string[] repeatEntities = arg0.GetAllEntityValues("repeat:repeat").Distinct().ToArray();
      Debug.Log("bbb");
      Debug.Log("apologiesEntities: " + apologiesEntities.Length);
      Debug.Log("repeatEntities: " + repeatEntities.Length);
      if (repeatEntities.Length > 0)
      {
        Debug.Log("OnRepeatRequest.Invoke");
        Debug.Log(VRRigRereferences.Singleton.playerNumber);
        OnRepeatRequest.Invoke(VRRigRereferences.Singleton.playerNumber);
      }
      else if (apologiesEntities.Length > 0)
      {
        Debug.Log("OnApology.Invoke");
        Debug.Log(VRRigRereferences.Singleton.playerNumber);
        OnApology.Invoke(VRRigRereferences.Singleton.playerNumber);
      }
    }
    
  }

  void OnChange(string str)
  {
    transcription = str;
    if (RestaurantOrder.Instance != null)
    {
      TableDisplayManager.Instance.SetTranscription(str);
    }
  }
  public void StartService()
  {
    Debug.Log("iniciando gravacao");
    _request = service.Activate(new WitRequestOptions(), new VoiceServiceRequestEvents());
    // recordedClip = Microphone.Start(null, false, MaxRecordingSeconds, SampleRate);
    IsRecording = true;
    if (RestaurantOrder.Instance != null)
    {
      RestaurantOrder.Instance.UpdateSpeakState(RestaurantOrder.SpeakState.PLAYER_SPEAKING);
    }
  }

  public void StopService()
  {
    if (RestaurantOrder.Instance != null)
    {
      RestaurantOrder.Instance.UpdateSpeakState(RestaurantOrder.SpeakState.WAITING_WAITER);
      EventManager.Instance.OnPlayerFinishedSpeaking?.Invoke(this, transcription);
    }
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
    if (FruitShop.Instance.PlayerCanSpeak)
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
