using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemainingTimeController : MonoBehaviour
{
    [SerializeField] private Image circle;
    [SerializeField] private TextMeshProUGUI playerNumberDisplay;
    [SerializeField] private TextMeshProUGUI customerSpeaking;
    [SerializeField] private TextMeshProUGUI playerTranscription;
    private TentInfo info;
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponentInParent<TentInfo>();
        playerNumberDisplay.text = info.playerNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        circle.fillAmount = FruitShop.Instance.GetRemainingTime();
        customerSpeaking.text = FruitShop.Instance.IsCustomerTalking ? "Customer speaking. Wait they finish." : "";
        playerTranscription.text = VoiceServiceHandler.Instance.transcription;
    }
}
