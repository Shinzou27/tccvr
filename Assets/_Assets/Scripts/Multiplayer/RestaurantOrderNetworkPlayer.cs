using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantOrderNetworkPlayer : NetworkPlayer
{
  public override void OnNetworkSpawn()
  {
    InitialSetup();
  }
}
