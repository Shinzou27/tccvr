using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FruitSpawner : NetworkBehaviour
{
    public static FruitSpawner Instance;
    [SerializeField] private List<FruitSO> fruits;
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

    public void RequestSpawnFruit(Transform transform, FruitSO fruitToSpawn, int amount)
    {
        if (IsServer) {
            SpawnTent(transform.position, fruitToSpawn.type, amount);
        }
        else {
            SpawnTentServerRpc(transform.position, fruitToSpawn.type, amount);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnTentServerRpc(Vector3 position, FruitType fruitType, int amount)
    {
        // Debug.LogError($"Sou cliente e estou instanciando {amount} unidades de {fruitType}.");
        SpawnTent(position, fruitType, amount);
    }

    private void SpawnTent(Vector3 position, FruitType fruitType, int amount)
    {
        for (int i = 0; i < amount; i++) {
            GameObject fruit = Instantiate(GetFruit(fruitType), position, Quaternion.Euler(new(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));
            NetworkObject networkObject = fruit.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }
    }
    private GameObject GetFruit(FruitType fruitType) {
        foreach (FruitSO fruit in fruits) {
            if (fruit.type == fruitType) return fruit.prefab;
        }
        return null;
    }
}
