using System.Collections;
using System.Collections.Generic;
using Meta.WitAi.TTS.Utilities;
using Oculus.VoiceSDK.UX;
using UnityEngine;
using UnityEngine.UI;

public class GetTranscription : MonoBehaviour
{
    [SerializeField] private VoiceTranscriptionLabel voiceTranscriptionLabel;
    [SerializeField] private TTSSpeaker speaker;
    private float timeSinceChanged = 0;
    private float cooldown = 5;
    private string prev = "";
    public bool isListening = false;
    [SerializeField] private Button activateButton;
    void Update()
    {
        if (isListening) {
            if (prev == voiceTranscriptionLabel.Label.text) {
                timeSinceChanged += Time.deltaTime;
            } else {
                timeSinceChanged = 0;
                prev = voiceTranscriptionLabel.Label.text;
            }
            if (timeSinceChanged > cooldown){
                Debug.Log(voiceTranscriptionLabel.Label.text);
                activateButton.onClick.Invoke();
                DebugTTS();
            }
        }
    }
    public void OnClick() {
        isListening = !isListening;
    }
    private void DebugTTS() {
        // speaker.Speak("Hello! What can I do for you today?");
        Debug.Log("AAA");
        speaker.Speak("Hello! It's friday! Who did, did.");
    }
}
