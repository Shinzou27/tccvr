using System.Collections;
using System.Collections.Generic;
using Oculus.VoiceSDK.UX;
using UnityEngine;
using UnityEngine.UI;

public class GetTranscription : MonoBehaviour
{
    private float timeSinceChanged = 0;
    private float cooldown = 3;
    private string prev = "";
    void Update()
    {
        if (VoiceServiceHandler.Instance.IsRecording) {
            if (prev == VoiceServiceHandler.Instance.transcription) {
                timeSinceChanged += Time.deltaTime;
            } else {
                timeSinceChanged = 0;
                prev = VoiceServiceHandler.Instance.transcription;
            }
            if (timeSinceChanged > cooldown){
                Debug.Log($"Passaram-se {cooldown} segundos, então audio deu-se por encerrado.");
                timeSinceChanged = 0;
                Debug.Log(VoiceServiceHandler.Instance.transcription);
                VoiceServiceHandler.Instance.StopService();
                EventManager.Instance.OnPlayerFinishedSpeaking?.Invoke(this, VoiceServiceHandler.Instance.transcription);
            }
        }
    }
}
