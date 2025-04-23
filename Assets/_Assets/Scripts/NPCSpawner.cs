using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NPCSpawner : NetworkBehaviour
{
    public List<GameObject> prefabs;
    public int maxCount;
    public Transform defaultDestinationLeft;
    public Transform defaultDestinationRight;
    public float spawnInterval;

    private float spawnTimeElapsed;
    private int currentCount;
    private List<GameObject> spawned;

    void Start()
    {
        spawned = new();
    }

    void Update()
    {
        if (!IsServer) return;

        spawnTimeElapsed += Time.deltaTime;
        if (spawnTimeElapsed >= spawnInterval)
        {
            spawnTimeElapsed = 0;
            if (currentCount < maxCount)
            {
                int direction = Random.Range(0, 2) == 0 ? -1 : 1;
                int randomZ = Random.Range(-2, 2);
                Vector3 spawnPos = new(direction * 40, 0, randomZ);

                GameObject npc = Instantiate(prefabs[Random.Range(0, prefabs.Count)], spawnPos, Quaternion.identity);
                NetworkObject netObj = npc.GetComponent<NetworkObject>();
                netObj.Spawn();

                currentCount++;
                npc.GetComponent<NPCBehavior>().SetDefaultDestination(direction == 1 ? defaultDestinationLeft : defaultDestinationRight);
                npc.GetComponent<NPCBehavior>().OnDestroyBehavior = () => {
                    spawned.Remove(npc);
                    currentCount--;
                };
                spawned.Add(npc);
            }
        }
    }
}