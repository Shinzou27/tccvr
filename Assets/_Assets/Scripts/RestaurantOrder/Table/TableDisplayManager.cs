using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableDisplayManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TextMeshProUGUI orderState;
    [SerializeField] private TextMeshProUGUI playerTranscription;
    [SerializeField] private Image waiter;
    [SerializeField] private Sprite waiterRed;
    [SerializeField] private Sprite waiterGreen;
    public static TableDisplayManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void SetLabel(string text)
    {
        label.text = text;
    }
    public void SetOrderState(string text)
    {
        orderState.text = text;
    }
    public void SetTranscription(string text)
    {
        playerTranscription.text = text;
    }
    public void SetWaiterToRed()
    {
        waiter.sprite = waiterRed;
    }
    public void SetWaiterToGreen()
    {
        waiter.sprite = waiterGreen;
    }
}
