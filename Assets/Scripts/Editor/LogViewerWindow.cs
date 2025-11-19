using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LogViewerWindow : EditorWindow
{
    private class NodeView
    {
        public FlowLogger.LogEntry log;
        public Rect rect;
        public NodeView nextNode;
        public bool isDragging;
        public Vector2 dragOffset;
        public string title;

        public NodeView(FlowLogger.LogEntry _log, Vector2 pos)
        {
            log = _log;
            rect = new Rect(pos.x, pos.y, nodeWidth, nodeHeight);
        }

        public NodeView(string title, Vector2 position)
        {
            this.title = title;
            rect = new Rect(position.x, position.y, 200, 80);
        }
    }

#region Field
    private const string SessionKey = "MyWindow_Initialized";
    private List<NodeView> _nodeViews = new List<NodeView>();
    private GUIStyle _logStyle;
    private GUIStyle _nodeStyle;
    private Vector2 _scroll;
    private Vector2 panOffset = Vector2.zero;
    private const int MaxLogs = 100;
    private const float nodeWidth = 200f;
    private const float nodeHeight = 100f;
    private const float spacing = 50f;
    private float zoom = 1f;
#endregion

    [MenuItem("Tools/Debug Log Viewer")]
    public static void ShowWindow()
    {
        GetWindow<LogViewerWindow>("Debug Log Viewer");
    }

    private void OnEnable()
    {
        if (SessionState.GetBool(SessionKey, false))
        {
            _nodeStyle = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true,
                fontSize = 11,
                alignment = TextAnchor.UpperLeft,
                padding = new RectOffset(10, 10, 5, 5)
            };
            SessionState.SetBool(SessionKey, true);
        }
        EditorApplication.update += Repaint; // フレームごとに再描画
    }

    private void OnDisable()
    {
        EditorApplication.update -= Repaint;
    }

    void OnGUI()
    {

        if (GUILayout.Button("Clear Logs"))
        {
            FlowLogger.Clear();
            _nodeViews.Clear();
        }

        var logs = FlowLogger.GetLogs();
        var count = FlowLogger.GetLogs().Count;

        // 表示上限に制限（最新 MaxLogs 件）
        int skipCount = Mathf.Max(0, logs.Count - MaxLogs);

        // 新しく追加されたログをノードに変換
        for (int i = _nodeViews.Count; i < logs.Count; ++i)
        {
            var log = logs[i];
            Vector2 pos = new Vector2(50, i * (nodeHeight + spacing));
            var node = new NodeView(log, pos);

            if (i > 0)
            {
                _nodeViews[i - 1].nextNode = node;
            }

            _nodeViews.Add(node);
        }

        var canvasSize = new Vector2(5000, 5000);
        Rect canvasRect = new Rect(0, 0, canvasSize.x, canvasSize.y);

        HandlePanAndZoom();

        // GUI.matrix をズームとパンに合わせる
        Matrix4x4 prevMatrix = GUI.matrix;
        GUI.matrix = Matrix4x4.TRS(panOffset, Quaternion.identity, Vector3.one * zoom);

        //DrawConnections();
        //DrawNode();

        GUI.BeginGroup(canvasRect);

        // はみ出し確認用：大きく外側にノードを配置する
        for (int i = 0; i < 20; i++)
        {
            var rect = new Rect(50, i * 150, 200, 100);
            GUI.Box(rect, $"Node {i}");
        }

        GUI.EndGroup();
        GUI.matrix = prevMatrix; // 元に戻す
    }

    private void DrawBox()
    {

        // ノードの合計表示サイズ
        //float contentHeight = (nodeHeight + spacing) * (logs.Count - skipCount);
        //Rect scrollViewRect = new Rect(0, 50, position.width, position.height - 50);
        //Rect contentRect = new Rect(0, 0, position.width - 20, contentHeight + 20);

        // ボックスで描画処理
        //float y = 10f;
        //for (int i = skipCount; i < logs.Count; ++i)
        //{
        //    float top = y;
        //    float bottom = y + nodeHeight;

        //    // カリング処理画面の外は描画しない
        //    if((bottom >= _scroll.y) && (top <= _scroll.y + scrollViewRect.height))
        //    {
        //        var log = logs[i];
        //        Rect nodeRect = new Rect(10,y,nodeWidth,nodeHeight);
        //        GUI.Box(nodeRect,$"{ log.Info.FileName}\n{ log.Info.MethodName}\n"
        //        + $"Frame:{log.Frame} Time:{log.Time:F2}",_logStyle);
        //    }

        //    y += nodeHeight + spacing;

        //    //EditorGUILayout.LabelField($"[Frame:{log.Frame}] [Time:{log.Time:F2}] {log.Message}");
        //}
    }

    /// <summary>
    /// ノードを描画する
    /// </summary>
    private void DrawNode()
    {
        foreach (var node in _nodeViews)
        {
            GUI.Box(node.rect, "", EditorStyles.helpBox);

            var labelRect = new Rect(node.rect.x + 10, node.rect.y + 10, node.rect.width - 20, 20);
            GUI.Label(labelRect, node.title);

            // ドラッグ処理
            if (node.isDragging)
            {
                Vector2 mouse = (Event.current.mousePosition - panOffset) / zoom;
                node.rect.position = mouse - node.dragOffset;
                GUI.changed = true;
            }
        }
    }

    private void DrawLine()
    {
        Handles.color = Color.green;

        for (int i = 0; i < _nodeViews.Count; i++)
        {
            var node = _nodeViews[i];
            if (node.nextNode != null)
            {
                Vector3 start = new Vector3(node.rect.center.x, node.rect.yMax);
                Vector3 end = new Vector3(node.nextNode.rect.center.x, node.nextNode.rect.yMin);
                Handles.DrawLine(start, end);
            }
        }
    }

    private void DrawArrow(Vector3 position, Vector3 direction, float size = 8f)
    {
        Vector3 right = Quaternion.Euler(0, 0, 20) * direction;
        Vector3 left = Quaternion.Euler(0, 0, -20) * direction;

        Vector3 p1 = position;
        Vector3 p2 = position + right.normalized * size;
        Vector3 p3 = position + left.normalized * size;

        Handles.DrawAAConvexPolygon(p1, p2, p3);
    }

    private void DrawConnections()
    {
        Handles.BeginGUI();
        Handles.color = Color.green;

        for (int i = 0; i < _nodeViews.Count - 1; i++)
        {
            var from = _nodeViews[i].rect;
            var to = _nodeViews[i + 1].rect;

            Vector3 start = new Vector3(from.center.x, from.yMax);
            Vector3 end = new Vector3(to.center.x, to.yMin);

            Handles.DrawLine(start, end);
            DrawArrow(end, Vector3.up * -1f);
        }

        Handles.EndGUI();
    }

    private void HandlePanAndZoom()
    {
        // パン（右ドラッグ）
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
        {
            panOffset += Event.current.delta;
            Event.current.Use();
        }

        // ズーム（Ctrl + ホイール）
        if (Event.current.type == EventType.ScrollWheel)
        {
            float zoomDelta = -Event.current.delta.y * 0.01f;
            zoom = Mathf.Clamp(zoom + zoomDelta, 0.5f, 2f);
            Event.current.Use();
        }
    }
}
