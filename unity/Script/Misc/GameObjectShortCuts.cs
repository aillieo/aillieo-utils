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
    public class ComponentAccessor
    {
        public ComponentAccessor(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.transform = gameObject.transform;
        }

        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }
        public T GetComponent<T>() where T : Component
        {
            if(cachedComponents.ContainsKey(typeof(T)))
            {
                return cachedComponents[typeof(T)] as T; 
            }
            T comp = gameObject.GetComponent<T>();
            if(comp != null)
            {
                cachedComponents.Add(typeof(T),comp);
                return comp;
            }
            return null;
        }
        private Dictionary<Type, Component> cachedComponents = new Dictionary<Type, Component>();
    }


    [Serializable]
    public struct GORefPair
    {
        public string name;
        public GameObject gameObject;
    }

    public class GameObjectShortCuts : MonoBehaviour
    {
        [SerializeField][HideInInspector]
        private GORefPair[] goRefs;

#if UNITY_EDITOR
        public void CleanUp()
        {
            int newCount = 0;
            for (int i = 0; i < goRefs.Length; i++)
            {
                if (goRefs[i].gameObject != null)
                {
                    newCount++;
                }
                else
                {
                    for (int j = i + 1; j < goRefs.Length; j++)
                    {
                        if (goRefs[j].gameObject != null)
                        {
                            goRefs[i] = goRefs[j];
                            goRefs[j].gameObject = null;
                            newCount++;
                            break;
                        }
                    }
                }
            }
            Array.Resize(ref goRefs, newCount);
        }

#endif

        public ComponentAccessor this[string key]
        {
            get
            {
                ComponentAccessor componentAccessor = null;
                dict.TryGetValue(key, out componentAccessor);
                return componentAccessor;
            }
        }

        private Dictionary<string, ComponentAccessor> dict = new Dictionary<string, ComponentAccessor>();
        private void Awake()
        {
            foreach (var pair in this.goRefs)
            {
                this.dict[pair.name] = new ComponentAccessor(pair.gameObject);
            }
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(GameObjectShortCuts))]
    public class GameObjectShortCutsEditor : Editor
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
                    GameObjectShortCuts tar = target as GameObjectShortCuts;
                    tar.CleanUp();
                    serializedObject.Update();
                }
            }

            this.serializedObject.ApplyModifiedProperties();

        }

        private void OnEnable()
        {
            goRefs = serializedObject.FindProperty("goRefs");
            objRefList = new ReorderableList(serializedObject, this.goRefs);
            objRefList.drawHeaderCallback += rect => GUI.Label(rect, "GameObject ShortCuts");
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
            var objProp = element.FindPropertyRelative("gameObject");

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
