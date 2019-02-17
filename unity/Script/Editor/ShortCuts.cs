using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UObject = UnityEngine.Object;
using Process = System.Diagnostics.Process;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace AillieoUtils.Editor
{

    public class ShortCuts
    {

        [MenuItem("AillieoUtils/ShortCuts/StartUp")]
        static void StartUp()
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Game/Startup.unity");
            EditorApplication.isPlaying = true;
        }



        [MenuItem("GameObject/AillieoUtils/GetFullPathToRoot", false, 0)]
        public static void CopyTransPath()
        {
            Transform trans = Selection.activeTransform;
            if (trans == null)
                return;
            string path = GetTransPath(trans);
            Debug.Log(path);
            GUIUtility.systemCopyBuffer = path;
        }

        static string GetTransPath(Transform trans)
        {
            if (null == trans)
                return string.Empty;
            if (null == trans.parent)
                return trans.name;
            return GetTransPath(trans.parent) + "/" + trans.name;
        }

        [MenuItem("AillieoUtils/ShortCuts/Fix Prefabs", false, 100)]
        public static void FixPrefabs()
        {
            ProcessAllPrefabs("Game/Prefabs/UI");
        }

        private static void ProcessAllPrefabs(string path)
        {
            string fullPath = Path.Combine(Application.dataPath, path);
            string[] allPrefab = Directory.GetFiles(fullPath, "*.prefab", SearchOption.AllDirectories);
            foreach (var item in allPrefab)
            {
                string newPath = item.Replace(Application.dataPath, string.Empty).Replace("\\", "/");
                newPath = "Assets" + newPath;
                ProcessSinglePrefabs(newPath);
            }
            AssetDatabase.SaveAssets();

            EditorUtility.DisplayDialog("提示", "处理完成", "真棒");
        }

        private static void ProcessSinglePrefabs(string fullPath)
        {
            Debug.Log("正在处理" + fullPath);

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(fullPath);
            RectTransform[] rects = prefab.GetComponentsInChildren<RectTransform>(true);
            foreach (var rect in rects)
            {
                Vector3 oldPos = rect.anchoredPosition3D;
                oldPos.z = 0f;
                rect.anchoredPosition3D = oldPos;
                Vector3 oldScale = rect.transform.localScale;
                bool willFix = true;
                for (int i = 0; i < 3; i++)
                {
                    float s = oldScale[i];
                    if (s > 1.01 || s < 0.99)
                    {
                        willFix = false;
                        break;
                    }
                }
                if (willFix)
                {
                    rect.transform.localScale = Vector3.one;
                }
            }
        }


        private static void InternalOpenFolder(string folder)
        {
            folder = string.Format("\"{0}\"", folder);
            switch (Application.platform)
            {
            case RuntimePlatform.WindowsEditor:
                Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                break;

            case RuntimePlatform.OSXEditor:
                Process.Start("open", folder);
                break;

            default:
                throw new Exception(string.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
            }
        }

        [MenuItem("AillieoUtils/ShortCuts/ProjectFolder", false, 104)]
        public static void OpenFolderAssets()
        {
            InternalOpenFolder(Path.Combine(Application.dataPath, ".."));
        }





    }

}
