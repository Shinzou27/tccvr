using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRigRereferences : MonoBehaviour
{
    public static VRRigRereferences Singleton;
    public Transform root;    
    public Transform leftHand;
    public Transform rightHand;
    void Awake()
    {
        if (Singleton == null) Singleton = this;
    }

  void Start()
  {
    EventManager.Instance.OnPlayerEnter += SetPosition;
  }
  void OnDestroy()
  {
    EventManager.Instance.OnPlayerEnter -= SetPosition;
  }

  private void SetPosition(object sender, Transform e)
  {
    transform.SetPositionAndRotation(e.position, e.rotation);
  }
}
