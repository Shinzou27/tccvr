using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OnFruitPlaceHandler : MonoBehaviour
{
    // [SerializeField] private TentInfo tent;
    [SerializeField] private FruitsOnSackController sackController;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        Debug.Log(gameObject.name);
        if (other.gameObject.CompareTag("Fruit"))
        {
            // Debug.Log("Deixei uma fruta aqui: " + other.gameObject.name);
            if (other.gameObject.TryGetComponent(out FruitInfo info))
            {
                sackController.fruits.PlaceFruit(info.data);
                Destroy(other.gameObject); // solução provisória pra lidar com o ontriggerenter sendo ativado 2x
                // other.transform.SetParent(gameObject.transform);
                // other.GetComponent<Rigidbody>().isKinematic = true;
                // other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                // other.GetComponent<XRGrabInteractable>().enabled = false;
            }
        }
        else if (other.gameObject.TryGetComponent(out OnSackPlaceHandler sackPlaceHandler))
        {
            sackPlaceHandler.Check(sackController.fruits, () => GetComponent<Collider>().enabled = false);
        }
    }
    // public void SetTent(TentInfo tentInfo)
    // {
    //     tent = tentInfo;
    // }
}
