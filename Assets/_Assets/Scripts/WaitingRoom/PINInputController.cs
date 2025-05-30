using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PINInputController : MonoBehaviour
{
    [SerializeField] private TMP_InputField pinInput;
    [SerializeField] private Button proceedButton;
    // [SerializeField] private TextMeshProUGUI errorLog;
    public Action onProceedClick;
    private void Start()
    {
        proceedButton.interactable = false;
    }
    public void CheckLength(string input)
    {
        proceedButton.interactable = input.Length == 6;
    }
    public void SendRequest()
    {
        Utils.OnlineHandle(() => GetExperienceStudentList.Instance.StartRequest(pinInput.text, onProceedClick, error => Debug.LogError(error)));
        Utils.OfflineHandle(onProceedClick);
    }
    public string GetPIN()
    {
        return pinInput.text;
    }
    public void ChangeVisiblity(bool toggleValue)
    {
        proceedButton.interactable = toggleValue;
        pinInput.interactable = !toggleValue;
    }
}
