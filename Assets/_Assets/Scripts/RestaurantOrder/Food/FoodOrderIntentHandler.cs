using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOrderIntentHandler : MonoBehaviour
{
  public static FoodOrderIntentHandler Instance;
  public List<FoodOrder> foods;
  [SerializeField] private Transform tray;
  [SerializeField] private Transform plate;
  [SerializeField] private Transform cup;

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
      GameObject go = GetFood(value);
      if (go != null)
      {
        GameObject food = Instantiate(go, tray);
        FoodInfo foodInfo = food.GetComponent<FoodInfo>();
        if (foodInfo.IsDrinkable())
        {
          food.GetComponent<FoodTrayTransporter>().destination = cup;
        }
        else
        {
          food.GetComponent<FoodTrayTransporter>().destination = plate;
        }
      }
    }

  }
  private GameObject GetFood(string value)
  {
    foreach (FoodOrder food in foods)
    {
      if (value == food.keyword)
      {
        RestaurantOrder.Instance.AddToOrder(food); //TODO: com ctz tem algum canto melhor pra chamar isso 
        return food.prefab;
      }
    }
    return null;
  }
}
