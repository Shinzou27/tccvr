using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TendSpawner : NetworkBehaviour
{
  [SerializeField] private List<GameObject> prefabs;
  public int players = 16;
  public static TendSpawner Instance;
  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public void RequestSpawnTent(int playerNumber)
  {
    if (IsServer)
    {
      SpawnTent(playerNumber);
    }
    else
    {
      SpawnTentServerRpc(playerNumber);
    }
  }

  [ServerRpc(RequireOwnership = false)]
  private void SpawnTentServerRpc(int playerNumber)
  {
    Debug.LogError("Sou client e quero instanciar uma tenda.");
    SpawnTent(playerNumber);
  }

  private void SpawnTent(int playerNumber)
  {
    Vector3 position = Utils.GetSpawnTransform(playerNumber, 16);
    Quaternion rotation = Quaternion.Euler(0, (playerNumber % 2 == 0) ? 0 : 180, 0);
    GameObject tent = Instantiate(prefabs[Random.Range(0, prefabs.Count)], position, rotation);
    NetworkObject networkObject = tent.GetComponent<NetworkObject>();
    networkObject.Spawn();
  }
}
