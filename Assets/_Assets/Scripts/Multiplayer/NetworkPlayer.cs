using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class NetworkPlayer : NetworkBehaviour
{
  public Transform root;
  public Transform leftHand;
  public Transform rightHand;
  public Renderer[] meshToDisable;

  // Update is called once per frame
  void Update()
  {
    if (IsOwner)
    {
      root.SetPositionAndRotation(VRRigRereferences.Singleton.root.position, VRRigRereferences.Singleton.root.rotation);
      leftHand.SetPositionAndRotation(VRRigRereferences.Singleton.leftHand.position, VRRigRereferences.Singleton.leftHand.rotation);
      rightHand.SetPositionAndRotation(VRRigRereferences.Singleton.rightHand.position, VRRigRereferences.Singleton.rightHand.rotation);
    }
  }
  public void InitialSetup()
  {
    if (IsOwner)
    {
      foreach (var mesh in meshToDisable)
      {
        mesh.enabled = false;
      }
      VRRigRereferences rig = VRRigRereferences.Singleton;
      rig.leftHand.GetComponent<InteractorReference>().linkedPlayer = this;
      rig.rightHand.GetComponent<InteractorReference>().linkedPlayer = this;
    }
  }
}
