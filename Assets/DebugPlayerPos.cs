using UnityEngine;

public class DebugPlayerPos : MonoBehaviour
{
  void Start()
  {
    Transform randomT = PlayerPositionManager.Instance.GetPlayerSpawn(Random.Range(0, 16));
    transform.SetPositionAndRotation(randomT.position, randomT.rotation);
    AssignPlayerToTable(randomT);
  }

  private void AssignPlayerToTable(Transform chosenTransform) {
    foreach (Table table in RestaurantOrder.Instance.tables) {
      foreach (Transform tableTransform in table.tableSeats) {
        if (transform.position == tableTransform.position) {
          table.InsertPlayerOnTable(1);
        }
      }
    }
  }
}
