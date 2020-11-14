using UnityEditor;
using System.IO;
using UnityEngine;

namespace TFrame.Editor
{
    public class CreateScriptEditor : UnityEditor.AssetModificationProcessor
    {
        [MenuItem("TFrame/Editor/Tool/更新头文件")]
        public static void UpdateFileHead()
        {
            string NewBehaviourScriptPath = EditorApplication.applicationContentsPath + "/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs.txt";
            string head = "" +
                        //自定义头文件
                        "/*******************************************************************\n"
                        + "* Copyright(c) #YEAR# #AUTHOR#\n"
                        + "* All rights reserved.\n"
                        + "*\n"
                        + "* 文件名称: #SCRIPTFULLNAME# #VERSION#\n"
                        + "* Unity版本: #UNITYVERSION#\n"
                        + "* 简要描述:\n"
                        + "* \n"
                        + "* 创建日期: #DATE#\n"
                        + "* 作者:     #AUTHOR#\n"
                        + "* 说明:  \n"
                        + "******************************************************************/\n"
                        //以下部分unity默认文件
                        + "using UnityEngine;\n"
                        + "\n"
                        + "namespace TFrame\n"
                        + "{\n"
                        + "\tpublic class #SCRIPTNAME# : MonoBehaviour\n"
                        + "\t{\n"
                        + "\t\tvoid Start()\n"
                        + "\t\t{\n"
                        + "\t\t\t#NOTRIM#\n"
                        + "\t\t}\n"
                        + "\n"
                        + "\t\tvoid Update()\n"
                        + "\t\t{\n"
                        + "\t\t\t#NOTRIM#\n"
                        + "\t\t}\n"
                        + "\t}\n"
                        + "}";

            byte[] curTexts = System.Text.Encoding.UTF8.GetBytes(head);
            using (FileStream fs = new FileStream(NewBehaviourScriptPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                if (fs != null)
                {
                    fs.SetLength(0);    //清空文件
                    fs.Write(curTexts, 0, curTexts.Length);
                    fs.Flush();
                    fs.Dispose();

                    Debug.Log("Update File: 81-C# Script-NewBehaviourScript.cs.txt, Success");
                }
            }
        }
    }

    public class ParseFileHead : UnityEditor.AssetModificationProcessor
    {
        /// <summary>  
        /// 此函数在asset被创建完，文件已经生成到磁盘上，但是没有生成.meta文件和import之前被调用  
        /// </summary>  
        /// <param name="newFileMeta">newfilemeta 是由创建文件的path加上.meta组成的</param>  
        public static void OnWillCreateAsset(string newFileMeta)
        {
            string newFilePath = newFileMeta.Replace(".meta", "");
            string fileExt = Path.GetExtension(newFilePath);
            if (fileExt == ".cs")
            {
                //注意，Application.datapath会根据使用平台不同而不同  
                string realPath = Application.dataPath.Replace("Assets", "") + newFilePath;
                string scriptContent = File.ReadAllText(realPath);

                //自定义修改系统信息
                //PlayerSettings.companyName = "";

                //这里现自定义的一些规则  
                scriptContent = scriptContent.Replace("#SCRIPTFULLNAME#", Path.GetFileName(newFilePath));
                scriptContent = scriptContent.Replace("#COMPANY#", PlayerSettings.companyName);
                scriptContent = scriptContent.Replace("#AUTHOR#", "tianci");
                scriptContent = scriptContent.Replace("#VERSION#", "1.0");
                scriptContent = scriptContent.Replace("#UNITYVERSION#", Application.unityVersion);
                scriptContent = scriptContent.Replace("#DATE#", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                scriptContent = scriptContent.Replace("#YEAR#", System.DateTime.Now.ToString("yyyy"));

                File.WriteAllText(realPath, scriptContent);
            }
        }
    }
}
