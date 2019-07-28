using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CustomEditor(typeof(UIButtonExt), true)]
[CanEditMultipleObjects]

namespace AillieoUtils.UI
{
    public class UIButtonExtEditor : ButtonEditor
    {
        SerializedProperty onDoubleClick;
        SerializedProperty onLongPressed;

        protected override void OnEnable()
        {
            base.OnEnable();
            onDoubleClick = serializedObject.FindProperty("onDoubleClick");
            onLongPressed = serializedObject.FindProperty("onLongPressed");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(onDoubleClick);
            EditorGUILayout.PropertyField(onLongPressed);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
