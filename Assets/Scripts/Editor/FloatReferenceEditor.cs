using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(FloatReference))]
public class FloatReferenceEditor : PropertyDrawer
{
    private Editor floatVarEditor; // Store the editor for the FloatVariable instance
    private GUIContent useConstantLabel = new GUIContent("Use Constant");
    private SerializedProperty useConstant;
    private SerializedProperty constantValue;
    private SerializedProperty variable;
    // Calculate space for the dropdown and the field
    private Rect dropDownRect;
    private Rect valueRect;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Cache properties the first time OnGUI runs
        if (useConstant == null)
        {
            useConstant = property.FindPropertyRelative(nameof(FloatReference.UseConstant));
            constantValue = property.FindPropertyRelative(nameof(FloatReference.ConstantValue));
            variable = property.FindPropertyRelative(nameof(FloatReference.Variable));
        }

        //Mark start of draw
        EditorGUI.BeginProperty(position, label, property);
        //Check to see if changes need to be made in draw
        EditorGUI.BeginChangeCheck();
        
        // Draw the label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        dropDownRect = new Rect(position.x, position.y, 80, position.height);
        valueRect = new Rect(position.x + 85, position.y, position.width - 85, position.height);
        // Display a dropdown instead of a checkbox
        useConstant.boolValue = EditorGUI.Popup(dropDownRect, useConstant.boolValue ? 0 : 1, new[] { "Constant", "Variable" }) == 0;

        if (useConstant.boolValue)
        {
            EditorGUI.PropertyField(valueRect, constantValue, GUIContent.none);
        }
        else
        {
            EditorGUI.PropertyField(valueRect, variable, GUIContent.none);

            if (variable.objectReferenceValue != null)
            {
                // Create or reuse the FloatVariable editor
                Editor.CreateCachedEditor(variable.objectReferenceValue, null, ref floatVarEditor);

                // Draw the FloatVariable's value field
                if (floatVarEditor != null)
                {
                    EditorGUI.indentLevel++;
                    floatVarEditor.OnInspectorGUI();
                    EditorGUI.indentLevel--;
                }
            }
        }

        // Apply property modifications if anything changed
        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.EndProperty();
    }
}
