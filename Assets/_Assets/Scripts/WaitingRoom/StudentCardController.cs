using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class StudentCardController : MonoBehaviour {
  public Action OnCardClick;
  [SerializeField] private TextMeshProUGUI stName;
  [SerializeField] private Button checkbox;
  private Student student;
  public void SetStudent(Student student) {
    this.student = student;
    stName.text = student.name;
  }
  void Start()
  {
    checkbox.onClick.AddListener(() => OnCardClick.Invoke());
  }
}