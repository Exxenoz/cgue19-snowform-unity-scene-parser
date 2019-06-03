using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFParticleSystemComponent : MonoBehaviour
{
    public bool Enabled = true;
    public float PrewarmTime = 0f;
    public int ParticleCount;
    public string ParticleMaterial;
    public string ParticleMesh;
    public Vector3 MinRelativeEmitPosition;
    public Vector3 MaxRelativeEmitPosition;
    public Vector3 MinParticleVelocity;
    public Vector3 MaxParticleVelocity;
    public float MinParticleSize = 1f;
    public float MaxParticleSize = 1f;
    public float MinParticleLifetime = 1f;
    public float MaxParticleLifetime = 10f;
}
