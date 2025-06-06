using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnFruitsOnBox : MonoBehaviour
{
    [SerializeField] private FruitSO fruitToSpawn;
    [SerializeField] private int amount;
    [SerializeField] private Transform pivot;
    private void Start() {
        Spawn();
    }
    private void Spawn() {
        FruitSpawner.Instance.RequestSpawnFruit(pivot, fruitToSpawn, amount);
    }
}
