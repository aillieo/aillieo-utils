using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace AillieoUtils
{
    public class LogWriter
    {
        private readonly string basePath = "/../EditorLogFile/";
        private string fileFullPath;
        private bool autoFlush = false;
        private StringBuilder builder = new StringBuilder();
        private static LogWriter defaultLogWriter;
        public static LogWriter Default
        {
            get
            {
                if (defaultLogWriter == null)
                {
                    defaultLogWriter = new LogWriter("LastEditorLog.txt", true);
                }
                return defaultLogWriter;
            }
        }
        public LogWriter(string fileName = null, bool autoFlush = true)
        {
            this.fileFullPath = GetFileFullName(fileName);
            EnsureDirectory();
            EnsureFile();
            WriteHead();
            this.autoFlush = autoFlush;
        }

        private string GetFileFullName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            }
            if (string.IsNullOrWhiteSpace(Path.GetExtension(fileName)))
            {
                fileName += ".txt";
            }
            return Application.dataPath + basePath + fileName;
        }

        [Conditional("UNITY_EDITOR")]
        private void WriteHead()
        {
            Write(string.Format("{0}@{1}\n{2}\n{3}",SystemInfo.deviceName,SystemInfo.deviceModel,DateTime.Now.ToString(),new string('-', 80)));
            Flush();
        }

        [Conditional("UNITY_EDITOR")]
        private void EnsureDirectory()
        {
            string dir = Application.dataPath + basePath;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void EnsureFile()
        {
            if(File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
            }
        }

        [Conditional("UNITY_EDITOR")]
        public void Write(object obj)
        {
            builder.Append(obj);
            builder.AppendLine();
            if (autoFlush)
            {
                Flush();
            }
        }

        [Conditional("UNITY_EDITOR")]
        public void Write(string fmt, params object[] objs)
        {
            builder.AppendFormat(fmt, objs);
            builder.AppendLine();
            if (autoFlush)
            {
                Flush();
            }
        }

        [Conditional("UNITY_EDITOR")]
        public void Flush()
        {
            using (StreamWriter writer = new StreamWriter(fileFullPath, true))
            {
                writer.Write(builder.ToString());
                builder.Clear();
                writer.Flush();
            }
        }
    }
}
