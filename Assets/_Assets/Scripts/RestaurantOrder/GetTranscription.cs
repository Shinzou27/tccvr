using System.Collections;
using System.Collections.Generic;
using Oculus.VoiceSDK.UX;
using UnityEngine;
using UnityEngine.UI;

public class GetTranscription : MonoBehaviour
{
    [SerializeField] private VoiceTranscriptionLabel voiceTranscriptionLabel;
    private float timeSinceChanged = 0;
    private float cooldown = 3;
    private string prev = "";
    public bool isListening = false;
    [SerializeField] private Button activateButton;
    void Update()
    {
        isListening = voiceTranscriptionLabel.Label.text != "Press activate to begin listening";
        if (isListening) {
            if (prev == voiceTranscriptionLabel.Label.text) {
                timeSinceChanged += Time.deltaTime;
            } else {
                timeSinceChanged = 0;
                prev = voiceTranscriptionLabel.Label.text;
            }
            if (timeSinceChanged > cooldown){
                timeSinceChanged = 0;
                Debug.Log(voiceTranscriptionLabel.Label.text);
                OpenAIRequest.Perform(voiceTranscriptionLabel.Label.text);
                activateButton.onClick.Invoke();
                EventManager.Instance.OnWaiterCalled?.Invoke(this, RestaurantOrder.Instance.GetTableById(2));
            }
        }
    }
}
