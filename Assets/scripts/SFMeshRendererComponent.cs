using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFMeshRendererComponent : MonoBehaviour
{
    public string Material = "";
    public string Mesh = "";
    public SFDirectionalLightComponent[] directionalLights;
    public SFPointLightComponent[] pointLights;
    public SFSpotLightComponent[] spotLights;
}
