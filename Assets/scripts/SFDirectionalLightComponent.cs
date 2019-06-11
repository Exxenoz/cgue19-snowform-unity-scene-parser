using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFDirectionalLightComponent : MonoBehaviour
{
    public Color Color = Color.white;
    public float Intensity = 1f;
    public bool Shadow = false;
    public int ShadowMapWidth = 1024;
    public int ShadowMapHeight = 1024;
}
