using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// Custom inspector class for <see cref="ScriptableEvent"/> objects, allowing for quick testing of the serialized events during play mode.
/// </summary>
[CustomEditor(typeof(ScriptableEvent))]
public class ScriptableEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        using (new EditorGUI.DisabledGroupScope(!EditorApplication.isPlaying))
        {
            if (GUILayout.Button("Force Raise Event"))
            {
                (target as ScriptableEvent).Raise();
            }
        }
    }
}
