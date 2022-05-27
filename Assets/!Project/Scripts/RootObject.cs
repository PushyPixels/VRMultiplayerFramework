using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A useful script to put on the root of an "object" to specify where it starts.  It also has useful caching options.
// The cache is not perfect though; any components added after a cached GetComponent call will not be present.
// One workaround could be a replacement to AddComponent that goes through here.
public class RootObject : MonoBehaviour
{
    private Dictionary<Type, Component> shallowComponentCache = new Dictionary<Type, Component>();
    private Dictionary<Type, List<Component>> fullComponentCache = new Dictionary<Type, List<Component>>();

    public GameObject GetRoot()
    {
        return gameObject;
    }

    public T GetComponentInRootObject<T>() where T : Component
    {
        Component newComponent = null;

        // Attempt to get from shallow cache
        shallowComponentCache.TryGetValue(typeof(T), out newComponent);

        if(newComponent == null)
        {
            // Attempt to get from heirarchy
            newComponent = GetComponentInChildren<T>();

            // Cache the component if we get one
            if(newComponent != null)
            {
                shallowComponentCache.Add(typeof(T),newComponent);

                List<Component> newComponentList = new List<Component>();
                newComponentList.AddRange(GetComponentsInChildren<T>()); // No need to check for existence, should have at least 1 entry
                fullComponentCache.Add(typeof(T),newComponentList);
            }
        }

        return newComponent as T;
    }

    public T[] GetComponentsInRootObject<T>() where T : Component
    {
        List<Component> newComponentList = null;

        // Attempt to get from full cache
        fullComponentCache.TryGetValue(typeof(T), out newComponentList);

        if(newComponentList == null || newComponentList.Count == 0)
        {
            newComponentList = new List<Component>();
            newComponentList.AddRange(GetComponentsInChildren<T>());

            // Cache the component if we get one
            if(newComponentList.Count > 0)
            {
                fullComponentCache.Add(typeof(T),newComponentList);
                shallowComponentCache.Add(typeof(T),newComponentList[0]);
            }
        }

        return newComponentList.ToArray() as T[];
    }
}

public static class RootObjectExtentionMethods
{
    public static GameObject GetRoot(this GameObject obj)
    {
        RootObject root = obj.GetComponentInParent<RootObject>();
        
        if(root == null)
        {
            Debug.LogWarning("No RootObject found in parents!",obj);
            return null;
        }

        return root.GetRoot();
	}

    public static T GetComponentInRootObject<T>(this GameObject obj) where T : Component
    {
        RootObject root = obj.GetComponentInParent<RootObject>();
        
        if(root == null)
        {
            Debug.LogWarning("No RootObject found in parents!",obj);
            return null;
        }

        return root.GetComponentInRootObject<T>();
	}

    public static T[] GetComponentsInRootObject<T>(this GameObject obj) where T : Component
    {
        RootObject root = obj.GetComponentInParent<RootObject>();
        
        if(root == null)
        {
            Debug.LogWarning("No RootObject found in parents!",obj);
            return null;
        }

        return root.GetComponentsInRootObject<T>();
	}
}
