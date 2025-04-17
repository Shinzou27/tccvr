using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class NetworkPlayer : NetworkBehaviour
{
  private int playerNumber;
  public Transform root;    
  public Transform leftHand;
  public Transform rightHand;
  public Renderer[] meshToDisable;
  [SerializeField] private TextMeshProUGUI debugPlayerNumberLabel;
  // public UnityEvent<string> OnNumberSet;
  public override void OnNetworkSpawn()
  {
    if (IsOwner) {
      playerNumber = Utils.GetPlayersActive();
      // OnNumberSet.Invoke(playerNumber.ToString());
      debugPlayerNumberLabel.text = playerNumber.ToString();
      foreach (var mesh in meshToDisable) {
          mesh.enabled = false;
      }
      EventManager.Instance.OnPlayerEnter?.Invoke(this, playerNumber);
      TendSpawner.Instance.RequestSpawnTent(playerNumber);
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
