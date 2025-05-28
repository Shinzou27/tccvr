using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOrderIntentHandler : MonoBehaviour
{
  public static FoodOrderIntentHandler Instance;
  public List<FoodOrder> foods;
  [SerializeField] private Transform tray;

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(this);
    }
  }

  public void HandleSpawn(string[] values)
  {
    foreach (string value in values)
    {
      GameObject go = GetFoodPrefab(value);
      if (go != null)
      {
        Instantiate(go, tray);
      }
    }

  }
  private GameObject GetFoodPrefab(string value)
  {
    foreach (FoodOrder food in foods)
    {
      if (value == food.keyword)
      {
        return food.prefab;
      }
    }
    return null;
  }
}
