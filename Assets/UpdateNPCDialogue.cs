using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateNPCDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogue;
    void Start()
    {
        EventManager.Instance.OnOrderCreated += UpdateUI;
    }
    void UpdateUI(object sender, EventArgs e) {
        dialogue.text = $"I would like {Utils.FormatFruitList(FruitShop.Instance.currentOrder.list)}.";
    }
}
