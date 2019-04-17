using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneExporter))]
public class SceneExporterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneExporter exporter = (SceneExporter)target;
        if (GUILayout.Button("Export Scene"))
        {
            exporter.ExportScene();
        }
    }
}
