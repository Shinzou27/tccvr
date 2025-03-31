using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RayVisualToggler : MonoBehaviour
{
    [SerializeField] private GameObject leftRay;
    [SerializeField] private GameObject rightRay;
    [SerializeField] private InputActionReference leftRayInputAction;
    [SerializeField] private InputActionReference rightRayInputAction;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftRay.SetActive(leftRayInputAction.action.ReadValue<float>() > 0.1f);
        rightRay.SetActive(rightRayInputAction.action.ReadValue<float>() > 0.1f);
    }
}
