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

    private string ToString(Vector3 v)
    {
        return v.x.ToString(new CultureInfo("en-US")) + " " + v.y.ToString(new CultureInfo("en-US")) + " " + v.z.ToString(new CultureInfo("en-US"));
    }

    private string LocalPositionToString(Vector3 v)
    {
        v.x *= -1f;
        return ToString(v);
    }

    private void ExportGameObject(Transform transform, XElement gameObjectElement)
    {
        gameObjectElement.Add(new XAttribute("name", transform.gameObject.name));

        XElement componentsElement = new XElement("Components");

        {
            XElement e = new XElement("TransformComponent");
            e.Add(new XAttribute("localPosition", LocalPositionToString(transform.localPosition)));
            e.Add(new XAttribute("localRotation", ToString(transform.localEulerAngles)));
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
                componentsElement.Add(e);
            }
        }

        {
            SFSphereColliderComponent c = transform.GetComponent<SFSphereColliderComponent>();
            if (c != null)
            {
                XElement e = new XElement("SphereColliderComponent");
                e.Add(new XAttribute("physicsMaterial", c.PhysicsMaterial));
                e.Add(new XAttribute("radius", c.Radius.ToString(new CultureInfo("en-US"))));
                componentsElement.Add(e);
            }
        }

        {
            SFRigidDynamicComponent c = transform.GetComponent<SFRigidDynamicComponent>();
            if (c != null)
            {
                XElement e = new XElement("RigidDynamicComponent");
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

        gameObjectElement.Add(componentsElement);

        XElement childrenElement = new XElement("Children");

        for (int i = 0; i < transform.childCount; i++)
        {
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
            XElement e = new XElement("GameObject");
            ExportGameObject(SceneTransform.GetChild(i), e);
            sceneElement.Add(e);
        }

        Output = sceneElement.ToString();
    }
}
