using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Unity.VisualScripting;

[CustomEditor(typeof(BaseScriptableVariable), editorForChildClasses: true)]
public class ScriptableVariableInspector : Editor
{
    private bool _lockedInitialValue = true;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_developerDescription"));
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(_lockedInitialValue && EditorApplication.isPlayingOrWillChangePlaymode);
        TryDrawSerializedField(serializedObject.FindProperty("_initialValue"), drawWarningWhenUnserializable: true);
        EditorGUI.EndDisabledGroup();
        if (EditorApplication.isPlaying)
        {
            _lockedInitialValue = GUILayout.Toggle(_lockedInitialValue, "", new GUIStyle("IN LockButton") { fixedHeight = 16, margin = new RectOffset(0, 2, 4, 0) });
        }
        EditorGUILayout.EndHorizontal();

        using (new EditorGUI.DisabledGroupScope(!EditorApplication.isPlaying))
        {
            TryDrawSerializedField(serializedObject.FindProperty("_value"));
        }

        using (new EditorGUI.DisabledGroupScope(true))
        {
            TryDrawSerializedField(serializedObject.FindProperty("_oldValue"));
        }

    }

    private void TryDrawSerializedField(SerializedProperty property, bool drawWarningWhenUnserializable = false)
    {
        if (property == null)
        {
            if (drawWarningWhenUnserializable)
            {
                EditorGUILayout.HelpBox("Can't display values because the type is not serializable! You can still use this type, but won't be able to show values in the Editor.", MessageType.Warning);
            }
        }
        else
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(property);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
