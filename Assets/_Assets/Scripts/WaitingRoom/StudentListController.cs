using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentListController : MonoBehaviour {
  [SerializeField] private Button redButton;
  [SerializeField] private Button greenButton;
  [SerializeField] private ExperienceInfo experienceInfo;
  [SerializeField] private Transform gridContent;
  [SerializeField] private GameObject stCardPrefab;
  private Student selected;
  public Action onProceedClick;
  public List<GameObject> instantiatedCards;
  private void OnEnable() {
    redButton.onClick.AddListener(GoBack);
    greenButton.onClick.AddListener(GoForth);
    InstantiateStudentCards();
    greenButton.interactable = false;
  }
  private void OnDisable() {
    redButton.onClick.RemoveListener(GoBack);
    greenButton.onClick.RemoveListener(GoForth);
  }
  public void GoBack() {

  }
  public void GoForth() {
    onProceedClick.Invoke();
  }
  public void InstantiateStudentCards() {
    foreach (GameObject children in instantiatedCards) {
      Destroy(children);
    }
    instantiatedCards.Clear();
    foreach (Student student in experienceInfo.response.students) {
      GameObject stCard = Instantiate(stCardPrefab, gridContent);
      stCard.GetComponent<StudentCardController>().OnCardClick = () => SetSelected(student);
      stCard.GetComponent<StudentCardController>().SetStudent(student);
    }
  }
  public void SetSelected(Student student) {
    selected = student;
    greenButton.interactable = true;
  }
  public Student GetSelected() {
    return selected;
  }
}