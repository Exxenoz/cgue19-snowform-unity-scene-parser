using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFTextComponent : MonoBehaviour
{
    public bool Enabled = true;
    public string Material;
    public string Font;
    public float FontSize = 10;
    [TextArea]
    public string Text;
    public string TextAlignment = "CenterCenter";
    public string TextOverflowMode = "Overflow";
    public bool WordWrapping = false;
    public float MaxWidth;
    public float MaxHeight;
    public bool Outline = false;
    public string OutlineMaterial;
    public float OutlineThickness = 1.0f;
    public Vector3 OutlineOffset = Vector3.zero;
}
