using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class PlaySceneEditor
{
    /// <summary>
    /// 提供两个场景GUI按钮
    /// 点击"Main"切换至Main场景跑游戏
    /// 点击"UI"切换至UI场景制作UI预设体
    /// </summary>
    static PlaySceneEditor()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView scene_view)
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        Handles.BeginGUI();

        GUI.color = Color.green;

        if (GUI.Button(new Rect(0, 0, 50, 30), "Main"))
        {
            PlayMain();
        }

        if (GUI.Button(new Rect(0, 40, 50, 30), "UI"))
        {
            SwitchUI();
        }

        Handles.EndGUI();
    }

    /// <summary>
    /// 运行主场景，需要自定义路径
    /// </summary>

    public static void PlayMain()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(Application.dataPath + "/TFrame/Main.unity", OpenSceneMode.Single);
            EditorApplication.isPlaying = true;
        }

    }

    /// <summary>
    /// 切换到 UI 场景，需要自定义路径
    /// </summary>

    public static void SwitchUI()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/TFrame/UI.unity", OpenSceneMode.Single);
        EditorApplication.isPlaying = false;
    }

}
