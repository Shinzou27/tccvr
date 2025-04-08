using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public Transform root;    
    public Transform leftHand;
    public Transform rightHand;
    public Renderer[] meshToDisable;
  public override void OnNetworkSpawn()
  {
    if (IsOwner) {
        foreach (var mesh in meshToDisable) {
            mesh.enabled = false;
        }
    }
  }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner) {
            root.SetPositionAndRotation(VRRigRereferences.Singleton.root.position, VRRigRereferences.Singleton.root.rotation);
            leftHand.SetPositionAndRotation(VRRigRereferences.Singleton.leftHand.position, VRRigRereferences.Singleton.leftHand.rotation);
            rightHand.SetPositionAndRotation(VRRigRereferences.Singleton.rightHand.position, VRRigRereferences.Singleton.rightHand.rotation);
        }
  }
}
