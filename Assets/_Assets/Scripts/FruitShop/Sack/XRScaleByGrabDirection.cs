using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRScaleByGrabDirection : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public Collider leftHandCollider;
    public Collider rightHandCollider;
    public float scaleMultiplier = 1f;
    public float minScaleDistortion = 0.25f;
    public float maxScaleDistortion = 2f;

    private XRGrabInteractable grabInteractable;
    private Transform pointA;
    private Transform pointB;
    private Vector3 initialScale;
    private float defaultDistance;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        leftHandCollider = VRRigRereferences.Singleton.leftHand.GetComponent<Collider>();
        rightHandCollider = VRRigRereferences.Singleton.rightHand.GetComponent<Collider>();
        initialScale = transform.localScale;
        defaultDistance = Vector3.Distance(leftPoint.position, rightPoint.position);
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

        float yDiff = pointA.position.y - pointB.position.y;
        // Debug.Log(yDiff);
        float t = Mathf.InverseLerp(-defaultDistance, defaultDistance, yDiff);
        // Debug.Log(t);
        float scaleFactor = Mathf.Lerp(minScaleDistortion, maxScaleDistortion, t);
        // Debug.Log(scaleFactor);

        Vector3 newScale = new Vector3(initialScale.x * scaleFactor, initialScale.y, initialScale.z);
        transform.localScale = newScale;
    }

}
