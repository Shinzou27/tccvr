using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeFruitOrder : MonoBehaviour
{
    private void Start() {
        EventManager.Instance.OnCustomerEnter += GenerateNewSet;
    }
    public void GenerateNewSet(object sender, EventArgs e) {
        int fruitAmount = UnityEngine.Random.Range(0, FruitShop.Instance.maxFruitAmount) + 1;
        int uniqueFruits = UnityEngine.Random.Range(0, FruitShop.Instance.maxUniqueFruits) + 1;
        if (fruitAmount < uniqueFruits) fruitAmount = uniqueFruits;
        Debug.Log($"Frutas no total: {fruitAmount} | Tipos únicos de frutas: {uniqueFruits}");
        FruitSO[] fruits = FruitShop.Instance.fruits.ToArray();
        Utils.Shuffle(fruits);
        int[] sets = Utils.GenerateRandomFruitAmountDivisions(fruitAmount, uniqueFruits);
        Order order = new();
        for(int i = 0; i < sets.Length; i++) {
            order.list.Add(new(fruits[i], sets[i]));
        }
        order.amountOnOrder = fruitAmount;
        FruitShop.Instance.UpdateOrder(order);
    }

}
