using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UObject = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace AillieoUtils
{
    [Serializable]
    public struct ObjRefPair
    {
        public string name;
        public UObject obj;
    }

    public class ObjectShortCuts : MonoBehaviour
    {
        [SerializeField][HideInInspector]
        private ObjRefPair[] objRefs;

#if UNITY_EDITOR
        public void CleanUp()
        {
            int newCount = 0;
            for (int i = 0; i < objRefs.Length; i++)
            {
                if (objRefs[i].obj != null)
                {
                    newCount++;
                }
                else
                {
                    for (int j = i + 1; j < objRefs.Length; j++)
                    {
                        if (objRefs[j].obj != null)
                        {
                            objRefs[i] = objRefs[j];
                            objRefs[j].obj = null;
                            newCount++;
                            break;
                        }
                    }
                }
            }
            Array.Resize(ref objRefs, newCount);
        }

#endif

        public UObject this[string key]
        {
            get
            {
                foreach (var pair in this.objRefs)
                {
                    if (pair.name == key)
                    {
                        return pair.obj;
                    }
                }
                return null;
            }
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ObjectShortCuts))]
    public class ObjectShortCutsEditor : Editor
    {

        private ReorderableList objRefList;
        private SerializedProperty goRefs;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            this.objRefList.DoLayoutList();

            if (!EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Clean up"))
                {
                    ObjectShortCuts tar = (ObjectShortCuts)target;
                    tar.CleanUp();
                    serializedObject.Update();
                }
            }

            this.serializedObject.ApplyModifiedProperties();

        }

        private void OnEnable()
        {
            goRefs = serializedObject.FindProperty("objRefs");
            objRefList = new ReorderableList(serializedObject, this.goRefs);
            objRefList.drawHeaderCallback += rect => GUI.Label(rect, "Object Refs");
            objRefList.elementHeight = EditorGUIUtility.singleLineHeight;
            objRefList.drawElementCallback += (rect, index, isActive, isFocused) => {
                this.DrawObjRefs(this.goRefs,rect,index,isActive,isFocused);
            };
        }

        private void OnDisable()
        {

        }

        private void DrawObjRefs(SerializedProperty property, Rect rect,int index,bool isActive,bool isFocused)
        {
            var element = property.GetArrayElementAtIndex(index);
            var nameProp = element.FindPropertyRelative("name");
            var objProp = element.FindPropertyRelative("obj");

            var halfWidth = rect.width / 2;
            var rectLeft = new Rect(
                rect.x,
                rect.y,
                halfWidth,
                rect.height);

            EditorGUI.PropertyField(rectLeft, nameProp, GUIContent.none);

            var rectRight = new Rect(
                rect.x + halfWidth,
                rect.y,
                halfWidth,
                rect.height);
            EditorGUI.PropertyField(rectRight, objProp, GUIContent.none);
        }
    }
#endif

}
