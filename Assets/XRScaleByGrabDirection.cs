using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRScaleByGrabDirection : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform targetToScale;
    public Collider leftHandCollider;
    public Collider rightHandCollider;
    public float scaleMultiplier = 1f;

    private XRGrabInteractable grabInteractable;
    private Transform pointA;
    private Transform pointB;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        Collider interactorCollider = args.interactorObject.transform.GetComponentInChildren<Collider>();

        if (interactorCollider == leftHandCollider)
        {
            pointA = leftPoint;
            pointB = rightPoint;
        }
        else if (interactorCollider == rightHandCollider)
        {
            pointA = rightPoint;
            pointB = leftPoint;
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        pointA = null;
        pointB = null;
    }

    void Update()
    {
        if (pointA == null || pointB == null)
            return;

        float yDiff = pointB.position.y - pointA.position.y;
        Vector3 newScale = targetToScale.localScale;
        newScale.y = Mathf.Max(0.01f, newScale.y + (yDiff * scaleMultiplier * Time.deltaTime));
        targetToScale.localScale = newScale;
    }
}
