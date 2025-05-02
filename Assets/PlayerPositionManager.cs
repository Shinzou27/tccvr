using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    [SerializeField] private List<Transform> seats;
    public static PlayerPositionManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public Transform GetPlayerSpawn(int playerNumber) {
        return seats[playerNumber];
    }
}
