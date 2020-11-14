using System;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace UF.Editor
{
    public class OnOpenAssetEditor : UnityEditor.AssetModificationProcessor
    {
        public struct FileBundle
        {
            public string environmentPath;//自己在本机设置环境变量，第一次设置需要重启Unity，因为软件打开时会自动映射一份系统变量
            public string exeName;
            public string suffix;
        }

        [UnityEditor.Callbacks.OnOpenAsset(1)]//打开任意Asset文件会调用下面方法
        public static bool OpenAssetBySuffix(int instanceID, int line)
        {
            string strFilePath = AssetDatabase.GetAssetPath(EditorUtility.InstanceIDToObject(instanceID));
            strFilePath = strFilePath.Replace("/", "\\");
            string strFileName = Directory.GetParent(Application.dataPath) + "\\" + strFilePath;
            
            if (strFileName.EndsWith(".shader"))
            {
                var bundle = new FileBundle()
                {
                    environmentPath = "VSCodePath",
                    exeName = "Code.exe",
                    suffix = ".shader"
                };
                return OpenFile(strFileName, bundle);
            }
            else if(strFileName.EndsWith(".md"))
            {
                var bundle = new FileBundle()
                {
                    environmentPath = "VSCodePath",
                    exeName = "Code.exe",
                    suffix = ".md"
                };
                return OpenFile(strFileName, bundle);
            }
            return false;
        }

        public static bool OpenFile(string strFileName, FileBundle bundle)
        {
            string editorPath = Environment.GetEnvironmentVariable(bundle.environmentPath);
            if (editorPath != null && editorPath.Length > 0)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = editorPath + (editorPath.EndsWith("\\") ? "" : "\\") + bundle.exeName;
                startInfo.Arguments = "\"" + strFileName + "\"";
                process.StartInfo = startInfo;
                Debug.Log("Open " + strFileName + " By " + startInfo.FileName);
                process.Start();
                return true;
            }
            else
            {
                Debug.LogError("null environment : " + bundle.environmentPath);
                return false;
            }
        }
    }
}
