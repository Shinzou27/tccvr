using System;
using System.Collections.Generic;
using System.Linq;
public static class Utils
{
  public static int[] GenerateRandomFruitAmountDivisions(int totalAmount, int differentFruitsCount)
  {
    if (differentFruitsCount <= 0) differentFruitsCount = 1;

    if (totalAmount < differentFruitsCount) totalAmount = differentFruitsCount;

    if (totalAmount == differentFruitsCount)
      return Enumerable.Repeat(1, differentFruitsCount).ToArray();

    Random rnd = new();
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
    Random rng = new();
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
    if (list == null || !list.Any())
      return string.Empty;

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
}