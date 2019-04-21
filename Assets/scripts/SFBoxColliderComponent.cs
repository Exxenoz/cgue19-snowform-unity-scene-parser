using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFBoxColliderComponent : MonoBehaviour
{
    public string PhysicsMaterial;
    public Vector3 Offset = new Vector3(0f, 0f, 0f);
    public Vector3 HalfExtent = new Vector3(1f, 1f, 1f);
    public bool Trigger;
}
