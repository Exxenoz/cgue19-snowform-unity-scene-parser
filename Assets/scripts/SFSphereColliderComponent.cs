﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFSphereColliderComponent : MonoBehaviour
{
    public string PhysicsMaterial;
    public Vector3 Offset = new Vector3(0f, 0f, 0f);
    public float Radius;
    public bool Trigger;
}
