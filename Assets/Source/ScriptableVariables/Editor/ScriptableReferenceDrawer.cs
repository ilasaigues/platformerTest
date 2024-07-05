using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BaseScriptableReference), true)]
public class ScriptableReferenceDrawer : PropertyDrawer
{
    private GUIStyle _popupStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        _popupStyle ??= new GUIStyle(GUI.skin.GetStyle("PaneOptions"))
        {
            imagePosition = ImagePosition.ImageOnly
        };

        using (var scope = new EditorGUI.PropertyScope(position, label, property))
        {
            GuiData guiData = new GuiData()
            {
                Position = position,
                Property = property,
                Label = label
            };

            guiData.Label = scope.content;
            guiData.Position = EditorGUI.PrefixLabel(position, label);
            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            {
                EditorGUI.BeginChangeCheck();
                {
                    CheckForDraggedReference(guiData);
                    DrawUsageButton(guiData, property);
                    DrawField(guiData);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
            EditorGUI.indentLevel = indent;
        }
    }

    void DrawField(GuiData guiData)
    {
        var property = guiData.Property.FindPropertyRelative(GetUsageIndex(guiData.Property) == 0 ? "_localValue" : "_reference");
        EditorGUI.PropertyField(guiData.Position, property, GUIContent.none);
    }

    private void DrawUsageButton(GuiData guiData, SerializedProperty property)
    {
        var options = new GUIContent[2];
        options[0] = new GUIContent("Local");
        options[1] = new GUIContent("Reference");
        Rect button = new(guiData.Position);
        button.yMin += _popupStyle.margin.top;
        button.yMax = button.yMin + EditorGUIUtility.singleLineHeight;
        button.width = _popupStyle.fixedWidth + _popupStyle.margin.right;
        button.x -= button.width;
        SetUsageIndex(property, EditorGUI.Popup(button, GetUsageIndex(property), options, _popupStyle));
    }

    private void CheckForDraggedReference(in GuiData guiData)
    {
        EventType mouseEventType = Event.current.type;

        if (mouseEventType != EventType.DragUpdated && mouseEventType != EventType.DragPerform)
        {
            return;
        }

        if (!IsMouseHoveringOverProperty(guiData.Position))
        {
            return;
        }

        var draggedObjects = DragAndDrop.objectReferences;
        if (draggedObjects.Length < 1)
        {
            return;
        }

        Object draggedObject = draggedObjects[0];

        if (draggedObject is BaseScriptableVariable draggedScriptableVariable)
        {
            SetToReferenceFromDrop(guiData.Property, draggedScriptableVariable);
        }
    }

    private int GetUsageIndex(SerializedProperty property)
    {
        return property.FindPropertyRelative("_usage").intValue;
    }

    private void SetUsageIndex(SerializedProperty property, int index)
    {
        property.FindPropertyRelative("_usage").intValue = index;
    }

    private void SetToReferenceFromDrop(SerializedProperty property, BaseScriptableVariable draggedScriptableVariable)
    {
        property.FindPropertyRelative("_reference").objectReferenceValue = draggedScriptableVariable;
        SetUsageIndex(property, 1);
    }

    private static bool IsMouseHoveringOverProperty(in Rect rectPosition)
    {
        const int HEIGHT_OFFSET_TO_AVOID_OVERLAP = 1;
        Rect controlRect = rectPosition;
        controlRect.height -= HEIGHT_OFFSET_TO_AVOID_OVERLAP;

        return controlRect.Contains(Event.current.mousePosition);
    }
}
