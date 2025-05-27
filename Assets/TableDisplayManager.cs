using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableDisplayManager : MonoBehaviour
{
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
    [SerializeField] private TextMeshProUGUI label;
    public void SetLabel(string text)
    {
        label.text = text;
    }
}
