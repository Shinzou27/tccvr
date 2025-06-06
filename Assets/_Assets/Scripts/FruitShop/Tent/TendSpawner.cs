using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TendSpawner : NetworkBehaviour
{
  [SerializeField] private List<GameObject> prefabs;
  private List<GameObject> spawned;
  public int players = 16;
  public static TendSpawner Instance;
  void Awake()
  {
    spawned = new List<GameObject>();
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
    StartCoroutine(WaitToSpawn(playerNumber));
  }

  private IEnumerator WaitToSpawn(int playerNumber) {
    yield return new WaitForSeconds(1);
    if (IsServer)
    {
      SpawnTent(playerNumber);
      FruitShop.Instance.StartCounter();
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
    tent.GetComponent<TentInfo>().direction = (playerNumber % 2 == 0) ? -1 : 1;
    tent.GetComponent<TentInfo>().playerNumber = playerNumber;
    tent.GetComponent<TentInfo>().hasPlayer = true;
    NetworkObject networkObject = tent.GetComponent<NetworkObject>();
    networkObject.Spawn();
    Transform playerSpawnPoint = tent.GetComponent<TentInfo>()._transform;
    EventManager.Instance.OnPlayerEnter?.Invoke(this, new()
    {
      playerNumber = playerNumber,
      pos = playerSpawnPoint
    });
    spawned.Add(tent);
    TriggerOnPlayerEnterClientRpc(playerNumber, playerSpawnPoint.position, playerSpawnPoint.rotation);
    // Debug.Log("spawn tent");
  }
  [ClientRpc]
  private void TriggerOnPlayerEnterClientRpc(int playerNumber, Vector3 position, Quaternion rotation)
  {
    Transform temp = new GameObject("TempTransform").transform;
    temp.SetPositionAndRotation(position, rotation);
    EventManager.Instance.OnPlayerEnter?.Invoke(this, new()
    {
        playerNumber = playerNumber,
        pos = temp
    });
  }

}
