using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SceneExporter : MonoBehaviour
{
    public Transform SceneTransform;

    [TextArea(8, 8)]
    public string Output;

    private string ToString(bool b)
    {
        return b ? "true" : "false";
    }

    private string ToString(float f)
    {
        return f.ToString(new CultureInfo("en-US"));
    }

    private string ToString(Vector3 v)
    {
        return v.x.ToString(new CultureInfo("en-US")) + " " + v.y.ToString(new CultureInfo("en-US")) + " " + v.z.ToString(new CultureInfo("en-US"));
    }

    private string LocalPositionToString(Vector3 v)
    {
        v.x *= -1f;
        return ToString(v);
    }

    private string LocalRotationToString(Vector3 v)
    {
        v.y *= -1f;
        return ToString(v);
    }

    private string LocalCameraRotationToString(Vector3 v)
    {
        v.x += 180f;
        v.x *=  -1f;
        v.z += 180f;
        return ToString(v);
    }

    private void ExportGameObject(Transform transform, XElement gameObjectElement)
    {
        if (!string.IsNullOrEmpty(transform.gameObject.tag) && transform.gameObject.tag != "Untagged")
        {
            gameObjectElement.Add(new XAttribute("id", transform.gameObject.tag));
        }
        gameObjectElement.Add(new XAttribute("name", transform.gameObject.name));

        XElement componentsElement = new XElement("Components");

        {
            XElement e = new XElement("TransformComponent");
            e.Add(new XAttribute("localPosition", LocalPositionToString(transform.localPosition)));
            e.Add(new XAttribute("localRotation", transform.GetComponent<SFCameraComponent>() ?
                LocalCameraRotationToString(transform.localEulerAngles) : LocalRotationToString(transform.localEulerAngles)));
            e.Add(new XAttribute("localScale", ToString(transform.localScale)));
            componentsElement.Add(e);
        }

        {
            SFMeshRendererComponent c = transform.GetComponent<SFMeshRendererComponent>();
            if (c != null)
            {
                XElement e = new XElement("MeshRendererComponent");
                e.Add(new XAttribute("material", c.Material));
                e.Add(new XAttribute("mesh", c.Mesh));
                foreach (SFDirectionalLightComponent l in c.directionalLights)
                {
                    if (!string.IsNullOrEmpty(l.gameObject.tag) && l.gameObject.tag != "Untagged")
                    {
                        XElement e2 = new XElement("Light");
                        e2.Add(new XAttribute("gameObjectId", l.tag));
                        e.Add(e2);
                    }
                    else
                    {
                        Debug.LogError("Could not add directional light of game object '" + l.name + "' to mesh renderer of game object '" + c.name + "', because the light game object has no tag set!");
                    }
                }
                foreach (SFPointLightComponent l in c.pointLights)
                {
                    if (!string.IsNullOrEmpty(l.gameObject.tag) && l.gameObject.tag != "Untagged")
                    {
                        XElement e2 = new XElement("Light");
                        e2.Add(new XAttribute("gameObjectId", l.tag));
                        e.Add(e2);
                    }
                    else
                    {
                        Debug.LogError("Could not add point light of game object '" + l.name + "' to mesh renderer of game object '" + c.name + "', because the light game object has no tag set!");
                    }
                }
                foreach (SFSpotLightComponent l in c.spotLights)
                {
                    if (!string.IsNullOrEmpty(l.gameObject.tag) && l.gameObject.tag != "Untagged")
                    {
                        XElement e2 = new XElement("Light");
                        e2.Add(new XAttribute("gameObjectId", l.tag));
                        e.Add(e2);
                    }
                    else
                    {
                        Debug.LogError("Could not add spot light of game object '" + l.name + "' to mesh renderer of game object '" + c.name + "', because the light game object has no tag set!");
                    }
                }
                componentsElement.Add(e);
            }
        }

        {
            SFMeshColliderComponent c = transform.GetComponent<SFMeshColliderComponent>();
            if (c != null)
            {
                XElement e = new XElement("MeshColliderComponent");
                e.Add(new XAttribute("physicsMaterial", c.PhysicsMaterial));
                e.Add(new XAttribute("collisionMesh", c.CollisionMesh));
                e.Add(new XAttribute("offset", LocalPositionToString(c.Offset)));
                componentsElement.Add(e);
            }
        }

        {
            SFSphereColliderComponent c = transform.GetComponent<SFSphereColliderComponent>();
            if (c != null)
            {
                XElement e = new XElement("SphereColliderComponent");
                e.Add(new XAttribute("physicsMaterial", c.PhysicsMaterial));
                e.Add(new XAttribute("offset", LocalPositionToString(c.Offset)));
                e.Add(new XAttribute("radius", ToString(c.Radius)));
                e.Add(new XAttribute("trigger", ToString(c.Trigger)));
                componentsElement.Add(e);
            }
        }

        {
            SFBoxColliderComponent c = transform.GetComponent<SFBoxColliderComponent>();
            if (c != null)
            {
                XElement e = new XElement("BoxColliderComponent");
                e.Add(new XAttribute("physicsMaterial", c.PhysicsMaterial));
                e.Add(new XAttribute("offset", LocalPositionToString(c.Offset)));
                e.Add(new XAttribute("halfExtent", ToString(c.HalfExtent)));
                e.Add(new XAttribute("trigger", ToString(c.Trigger)));
                componentsElement.Add(e);
            }
        }

        {
            SFRigidDynamicComponent c = transform.GetComponent<SFRigidDynamicComponent>();
            if (c != null)
            {
                XElement e = new XElement("RigidDynamicComponent");
                e.Add(new XAttribute("CCD", ToString(c.CCD)));
                e.Add(new XAttribute("kinematic", ToString(c.Kinematic)));
                e.Add(new XAttribute("mass", ToString(c.Mass)));
                e.Add(new XAttribute("maxLinearVelocity", ToString(c.MaxLinearVelocity)));
                e.Add(new XAttribute("maxAngularVelocity", ToString(c.MaxAngularVelocity)));
                e.Add(new XAttribute("raycastLayerMask", ToString(c.RaycastLayerMask)));
                componentsElement.Add(e);
            }
        }

        {
            SFRigidStaticComponent c = transform.GetComponent<SFRigidStaticComponent>();
            if (c != null)
            {
                XElement e = new XElement("RigidStaticComponent");
                e.Add(new XAttribute("raycastLayerMask", ToString(c.RaycastLayerMask)));
                componentsElement.Add(e);
            }
        }

        {
            SFArcBallControllerComponent c = transform.GetComponent<SFArcBallControllerComponent>();
            if (c != null)
            {
                XElement e = new XElement("ArcBallControllerComponent");
                componentsElement.Add(e);
            }
        }

        {
            SFCameraComponent c = transform.GetComponent<SFCameraComponent>();
            if (c != null)
            {
                XElement e = new XElement("CameraComponent");
                componentsElement.Add(e);
            }
        }

        {
            SFThirdPersonControllerComponent c = transform.GetComponent<SFThirdPersonControllerComponent>();
            if (c != null)
            {
                XElement e = new XElement("ThirdPersonControllerComponent");
                e.Add(new XAttribute("targetId", c.targetId));
                e.Add(new XAttribute("distance", ToString(c.distance)));
                componentsElement.Add(e);
            }
        }

        {
            SFMovingPlatformComponent c = transform.GetComponent<SFMovingPlatformComponent>();
            if (c != null)
            {
                XElement e = new XElement("MovingPlatformComponent");
                e.Add(new XAttribute("moveDirection", ToString(c.MoveDirection)));
                e.Add(new XAttribute("length", ToString(c.Length)));
                e.Add(new XAttribute("time", ToString(c.Time)));
                componentsElement.Add(e);
            }
        }

        {
            SFRotateComponent c = transform.GetComponent<SFRotateComponent>();
            if (c != null)
            {
                XElement e = new XElement("RotateComponent");
                e.Add(new XAttribute("rotationDirection", ToString(c.rotationDirection)));
                e.Add(new XAttribute("speed", ToString(c.speed)));
                componentsElement.Add(e);
            }
        }

        {
            SFCoinComponent c = transform.GetComponent<SFCoinComponent>();
            if (c != null)
            {
                XElement e = new XElement("CoinComponent");
                componentsElement.Add(e);
            }
        }

        {
            SFFlagComponent c = transform.GetComponent<SFFlagComponent>();
            if (c != null)
            {
                XElement e = new XElement("FlagComponent");
                componentsElement.Add(e);
            }
        }

        {
            SFSceneOptionComponent[] cs = transform.GetComponents<SFSceneOptionComponent>();

            foreach (SFSceneOptionComponent c in cs)
            {
                XElement e = new XElement("SceneOptionComponent");
                e.Add(new XAttribute("key", c.Key));
                e.Add(new XAttribute("value", c.Value));
                componentsElement.Add(e);
            }
        }

        {
            SFPlayerComponent c = transform.GetComponent<SFPlayerComponent>();
            if (c != null)
            {
                XElement e = new XElement("PlayerComponent");
                componentsElement.Add(e);
            }
        }

        {
            SFDirectionalLightComponent c = transform.GetComponent<SFDirectionalLightComponent>();
            if (c != null)
            {
                XElement e = new XElement("DirectionalLightComponent");
                e.Add(new XAttribute("color", ToString(new Vector3(c.Color.r, c.Color.g, c.Color.b))));
                e.Add(new XAttribute("intensity", ToString(c.Intensity)));
                e.Add(new XAttribute("shadow", c.Shadow));
                if (c.ShadowMapWidth > 0)
                {
                    e.Add(new XAttribute("shadowMapWidth", c.ShadowMapWidth));
                }
                if (c.ShadowMapHeight > 0)
                {
                    e.Add(new XAttribute("shadowMapHeight", c.ShadowMapHeight));
                }
                e.Add(new XAttribute("shadowMapOrthoLeft", c.ShadowMapOrthoLeft));
                e.Add(new XAttribute("shadowMapOrthoRight", c.ShadowMapOrthoRight));
                e.Add(new XAttribute("shadowMapOrthoBottom", c.ShadowMapOrthoBottom));
                e.Add(new XAttribute("shadowMapOrthoTop", c.ShadowMapOrthoTop));
                e.Add(new XAttribute("shadowMapOrthoNear", c.ShadowMapOrthoNear));
                e.Add(new XAttribute("shadowMapOrthoFar", c.ShadowMapOrthoFar));
                componentsElement.Add(e);
            }
        }

        {
            SFPointLightComponent c = transform.GetComponent<SFPointLightComponent>();
            if (c != null)
            {
                XElement e = new XElement("PointLightComponent");
                e.Add(new XAttribute("color", ToString(new Vector3(c.Color.r, c.Color.g, c.Color.b))));
                e.Add(new XAttribute("intensity", ToString(c.Intensity)));
                e.Add(new XAttribute("constant", ToString(c.Constant)));
                e.Add(new XAttribute("linear", ToString(c.Linear)));
                e.Add(new XAttribute("quadratic", ToString(c.Quadratic)));
                componentsElement.Add(e);
            }
        }

        {
            SFSpotLightComponent c = transform.GetComponent<SFSpotLightComponent>();
            if (c != null)
            {
                XElement e = new XElement("SpotLightComponent");
                e.Add(new XAttribute("direction", ToString(c.Direction)));
                e.Add(new XAttribute("color", ToString(new Vector3(c.Color.r, c.Color.g, c.Color.b))));
                e.Add(new XAttribute("intensity", ToString(c.Intensity)));
                e.Add(new XAttribute("innerCutoff", ToString(c.InnerCutoff)));
                e.Add(new XAttribute("outerCutoff", ToString(c.OuterCutoff)));
                e.Add(new XAttribute("constant", ToString(c.Constant)));
                e.Add(new XAttribute("linear", ToString(c.Linear)));
                e.Add(new XAttribute("quadratic", ToString(c.Quadratic)));
                componentsElement.Add(e);
            }
        }

        {
            SFFollowComponent c = transform.GetComponent<SFFollowComponent>();
            if (c != null)
            {
                XElement e = new XElement("FollowComponent");
                if (c.Target != null)
                {
                    if (!string.IsNullOrEmpty(c.Target.tag) && c.Target.tag != "Untagged")
                    {
                        e.Add(new XAttribute("targetId", c.Target.tag));
                    }
                    else
                    {
                        Debug.LogError("Could not set target id for follow component of game object '" + c.gameObject.name + "', because target has no tag set!");
                    }
                }
                else
                {
                    Debug.LogError("Could not set target id for follow component of game object '" + c.gameObject.name + "', because target is null!");
                }
                componentsElement.Add(e);
            }
        }

        {
            SFParticleSystemComponent c = transform.GetComponent<SFParticleSystemComponent>();
            if (c != null)
            {
                XElement e = new XElement("ParticleSystemComponent");
                e.Add(new XAttribute("enabled", ToString(c.Enabled)));
                e.Add(new XAttribute("prewarmTime", ToString(c.PrewarmTime)));
                e.Add(new XAttribute("particleCount", c.ParticleCount));
                e.Add(new XAttribute("particleMaterial", c.ParticleMaterial));
                e.Add(new XAttribute("particleMesh", c.ParticleMesh));
                e.Add(new XAttribute("minRelativeEmitPosition", ToString(c.MinRelativeEmitPosition)));
                e.Add(new XAttribute("maxRelativeEmitPosition", ToString(c.MaxRelativeEmitPosition)));
                e.Add(new XAttribute("minParticleVelocity", ToString(c.MinParticleVelocity)));
                e.Add(new XAttribute("maxParticleVelocity", ToString(c.MaxParticleVelocity)));
                e.Add(new XAttribute("minParticleSize", ToString(c.MinParticleSize)));
                e.Add(new XAttribute("maxParticleSize", ToString(c.MaxParticleSize)));
                e.Add(new XAttribute("minParticleLifetime", ToString(c.MinParticleLifetime)));
                e.Add(new XAttribute("maxParticleLifetime", ToString(c.MaxParticleLifetime)));
                componentsElement.Add(e);
            }
        }

        gameObjectElement.Add(componentsElement);

        XElement childrenElement = new XElement("Children");

        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeInHierarchy)
            {
                continue;
            }

            XElement childGameObjectElement = new XElement("GameObject");
            ExportGameObject(transform.GetChild(i), childGameObjectElement);
            childrenElement.Add(childGameObjectElement);
        }

        gameObjectElement.Add(childrenElement);
    }

    public void ExportScene()
    {
        XElement sceneElement = new XElement("Scene");

        for (int i = 0; i < SceneTransform.childCount; i++)
        {
            if (!SceneTransform.GetChild(i).gameObject.activeInHierarchy)
            {
                continue;
            }

            XElement e = new XElement("GameObject");
            ExportGameObject(SceneTransform.GetChild(i), e);
            sceneElement.Add(e);
        }

        Output = sceneElement.ToString();
    }
}
