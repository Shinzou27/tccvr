using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    [SerializeField] private List<Transform> seats;
    [SerializeField] private Transform fixedSeat;
    public bool randomizePosition;
    public static PlayerPositionManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public Transform GetPlayerSpawn(int playerNumber)
    {
        if (randomizePosition)
        {
            return seats[playerNumber];
        }
        return fixedSeat;
    }
}
