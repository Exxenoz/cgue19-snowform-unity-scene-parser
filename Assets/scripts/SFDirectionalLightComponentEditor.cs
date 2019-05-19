using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SFDirectionalLightComponent))]
public class SFDirectionalLightComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SFDirectionalLightComponent directionalLightComponent = (SFDirectionalLightComponent)target;
        if (GUILayout.Button("Use Rotation as Direction"))
        {
            directionalLightComponent.Direction = directionalLightComponent.transform.forward;
        }
    }
}
