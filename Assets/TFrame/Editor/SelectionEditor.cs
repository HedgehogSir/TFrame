using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UF.Editor
{
    public class SelectionEditor
    {
        [InitializeOnLoadMethod]
        private static void Start()
        {
            //在Hierarchy面板按空格键相当于开关GameObject
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;

            //在Project面板按空格键相当于Show In Explorer
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        }

        private static void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
        {
            if (Event.current.type == EventType.KeyDown
                && Event.current.keyCode == KeyCode.Space
                && selectionRect.Contains(Event.current.mousePosition))
            {
                string strPath = AssetDatabase.GUIDToAssetPath(guid);

                if (Path.GetExtension(strPath) == string.Empty) //文件夹
                {
                    Process.Start(Path.GetFullPath(strPath));
                }
                else //文件
                {
                    Process.Start("explorer.exe", "/select," + Path.GetFullPath(strPath));
                }

                Event.current.Use();
            }
        }

        private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                switch (e.keyCode)
                {
                    case KeyCode.Space:
                        ToggleGameObjcetActiveSelf();
                        e.Use();
                        break;
                    case KeyCode.C:

                        e.Use();
                        break;
                }
            }
            else if (e.type == EventType.MouseDown && e.button == 2)
            {
                SetAllActive();
                e.Use();
            }
        }

        private static void ToggleGameObjcetActiveSelf()
        {
            Undo.RecordObjects(Selection.gameObjects, "Active");
            foreach (var go in Selection.gameObjects)
            {
                go.SetActive(!go.activeSelf);
            }
        }

        //按鼠标中键，将Root节点下的所有子物体显示出来
        private static void SetAllActive()
        {
            var children = Selection.activeGameObject.GetComponentsInChildren<Transform>(true);
            foreach (var child in children)
            {
                var gameObj = child.gameObject;
                Undo.RecordObject(gameObj, "SetActive");
                gameObj.SetActive(true);
            }
        }
    }
}