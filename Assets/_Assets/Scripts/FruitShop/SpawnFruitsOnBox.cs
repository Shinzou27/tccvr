using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnFruitsOnBox : NetworkBehaviour
{
    [SerializeField] private FruitSO fruitToSpawn;
    [SerializeField] private int amount;
    [SerializeField] private Transform pivot;
    private void Start() {
        Spawn();
    }
    private void Spawn() {
        ;
        for (int i = 0; i < amount; i++) {
            Instantiate(fruitToSpawn.prefab, pivot.position, Quaternion.Euler(new(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));
        }
    }
}
