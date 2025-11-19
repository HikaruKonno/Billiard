using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// UnityEditorを再起動するクラス
/// </summary>
public class EditorRestartUnity
{
    [MenuItem("File/Restart")]
    static void RestartUnity()
    {
        string unityPath = EditorApplication.applicationPath;
        string projectPath = System.IO.Directory.GetCurrentDirectory(); // 現在のプロジェクトパス

        // 別のUnityを起動したあとに自身を終了
        Process.Start(unityPath, $"-projectPath \"{projectPath}\"");
        EditorApplication.Exit(0);
    }
}
