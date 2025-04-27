using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonFollowVisual : MonoBehaviour
{
    private XRBaseInteractable interactable;
    private bool isFollowing = false;
    private bool freeze = false;
    public Transform visualTarget;
    private Transform pokeAttachTransform;
    private Vector3 offset;
    public Vector3 localAxis;
    public Vector3 initialLocalPos;
    public float resetSpeed = 5;
    public float followAngleThreshold = 45;
    void Start()
    {
        initialLocalPos = visualTarget.localPosition;
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(ResetPos);
        interactable.selectEntered.AddListener(Freeze);
    }
    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;
            float pokeAngle = Vector3.Angle(offset, visualTarget.TransformDirection(localAxis));
            if (pokeAngle < followAngleThreshold)
            isFollowing = true;
            freeze = false;
        }
    }
    public void Freeze(BaseInteractionEventArgs args) {
        if (args.interactorObject is XRPokeInteractor) {
            freeze = true;
            EventManager.Instance.OnWaiterShouldMove?.Invoke(this, new() {
                destination = transform,
                waiting = false
            });
        }
    }
    public void ResetPos(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor) {
            isFollowing = false;
            freeze = false;
        }
    }
    void Update()
    {
        if (freeze) return;
        if (isFollowing)
        {
            Vector3 localTargetPos = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constrainedLocalTargetPos = Vector3.Project(localTargetPos, localAxis);
            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPos);
        } else {
            visualTarget.localPosition = Vector3.Lerp(visualTarget.localPosition, initialLocalPos, Time.deltaTime * resetSpeed);
        }
    }
}