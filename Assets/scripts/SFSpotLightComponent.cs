using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFSpotLightComponent : MonoBehaviour
{
    public Vector3 Direction = new Vector3();
    public Color Color = Color.white;
    public float Intensity = 1f;
    public float InnerCutoff = 1f;
    public float OuterCutoff = 1f;
    public float Constant = 1f;
    public float Linear = 1f;
    public float Quadratic = 1f;
}
