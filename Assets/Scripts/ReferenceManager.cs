using UnityEngine;
using System;
using System.Collections.Generic;

//still unsure about this script. gets and stores references. Probably should be used for references that will change during the scene.
//references that must be updated on sceneload (maybe for objects that are not destroyed on scene load) should be updated somewhere else
public class ReferenceManager : MonoBehaviour
{
    private static ReferenceManager _instance;
    public static ReferenceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ReferenceManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ReferenceManager");
                    _instance = obj.AddComponent<ReferenceManager>();
                }
            }
            return _instance;
        }
    }

    private Dictionary<string, object> references = new Dictionary<string, object>();

    //event that, in case transform stored in the register changes (because player switches characters),updates the player transform
    public event Action<Transform> OnPlayerTransformChanged;

    public void Register<T>(string key, T reference)
    {
        references[key] = reference;
        if (key == "PlayerTransform" && reference is Transform transform)
        {
            OnPlayerTransformChanged?.Invoke(transform);
        }
    }

    public T Get<T>(string key)
    {
        references.TryGetValue(key, out var reference);
        return (T)reference;
    }
}