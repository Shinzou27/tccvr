using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using Unity.Netcode.Transports.UTP;
public class NetworkConnect : MonoBehaviour
{
  private readonly int maxConnection = 20;
  public UnityTransport transport;
  private Lobby currentLobby;
  private float heartBeatTimer;
  private async void Awake()
  {
    await UnityServices.InitializeAsync();
    await AuthenticationService.Instance.SignInAnonymouslyAsync();
    JoinOrCreate();
  }
  void Update()
  {
    if (heartBeatTimer > 15) {
      heartBeatTimer -= 15;
      if (currentLobby != null && currentLobby.HostId == AuthenticationService.Instance.PlayerId) {
        LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
      }
    }
    heartBeatTimer += Time.deltaTime;
  }
  public async void StartHost() {
    Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);
    string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    Debug.LogError(joinCode);
    transport.SetHostRelayData(
                                allocation.RelayServer.IpV4,
                                (ushort)allocation.RelayServer.Port,
                                allocation.AllocationIdBytes,
                                allocation.Key,
                                allocation.ConnectionData);
    CreateLobbyOptions createLobbyOptions = new()
    {
      IsPrivate = false,
      Data = new()
    };
    DataObject dataObject = new(DataObject.VisibilityOptions.Public, joinCode);
    createLobbyOptions.Data.Add("JOIN_CODE", dataObject);
    currentLobby = await Lobbies.Instance.CreateLobbyAsync("Lobby Test", maxConnection, createLobbyOptions);
    NetworkManager.Singleton.StartHost();
    // SceneManager.LoadScene("FruitShop");
  }
  public async void JoinOrCreate() {
    try {
      Debug.LogError("tentando entrar numa sessão");
      currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync();
      Debug.LogError(currentLobby.Data["JOIN_CODE"].Value);
      string relayJoinCode = currentLobby.Data["JOIN_CODE"].Value;
      JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);
      transport.SetClientRelayData(allocation.RelayServer.IpV4,
                                  (ushort)allocation.RelayServer.Port,
                                  allocation.AllocationIdBytes,
                                  allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);

      NetworkManager.Singleton.StartClient();
    } catch {
      StartHost();
    }
  }
  public async void StartClient() {
    currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync();
    string relayJoinCode = currentLobby.Data["JOIN_CODE"].Value;
    JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);
    transport.SetClientRelayData(allocation.RelayServer.IpV4,
                                (ushort)allocation.RelayServer.Port,
                                allocation.AllocationIdBytes,
                                allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);

    NetworkManager.Singleton.StartClient();
    // SceneManager.LoadScene("FruitShop");
  }
}
