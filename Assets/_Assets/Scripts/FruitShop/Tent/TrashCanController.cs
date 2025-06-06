using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanController : MonoBehaviour
{
  void OnTriggerEnter(Collider other)
  {
    Destroy(other.gameObject);
  }
}
