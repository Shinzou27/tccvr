using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public List<GameObject> prefabs;
    public int maxCount;
    public Transform defaultDestinationLeft;
    public Transform defaultDestinationRight;
    public float spawnInterval;
    public float spawnTimeElapsed;
    private int currentCount;
    private List<GameObject> spawned;
    void Start()
    {
        spawned = new();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimeElapsed += Time.deltaTime;
        if (spawnTimeElapsed >= spawnInterval) {
            spawnTimeElapsed = 0;
            if (currentCount < maxCount) {
                int direction = Random.Range(0, 2) == 0 ? -1 : 1;
                int randomZ = Random.Range(-2, 2);
                Vector3 spawnPos = new(direction * 40, 0, randomZ);
                GameObject npc = Instantiate(prefabs[Random.Range(0, prefabs.Count)], spawnPos, Quaternion.identity);
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
