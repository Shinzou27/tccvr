using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Requests;
using Meta.WitAi.Configuration;
using System;
using Oculus.VoiceSDK.UX;

public class VoiceServiceHandler : MonoBehaviour
{
    [SerializeField] private VoiceService service;
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
    }

    public void StopService()
    {
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
        if (IsRecording) {
            if (prev == transcription) {
                timeSinceChanged += Time.deltaTime;
            } else {
                timeSinceChanged = 0;
                prev = transcription;
            }
            if (timeSinceChanged > cooldown){
                Debug.Log($"Passaram-se {cooldown} segundos, então audio deu-se por encerrado.");
                timeSinceChanged = 0;
                Debug.Log(transcription);
                if (transcription != "") {
                    EventManager.Instance.OnPlayerFinishedSpeaking?.Invoke(this, transcription);
                    StopService();
                }
            }
        }
    }
}
