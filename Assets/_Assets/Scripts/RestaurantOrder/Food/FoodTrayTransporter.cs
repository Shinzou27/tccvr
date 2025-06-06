using System;
using UnityEngine;

public class FoodTrayTransporter : MonoBehaviour
{
  public Transform destination;
  private void Start()
  {
    EventManager.Instance.OnDropFoodOnTable += SetPos;
  }
  private void OnDestroy()
  {
    EventManager.Instance.OnDropFoodOnTable -= SetPos;
  }
  public void SetPos(object sender, EventArgs e)
  {
    Quaternion rotationBeforeTransport = transform.localRotation;
    transform.SetParent(destination);
    transform.SetLocalPositionAndRotation(Vector3.zero, rotationBeforeTransport);
    if (destination.TryGetComponent(out MeshRenderer renderer))
    {
      renderer.enabled = !GetComponent<FoodInfo>().NeedToHidePlate();
    }
  }
}