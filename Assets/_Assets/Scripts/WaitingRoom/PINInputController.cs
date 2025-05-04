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
    public Action onProceedClick;
    private void Start() {
        proceedButton.interactable = false;
    }
    public void CheckLength(string input) {
        proceedButton.interactable = input.Length == 6;
    }
    public void SendRequest() {
        GetExperienceStudentList.Instance.StartRequest(pinInput.text, onProceedClick);
    }
    public string GetPIN() {
        return pinInput.text;
    }
}
