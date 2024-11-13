using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FloatVariable))]
public class FloatVariableEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    // We want to draw everything but the script field, so we exclude it explicitly.
    //    serializedObject.Update();

    //    // Hide the "Script" field: Disable the drawing of the "Script" field.
    //    SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");
    //    if (scriptProperty != null)
    //    {
    //        // Do not display the "m_Script" field in the custom editor
    //        EditorGUI.BeginDisabledGroup(true);
    //        EditorGUILayout.PropertyField(scriptProperty, true);  // Hide the script field by making it uneditable
    //        EditorGUI.EndDisabledGroup();
    //    }

    //    // Draw other properties manually (without the script field)
    //    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(FloatVariable.Value)));

    //    serializedObject.ApplyModifiedProperties();
    //}
}
