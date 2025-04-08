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

    // Update is called once per frame
    void Update()
    {
        
    }
}
