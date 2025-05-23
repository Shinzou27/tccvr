using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class VRRigRereferences : MonoBehaviour
{
  public static VRRigRereferences Singleton;
  public Transform root;    
  public Transform leftHand;
  public Transform rightHand;
  public int playerNumber;
  void Awake()
  {
      if (Singleton == null) Singleton = this;
  }
}
