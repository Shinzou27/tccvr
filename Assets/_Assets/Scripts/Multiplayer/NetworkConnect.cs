using Unity.Netcode;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode.Transports.UTP;

public class NetworkConnect : MonoBehaviour
{
  // private readonly int maxConnection = 20;
  public UnityTransport transport;

  private async void Start()
  {
    await UnityServices.InitializeAsync();
    if (!AuthenticationService.Instance.IsSignedIn)
    {
      await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    JoinOrCreate();
  }

  public async void JoinOrCreate()
  {
    Debug.LogError($"Sou host? {Utils.isHost}");
    if (Utils.isHost)
    {
      StartHost();
    }
    else
    {
      await StartClient();
    }
  }

  public void StartHost()
  {
    Debug.LogError("StartHost");
    Debug.Log(StoredNetworkData.allocation);
    Debug.Log(StoredNetworkData.joinCode);

    transport.SetHostRelayData(
      StoredNetworkData.allocation.RelayServer.IpV4,
      (ushort)StoredNetworkData.allocation.RelayServer.Port,
      StoredNetworkData.allocation.AllocationIdBytes,
      StoredNetworkData.allocation.Key,
      StoredNetworkData.allocation.ConnectionData
    );

    NetworkManager.Singleton.StartHost();
  }

  public async Task StartClient()
  {
    Debug.LogError("StartClient");

    JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(StoredNetworkData.joinCode);

    transport.SetClientRelayData(
      allocation.RelayServer.IpV4,
      (ushort)allocation.RelayServer.Port,
      allocation.AllocationIdBytes,
      allocation.Key,
      allocation.ConnectionData,
      allocation.HostConnectionData
    );

    NetworkManager.Singleton.StartClient();
  }
}
