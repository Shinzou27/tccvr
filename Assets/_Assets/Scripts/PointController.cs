using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PointController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    public int points;
    public void UpdateLabel()
    {
        label.text = points.ToString();
    }
}
