using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class Utils
{
  public static bool isHost;
  public static int[] GenerateRandomFruitAmountDivisions(int totalAmount, int differentFruitsCount)
  {
    if (differentFruitsCount <= 0) differentFruitsCount = 1;

    if (totalAmount < differentFruitsCount) totalAmount = differentFruitsCount;

    if (totalAmount == differentFruitsCount)
      return Enumerable.Repeat(1, differentFruitsCount).ToArray();

    System.Random rnd = new();
    int[] divisions = new int[differentFruitsCount];
    for (int i = 0; i < differentFruitsCount; i++) divisions[i] = 1;
    int remaining = totalAmount - differentFruitsCount;
    for (int i = 0; i < remaining; i++) divisions[rnd.Next(0, differentFruitsCount)]++;
    for (int i = divisions.Length - 1; i > 0; i--)
    {
      int j = rnd.Next(0, i + 1);
      (divisions[j], divisions[i]) = (divisions[i], divisions[j]);
    }
    return divisions;
  }

  public static void Shuffle<T>(this IList<T> list)
  {
    System.Random rng = new();
    int n = list.Count;
    while (n > 1)
    {
      n--;
      int k = rng.Next(n + 1);
      (list[n], list[k]) = (list[k], list[n]);
    }
  }
  public static string FormatFruitList(List<OrderSet> list)
  {
    if (list == null || !list.Any()) {
      Debug.Log("N funfou");
      return string.Empty;
    }

    var wordMap = new Dictionary<int, string> {
        {1, "one"}, {2, "two"}, {3, "three"}, {4, "four"}, {5, "five"},
        {6, "six"}, {7, "seven"}, {8, "eight"}, {9, "nine"}, {10, "ten"}
    };

    var parts = list.Select(f =>
    {
      string name = f.amount == 1 ? f.fruit.fruitName.ToLower() : f.fruit.fruitName.ToLower() + "s";
      string amount = wordMap.TryGetValue(f.amount, out var word) ? word : f.amount.ToString();
      return $"{amount} {name}";
    }).ToList();

    if (parts.Count == 1)
      return parts[0];

    string last = parts.Last();
    string joined = string.Join(", ", parts.Take(parts.Count - 1));
    return $"{joined} and {last}";
  }
  public static int GetPlayersActive()
  {
    return GameObject.FindGameObjectsWithTag("NetworkPlayer").Length;
  }
  public static Vector3 GetSpawnTransform(int i, int players)
  {
    int pairCount = (players + 1) / 2;
    float offset = (pairCount - 1) * 1.5f;
    int pairIndex = i / 2;

    float x = (pairIndex * 3) - offset;
    float z = (i % 2 == 0) ? 6.5f : -6.5f;
    Vector3 position = new(x, 0, z);

    return position;
  }
  public static Order GenerateOrder() {
    int fruitAmount = UnityEngine.Random.Range(0, FruitShop.Instance.maxFruitAmount) + 1;
    int uniqueFruits = UnityEngine.Random.Range(0, FruitShop.Instance.maxUniqueFruits) + 1;
    if (fruitAmount < uniqueFruits) fruitAmount = uniqueFruits;
    Debug.Log($"Frutas no total: {fruitAmount} | Tipos únicos de frutas: {uniqueFruits}");
    FruitSO[] fruits = FruitShop.Instance.fruits.ToArray();
    Shuffle(fruits);
    int[] sets = GenerateRandomFruitAmountDivisions(fruitAmount, uniqueFruits);
    Order order = new();
    for(int i = 0; i < sets.Length; i++) {
        order.list.Add(new(fruits[i], sets[i]));
    }
    order.amountOnOrder = fruitAmount;
    return order;
  }
  public static Order GetOrderFromTentCustomer(TentInfo info) {
    if (!info.customer) return null;
    info.customer.TryGetComponent(out NPCFruitOrderController component);
    if (component != null) {
      return component.order;
    }
    return null;
  }
}