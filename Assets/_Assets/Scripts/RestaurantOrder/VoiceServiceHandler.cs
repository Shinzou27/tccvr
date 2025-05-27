using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Requests;
using Meta.WitAi.Configuration;
using System;
using Oculus.VoiceSDK.UX;
using UnityEngine.InputSystem;

public class VoiceServiceHandler : MonoBehaviour
{
    [SerializeField] private VoiceService service;
    [SerializeField] private InputActionReference speakButton;
    private VoiceServiceRequest _request;
    public static VoiceServiceHandler Instance;

    private AudioClip recordedClip;
    private const int SampleRate = 16000;
    private const int MaxRecordingSeconds = 10;
    public bool IsRecording = false;
    public string transcription;
    private float timeSinceChanged = 0;
    private float cooldown = 3;
    private string prev = "";
    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    void Start()
    {
        service.VoiceEvents.OnPartialTranscription.AddListener(OnChange); // teoricamente n precisa desse
        service.VoiceEvents.OnFullTranscription.AddListener(OnChange);
    }
    void OnChange(string str) {
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
}
