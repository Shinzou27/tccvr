using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardHandler : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private Button backspace;
    [SerializeField] private TMP_InputField input;
    void Start()
    {
        backspace.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(input.text))
            {
                input.text = input.text[..^1];
            }
        });
    }
    public void Write(string digit) {
        input.text += digit;
    }
}
