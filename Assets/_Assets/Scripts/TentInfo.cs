using Unity.Netcode;
using UnityEngine;

public class TentInfo : NetworkBehaviour, IPointBearer {
  public Transform _transform;
  public Transform npcStopPoint;
  public int direction;
  public bool IsFree = true;
  public GameObject customer;
  public PointController pointController;
  public int playerNumber;
  public bool hasPlayer;
  void Start()
  {
    pointController = GetComponent<PointController>();
  }

  public void Add(int points)
  {
    pointController.points += points;
    pointController.UpdateLabel();
  }

  public int Get()
  {
    return pointController.points;
  }

  public void Subtract(int points)
  {
    pointController.points -= points;
    pointController.UpdateLabel();
  }
}