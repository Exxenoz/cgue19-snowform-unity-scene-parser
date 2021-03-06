using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFRigidDynamicComponent : MonoBehaviour
{
    public bool CCD = false;
    public bool Kinematic = false;
    public float Mass = 1f;
    public float MaxLinearVelocity = 100000f;
    public float MaxAngularVelocity = 100f;
    public uint RaycastLayerMask = 1;
}
