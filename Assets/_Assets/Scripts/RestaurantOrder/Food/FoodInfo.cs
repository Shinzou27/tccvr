using UnityEngine;

public class FoodInfo : MonoBehaviour
{
  [SerializeField] private bool IsDrink;
  [SerializeField] private bool NeedPlate;

  public bool IsDrinkable()
  {
    return IsDrink;
  }
  public bool IsPlateNeeded()
  {
    return NeedPlate;
  }
  public bool NeedToHidePlate()
  {
    return !NeedPlate && !IsDrink;
  }
}