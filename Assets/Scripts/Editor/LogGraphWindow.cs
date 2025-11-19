using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class LogGraphWindow : EditorWindow
{
    private LogGraphView graphView;

    [MenuItem("Tools/LogGraphWindow")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<LogGraphWindow>();
        wnd.titleContent = new GUIContent("LogGraphWindow");
    }

    private void OnEnable()
    {
        // ここで一度だけ GraphView を作成
        graphView = new LogGraphView();
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);

        LoadLogs(); // 初期ログを読み込み
    }

    private void OnDisable()
    {
        // イベント解除（リーク防止）
        FlowLogger.OnLogAdded -= graphView.AddLogNode;
    }

    private void LoadLogs()
    {
        graphView.ClearGraph();

        var logs = FlowLogger.GetLogs();

        NodeView prevNode = null;
        foreach (var log in logs)
        {
            var node = new NodeView(log);
            graphView.AddElement(node);

            if (prevNode != null)
            {
                var edge = prevNode.output.ConnectTo(node.input);
                graphView.AddElement(edge);
            }

            prevNode = node;
        }
    }
}
