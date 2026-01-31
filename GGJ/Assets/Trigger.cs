using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Trigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private LayerMask triggerLayers = -1;

    private List<GameObject> objectsInRange = new List<GameObject>();

    public event Action<GameObject> OnObjectEnter;
    public event Action<GameObject> OnObjectExit;

    public IReadOnlyList<GameObject> ObjectsInRange => objectsInRange;
    public int ObjectCount => objectsInRange.Count;
    public bool HasObjectsInRange => objectsInRange.Count > 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsInLayerMask(other.gameObject.layer))
            return;

        if (!objectsInRange.Contains(other.gameObject))
        {
            objectsInRange.Add(other.gameObject);
            OnObjectEnter?.Invoke(other.gameObject);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsInRange.Remove(other.gameObject))
        {
            OnObjectExit?.Invoke(other.gameObject);
        }
    }

    public bool IsObjectInRange(GameObject obj)
    {
        return objectsInRange.Contains(obj);
    }

    public T GetObjectInRange<T>() where T : Component
    {
        foreach (var obj in objectsInRange)
        {
            T component = obj.GetComponent<T>();
            if (component != null)
                return component;
        }
        return null;
    }

    public List<T> GetAllObjectsInRange<T>() where T : Component
    {
        List<T> results = new List<T>();
        foreach (var obj in objectsInRange)
        {
            T component = obj.GetComponent<T>();
            if (component != null)
                results.Add(component);
        }
        return results;
    }

    public void Clear()
    {
        objectsInRange.Clear();
    }

    private bool IsInLayerMask(int layer)
    {
        return (triggerLayers.value & (1 << layer)) != 0;
    }
}
