using UnityEditor;

[CustomEditor(typeof(PlayerInputController))]
public class PlayerInputControllerEditor : Editor
{
    //SerializedProperty floatReference;

    //private void OnEnable()
    //{
    //    floatReference = serializedObject.FindProperty(nameof(PlayerInputController._moveSpeed));
    //}

    //public override void OnInspectorGUI()
    //{
    //    //pulls data from object into inspector
    //    serializedObject.Update();

    //    // Draw the default inspector for MonoBehaviour fields
    //    DrawDefaultInspector();

    //    // If the ScriptableObject is assigned, display its fields
    //    if (floatReference.objectReferenceValue != null)
    //    {
    //        EditorGUILayout.LabelField("ScriptableObject Settings", EditorStyles.boldLabel);

    //        // Draw the embedded ScriptableObject inspector
    //        Editor scriptableObjectEditor = CreateEditor(floatReference.objectReferenceValue);
    //        scriptableObjectEditor.OnInspectorGUI();
    //    }
    //    else
    //    {
    //        EditorGUILayout.HelpBox("Assign a ScriptableObject to edit its values here.", MessageType.Info);
    //    }


    //    //Commit changes to object
    //    serializedObject.ApplyModifiedProperties();
    //}
}
