using UnityEngine;

[CreateAssetMenu(fileName = "Fruit", menuName = "Fruit", order = 51)]
public class FruitSO : ScriptableObject {
    public string fruitName;
    public FruitType type;
    public GameObject prefab;
    public Sprite icon;
}