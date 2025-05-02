using TMPro;
using UnityEngine;

public class FruitShopNetworkPlayer : NetworkPlayer {
  [SerializeField] private TextMeshProUGUI debugPlayerNumberLabel;
  private int playerNumber;
  public override void OnNetworkSpawn() {
    InitialSetup();
    if (IsOwner) {
      playerNumber = Utils.GetPlayersActive();
      debugPlayerNumberLabel.text = playerNumber.ToString();
      TendSpawner.Instance.RequestSpawnTent(playerNumber);
    }
  }
}