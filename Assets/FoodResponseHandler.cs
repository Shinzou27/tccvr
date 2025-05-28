using UnityEngine;

public class FoodResponseHandler : MonoBehaviour
{
  public void SpawnFood(string[] values)
  {
    Debug.Log("ASDASD");
    foreach (string value in values)
    {
      Debug.Log(value);
    }
  }
  public void OnError(string err)
  {
    Debug.Log(err);
  }
}