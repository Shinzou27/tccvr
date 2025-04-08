using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TendSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    public int players;

    void Start()
    {
        int pairCount = (players + 1) / 2;
        float offset = (pairCount - 1) * 1.5f;
        int pairIndex = 0;

        for (int i = 0; i < players; i++)
        {
            float x = (pairIndex * 3) - offset;
            float z = (i % 2 == 0) ? 5 : -5;
            Quaternion quaternion = Quaternion.Euler(0, (i % 2 == 0) ? 0 : 180, 0);
            Vector3 spawnPos = new(x, 0, z);
            Instantiate(prefabs[Random.Range(0, prefabs.Count)], spawnPos, quaternion);

            if (i % 2 == 1)
                pairIndex++;
        }
    }
}
