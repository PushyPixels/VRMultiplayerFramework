using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

public class EditorTools : MonoBehaviour
{
    [MenuItem("GameObject/Convert To Networked Physics Object", false, 32)]
    static void ConvertToNetworkedPhysicsObject()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            // Get undo group now (For flattening operations)
            int undoGroup = Undo.GetCurrentGroup();

            // Instantiate prefab parent that contains logic
            GameObject objectBase = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/NetworkedPhysicsObjectBase.prefab");
            if (objectBase == null)
            {
                Debug.LogError("Please check to see if Assets/Editor/NetworkedPhysicsObjectBase.prefab exists.");
                return;
            }
            GameObject newParent = (GameObject)PrefabUtility.InstantiatePrefab(objectBase);
            Undo.RegisterCreatedObjectUndo(newParent, "Convert To Networked Physics Object");

            // Move parent to object position/rotation
            newParent.transform.position = obj.transform.position;
            newParent.transform.rotation = obj.transform.rotation;

            // Make parent share the same parent as target object
            newParent.transform.parent = obj.transform.parent;

            // Make object child of new parent (And maintain position in heirarchy)
            int siblingIndex = obj.transform.GetSiblingIndex();
            Undo.SetTransformParent(obj.transform, newParent.transform, "");
            obj.transform.parent = newParent.transform;

            // Offset siblingIndex by one if root object (Otherwise there's an off-by-one.  Not sure why.)
            newParent.transform.SetSiblingIndex(newParent.transform.parent ? siblingIndex : siblingIndex - 1);

            // Add GrabbableChild to object (This has to happen before RecordObject or Undo doesn't work right)
            //Undo.AddComponent<HVRGrabbableChild>(obj);

            // Tweak names around
            Undo.RecordObject(obj, "");
            newParent.name = obj.name;
            obj.name = "Model";

            // Make sure objects aren't static
            obj.isStatic = false;

            // Make sure objects aren't static and put GrabbableChild on all children
            foreach (Transform child in obj.GetComponentsInChildren<Transform>())
            {
                // GetComponentsInChildren returns the object itself too, we skip it here
                if (child == obj.transform)
                {
                    continue;
                }
                if (child.GetComponent<Collider>())
                {
                    //HVRGrabbableChild grabbableChild = Undo.AddComponent<HVRGrabbableChild>(child.gameObject);
                    //grabbableChild.ParentGrabbable = newParent.GetComponent<HVRGrabbable>();
                }
                Undo.RecordObject(child.gameObject, "");
                child.gameObject.isStatic = false;
            }
        }
    }

    [MenuItem("GameObject/Convert To Networked Physics Object", true)]
    static bool ConvertToNetworkedPhysicsObjectValidation()
    {
        if (Selection.GetFiltered(typeof(GameObject), SelectionMode.ExcludePrefab).Length == 0)
        {
            return false;
        }
        return true;
    }

    [MenuItem("GameObject/Attach Physics Object To Wall", false, 32)]
    static void AttachPhysicsObjectToWall()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Preset preset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Editor/ConfigurableJointPreset.preset");
            if (preset == null)
            {
                Debug.LogError("Please check to see if Assets/Editor/ConfigurableJointPreset.preset exists.");
                return;
            }

            ConfigurableJoint newJoint = Undo.AddComponent<ConfigurableJoint>(obj);
            preset.ApplyTo(newJoint);
            Undo.AddComponent<NetworkJointDestruction>(obj);
        }
    }

    [MenuItem("GameObject/Attach Physics Object To Wall", true, 32)]
    static bool AttachPhysicsObjectToWallValidation()
    {
        if (Selection.GetFiltered(typeof(GameObject), SelectionMode.ExcludePrefab).Length == 0)
        {
            return false;
        }
        return true;
    }

    // This is not as useful as I had hoped, at least not while Unity has poor mesh collision.  Maybe with DOTS Havok?  Later.
    [MenuItem("GameObject/Use Model As Collision Mesh", false, 32)]
    static void UseModelAsCollisionMesh()
    {
        foreach(MeshFilter mesh in Selection.GetFiltered(typeof(MeshFilter),SelectionMode.ExcludePrefab))
        {
            // Disable all mesh colliders on object and children (GetComponentsInChildren also returns self so that works for us here
            foreach(Collider child in mesh.GetComponentsInChildren<Collider>())
            {
                Undo.RecordObject(child, "");
                child.enabled = false;
            }

            // Add mesh collider to object
            MeshCollider collider = Undo.AddComponent<MeshCollider>(mesh.gameObject);

            // Set mesh to same mesh as mesh filter
            collider.sharedMesh = mesh.sharedMesh;
        }
    }

    [MenuItem("GameObject/Use Model As Collision Mesh", true, 32)]
    static bool UseModelAsCollisionMeshValidation()
    {
        if (Selection.GetFiltered(typeof(MeshFilter),SelectionMode.ExcludePrefab).Length == 0)
        {
            return false;
        }
        return true;
    }
}
