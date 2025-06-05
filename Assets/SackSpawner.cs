using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SackSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private InputActionProperty gripL;
    [SerializeField] private InputActionProperty gripR;
    [SerializeField] private TentInfo tent;

    private bool pressed = false;
    private InputActionProperty pressedInput;
    private GameObject instantiated;
    private XRDirectInteractor handWhoInstantiated;
    private void OnTriggerStay(Collider other)
    {
        if (instantiated != null) return;
        if (!pressed && gripL.action.ReadValue<float>() > 0.99f)
        {
            GameObject sack = Instantiate(prefab);
            sack.transform.localScale *= 0.5f;
            instantiated = sack;
            sack.GetComponentInChildren<OnFruitPlaceHandler>().SetTent(tent);
            pressed = true;
            pressedInput = gripL;
            handWhoInstantiated = other.GetComponent<XRDirectInteractor>();
            handWhoInstantiated.interactionManager.SelectEnter(other.GetComponent<XRDirectInteractor>(), sack.GetComponent<XRGrabInteractable>());
        }
        else if (!pressed && gripR.action.ReadValue<float>() > 0.99f)
        {
            GameObject sack = Instantiate(prefab);
            sack.transform.localScale *= 0.5f;
            instantiated = sack;
            sack.GetComponentInChildren<OnFruitPlaceHandler>().SetTent(tent);
            pressed = true;
            pressedInput = gripR;
            handWhoInstantiated = other.GetComponent<XRDirectInteractor>();
            handWhoInstantiated.interactionManager.SelectEnter(other.GetComponent<XRDirectInteractor>(), sack.GetComponent<XRGrabInteractable>());
        }
    }
  void Update()
  {
        if (pressed && pressedInput.action.ReadValue<float>() <= 0.01f)
        {
            pressed = false;
            instantiated = null;
            handWhoInstantiated = null;
            handWhoInstantiated.interactionManager.SelectExit(handWhoInstantiated, instantiated.GetComponent<XRGrabInteractable>());
        }

  }
}
