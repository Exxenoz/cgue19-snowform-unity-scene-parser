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
    public float ShadowMapOrthoLeft = -10f;
    public float ShadowMapOrthoRight = 10f;
    public float ShadowMapOrthoBottom = -10f;
    public float ShadowMapOrthoTop = 10f;
    public float ShadowMapOrthoNear = 1f;
    public float ShadowMapOrthoFar = 50f;
}
