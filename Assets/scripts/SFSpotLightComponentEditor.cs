using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SFSpotLightComponent))]
public class SFSpotLightComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SFSpotLightComponent spotLightComponent = (SFSpotLightComponent)target;
        if (GUILayout.Button("Use Rotation as Direction"))
        {
            spotLightComponent.Direction = spotLightComponent.transform.forward;
        }
    }
}
