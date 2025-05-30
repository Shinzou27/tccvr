using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {
  [SerializeField] private PINInputController pinInputController;
  [SerializeField] private StudentListController studentListController;
  [SerializeField] private ChooseExperienceController chooseExperienceController;
  [SerializeField] private GameObject pinInputContainer;
  [SerializeField] private GameObject studentListContainer;
  [SerializeField] private GameObject chooseExperienceContainer;
  [SerializeField] private Toggle offline;
  public enum MenuState { PIN, LIST, EXPERIENCE }
  public MenuState current;
  public EnterExperienceParams enterExperienceParams;
  void Start()
  {
    ChangeToPIN();
    offline.onValueChanged.AddListener(ToggleNetwork);
    pinInputController.onProceedClick = ChangeToList;
    studentListController.onProceedClick = ChangeToExperience;
    chooseExperienceController.UpdateEnterExperienceParams = UpdateEnterExperienceParams;
  }

  private void ToggleNetwork(bool arg0)
  {
    Utils.isOffline = arg0;
    pinInputController.ChangeVisiblity(arg0);
  }

  private void UpdateEnterExperienceParams(EnterExperienceParams _params)
  {
    _params.pin = pinInputController.GetPIN();
    _params.studentId = studentListController.GetSelected().id;
  }

  private EnterExperienceParams GetParams()
  {
    return enterExperienceParams;
  }

  void Update()
  {
    pinInputContainer.SetActive(current == MenuState.PIN);
    studentListContainer.SetActive(current == MenuState.LIST);
    chooseExperienceContainer.SetActive(current == MenuState.EXPERIENCE);
  }
  public void ChangeToPIN() {
    current = MenuState.PIN;
  }
  public void ChangeToList() {
    current = MenuState.LIST;
  }
  public void ChangeToExperience() {
    current = MenuState.EXPERIENCE;
  }
}