using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EditorLoggerAttribute : Attribute
    {
        public string scriptFileName = string.Empty;
        public EditorLoggerAttribute(string scriptFileName = null)
        {
            if (scriptFileName != null)
            {
                this.scriptFileName = scriptFileName;
            }
        }
    }

    public static class EditorLogger
    {
        static EditorLogger()
        {
            var assems = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var ass in assems)
            {
                var types = ass.GetTypes().Where(t => t.GetCustomAttribute<EditorLoggerAttribute>() != null).ToArray();
                foreach (var t in types)
                {
                    EditorLoggerAttribute editorLoggerAttribute = t.GetCustomAttribute<EditorLoggerAttribute>();
                    if (string.IsNullOrEmpty(editorLoggerAttribute.scriptFileName))
                    {
                        editorLoggerAttribute.scriptFileName = t.Name;
                    }
                    string[] assetPaths = AssetDatabase.FindAssets(string.Format("{0} t:script", editorLoggerAttribute.scriptFileName)).Select(AssetDatabase.GUIDToAssetPath).ToArray();
                    if (assetPaths.Length > 1)
                    {
                        assetPaths = assetPaths.Where(assetPath => Path.GetFileNameWithoutExtension(assetPath) == editorLoggerAttribute.scriptFileName).ToArray();
                    }
                    if (assetPaths.Length == 1)
                    {
                        string path = assetPaths[0];
                        UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                        var id = asset.GetInstanceID();
                        loggerFullNames[id] = path;
                    }
                }
            }
        }

        private static readonly Dictionary<int, string> loggerFullNames = new Dictionary<int, string>();

        private static readonly Regex rgFileLine = new Regex(@"\(at (.*.cs):(\d+)\)");

        [UnityEditor.Callbacks.OnOpenAsset(-1)]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            if (loggerFullNames.ContainsKey(instanceID))
            {
                string assetName = loggerFullNames[instanceID];
                var lines = GetActiveStackTraceLines();

                string fileName = null;
                int fileLine = 0;

                string fileNameCurLine;
                int fileLineCurLine;

                for (int i = lines.Length - 1; i >= 0; --i)
                {
                    if (ExtractAssetNameAndLineNum(lines[i], out fileNameCurLine, out fileLineCurLine))
                    {
                        if (fileNameCurLine == assetName)
                        {
                            // retrieve the last saved
                            break;
                        }
                        else
                        {
                            // save for next
                            fileName = fileNameCurLine;
                            fileLine = fileLineCurLine;
                        }
                    }
                }

                if(!string.IsNullOrEmpty(fileName))
                {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(fileName), fileLine);
                    return true;
                }
            }
            return false;
        }

        private static string[] GetActiveStackTraceLines()
        {
            var consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            var fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            var consoleWindowInstance = fieldInfo.GetValue(null);

            if (null != consoleWindowInstance)
            {
                if ((object)EditorWindow.focusedWindow == consoleWindowInstance)
                {
                    fieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
                    string activeText = fieldInfo.GetValue(consoleWindowInstance).ToString();
                    return activeText.Split('\n');
                }
            }
            return Array.Empty<string>();
        }


        private static bool ExtractAssetNameAndLineNum(string stackTraceLine, out string assetName, out int lineNum)
        {
            assetName = string.Empty;
            lineNum = -1;

            MatchCollection mc = rgFileLine.Matches(stackTraceLine);
            for (int i = 0; i < mc.Count; i++)
            {
                GroupCollection gc = mc[i].Groups;
                if (gc.Count == 3)
                {
                    assetName = gc[1].ToString();

                    if (string.IsNullOrEmpty(assetName))
                    {
                        continue;
                    }
                    if (int.TryParse(gc[2].ToString(), out lineNum))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

