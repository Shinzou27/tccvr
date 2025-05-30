using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
public static class StoredNetworkData
{
  public static Allocation allocation;
  public static string joinCode;
  public static string lobbyId;
  private static readonly int maxConnection = 20;

  public static async Task CreateRelay()
  {
    Debug.LogError("Criando a sala, sou o primeiro a entrar");
    Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnection);
    string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    StoredNetworkData.allocation = allocation;
    StoredNetworkData.joinCode = joinCode;
    Debug.LogError(joinCode);
  }
  public static bool IsRoomCreated()
  {
    return allocation != null;
  }
}