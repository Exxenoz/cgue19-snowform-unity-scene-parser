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
                componentsElement.Add(e);
            }
        }

        {
            SFRigidStaticComponent c = transform.GetComponent<SFRigidStaticComponent>();
            if (c != null)
            {
                XElement e = new XElement("RigidStaticComponent");
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
