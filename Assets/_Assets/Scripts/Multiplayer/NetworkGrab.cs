using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkedGrab : NetworkBehaviour
{
    XRGrabInteractable grab;
    NetworkTransformClient netTransform;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        netTransform = GetComponent<NetworkTransformClient>();

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnExitGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (!args.interactorObject.transform.TryGetComponent<InteractorReference>(out var interactorRef)) return;

        var netPlayer = interactorRef.linkedPlayer;
        if (netPlayer != null && netPlayer.IsOwner)
        {
            RequestOwnershipServerRpc(netPlayer.OwnerClientId);
        }
    }


    void OnExitGrab(SelectExitEventArgs args)
    {
        Debug.Log($"acabei de dar SelectExit");
        EnableNetworkTransformServerRpc(true);
    }

    [ServerRpc(RequireOwnership = false)]
    void RequestOwnershipServerRpc(ulong clientId)
    {
        var netObj = GetComponent<NetworkObject>();
        netObj.ChangeOwnership(clientId);
        EnableNetworkTransformClientRpc(false);
        Debug.Log($"Dono desse GameObject: {netObj.OwnerClientId}");
    }

    [ClientRpc]
    void EnableNetworkTransformClientRpc(bool enabled)
    {
        if (NetworkManager.Singleton.LocalClientId == GetComponent<NetworkObject>().OwnerClientId)
        {
            netTransform.enabled = enabled;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void EnableNetworkTransformServerRpc(bool enabled)
    {
        EnableNetworkTransformClientRpc(enabled);
    }
}
