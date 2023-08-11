using UnityEditor.UI;
using UnityEditor;

namespace Assets.Scripts.UI.Widgets.Editor
{
    [CustomEditor(typeof(CustomButton), true)]
    [CanEditMultipleObjects]
    public class CustomButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_highlighted"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressed"));
            serializedObject.ApplyModifiedProperties();
            
            base.OnInspectorGUI();
        }
    }
}