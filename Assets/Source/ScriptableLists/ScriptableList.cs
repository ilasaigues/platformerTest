using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract ScriptableObject implementation of List.
/// Intended for object enumeration without a need to have a rigid subscription to a manager.
/// Since this is an object that exists in memory, references to runtime objects don't automatically clear themselves from the list.
/// This is solved by calling a function that forcibly cleanses the list on runtime initialization.
/// </summary>
/// <typeparam name="T">Type of objects this list will contain</typeparam>
public abstract class ScriptableList<T> : ScriptableObject
{
    [HideInInspector]
    public List<T> Objects = new();

    public Action<T> OnObjectAdded = (obj) => { };
    public Action<T> OnObjectRemoved = (obj) => { };

    public void Add(T obj)
    {
        if (!Objects.Contains(obj))
        {
            Objects.Add(obj);
            OnObjectAdded(obj);
        }
    }

    public void Remove(T obj)
    {
        if (!Objects.Contains(obj))
        {
            Objects?.Remove(obj);
            OnObjectRemoved(obj);
        }
    }

    public void ForceClear()
    {
        Objects.Clear();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void ClearOnLoad()
    {
        foreach (ScriptableList<T> so in FindObjectsOfType<ScriptableList<T>>())
        {
            so.ForceClear();
        }
    }
}
