using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class PlaySceneEditor
{
    /// <summary>
    /// �ṩ��������GUI��ť
    /// ���"Main"�л���Main��������Ϸ
    /// ���"UI"�л���UI��������UIԤ����
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
    /// ��������������Ҫ�Զ���·��
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
    /// �л��� UI ��������Ҫ�Զ���·��
    /// </summary>

    public static void SwitchUI()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/TFrame/UI.unity", OpenSceneMode.Single);
        EditorApplication.isPlaying = false;
    }

}
